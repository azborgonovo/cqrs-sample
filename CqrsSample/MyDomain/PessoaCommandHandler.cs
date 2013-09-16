using CqrsSample.Infrastructure.Logging;
using CqrsSample.Messaging;
using CqrsSample.Messaging.Handling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsSample.MyDomain
{
    public class PessoaCommandHandler : ICommandHandler<InserirPessoaCommand>
    {
        IEventBus _eventBus;
        ILogger _logger;
        RepositorioDePessoas _repositorio;

        public PessoaCommandHandler(IEventBus eventBus, ILogger logger, RepositorioDePessoas repositorio)
        {
            if (eventBus == null) throw new ArgumentNullException("eventBus");
            if (logger == null) throw new ArgumentNullException("logger");
            if (repositorio == null) throw new ArgumentNullException("repositorio");

            _eventBus = eventBus;
            _logger = logger;
            _repositorio = repositorio;
        }

        public void Handle(InserirPessoaCommand command)
        {
            _logger.Info(string.Format("Comando -> Inserindo a pessoa {0}...", command.Pessoa.Nome));
            
            try
            {
                // Utiliza o modelo
                _repositorio.Add(command.Pessoa);
                _eventBus.Publish(new PessoaCriadaEvent(command.Pessoa));
            }
            catch (Exception ex)
            {
                _eventBus.Publish(new ErroAoCriarPessoaEvent(command.Pessoa, ex));
            }
        }
    }
}
