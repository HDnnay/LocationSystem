using LocationSystem.Api.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Text.Json;

namespace LocationSystem.Application.Utilities.RabbitMQs
{
    public class RabbitMQService : IRabbitMQService
    {
        //根据之前的代码，_connectionTask 的初始化是在构造函数中，但是它的值是一个异步委托，这个委托只有在访问 _connectionTask.MonthlyRent 时才会执行。
        //所以，如果消费者服务一直在等待连接，而 _connectionTask.MonthlyRent 没有被访问，那么连接就永远不会被建立。
        //解决方案：在消费者服务中，我们需要触发连接初始化，即访问 _connectionTask.MonthlyRent。
        private readonly Lazy<Task<IConnection>> _connectionTask;
        private readonly ILogger<RabbitMQService> _logger;
        private readonly IConfiguration _configuration;
        private IConnection? _connection; // 缓存连接实例

        public bool IsConnected => _connection?.IsOpen ?? false;

        public IConnection Connection => _connection?? throw new InvalidOperationException(nameof(_connection));

        public RabbitMQService(IConfiguration configuration, ILogger<RabbitMQService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            // 使用 Lazy 实现延迟且线程安全的异步初始化
            _connectionTask = new Lazy<Task<IConnection>>(async () =>
            {
                var connection = await CreateConnectionAsync();
                _connection = connection; // 设置缓存
                return connection;
            });
        }

        private async Task<IConnection> CreateConnectionAsync()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
                    Port = _configuration.GetValue<int>("RabbitMQ:Port", 15672),
                    UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
                    Password = _configuration["RabbitMQ:Password"] ?? "guest",
                    VirtualHost = _configuration["RabbitMQ:VirtualHost"] ?? "/",
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
                };

                var connection = await factory.CreateConnectionAsync();

                connection.ConnectionShutdownAsync += (sender, args) =>
                {
                    _logger.LogWarning("连接关闭: {ReplyText}", args.ReplyText);
                    return Task.CompletedTask;
                };

                _logger.LogInformation("RabbitMQ 连接已建立");
                return connection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RabbitMQ 连接失败");
                throw;
            }
        }

        public async Task<IChannel> CreateChannelAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var connection = await _connectionTask.Value; // 首次调用时初始化
                return await connection.CreateChannelAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建通道失败");
                throw;
            }
        }

        public async Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default)
        {
            await using var channel = await CreateChannelAsync(cancellationToken);
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);
            var properties = new BasicProperties { Persistent = true };

            await channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: properties,
                body: body,
                cancellationToken: cancellationToken
            );

            _logger.LogDebug("消息已发布到 {Exchange}/{RoutingKey}", exchange, routingKey);
        }

        public async void Dispose()
        {
            if (_connection != null && _connection.IsOpen)
            {
                await _connection.CloseAsync();
                _connection.Dispose();
                _logger.LogInformation("RabbitMQ 连接已关闭");
            }
        }

        public Task EnsureConnectedAsync(CancellationToken cancellationToken = default)
        {
            // 触发连接初始化，并返回任务
            return _connectionTask.Value;
        }
    }
}
