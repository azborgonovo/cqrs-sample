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
    public class PessoaEventHandler : IEventHandler<PessoaCriadaEvent>, IEventHandler<ErroAoCriarPessoaEvent>
    {
        ILogger _logger;

        public PessoaEventHandler(ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            _logger = logger;
        }

        public void Handle(PessoaCriadaEvent @event)
        {
            _logger.Info(string.Format("Evento -> Pessoa {0} criada com sucesso!", @event.Pessoa.Nome));
        }

        public void Handle(ErroAoCriarPessoaEvent @event)
        {
            _logger.Info(string.Format("Evento -> Erro ao criar pessoa: {0}", @event.Mensagem));
        }
    }
}
