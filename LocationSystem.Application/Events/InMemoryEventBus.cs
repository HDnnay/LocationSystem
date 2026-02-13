using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LocationSystem.Application.Events
{
    public class InMemoryEventBus : IEventBus
    {
        // 事件处理器字典，键为事件类型，值为该事件的所有处理器
        private readonly Dictionary<Type, List<Delegate>> _handlers = new Dictionary<Type, List<Delegate>>();
        
        // 发布事件
        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));
            
            var eventType = typeof(TEvent);
            if (_handlers.TryGetValue(eventType, out var handlers))
            {
                foreach (var handler in handlers)
                {
                    if (handler is Func<TEvent, Task> typedHandler)
                    {
                        await typedHandler(@event);
                    }
                }
            }
        }
        
        // 订阅事件
        public void Subscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : class
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            
            var eventType = typeof(TEvent);
            if (!_handlers.ContainsKey(eventType))
            {
                _handlers[eventType] = new List<Delegate>();
            }
            
            _handlers[eventType].Add(handler);
        }
        
        // 取消订阅事件
        public void Unsubscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : class
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            
            var eventType = typeof(TEvent);
            if (_handlers.TryGetValue(eventType, out var handlers))
            {
                handlers.Remove(handler);
                if (handlers.Count == 0)
                {
                    _handlers.Remove(eventType);
                }
            }
        }
    }
}