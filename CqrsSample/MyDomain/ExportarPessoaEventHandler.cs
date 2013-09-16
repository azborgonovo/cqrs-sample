using CqrsSample.Infrastructure.Logging;
using CqrsSample.Messaging.Handling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsSample.MyDomain
{
    public class ExportarPessoaEventHandler : IEventHandler<PessoaCriadaEvent>
    {
        ILogger _logger;

        public ExportarPessoaEventHandler(ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            _logger = logger;
        }

        public void Handle(PessoaCriadaEvent @event)
        {
            _logger.Info(string.Format("Evento -> Pessoa {0} exportada com sucesso!", @event.Pessoa.Nome));
        }
    }
}
