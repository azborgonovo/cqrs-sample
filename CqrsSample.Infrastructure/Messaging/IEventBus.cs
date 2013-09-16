using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsSample.Messaging
{
    public interface IEventBus
    {
        void Publish(params IEvent[] events);
    }
}
