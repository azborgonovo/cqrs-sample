using CqrsSample.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsSample.Implementation
{
    public class CommandBus : ICommandBus
    {
        public static object CommandQueueLock = new object();
        public static ICollection<ICommand> CommandQueue = new List<ICommand>();
        
        public void Send(params ICommand[] commands)
        {
            lock (CommandQueueLock)
            {
                foreach (var command in commands)
	            {
                    CommandQueue.Add(command);
	            }
            }
        }
    }
}
