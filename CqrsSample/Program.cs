using CqrsSample.Implementation;
using CqrsSample.Infrastructure.Logging;
using CqrsSample.Messaging;
using CqrsSample.MyDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CqrsSample
{
    class Program
    {
        public static int DefaultDelay = 1000;
        static ILogger _logger;
        static ICommandBus _commandBus;
        static IEventBus _eventBus;

        static void Init()
        {
            _logger = new InMemoryLogger();
            _eventBus = new EventBus();
            _commandBus = new CommandBus();

            var commandProcessor = new CommandProcessor(_logger, DefaultDelay);
            commandProcessor.Register(new PessoaCommandHandler(_eventBus, _logger, new RepositorioDePessoas()));
            commandProcessor.Start();

            var eventPublisher = new EventPublisher(_logger, DefaultDelay);
            eventPublisher.Subscribe(new PessoaEventHandler(_logger));
            eventPublisher.Subscribe(new ExportarPessoaEventHandler(_logger));
            eventPublisher.Start();
        }

        static void Main(string[] args)
        {
            // Inicializa os seguintes componentes da aplicação:
            // - Processador de comandos e eventos
            // - Fila de comandos e eventos
            // - Registro dos handlers ("tratadores") de comandos e eventos
            Init();
            


            // Código "efetivo" da minha aplicação
            var pessoa = new Pessoa { Nome = "André" };
            
            var command = new InserirPessoaCommand { Pessoa = pessoa };
            // TODO: Validar o comando antes de enviar, só devem ser enviados comandos válidos para a aplicação
            _commandBus.Send(command);
            _commandBus.Send(command);


            
            // Aguarda o processo assíncrono ser executado
            Thread.Sleep(Program.DefaultDelay * 3);
            
            // Escreve os logs na tela
            foreach (var log in _logger.GetAllLogs())
            {
                Console.WriteLine(log);
            }
            
            Console.ReadKey();
        }
    }
}
