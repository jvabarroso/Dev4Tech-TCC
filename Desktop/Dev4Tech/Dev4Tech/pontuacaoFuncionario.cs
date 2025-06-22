using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev4Tech
{
    class pontuacaoFuncionario : conexao
    {
        private int tarefasSimples;
        private int tarefasNormais;
        private int tarefasDificeis;

        private int tarefasRuins;

        private int atrasosJustificados;
        private int atrasosInjustificados;

        public void setTarefasSimples(int valor)
        {
            this.tarefasSimples = valor;
        }

        public int getTarefasSimples()
        {
            return this.tarefasSimples;
        }

        public void setTarefasNormais(int valor)
        {
            this.tarefasNormais = valor;
        }

        public int getTarefasNormais()
        {
            return this.tarefasNormais;
        }

        public void setTarefasDificeis(int valor)
        {
            this.tarefasDificeis = valor;
        }

        public int getTarefasDificeis()
        {
            return this.tarefasDificeis;
        }

        public void setTarefasRuins(int valor)
        {
            this.tarefasRuins = valor;
        }

        public int getTarefasRuins()
        {
            return this.tarefasRuins;
        }

        public void setAtrasosJustificados(int valor)
        {
            this.atrasosJustificados = valor;
        }

        public int getAtrasosJustificados()
        {
            return this.atrasosJustificados;
        }

        public void setAtrasosInjustificados(int valor)
        {
            this.atrasosInjustificados = valor;
        }

        public int getAtrasosInjustificados()
        {
            return this.atrasosInjustificados;
        }

        public int calcularPontuacao()
        {
            int pontos = 0;

            // Pontuação por tarefa entregue com qualidade e prazo
            pontos += tarefasSimples * 2;
            pontos += tarefasNormais * 4;
            pontos += tarefasDificeis * 6;

            // Penalizações
            pontos -= tarefasRuins * 3;
            pontos -= atrasosInjustificados * 5;

            // Regras de metas mensais
            int tarefasValidas = tarefasSimples + tarefasNormais + tarefasDificeis;
            bool houveAtraso = (atrasosJustificados + atrasosInjustificados) > 0;
            bool bloqueiaBonus = tarefasRuins >= 2;

            if (!bloqueiaBonus)
            {
                if (tarefasValidas >= 3 && tarefasValidas <= 4)
                    pontos += 15;

                if (tarefasValidas > 4)
                    pontos += Math.Min(tarefasValidas - 4, 10); // máximo de 10 extras

                if (!houveAtraso)
                    pontos += 15;
            }

            // Limite máximo de pontos por mês
            if (pontos > 150)
                pontos = 150;

            return pontos;
        }

        public string mostrarProgresso()
        {
            int pontos = calcularPontuacao();
            double porcentagem = (pontos / 150.0) * 100;
            return $"Você atingiu {porcentagem:F1}% da meta mensal.";
        }


    }
}
