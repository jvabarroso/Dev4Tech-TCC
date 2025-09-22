using System;

namespace Dev4Tech
{
    // Na sua classe Sessao, adicione:
    public static class Sessao
    {
        public static empresaCadFuncionario FuncionarioLogado { get; set; }
        public static empresaCadAdmin AdminLogado { get; set; }
        public static int IdEquipeSelecionada { get; set; }
        public static string NomeEquipeSelecionada { get; set; }
        public static string CategoriaEquipeSelecionada { get; set; }

        // Método para limpar a equipe selecionada
        public static void LimparEquipeSelecionada()
        {
            IdEquipeSelecionada = 0;
            NomeEquipeSelecionada = null;
            CategoriaEquipeSelecionada = null;
        }

        // Método para definir a equipe selecionada
        public static void DefinirEquipeSelecionada(int id, string nome, string categoria)
        {
            IdEquipeSelecionada = id;
            NomeEquipeSelecionada = nome;
            CategoriaEquipeSelecionada = categoria;
        }
    }
}