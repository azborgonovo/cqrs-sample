using CqrsSample.Implementation;
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
        public static ICollection<string> Log = new List<string>();
        static ICommandBus _commandBus;
        static IEventBus _eventBus;

        static void Init()
        {
            _eventBus = new EventBus();
            _commandBus = new CommandBus();

            var commandProcessor = new CommandProcessor();
            commandProcessor.Register(new PessoaCommandHandler(_eventBus, new RepositorioDePessoas()));
            commandProcessor.Start();

            var eventPublisher = new EventPublisher();
            eventPublisher.Subscribe(new PessoaEventHandler());
            eventPublisher.Subscribe(new ExportarPessoaEventHandler());
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
            foreach (var log in Log)
            {
                Console.WriteLine(log);
            }
            
            Console.ReadKey();
        }
    }
}
