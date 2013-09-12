using CqrsSample.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsSample.MyDomain
{
    public class PessoaCriadaEvent : IEvent
    {
        public PessoaCriadaEvent(Pessoa pessoa)
        {
            if (pessoa == null)
                throw new ArgumentNullException("pessoa");
            
            this.Pessoa = pessoa;
        }

        public Pessoa Pessoa { get; set; }
    }
}
