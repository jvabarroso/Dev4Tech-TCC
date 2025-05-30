using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev4Tech
{
    class FiltroEquipes
    {
        string nome, setor;

        public void setNome (string nome)
        {
            this.nome = nome;
        }
        public void setSetor (string setor)
        {
            this.setor = setor;
        }

        public string getNome()
        {
            return this.nome;
        }
        public string getSetor()
        {
            return this.setor;
        }

    }
}
