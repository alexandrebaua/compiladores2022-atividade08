using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Compilador.Semantico.Tipos;
using Compilador.Semantico.Tipos.Auxiliar;
using Compilador.Sintatico.Ascendente.SLR;

namespace Compilador.Semantico
{
    /// <summary>
    /// Classe principal do analisador semântico.
    /// </summary>
    public class SemanticoClass
    {
        #region Variáveis Privadas
        private int index;
        private ListBox debug;
        #endregion

        /// <summary>
        /// Obtém ou Define a lista de tokens.
        /// </summary>
        public SemanticoToken[] ListaTokens { get; set; }

        /// <summary>
        /// Obtém ou define árvore de componentes do algoritmo.
        /// </summary>
        public Algoritmo Algoritmo { get; set; } = null;

        /// <summary>
        /// Executa a criação dos comandos (árvore de análise sintática) e a verificação semântica.
        /// </summary>
        /// <param name="debug">Saida de debugação.</param>
        /// <param name="treeView">Um componente do tipo árvore de visualização.</param>
        public void Processar(ListBox debug, TreeView treeView)
        {
            this.debug = debug;
            this.index = 0;
            
            this.debug.Items.Add("======>>> Geração de tipos iniciada <<<======");

            List<ITipo> item = this.GerarTipos();

            this.debug.Items.Add("======>>> Geração de tipos concluida <<<======");

            if (item.Count == 1 && item[0] is Algoritmo)
                this.Algoritmo = (Algoritmo)item.First();

            if (this.Algoritmo == null)
                throw new Exception("O algoritmo está vazio!");

            this.Algoritmo.GerarArvore(treeView);

            this.debug.Items.Add("++++++>>> Verificação de variáveis iniciada <<<++++++");

            this.Algoritmo.VerificarVariáveis(this.debug);

            this.debug.Items.Add("++++++>>> Verificação de variáveis concluida <<<++++++");
        }

        /// <summary>
        /// Gera uma lista de comandos, apartir dos elementos definidos na lista de tokens.
        /// </summary>
        /// <returns>A lista de comandos gerados.</returns>
        private List<ITipo> GerarTipos()
        {
            List<SemanticoToken> elementos = new List<SemanticoToken>();
            Dictionary<int, List<SemanticoToken>> trocaGrupo = new Dictionary<int, List<SemanticoToken>>();
            int escopo = this.ListaTokens[index].Escopo;
            int grupo = this.ListaTokens[index].Grupo;

            List<ITipo> listaTipos = new List<ITipo>();
            List<ITipo> listaTiposEscopoMaior = new List<ITipo>();

            // Enquanto não chegou ao último elemento da lista de tokens, repete:
            while (index < this.ListaTokens.Length)
            {
                // Se o escopo do token analisado é maior que o escopo que está sendo criado, então:
                if (this.ListaTokens[index].Escopo > escopo)
                {
                    this.debug.Items.Add($"----> Escopo: {this.ListaTokens[index].Escopo}");
                    // Gera os comandos dos escopos seguintes, e adiciona à lista de comandos de escopo maior:
                    listaTiposEscopoMaior.AddRange(this.GerarTipos());
                    this.debug.Items.Add($"<---- Escopo: {this.ListaTokens[index].Escopo}");
                    continue;
                }
                // Senão, se o escopo do token analisado é menor que o escopo que está sendo criado, então:
                else if (this.ListaTokens[index].Escopo < escopo)
                {
                    // Se for o elemento 'senão' do comando condicional 'se-senão', então adiciona à lista de comandos do escopo atual:
                    if (this.ListaTokens[index].Token.Equals("SENAO"))
                    {
                        listaTipos.Add(new Senão(this.ListaTokens[index]));
                        this.debug.Items.Add($">>> {listaTipos.Last()}");
                        index++;
                        continue;
                    }

                    break;
                }

                // Se o grupo do token analisado é o que está sendo criado, então:
                if (this.ListaTokens[index].Grupo == grupo)
                {
                    elementos.Add(this.ListaTokens[index]);

                    if (this.ListaTokens[index].ReduceType != ReduceTypeEnum.None)
                    {
                        if (this.ListaTokens[index].ReduceType == ReduceTypeEnum.SeleçãoIf)
                        {
                            ITipo senao = listaTiposEscopoMaior.Find(x => x is Senão);
                            if (senao != null)
                                elementos.Insert(elementos.Count - 2, ((Senão)senao).Token);
                        }

                        listaTipos.Add(Tipo.Criar(elementos, listaTiposEscopoMaior));
                        elementos = new List<SemanticoToken>();
                        listaTiposEscopoMaior = new List<ITipo>();
                        this.debug.Items.Add($">>> {listaTipos.Last()}");
                    }
                }
                else
                {
                    if (elementos.Count > 0)
                    {
                        if (trocaGrupo.ContainsKey(grupo))
                        {
                            elementos.InsertRange(0, trocaGrupo[grupo]);
                            trocaGrupo.Remove(grupo);
                        }

                        trocaGrupo.Add(grupo, elementos);
                    }

                    grupo = this.ListaTokens[index].Grupo;

                    if (trocaGrupo.ContainsKey(grupo))
                    {
                        elementos = trocaGrupo[grupo];
                    }
                    else
                    {
                        elementos = new List<SemanticoToken>();
                    }

                    continue;
                }

                index++;
            }

            return listaTipos;
        }
    }
}
