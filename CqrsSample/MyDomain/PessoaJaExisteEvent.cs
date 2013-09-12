using CqrsSample.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsSample.MyDomain
{
    public class ErroAoCriarPessoaEvent : IEvent
    {
        public ErroAoCriarPessoaEvent(Pessoa pessoa, Exception exception)
        {
            if (pessoa == null)
                throw new ArgumentNullException("pessoa");
            
            this.Pessoa = pessoa;

            if (exception != null)
                this.Mensagem = exception.Message;
        }

        public Pessoa Pessoa { get; set; }
        public string Mensagem { get; set; }
    }
}
