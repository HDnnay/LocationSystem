using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LocationSystem.Application.Utilities.RabbitMQs
{
    public interface IRabbitMQService:IDisposable
    {
        IConnection Connection { get; }
        Task<IChannel> CreateChannelAsync(CancellationToken cancellationToken = default);
        Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default);
        bool IsConnected { get; }
        Task EnsureConnectedAsync(CancellationToken cancellationToken = default); // 添加这个方法
    }
}
