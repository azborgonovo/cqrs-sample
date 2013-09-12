using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsSample.MyDomain
{
    public class RepositorioDePessoas
    {
        ICollection<Pessoa> _pessoas = new List<Pessoa>();

        public void Add(Pessoa pessoa)
        {
            if (_pessoas.Any(p => p.Nome == pessoa.Nome))
                throw new InvalidOperationException(string.Format("Uma pessoa com o nome {0} já existe no repositório.", pessoa.Nome));
            
            _pessoas.Add(pessoa);
        }
    }
}
