using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Events
{
    public interface IEventBus
    {
        // 发布事件
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : class;
        
        // 订阅事件
        void Subscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : class;
        
        // 取消订阅事件
        void Unsubscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : class;
    }
}