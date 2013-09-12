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
        public void Handle(PessoaCriadaEvent @event)
        {
            Program.Log.Add(string.Format("Evento -> Pessoa {0} exportada com sucesso!", @event.Pessoa.Nome));
        }
    }
}
