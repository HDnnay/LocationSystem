using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LocationSystem.Application.Utilities.RabbitMQs
{
    public class RabbitMQTestService : BackgroundService
    {
        private readonly IRabbitMQService _rabbitMQService;
        private readonly ILogger<RabbitMQTestService> _logger;
        private IConnection? _connection;
        private IChannel? _channel; // 关键变更：使用 IChannel
        private AsyncEventingBasicConsumer? _consumer;
        private readonly string _queueName = "my_queue";
        private const int ReconnectDelayMs = 5000;

        public RabbitMQTestService(
            IRabbitMQService rabbitMQService,
            ILogger<RabbitMQTestService> logger)
        {
            _rabbitMQService = rabbitMQService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("🚀 RabbitMQ 消费者服务启动");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await StartConsumingAsync(stoppingToken);
                    // 如果正常退出循环，表示服务被请求停止
                    break;
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("消费者服务被取消。");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ 消费者发生不可恢复错误，将在 {Delay}ms 后尝试重连", ReconnectDelayMs);
                    await CleanupResourcesAsync();
                    await Task.Delay(ReconnectDelayMs, stoppingToken);
                }
            }
        }

        private async Task StartConsumingAsync(CancellationToken stoppingToken)
        {
            // 1. 建立连接
            await _rabbitMQService.EnsureConnectedAsync(stoppingToken);
            if(_rabbitMQService.Connection==null)
                 throw new InvalidOperationException("无法建立 RabbitMQ 连接。");
            else
                _connection = _rabbitMQService.Connection;

            _connection.ConnectionShutdownAsync += OnConnectionShutdown;
            _connection.CallbackExceptionAsync += OnCallbackException;

            // 2. 创建信道 (IChannel)
            _channel = await _connection.CreateChannelAsync();
            if (_channel == null) throw new InvalidOperationException("无法创建信道。");

            // 3. 声明队列并配置 QoS
            await _channel.QueueDeclareAsync(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: stoppingToken
            );

            await _channel.BasicQosAsync(
                prefetchSize: 0,
                prefetchCount: 1, // 一次只预取一条消息，实现能者多劳
                global: false,
                cancellationToken: stoppingToken
            );

            // 4. 创建并配置消费者
            _consumer = new AsyncEventingBasicConsumer(_channel);
            _consumer.ReceivedAsync += OnMessageReceivedAsync;
            _consumer.ShutdownAsync += OnConsumerShutdownAsync;

            // 5. 开始消费 (手动确认)
            await _channel.BasicConsumeAsync(
                queue: _queueName,
                autoAck: false, // 关键：关闭自动确认，改为手动[citation:2]
                consumer: _consumer,
                cancellationToken: stoppingToken
            );

            _logger.LogInformation($"✅ 已开始监听队列：{_queueName}");

            // 6. 保持任务运行，直到停止请求或连接中断
            while (!stoppingToken.IsCancellationRequested &&
                   _connection?.IsOpen == true &&
                   _channel?.IsOpen == true)
            {
                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogWarning("监听循环结束，连接或信道可能已关闭。");
        }

        private async Task OnMessageReceivedAsync(object sender, BasicDeliverEventArgs ea)
        {
            string message = string.Empty;
            try
            {
                message = Encoding.UTF8.GetString(ea.Body.Span);
                _logger.LogDebug("收到消息，投递标签: {DeliveryTag}", ea.DeliveryTag);

                // 处理业务逻辑
                await ProcessMessageAsync(message);

                // 手动确认消息处理成功
                if (_channel?.IsOpen == true)
                {
                    await _channel.BasicAckAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false,
                        cancellationToken: CancellationToken.None
                    );
                    _logger.LogDebug("消息已确认: {DeliveryTag}", ea.DeliveryTag);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理消息时发生异常，消息内容: {Message}", message);
                // 处理失败，拒绝消息。可根据业务决定是否重新入队 (requeue)[citation:4]
                if (_channel?.IsOpen == true)
                {
                    await _channel.BasicNackAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false,
                        requeue: false, // false表示不重新入队，通常消息会进入死信队列
                        cancellationToken: CancellationToken.None
                    );
                }
            }
        }

        private async Task ProcessMessageAsync(string message)
        {
            // 模拟你的业务处理
            await Task.Delay(100);
            _logger.LogInformation("处理消息: {Message}", message);
        }

        #region 连接与消费者事件处理
        private Task  OnConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            _logger.LogWarning("⚠️ RabbitMQ 连接关闭。原因: {ReplyText}", e.ReplyText);
            return Task.CompletedTask;
        }

        private Task OnCallbackException(object? sender, CallbackExceptionEventArgs e)
        {
            _logger.LogError(e.Exception, "⚠️ RabbitMQ 连接回调发生异常");
            return Task.CompletedTask;
        }

        private Task OnConsumerShutdownAsync(object sender, ShutdownEventArgs e)
        {
            _logger.LogWarning("消费者关闭。原因: {ReplyText}", e.ReplyText);
            return Task.CompletedTask;
        }
        #endregion

        private async Task CleanupResourcesAsync()
        {
            try
            {
                if (_consumer != null)
                {
                    _consumer.ReceivedAsync -= OnMessageReceivedAsync;
                    _consumer.ShutdownAsync -= OnConsumerShutdownAsync;
                    _consumer = null;
                }

                if (_channel?.IsOpen == true)
                {
                    await _channel.CloseAsync();
                }
                _channel?.Dispose();
                _channel = null;

                if (_connection != null)
                {
                    _connection.ConnectionShutdownAsync -= OnConnectionShutdown;
                    _connection.CallbackExceptionAsync -= OnCallbackException;
                }

                _logger.LogDebug("资源清理完成。");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理资源时发生异常");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("RabbitMQ 消费者服务正在停止...");
            await CleanupResourcesAsync();

            if (_connection?.IsOpen == true)
            {
                await _connection.CloseAsync(cancellationToken: cancellationToken);
            }
            _connection?.Dispose();

            await base.StopAsync(cancellationToken);
            _logger.LogInformation("RabbitMQ 消费者服务已停止");
        }
    }
}