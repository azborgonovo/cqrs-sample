using CqrsSample.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsSample.Implementation
{
    public class EventBus : IEventBus
    {
        public static object EventQueueLock = new object();
        public static ICollection<IEvent> EventQueue = new List<IEvent>();

        public void Publish(params IEvent[] events)
        {
            lock (EventQueueLock)
            {
                foreach (var @event in events)
                {
                    EventQueue.Add(@event);
                }
            }
        }
    }
}
