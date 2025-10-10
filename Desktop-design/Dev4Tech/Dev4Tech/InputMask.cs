using System;
using System.Text.RegularExpressions;
using Guna.UI2.WinForms;

namespace Dev4Tech.Utils
{
    public static class InputMask
    {
        // --- MÁSCARA DE TELEFONE ---
        public static void MaskTelefone(Guna2TextBox textBox)
        {
            textBox.MaxLength = 15;
            textBox.TextChanged += (s, e) =>
            {
                int pos = textBox.SelectionStart;
                string oldText = textBox.Text;
                string input = OnlyDigits(oldText);

                if (input.Length > 11)
                    input = input.Substring(0, 11);

                string formatted;
                if (input.Length <= 10)
                {
                    // Formato: (00) 0000-0000
                    formatted = Regex.Replace(input, @"(\d{0,2})(\d{0,4})(\d{0,4})",
                        m => FormatTelefone(m));
                }
                else
                {
                    // Formato: (00) 00000-0000
                    formatted = Regex.Replace(input, @"(\d{0,2})(\d{0,5})(\d{0,4})",
                        m => FormatTelefone(m));
                }

                AtualizarTexto(textBox, formatted, pos);
            };
        }

        // --- MÁSCARA DE CPF ---
        public static void MaskCPF(Guna2TextBox textBox)
        {
            textBox.MaxLength = 14;
            textBox.TextChanged += (s, e) =>
            {
                int pos = textBox.SelectionStart;
                string input = OnlyDigits(textBox.Text);
                if (input.Length > 11)
                    input = input.Substring(0, 11);

                string formatted = Regex.Replace(input, @"(\d{3})(\d{3})(\d{3})(\d{0,2})", "$1.$2.$3-$4")
                                        .TrimEnd('-', '.');

                AtualizarTexto(textBox, formatted, pos);
            };
        }

        // --- MÁSCARA DE CNPJ ---
        public static void MaskCNPJ(Guna2TextBox textBox)
        {
            textBox.MaxLength = 18;
            textBox.TextChanged += (s, e) =>
            {
                int pos = textBox.SelectionStart;
                string input = OnlyDigits(textBox.Text);
                if (input.Length > 14)
                    input = input.Substring(0, 14);

                string formatted = Regex.Replace(input, @"(\d{2})(\d{3})(\d{3})(\d{4})(\d{0,2})", "$1.$2.$3/$4-$5")
                                        .TrimEnd('-', '/', '.');

                AtualizarTexto(textBox, formatted, pos);
            };
        }

        // --- MÁSCARA DE DATA DE NASCIMENTO (dd/mm/aaaa) ---
        public static void MaskData(Guna2TextBox textBox)
        {
            textBox.MaxLength = 10;
            textBox.TextChanged += (s, e) =>
            {
                int pos = textBox.SelectionStart;
                string input = OnlyDigits(textBox.Text);
                if (input.Length > 8)
                    input = input.Substring(0, 8);

                string formatted = Regex.Replace(input, @"(\d{2})(\d{0,2})(\d{0,4})", "$1/$2/$3")
                                        .TrimEnd('/');

                AtualizarTexto(textBox, formatted, pos);
            };
        }

        // --- FUNÇÕES AUXILIARES ---
        private static string OnlyDigits(string input)
        {
            return Regex.Replace(input ?? "", @"\D", "");
        }

        private static void AtualizarTexto(Guna2TextBox textBox, string novoTexto, int cursorPos)
        {
            textBox.TextChanged -= TextChangedTemp;
            textBox.Text = novoTexto;
            textBox.SelectionStart = Math.Min(cursorPos + 1, textBox.Text.Length);
            textBox.TextChanged += TextChangedTemp;
        }

        private static void TextChangedTemp(object sender, EventArgs e) { }

        private static string FormatTelefone(Match match)
        {
            string ddd = match.Groups[1].Value;
            string parte1 = match.Groups[2].Value;
            string parte2 = match.Groups[3].Value;

            string formatted = "";
            if (!string.IsNullOrEmpty(ddd))
                formatted = $"({ddd})";

            if (!string.IsNullOrEmpty(parte1))
                formatted += $" {parte1}";

            if (!string.IsNullOrEmpty(parte2))
                formatted += $"-{parte2}";

            return formatted;
        }
    }
}
