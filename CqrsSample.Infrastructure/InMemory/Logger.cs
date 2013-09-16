using CqrsSample.Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsSample.Infrastructure.InMemory.Logging
{
    public class Logger : ILogger
    {
        ICollection<string> _logs = new List<string>();

        public void Info(string message)
        {
            _logs.Add(message);
        }

        public IEnumerable<string> GetAllLogs()
        {
            return _logs;
        }
    }
}
