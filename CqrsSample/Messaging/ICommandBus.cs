using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsSample.Messaging
{
    interface ICommandBus
    {
        void Send(params ICommand[] commands);
    }
}
