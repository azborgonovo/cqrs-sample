using CqrsSample.Messaging;
using CqrsSample.Messaging.Handling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CqrsSample.Implementation
{
    class CommandProcessor
    {
        private Dictionary<Type, ICommandHandler> handlers = new Dictionary<Type, ICommandHandler>();

        public void Start()
        {
            var thread = new Thread(ProcessCommandQueue);
            thread.Start();
        }

        private void ProcessCommandQueue()
        {
            while(CommandBus.CommandQueue.Any())
            {
                lock (CommandBus.CommandQueueLock)
                {
                    var command = CommandBus.CommandQueue.FirstOrDefault();
                    
                    if (command == null)
                        break;
                    
                    HandleCommand(command);
                    
                    CommandBus.CommandQueue.Remove(command);
                }
            }

            // Infinite loop...
            Thread.Sleep(Program.DefaultDelay);
            this.ProcessCommandQueue();
        }

        void HandleCommand(ICommand command)
        {
            var commandType = command.GetType();
            ICommandHandler handler = null;

            if (this.handlers.TryGetValue(commandType, out handler))
            {
                Program.Log.Add("-- Command handled by " + handler.GetType().FullName);
                ((dynamic)handler).Handle((dynamic)command);
            }

            // There can be a generic logging/tracing/auditing handlers
            if (this.handlers.TryGetValue(typeof(ICommand), out handler))
            {
                Program.Log.Add("-- Command handled by " + handler.GetType().FullName);
                ((dynamic)handler).Handle((dynamic)command);
            }
        }

        public void Register(ICommandHandler commandHandler)
        {
            var genericHandler = typeof(ICommandHandler<>);
            var supportedCommandTypes = commandHandler.GetType()
                .GetInterfaces()
                .Where(iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == genericHandler)
                .Select(iface => iface.GetGenericArguments()[0])
                .ToList();

            if (handlers.Keys.Any(registeredType => supportedCommandTypes.Contains(registeredType)))
                throw new ArgumentException("The command handled by the received handler already has a registered handler.");

            // Register this handler for each of the handled types.
            foreach (var commandType in supportedCommandTypes)
            {
                this.handlers.Add(commandType, commandHandler);
            }
        }
    }
}
