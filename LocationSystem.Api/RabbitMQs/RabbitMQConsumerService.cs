using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace LocationSystem.Application.Utilities.RabbitMQs
{
    public class RabbitMQConsumerService : BackgroundService
    {
        private readonly IRabbitMQService _rabbitMQService;
        private readonly ILogger<RabbitMQConsumerService> _logger;
        private IChannel? _channel;

        public RabbitMQConsumerService(
            IRabbitMQService rabbitMQService,
            ILogger<RabbitMQConsumerService> logger)
        {
            _rabbitMQService = rabbitMQService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("RabbitMQ 消费者服务启动");
            try
            {
                // 等待连接建立
                await _rabbitMQService.EnsureConnectedAsync(stoppingToken);
                _logger.LogInformation("RabbitMQ 连接成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RabbitMQ 连接失败");
                return; // 连接失败，直接返回，不再继续执行
            }
            // 等待 RabbitMQ 服务初始化完成
            while (!_rabbitMQService.IsConnected && !stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("⏳ 等待 RabbitMQ 连接...");
                await Task.Delay(1000, stoppingToken);
            }

            if (stoppingToken.IsCancellationRequested) return;

            try
            {
                _channel = await _rabbitMQService.CreateChannelAsync();

                // 声明队列
                await _channel.QueueDeclareAsync(
                    queue: "my_queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false
                );

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (ch, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    _logger.LogInformation($"收到消息: {message}");

                    // 处理业务逻辑
                    await ProcessMessageAsync(message);

                    //await _channel.BasicAckAsync(ea.DeliveryTag);
                    // ✅ 正确调用：传递所有必需参数
                    await _channel.BasicAckAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false,  // 是否批量确认
                        cancellationToken: CancellationToken.None
                    );

                };

                await _channel.BasicConsumeAsync(
                    queue: "my_queue",
                    autoAck: false,
                    consumer: consumer
                );

                // 保持服务运行
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "消费者服务异常");
                throw;
            }
        }

        private async Task ProcessMessageAsync(string message)
        {
            // 业务逻辑处理
            await Task.Delay(100);
            Console.WriteLine($"处理: {message}");
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("RabbitMQ 消费者服务停止");
            await base.StopAsync(cancellationToken);
        }
    }
}
