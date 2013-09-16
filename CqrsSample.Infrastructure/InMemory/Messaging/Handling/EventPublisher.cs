using CqrsSample.Infrastructure.Logging;
using CqrsSample.Messaging;
using CqrsSample.Messaging.Handling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CqrsSample.Implementation
{
    public class EventPublisher
    {
        private ILogger _logger;
        private int _poolDelay;
        private Dictionary<Type, ICollection<IEventHandler>> handlersByEventType = new Dictionary<Type, ICollection<IEventHandler>>();

        public EventPublisher(ILogger logger, int poolDelay)
        {
            if (logger == null)
                throw new ArgumentNullException("logger");

            if (poolDelay <= 0)
                throw new ArgumentOutOfRangeException("poolDelay");

            _logger = logger;
            _poolDelay = poolDelay;
        }

        public void Start()
        {
            var thread = new Thread(ProcessEventQueue);
            thread.Start();
        }

        private void ProcessEventQueue()
        {
            while (EventBus.EventQueue.Any())
            {
                lock (EventBus.EventQueueLock)
                {
                    var @event = EventBus.EventQueue.FirstOrDefault();

                    if (@event == null)
                        break;

                    HandleEvent(@event);

                    EventBus.EventQueue.Remove(@event);
                }
            }

            // Infinite loop...
            Thread.Sleep(_poolDelay);
            this.ProcessEventQueue();
        }

        private void HandleEvent(IEvent @event)
        {
            var eventType = @event.GetType();
            ICollection<IEventHandler> handlers = null;

            if (this.handlersByEventType.TryGetValue(eventType, out handlers))
            {
                foreach (var handler in handlers)
                {
                    _logger.Info("-- Event handled by " + handler.GetType().FullName);
                    ((dynamic)handler).Handle((dynamic)@event);    
                }
            }

            // There can be a generic logging/tracing/auditing handlers
            if (this.handlersByEventType.TryGetValue(typeof(IEvent), out handlers))
            {
                foreach (var handler in handlers)
                {
                    _logger.Info("-- Event handled by " + handler.GetType().FullName);
                    ((dynamic)handler).Handle((dynamic)@event);
                }
            }
        }

        public void Subscribe(IEventHandler eventHandler)
        {
            var genericHandler = typeof(IEventHandler<>);
            var supportedEventTypes = eventHandler.GetType()
                .GetInterfaces()
                .Where(iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == genericHandler)
                .Select(iface => iface.GetGenericArguments()[0])
                .ToList();
            
            // Register this handler for each of the handled types.
            foreach (var eventType in supportedEventTypes)
            {
                if (handlersByEventType.ContainsKey(eventType))
                {
                    ICollection<IEventHandler> registeredHandlers;
                    if (handlersByEventType.TryGetValue(eventType, out registeredHandlers))
                    {
                        if (registeredHandlers.Contains(eventHandler))
                            throw new ArgumentException("The event handled by the received handler already has a subscribed handler.");

                        registeredHandlers.Add(eventHandler);
                    }
                    else
                    {
                        handlersByEventType[eventType] = new List<IEventHandler> { eventHandler };
                    }
                }
                else
                {
                    handlersByEventType.Add(eventType, new List<IEventHandler> { eventHandler });
                }
            }
        }
    }
}
