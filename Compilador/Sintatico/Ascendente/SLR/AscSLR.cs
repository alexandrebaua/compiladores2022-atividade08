using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Compilador.Semantico;
using Compilador.Exceptions;
using Compilador.Lexico;

namespace Compilador.Sintatico.Ascendente.SLR
{
    /// <summary>
    /// Analisador Sintático Ascendente SLR
    /// </summary>
    public static class AscSLR
    {
        private static ListBox debug = null;
        private static LexicoClass lexico = null;
        private static Stack<int> pilha = null;
        private static Stack<string> simbolos = null;

        /// <summary>
        /// Executa a verificação sintática utilizando os tokens passados pelo analisador léxico.
        /// </summary>
        /// <param name="lexico">O analisador léxico contendo os tokens a serem analisados.</param>
        /// <param name="resultado">O ListBox para exibir os resultados da verificação sintática.</param>
        public static void Verificar(LexicoClass lexico, ListBox resultado, SemanticoClass semantico)
        {
            if (lexico == null)
                throw new Exception("Analizador léxico não pode ser nulo!");

            if (lexico.ListaTokens.Length == 0)
                throw new Exception("Analizador léxico não possui tokens!");

            AscSLR.lexico = lexico;
            debug = resultado;

            // Inicia as pilhas de armazenamento de estados e de símbolos:
            pilha = new Stack<int>();
            simbolos = new Stack<string>();

            // Insere o estado inicial e símbolo inicial nas pilhas:
            pilha.Push(0);
            simbolos.Push("$");

            ImprimePilha();
            ImprimeSimbolos();

            int s = 0;      // Estado no topo da pilha
            string a = "";  // Simbolo atual na entrada
            int t = 0;      // Estado lido da tabela de ação

            ActionClass action;  // Variável auxiliar para armazenar a ação retornada pela tabela de ação.
            TokenClass aToken;   // Variável auxiliar para armazenar o token atual.

            Helper aux = new Helper(); // Auxiliar para controlar a identificação do escopo e grupo dos tokens.

            // Lê o primeiro token do analisador léxico.
            aToken = lexico.NextToken();
            a = aToken.Token;
            aux.OperaçãoEmpilhar(aToken);  // Identifica o primeiro token.

            while (true)
            {
                s = pilha.Peek();
                debug.Items.Add($"s: {s}");

                action = Tabelas.BuscaAcao(s, a);
                debug.Items.Add($"> Ação[{s}, {a}]: {action}");

                if (action == null)
                    throw new SintaticoException(aToken, $"Ação não encontrada! ACTION[{s}, {a}]");

                if (action.TipoAcao == ActionType.Shift)
                {
                    t = action.Estado;
                    pilha.Push(t);
                    simbolos.Push(a);
                    debug.Items.Add($"Empilha Pilha: {t}");
                    debug.Items.Add($"Empilha Simbolo: {a}");

                    // Lê o próximo token do analisador léxico.
                    aToken = lexico.NextToken();
                    a = aToken.Token;
                    debug.Items.Add($"Próximo Simbolo: {a}");

                    // Marca o escopo e o grupo do token atual.
                    aux.OperaçãoEmpilhar(aToken);
                }
                else if (action.TipoAcao == ActionType.Reduce)
                {
                    Desempilha(action);
                    t = pilha.Peek();
                    debug.Items.Add($"t: {t}");

                    if (Tabelas.TabelaGOTO[t] == null)
                        throw new SintaticoException(aToken, $"Desvio nulo! t: {t}");

                    int estado = Tabelas.BuscaGOTO(t, action.Reducao.To);
                    if (estado < 0)
                        throw new SintaticoException(aToken, $"Desvio não encontrado! GOTO[{t}, {action.Reducao.To}]");

                    debug.Items.Add($"> GOTO[{t}, {action.Reducao.To}]: {estado}");

                    pilha.Push(estado);
                    simbolos.Push(action.Reducao.To);
                    debug.Items.Add($"Empilha Pilha: {estado}");
                    debug.Items.Add($"Empilha Simbolo: {action.Reducao.To}");

                    // Marca o escopo e o grupo do token atual.
                    aux.OperaçãoDesvio(action.Reducao);
                }
                else if (action.TipoAcao == ActionType.Accept)
                {
                    break;
                }

                ImprimePilha();
                ImprimeSimbolos();
            }

            if (pilha.Count == 2)
                debug.Items.Add( "----> Linguagem aceita <----");
            else
                throw new SintaticoException(aToken);

            // Tansfere a lista de tokens processados para o analisador semântico.
            semantico.ListaTokens = aux.ListaTokens.ToArray();

            if (debug.Items.Count > 0)
                debug.SelectedIndex = debug.Items.Count - 1;
        }

        /// <summary>
        /// Desempilha o número de itens nas pilhas, relativo à redução contida na ação.
        /// </summary>
        /// <param name="action">A ação contendo a informação de redução.</param>
        private static void Desempilha(ActionClass action)
        {
            if (pilha.Count < action.Reducao.From.Length)
                throw new Exception($"A redução é maior que a quantidade de itens na pilha!{Environment.NewLine}Redução: {action.Reducao.From.Length} itens ({action.Reducao}){Environment.NewLine}Pilha: {pilha.Count} itens");

            for (int i = 0; i < action.Reducao.From.Length; i++)
                debug.Items.Add($"Desempilha Pilha: {pilha.Pop()}");

            for (int i = 0; i < action.Reducao.From.Length; i++)
                debug.Items.Add($"Desempilha Simbolo: {simbolos.Pop()}");
        }

        private static void ImprimePilha()
        {
            string p = String.Empty;
            foreach (var item in pilha)
                p = $"{item} {p}";

            debug.Items.Add($"Pilha: {p.Trim()}");
        }

        private static void ImprimeSimbolos()
        {
            string p = String.Empty;
            foreach (var item in simbolos)
                p = $"{item} {p}";

            debug.Items.Add($"Simbolos: {p.Trim()}");
        }

        #region Classe interna auxiliar

        /// <summary>
        /// Classe auxiliar usada durante a identificação do escopo e grupo em que um token pertence.
        /// </summary>
        private class Helper
        {
            /// <summary>
            /// Lista de tokens que podem anteceder uma constante negativa.
            /// </summary>
            private string[] _antConstNeg = new string[] { "ATRIBUICAO", "ABRE_PAR", "MENOS", "MAIS", "ASTERISTICO", "BARRA", "MOD", "DIV", "MAIOR", "MENOR", "MAIOR_IGUAL", "MENOR_IGUAL", "IGUAL", "DIFERENTE", "E", "OU" };

            public List<SemanticoToken> ListaTokens { get; set; } = new List<SemanticoToken>();

            /// <summary>
            /// Obtém ou define o escopo do token atual.
            /// </summary>
            public int Escopo { get; set; } = 0;

            /// <summary>
            /// Obtém ou define o grupo que o token atual pertence.
            /// </summary>
            public int Grupo { get; set; } = 0;

            /// <summary>
            /// Obtém ou define o token que token atual em análise.
            /// </summary>
            public SemanticoToken TokenAtual { get; set; } = null;

            /// <summary>
            /// Obtém ou define o token que antecede o tokem atual na análise.
            /// </summary>
            public SemanticoToken TokenAnterior { get; set; } = null;

            /// <summary>
            /// Pilha usada para armazenar os tokens que antecedem um escopo maior.
            /// </summary>
            public Stack<SemanticoToken> PilhaTrocaEscopo { get; set; } = new Stack<SemanticoToken>();

            private Queue<SemanticoToken> filaTrocaGrupo = new Queue<SemanticoToken>();
            private Stack<SemanticoToken> pilhaTrocaGrupo = new Stack<SemanticoToken>();

            private SemanticoToken tokenTrocaSeção = null;

            private List<SemanticoToken> lstDecVarGlobal = null;
            
            private byte estado = 0;

            private SemanticoToken _principalAguarda = null;

            private SemanticoToken _repitaAguarda = null;

            /// <summary>
            /// Converte os tokens léxicos em tokens semânticos, durante a operação empilhar do analisador semântico.
            /// </summary>
            /// <param name="aToken">O token léxico.</param>
            public void OperaçãoEmpilhar(TokenClass aToken)
            {
                // Converte o token léxico para um token semântico, e adiciona à lista de tokens semânticos:
                SemanticoToken sToken = new SemanticoToken(aToken);
                this.ListaTokens.Add(sToken);
                
                // Necessário atrasar a adição do token 'repita' na pilha do escopo, pois ele pode seguir reduções, e ele precisa ser empilhado após a redução.
                if (this._repitaAguarda != null)
                {
                    this.PilhaTrocaEscopo.Push(this._repitaAguarda);
                    this._repitaAguarda = null;
                }
                
                // Se estiver no escopo base, então:
                if (this.Escopo == 0)
                {
                    // Atualiza a referência do token o anterior.
                    this.TokenAnterior = this.TokenAtual;

                    // Atualiza o escopo e o grupo do token atual.
                    sToken.Escopo = this.Escopo;
                    sToken.Grupo = this.Grupo;

                    // Atualiza a referência do token atual.
                    this.TokenAtual = sToken;

                    // Se existirem 2 tokens (ALGORITMO "nome") na lista de tokens semânticos, então:
                    if (this.ListaTokens.Count == 2)
                    {
                        // Seta o escopo para o nível de escopo dos procedimentos, funções, função principal e variáveis globais.
                        this.Escopo = 1;
                    }
                    
                    return;
                }

                // Se estiver no escopo inicial, então:
                if (this.Escopo == 1)
                {
                    // Se as variáveis globais (ou locais da função principal) foram processadas, então:
                    if (this.estado > 0)
                    {
                        // Se for os tokens 'procedimento' ou 'função', então:
                        if (sToken.Token.Equals("PROCEDIMENTO") || sToken.Token.Equals("FUNCAO"))
                        {
                            // Seta o estado para um procedimento ou função.
                            this.estado = 2;
                        }
                        else if (sToken.Token.Equals("VARIAVEIS") || sToken.Token.Equals("INICIO"))  // Senão, se o token atual é os tokens 'variáveis' ou 'início', então:
                        {
                            // Se for um procedimento ou função, então muda para o escopo interno da função ou procedimento, e marca o fim da declaração do cabeçalho:
                            if (this.estado == 2)
                            {
                                this.Escopo = 2;
                                this.estado = 3;
                            }

                            // Incrementa o grupo, e limpa a memória de token das trocas de seção:
                            this.Grupo++;
                            this.tokenTrocaSeção = null;
                        }
                    }
                    else
                    {
                        // Se a lista de variáveis globais (ou locais da função principal) está vazia, então:
                        if (this.lstDecVarGlobal == null)
                        {
                            // Se o token atual é a palavra reservada 'variáveis', então:
                            if (sToken.Token.Equals("VARIAVEIS"))
                            {
                                // Pode ser as variáveis globais se seguido por um procedimento ou função, ou pode ser as variáveis locais da função principal se for seguido por 'início'.
                                // Inicializa a lista de variáveis globais, adiciona o token atual, e incrementa o grupo:
                                this.lstDecVarGlobal = new List<SemanticoToken>();
                                this.lstDecVarGlobal.Add(sToken);
                                this.Grupo++;
                            }
                            else if (sToken.Token.Equals("INICIO"))  // Senão, se o token atual é a palavra reservada 'início' (função principal), então:
                            {
                                // Não foi declarado variáveis globais, e foi encontrado o início da função principal.
                                // Seta o escopo para a função principal, incrementa o grupo, e informa que as variáveis globais foram processadas (sem variáveis declaradas):
                                // Incrementa o grupo, e informa que as variáveis globais foram processadas (sem variáveis declaradas), sendo seguido pela função principal:
                                this.Grupo++;
                                this.estado = 1;
                            }
                            else if (sToken.Token.Equals("PROCEDIMENTO") || sToken.Token.Equals("FUNCAO")) // Senão, se for os tokens 'procedimento' ou 'função', então:
                            {
                                // Não foi declarado variáveis globais, e foi encontrado a declaração de um procedimento ou função.
                                // Incrementa o grupo, e informa que as variáveis globais foram processadas (sem variáveis declaradas):
                                // Informa que as variáveis globais foram processadas (sem variáveis declaradas), sendo seguido por um procedimento ou função:
                                this.Grupo++;
                                this.estado = 2;

                                // Se o token anterior (ainda não foi substituido pelo atual passado) não for um ponto vírgula, então, armazena o token de troca de escopo (a declaração de procedimentos e funções ficam no escopo 0):
                                if (!this.TokenAtual.Token.Equals("PONTO_VIRGULA"))
                                    this.PilhaTrocaEscopo.Push(this.TokenAtual);
                            }
                        }
                        else // Senão, se a lista de variáveis globais (ou locais da função principal) foi iniciada, então:
                        {
                            if (sToken.Token.Equals("INICIO"))  // Senão, se o token atual é a palavra reservada 'início' (função principal), então:
                            {
                                // Foi declarado variáveis globais, mas foi encontrado o início da função principal, então não são globais, são locais da função principal.
                                // Marca as variáveis encontradas, com o escopo da função principal:
                                foreach (var item in this.lstDecVarGlobal)
                                    if (!item.Token.Equals("VARIAVEIS"))
                                        item.Escopo = 2;

                                // Exclui a lista de variáveis globais, e informa que as variáveis globais foram processadas (variáveis declaradas eram locais da função principal), sendo seguido pela função primcipal:
                                this.lstDecVarGlobal = null;
                                this.estado = 1;

                                // Muda para o escopo da função principal.
                                this.Escopo = 2;
                                
                                this.PilhaTrocaEscopo.Push(this.tokenTrocaSeção);
                            }
                            else if (sToken.Token.Equals("PROCEDIMENTO") || sToken.Token.Equals("FUNCAO")) // Senão, se for os tokens 'procedimento' ou 'função', então:
                            {
                                // Foi declarado variáveis globais, e foi encontrado o início de um procedimento ou função.
                                this.estado = 2;

                                // Se o token anterior (ainda não foi substituido pelo atual passado) não for um ponto vírgula, então, armazena o token de troca de escopo (a declaração de procedimentos e funções ficam no escopo 1):
                                if (!this.TokenAtual.Token.Equals("PONTO_VIRGULA"))
                                    this.PilhaTrocaEscopo.Push(this.TokenAtual);
                            }
                            else   // Senão, é algum outro token:
                            {
                                // Adiciona o token atual à lista das variáveis globais.
                                this.lstDecVarGlobal.Add(sToken);
                            }
                        }
                    }
                }

                if (sToken.Token.Equals("FIM_ENQUANTO") || sToken.Token.Equals("FIM_FACA") || sToken.Token.Equals("FIM_PARA") || sToken.Token.Equals("FIM_SE") || sToken.Token.Equals("SENAO"))
                {
                    // Retorna o escopo.
                    this.Escopo--;

                    //this.pilhaTrocaGrupo.Push(sToken);
                    if (sToken.Token.Equals("SENAO"))
                        this.pilhaTrocaGrupo.Push(sToken);
                    else
                        this.filaTrocaGrupo.Enqueue(sToken);
                }
                else if (sToken.Token.Equals("ATE"))
                {
                    if (this.TokenAtual.Token.Equals("PONTO_VIRGULA"))
                        this.Escopo--;
                }
                else if (sToken.Token.Equals("FIM"))   // Senão, se o token atual é a palavra reservada 'fim', então:
                {
                    // Retorna o escopo.
                    this.Escopo--;

                    // Marca para a possibilidade de encontrar a função principal.
                    this.estado = 1;
                }
                else if (sToken.Token.Equals("PONTO"))   // Senão, se o token atual é o símbolo reservado 'ponto', então:
                {
                    // Retorna o escopo base.
                    this.Escopo = 0;
                }

                // ------ Tratamento para números negativos ------
                // Se o token anterior for o sinal menos e o token atual for uma constante, então:
                if (this.TokenAtual.Token.Equals("MENOS") && (sToken.Token.Equals("CONST_INT") || sToken.Token.Equals("CONST_REAL")))
                {
                    // Se o token anterior ao sinal menos for um token que antecede uma constante negativa, então:
                    if (this._antConstNeg.Contains(this.TokenAnterior.Token))
                    {
                        sToken.Unir(this.TokenAtual);             // Faz a união do sinal negativo com a constante.
                        this.ListaTokens.Remove(this.TokenAtual); // Remove o sinal negativo da lista de tokens.
                    }
                }

                // Atualiza a referência do token o anterior.
                this.TokenAnterior = this.TokenAtual;

                // Atualiza o escopo e o grupo do token atual.
                sToken.Escopo = this.Escopo;
                sToken.Grupo = this.Grupo;
                
                // Atualiza a referência do token atual.
                this.TokenAtual = sToken;

                // Se o token atual é a palavra reservada 'variáveis', então:
                if (sToken.Token.Equals("VARIAVEIS"))
                {
                    // Se estiver no escopo principal e não for um procedimento ou função, então foi encontrado função principal, muda para o escopo interno da função principal:
                    if (this.Escopo == 1 && this.estado == 1)
                    {
                        this.Escopo = 2;
                        //this.PilhaTrocaEscopo.Push(this.TokenAtual);
                        this._principalAguarda = sToken;
                    }

                    // Incrementa o grupo, e atualiza a memória de token das trocas de seção:
                    this.Grupo++;
                    this.tokenTrocaSeção = sToken;

                    // Se o token anterior não for um ponto vírgula, então, armazena o token de troca de escopo:
                    if (!this.TokenAnterior.Token.Equals("PONTO_VIRGULA"))
                    {
                        this.PilhaTrocaEscopo.Push(this.TokenAnterior);
                        //this.pilhaTrocaGrupo.Push(this.TokenAnterior);
                    }
                }
                else if (sToken.Token.Equals("INICIO"))   // Senão, se o token atual é a palavra reservada 'início', então:
                {
                    // Se a memória de token das trocas de seção está vazio, então:
                    if (this.tokenTrocaSeção == null)
                    {
                        // Incrementa o grupo, e atualiza a memória de token das trocas de seção:
                        this.Grupo++;
                        this.tokenTrocaSeção = sToken;

                        // Se o token anterior não for um ponto vírgula, então, armazena o token de troca de escopo:
                        if (!this.TokenAnterior.Token.Equals("PONTO_VIRGULA"))
                        {
                            this.PilhaTrocaEscopo.Push(this.TokenAnterior);
                            //this.pilhaTrocaGrupo.Push(this.TokenAnterior);
                        }
                    }
                    else  // Senão, foi memorizado uma troca de seção quando declarado as variáveis:
                    {
                        // Atualiza a informação do grupo do token atual com o valor memorizada, e memoriza o token atual como sendo a troca de seção:
                        sToken.Grupo = this.tokenTrocaSeção.Grupo;
                        this.tokenTrocaSeção = sToken;
                    }

                    // Se estiver no escopo principal, então foi encontrado função principal, então, muda para o escopo interno da função principal:
                    if (this.Escopo == 1)
                    {
                        this.Escopo = 2;
                        // Armazena para inserção na pilha com atraso, pois se existir preocedimentos ou funções antes da função inicial, irá ocorrer troca dos grupos.
                        this._principalAguarda = sToken;
                    }

                }
                else if (sToken.Token.Equals("FIM"))   // Senão, se o token atual é a palavra reservada 'fim', então:
                {
                    // Se a memória de token das trocas de seção não está vazio, então:
                    if (this.tokenTrocaSeção != null)
                    {
                        // Atualiza a informação do grupo do token atual com o valor memorizada, e limpa a memória de token das trocas de seção:
                        sToken.Escopo = this.tokenTrocaSeção.Escopo;
                        sToken.Grupo = this.tokenTrocaSeção.Grupo;
                        this.tokenTrocaSeção = null;
                    }
                }
                else if(sToken.Token.Equals("PONTO_VIRGULA"))   // Senão, se o token atual é um 'ponto vírgula', então:
                {
                    // Se não estiver durante a declaração do cabeçalho de um procedimento ou função, então incrementa o grupo:
                    if (this.estado != 2)
                        this.Grupo++;
                }
                else if (sToken.Token.Equals("ENTAO") || sToken.Token.Equals("FACA") || sToken.Token.Equals("REPITA") || sToken.Token.Equals("SENAO"))
                {
                    // Incrementa o grupo e o escopo.
                    this.Grupo++;
                    this.Escopo++;

                    if (sToken.Token.Equals("REPITA"))
                    {
                        this._repitaAguarda = sToken;
                        return;
                    }

                    // Armazena o token de troca de escopo:
                    if (!sToken.Token.Equals("SENAO"))
                        this.PilhaTrocaEscopo.Push(sToken);
                }
            }

            /// <summary>
            /// Marca o tipo de redução do tokens semântico atual, durante a operação de desvio do analisador semântico.
            /// </summary>
            /// <param name="redução">Tipo de redução da expressão semântica.</param>
            public void OperaçãoDesvio(ReducaoClass redução)
            {
                if (redução.ReduceType == ReduceTypeEnum.None)
                    return;

                if (redução.ReduceType >= ReduceTypeEnum.Declaração)
                {
                    if (this.TokenAnterior != null)
                        this.TokenAnterior.ReduceType = redução.ReduceType;

                    return;
                }

                if (this.PilhaTrocaEscopo.Count > 0)
                {
                    if (this.TokenAnterior.Token.Equals("PONTO_VIRGULA") || this.TokenAnterior.Token.Equals("PONTO"))
                    {
                        SemanticoToken itemBloco = this.PilhaTrocaEscopo.Pop();

                        if (redução.ReduceType == ReduceTypeEnum.Repita)
                        {
                            IEnumerable<SemanticoToken> elements = this.ListaTokens.Where(x => x.Grupo.Equals(this.TokenAnterior.Grupo));
                            foreach (var elem in elements)
                            {
                                elem.Escopo = itemBloco.Escopo;
                                elem.Grupo = itemBloco.Grupo;
                            }
                        }

                        this.TokenAnterior.Escopo = itemBloco.Escopo;
                        this.TokenAnterior.Grupo = itemBloco.Grupo;

                        if (this.filaTrocaGrupo.Count > 0)
                        {
                            switch (redução.ReduceType)
                            {
                                case ReduceTypeEnum.SeleçãoIf:
                                    SemanticoToken itemGrupo = this.filaTrocaGrupo.Dequeue();
                                    itemGrupo.Grupo = itemBloco.Grupo;
                                    if (this.pilhaTrocaGrupo.Count > 0 && this.pilhaTrocaGrupo.Peek().Escopo == itemBloco.Escopo)
                                    {
                                        itemGrupo = this.pilhaTrocaGrupo.Pop();
                                        itemGrupo.Grupo = itemBloco.Grupo;
                                    }
                                    break;

                                case ReduceTypeEnum.Enquanto:
                                case ReduceTypeEnum.ParaFaça:
                                    itemGrupo = this.filaTrocaGrupo.Dequeue();
                                    itemGrupo.Grupo = itemBloco.Grupo;
                                    break;
                            }
                        }
                    }
                    else if (redução.ReduceType == ReduceTypeEnum.Principal)
                    {
                        if (this._principalAguarda != null)
                        {
                            this.TokenAnterior.Escopo = this._principalAguarda.Escopo;
                            this.TokenAnterior.Grupo = this._principalAguarda.Grupo;
                            this._principalAguarda = null;
                        }
                        else
                        {
                            SemanticoToken itemBloco = this.PilhaTrocaEscopo.Pop();

                            this.TokenAnterior.Escopo = itemBloco.Escopo;
                            this.TokenAnterior.Grupo = itemBloco.Grupo;
                        }
                    }
                }
                
                if (this.TokenAnterior != null)
                    this.TokenAnterior.ReduceType = redução.ReduceType;
            }
        }

        #endregion
    }
}
