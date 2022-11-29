using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Compilador.Exceptions;

namespace Compilador.Lexico
{
    /// <summary>
    /// Classe do analisador léxico.
    /// </summary>
    public class LexicoClass
    {
        #region Variáveis internas da classe
        private int contador;
        #endregion

        /// <summary>
        /// O construtor da classe.
        /// </summary>
        /// <param name="entrada">O texto de entrada a ser processado.</param>
        /// <param name="ignorarEspacos">Indica se deve ou não ignorar espaços em branco.</param>
        /// <param name="marcadorFinal">Informa símbolo para marcação do final da lista, 'null' = sem símbolo de marcação.</param>
        public LexicoClass(string entrada, string marcadorFinal = null)
        {
            // Limpa a lista de tokens e inicializa o contador:
            this.ListaTokens = null;
            this.contador = 0;

            // Efetua os testes básicos na entrada:
            if (String.IsNullOrWhiteSpace(entrada))
                throw new Exception("Sem dados de entrada!");

            // Cria um vetor com as linhas:
            string[] entradaLinhas = entrada.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.None);

            if (entradaLinhas.Length == 0)
                throw new Exception("Sem dados de entrada!");

            // Tenta criar uma nova lista de tokens
            this.ListaTokens = LexicoClass.ProcessaEntrada(entradaLinhas, marcadorFinal);
        }

        #region Propriedades da classe léxica

        /// <summary>
        /// Obtém ou Define a lista de tokens
        /// </summary>
        public TokenClass[] ListaTokens { get; set; }

        #endregion

        #region Métodos públicos

        /// <summary>
        /// Testa se existe tokens seguintes na lista.
        /// </summary>
        public bool AsNextToken()
        {
            return this.contador < this.ListaTokens.Length;
        }

        /// <summary>
        /// Obtém o token seguinte na lista (incrementa o contador).
        /// </summary>
        public TokenClass NextToken()
        {
            if (this.contador >= this.ListaTokens.Length)
                throw new Exception($"Indice de token requerido está fora dos limites da lista.{Environment.NewLine}Requisitado: {this.contador}, disponivel: {this.ListaTokens.Length - 1}.");

            return this.ListaTokens[this.contador++];
        }

        /// <summary>
        /// Espia o token seguinte na lista (NÃO incrementa o contador).
        /// </summary>
        public TokenClass PeekNextToken()
        {
            if (this.contador >= this.ListaTokens.Length)
                throw new Exception($"Indice de token requerido está fora dos limites da lista.{Environment.NewLine}Requisitado: {this.contador}, disponivel: {this.ListaTokens.Length - 1}.");

            return this.ListaTokens[this.contador];
        }

        /// <summary>
        /// Obtém um token da lista no índice especificado.
        /// </summary>
        /// <param name="index">O índice do token.</param>
        /// <returns>O token do índice solicitado.</returns>
        public TokenClass GetTokenAt(int index)
        {
            if (index >= this.ListaTokens.Length)
                throw new Exception($"Indice de token requerido está fora dos limites da lista.{Environment.NewLine}Requisitado: {index}, disponivel: {this.ListaTokens.Length - 1}.");

            this.contador = index;

            return this.ListaTokens[this.contador++];
        }

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Cria uma lista de tokens apartir de um vetor contendo um texto de entrada.
        /// </summary>
        /// <param name="entrada">Um vetor de linhas com o texto a ser processado.</param>
        /// <returns>Uma lista de tokens encontrados.</returns>
        private static TokenClass[] ProcessaEntrada(string[] entrada, string marcadorFinal)
        {
            int estado = 0;
            string lexema = String.Empty;
            List<TokenClass> listaTokens = new List<TokenClass>();
            int index = 0;  // Valor da posição do indice absoluto do token.
            List<LexicoError> listaErros = new List<LexicoError>();

            // Percorre as linhas:
            for (int i = 0; i < entrada.Length; i++)
            {
                // Converte a linha em um vetor de caracteres.
                char[] chrs = entrada[i].ToCharArray();

                // Percorre os caracteres da linha buscando por tokens:
                int j = 0;
                while (j < chrs.Length)
                {

                    if (TabelaEstados.AFD[estado] != null && TabelaEstados.AFD[estado].ContainsKey(chrs[j]))
                    {
                        estado = TabelaEstados.AFD[estado][chrs[j]];
                        lexema += chrs[j];
                        j++;
                        index++;
                    }
                    else
                    {
                        if (TabelaEstados.Finais.ContainsKey(estado))
                        {
                            if (TabelaEstados.Finais[estado] != null)
                                listaTokens.Add(new TokenClass(lexema, TabelaEstados.Finais[estado], i + 1, j, index));
                            estado = 0;
                            lexema = String.Empty;
                        }
                        else
                        {
                            //throw new Exception($"Erro léxico no lexema: '{lexema}{chrs[j]}'{Environment.NewLine}Linha: {i + 1}, Posição: {j}");
                            //listaErros.Add($"Erro léxico no lexema: '{lexema}{chrs[j]}', Linha: {i + 1}, Posição: {j + 1}");
                            listaErros.Add(new LexicoError($"{lexema}{chrs[j]}", i + 1, j + 1, index));
                            estado = 0;
                            lexema = String.Empty;
                            j++;
                            index++;
                        }
                    }
                }
                
                // Caso está processando um comentário de linhas múltiplas, e o último caracter da linha foi um asteristico, então:
                if (estado == 41)
                    estado = 40;  // Volta o estado para continuar a buscar o fechamento do comentário (asteristico barra: */).

                // Necessário para tokens não processados devido ao final de linha
                if (lexema.Length > 0 && estado != 40)
                {
                    if (TabelaEstados.Finais.ContainsKey(estado))
                    {
                        if (TabelaEstados.Finais[estado] != null)
                            listaTokens.Add(new TokenClass(lexema, TabelaEstados.Finais[estado], i + 1, j, index));
                        estado = 0;
                        lexema = String.Empty;
                    }
                    else
                    {
                        //throw new Exception($"Erro léxico no lexema: '{lexema}'{Environment.NewLine}Linha: {i + 1}, Posição: {j}");
                        //listaErros.Add($"Erro léxico no lexema: '{lexema}', Linha: {i + 1}, Posição: {j + 1}");
                        listaErros.Add(new LexicoError(lexema, i + 1, j + 1, index));
                        estado = 0;
                        lexema = String.Empty;
                    }
                }

                index++;
            }

            if (listaErros.Count > 0)
                //throw new LexicoListException(listaErros, "Erros léxicos encontrados.");
                throw new LexicoListException(listaErros);

            // Se acabou as linhas, e foi deixado um comentário de multiplas lilhas aberto, então, informa erro:
            if (estado == 40)
                throw new Exception("Comentário de múltiplas linhas não foi fechado!");

            // Se foi informado um símbolo para marcação do final da lista, então adiciona o símbolo:
            if (marcadorFinal != null)
                listaTokens.Add(new TokenClass(marcadorFinal, marcadorFinal));

            return listaTokens.ToArray();
        }

        #endregion
    }
}
