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
        RepositorioDePessoas _repositorio;

        public PessoaCommandHandler(IEventBus eventBus, RepositorioDePessoas repositorio)
        {
            _eventBus = eventBus;
            _repositorio = repositorio;
        }

        public void Handle(InserirPessoaCommand command)
        {
            Program.Log.Add(string.Format("Comando -> Inserindo a pessoa {0}...", command.Pessoa.Nome));

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
