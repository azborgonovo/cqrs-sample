using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsSample.Messaging
{
    public interface ICommand
    {
        /// <summary>
        /// Command unique identifier
        /// </summary>
        Guid Id { get; set; }
    }
}
