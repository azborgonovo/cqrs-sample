using CqrsSample.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsSample.MyDomain
{
    public class InserirPessoaCommand : ICommand
    {
        public Guid Id { get; set; }

        public Pessoa Pessoa { get; set; }
    }
}
