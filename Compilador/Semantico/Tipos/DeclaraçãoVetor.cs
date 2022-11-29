using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Compilador.GeradorCódigo.MIPS;
using Compilador.GeradorCódigo.MIPS.MipsData;
using Compilador.Semantico.Tipos.Auxiliar;

namespace Compilador.Semantico.Tipos
{
    /// <summary>
    /// Classe para o comando do tipo 'Declaração de Vetor'.
    /// </summary>
    public class DeclaraçãoVetor : Tipo
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="elementos">Elementos semânticos que compõem o comando.</param>
        public DeclaraçãoVetor(List<SemanticoToken> elementos)
        {
            if (elementos == null)
                throw new ArgumentNullException(nameof(elementos));
            if (elementos.Count < 9)
                throw new Exception("Um tipo 'declaração vetor' deve receber no mínimo 9 elementos!");

            this.Elementos = elementos.ToArray();
            
            IEnumerable<SemanticoToken> faixas = this.Elementos.Where(x => x.Token.Equals("CONST_FAIXA_VETOR"));
            int count = 1;
            this.FaixasVetor = new FaixaVetor[faixas.Count()];
            for (int i = 0; i < this.FaixasVetor.Length; i++)
            {
                this.FaixasVetor[i] = new FaixaVetor(faixas.ElementAt(i));
                count *= this.FaixasVetor[i].Tamanho;
            }

            this.ContagemRegistros = count;
            this.TipoDeDados = this.Elementos[this.Elementos.Length - 2];
            this.Identificador = this.Elementos[0];
        }

        #region Propriedades

        /// <summary>
        /// Obtém os elementos semânticos que compõem o comando.
        /// </summary>
        public SemanticoToken[] Elementos { get; }

        /// <summary>
        /// Obtém o token do tipo de dados da declaração do vetor.
        /// </summary>
        public SemanticoToken TipoDeDados { get; }

        /// <summary>
        /// Obtém o token do identificador (nome) da declaração do vetor.
        /// </summary>
        public SemanticoToken Identificador { get; }

        /// <summary>
        /// Obtém as faixas do vetor.
        /// </summary>
        public FaixaVetor[] FaixasVetor { get; }

        /// <summary>
        /// Obtém a contagem total de elementos que compõe os registros do vetor.
        /// </summary>
        public int ContagemRegistros { get; }

        #endregion

        /// <summary>
        /// Obtém uma variável referênte à esta declaração.
        /// </summary>
        public Variável ObterVariável()
        {
            return new Variável(this);
        }

        #region Validação Semântica dos índices do Vetor

        /// <summary>
        /// Executa a validação semântica dos argumentos de indice do vetor.
        /// </summary>
        /// <param name="arg">Argumento do comando leia.</param>
        /// <param name="listaVariáveisGlobal">A lista de variáveis de escopo global.</param>
        /// <param name="listaVariáveisLocal">A lista de variáveis de escopo local.</param>
        /// <param name="listaErrors">A lista de erros encontrados durante a análise.</param>
        /// <param name="debug">Saida de debugação.</param>
        public void ValidarIndice(Leia.Argumento arg, List<Variável> listaVariáveisGlobal, List<Variável> listaVariáveisLocal, List<SemanticoError> listaErrors, ListBox debug)
        {
            int index = 0;
            this.ValidarIndice(ref index, arg.Elementos, listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
        }

        /// <summary>
        /// Executa a validação semântica dos argumentos de indice do vetor.
        /// </summary>
        /// <param name="lista">Lista de elementos da expressão.</param>
        /// <param name="listaVariáveisGlobal">A lista de variáveis de escopo global.</param>
        /// <param name="listaVariáveisLocal">A lista de variáveis de escopo local.</param>
        /// <param name="listaErrors">A lista de erros encontrados durante a análise.</param>
        /// <param name="debug">Saida de debugação.</param>
        public void ValidarIndice(SemanticoToken[] lista, List<Variável> listaVariáveisGlobal, List<Variável> listaVariáveisLocal, List<SemanticoError> listaErrors, ListBox debug)
        {
            int index = 0;
            this.ValidarIndice(ref index, lista, listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
        }

        /// <summary>
        /// Executa a validação semântica dos argumentos de indice do vetor.
        /// </summary>
        /// <param name="index">Indice do elemento atual analisado na expressão.</param>
        /// <param name="lista">Lista de elementos da expressão.</param>
        /// <param name="listaVariáveisGlobal">A lista de variáveis de escopo global.</param>
        /// <param name="listaVariáveisLocal">A lista de variáveis de escopo local.</param>
        /// <param name="listaErrors">A lista de erros encontrados durante a análise.</param>
        /// <param name="debug">Saida de debugação.</param>
        public void ValidarIndice(ref int index, SemanticoToken[] lista, List<Variável> listaVariáveisGlobal, List<Variável> listaVariáveisLocal, List<SemanticoError> listaErrors, ListBox debug)
        {
            SemanticoToken identificadorToken = lista[index];

            if (!lista[index++].Lexema.Equals(this.Identificador.Lexema))
                throw new Exception("O identificador passado não corresponde ao identificador da declaração.");

            if (!lista[index++].Token.Equals("ABRE_COL"))
                throw new Exception("Esperado abre colchetes para o início dos argumentos de indíce.");

            debug.Items.Add($">> {this.Identificador.Lexema}[?]");

            int contaArgumentos = 1;
            while (index < lista.Length)
            {
                SemanticoToken itemExpr = lista[index];

                if (itemExpr.Token.Equals("ID"))
                {
                    // Verifica se a variável na operação foi declarada localmente:
                    Variável elementsExpr = listaVariáveisLocal.Find(x => x.Identificador.Equals(itemExpr.Lexema));
                    if (elementsExpr == null)  // Não encontrada no escopo local, então:
                    {
                        // Busca na lista de escopo global:
                        elementsExpr = listaVariáveisGlobal.Find(x => x.Identificador.Equals(itemExpr.Lexema));
                        if (elementsExpr == null)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, "A variável foi usada mas não está declarada!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            index++;
                            continue;
                        }
                    }

                    itemExpr.Variável = elementsExpr;
                    
                    if (!elementsExpr.ValorAtribuido)
                    {
                        listaErrors.Add(new SemanticoError(itemExpr, "A variável foi usada mas não possui valor atribuido!"));
                        debug.Items.Add($"X > {itemExpr.ToString()}");
                        index++;
                        continue;
                    }

                    if (elementsExpr.Tipo == VariávelTipo.Real)
                    {
                        listaErrors.Add(new SemanticoError(itemExpr, "O argumento de indíce do vetor não pode receber uma variável do tipo real!"));
                        debug.Items.Add($"X > {itemExpr.ToString()}");
                        index++;
                        continue;
                    }

                    if (elementsExpr.Tipo == VariávelTipo.Caracter)
                    {
                        listaErrors.Add(new SemanticoError(itemExpr, "O argumento de indíce do vetor não pode receber uma variável do tipo caracter!"));
                        debug.Items.Add($"X > {itemExpr.ToString()}");
                        index++;
                        continue;
                    }

                    debug.Items.Add($">> {elementsExpr.ToString()}");
                }
                else if (itemExpr.Token.Equals("CONST_LOGICA"))
                {
                    throw new Exception($"O argumento de indíce do vetor não pode receber uma constante '{itemExpr.Lexema}' do tipo lógico!");
                }
                else if (itemExpr.Token.Equals("CONST_TEXTO"))
                {
                    throw new Exception($"O argumento de indíce do vetor não pode receber uma constante '{itemExpr.Lexema}' do tipo caracter!");
                }
                else if (itemExpr.Token.Equals("CONST_INT"))
                {
                    debug.Items.Add($">> {itemExpr.Lexema} : {itemExpr.Token}");
                }
                else if (itemExpr.Token.Equals("CONST_REAL"))
                {
                    throw new Exception($"O argumento de indíce do vetor não pode receber uma constante '{itemExpr.Lexema}' do tipo real!");
                }
                else if (itemExpr.Token.Equals("VIRGULA"))
                {
                    contaArgumentos++;
                }
                else if(itemExpr.Token.Equals("FECHA_COL"))
                {
                    break;
                }

                index++;
            }

            if (contaArgumentos != this.FaixasVetor.Length)
            {
                int comprimento = lista[index].Index + lista[index].Lexema.Length - identificadorToken.Index;
                listaErrors.Add(new SemanticoError(this.Identificador.Lexema, identificadorToken.Linha, identificadorToken.Inicio, identificadorToken.Index, comprimento, $"Foram passados {contaArgumentos} argumentos, mas o vetor possui {this.FaixasVetor.Length} argumentos."));
                debug.Items.Add($"X >> {this.Identificador.Lexema}[Erro...]");
                return;
            }

            string tmp = "ok";
            for (int i = 1; i < contaArgumentos; i++)
                tmp += ", ok";
            debug.Items.Add($">> {this.Identificador.Lexema}[{tmp}]");
        }

        #endregion

        #region Geração de Código MIPS Assembly

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para este componente.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        public override void GerarMips(MipsClass mips)
        {
            if (!this.Identificador.Variável.Utilizado)
                return;

            switch (this.Identificador.Variável.Tipo)
            {
                case VariávelTipo.Lógico:
                    mips.HandlerLogico.AdicionarVetor(this.Identificador.ObterEtiqueta(), this.ContagemRegistros);
                    break;

                case VariávelTipo.Inteiro:
                    mips.SectionData.Adicionar(new MipsDataWord(this.Identificador.ObterEtiqueta(), this.ContagemRegistros));
                    break;

                case VariávelTipo.Real:
                    mips.SectionData.Adicionar(new MipsDataFloat(this.Identificador.ObterEtiqueta(), this.ContagemRegistros));
                    break;

                case VariávelTipo.Caracter:
                    mips.SectionData.Adicionar(new MipsDataSpace(this.Identificador.ObterEtiqueta(), this.ContagemRegistros * 34));

                    // Informa utilização de opcionais:
                    mips.HandlerOpcionais.VariáveisTextoUtilizado = true;
                    break;
            }

            // Informa utilização de opcionais:
            mips.HandlerOpcionais.ArraysUtilizados = true;
        }

        /// <summary>
        /// Posiciona o registro do vetor, conforme indice informado da lista.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        /// <param name="lista">Lista de elementos da chamada do vetor.</param>
        public void GerarMipsValorIndex(MipsClass mips, SemanticoToken[] lista)
        {
            int i = 0;
            this.GerarMipsValorIndex(mips, lista, ref i);
        }

        /// <summary>
        /// Posiciona o registro do vetor, conforme indice informado da lista, com base no indice e offset informados para a lista.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        /// <param name="lista">Lista de elementos da expressão.</param>
        /// <param name="i">Indice do elemento atual analisado na expressão.</param>
        /// <param name="offset">Offset de indice do elemento atual analisado na expressão.</param>
        public void GerarMipsValorIndex(MipsClass mips, SemanticoToken[] lista, ref int i, int offset = 0)
        {
            List<SemanticoToken> vetExpr = new List<SemanticoToken>();
            i += 2;
            for (; i < lista.Length; i++)
            {
                if (lista[i + offset].Token.Equals("FECHA_COL"))
                    break;

                vetExpr.Add(lista[i + offset]);
            }

            this.GerarMipsValorIndex(mips, vetExpr);
        }

        /// <summary>
        /// Posiciona o registro do vetor, conforme expressão de argumentos do indice.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        /// <param name="exprIndice">Expressão com os argumentos de indice do vetor.</param>
        public void GerarMipsValorIndex(MipsClass mips, List<SemanticoToken> exprIndice)
        {
            // Limpa o registrador $t6 que será utilizado para armazenar a posição calculada do elemento no array
            mips.SectionText.Adicionar(new MipsText("add", $"$t6,$zero,$zero"));
            
            // Inicia com o primeiro elemento no registrador do indice na dimensão que está sendo calculada:
            this.GerarMipsMoverValores(mips, exprIndice[0], "$t7");
            
            // Agora adiciona os demais elementos:
            int i = 1;
            int vetFaixa = 0, vetK = 1;
            while (i < exprIndice.Count)
            {
                if (exprIndice[i].Token.Equals("VIRGULA"))
                {
                    this.GerarMipsAdicionarTesteFaixa(mips, vetFaixa, vetK);

                    // Calcula o novo valor para o fator de correção dos indices.
                    vetK *= this.FaixasVetor[vetFaixa].Tamanho;

                    vetFaixa++; // Avança para o próximo indice de faixas.
                    i++;        // Avança para o próximo indice de argumento.

                    // Inicia o registrador do indice com a nova dimensão que está sendo calculada:
                    this.GerarMipsMoverValores(mips, exprIndice[i++], "$t7");
                    continue;
                }

                if (exprIndice[i].Token.Equals("ASTERISTICO") || exprIndice[i].Token.Equals("BARRA"))
                {
                    this.GerarMipsMoverValores(mips, exprIndice[i + 1], "$t8");
                    this.GerarMipsAdicionarOperação(mips, exprIndice[i], "$t7", "$t8");
                    i += 2;
                    continue;
                }

                this.GerarMipsMoverValores(mips, exprIndice[i + 1], "$t8");

                if (i < exprIndice.Count - 3)
                {
                    if (exprIndice[i + 2].Token.Equals("ASTERISTICO") || exprIndice[i + 2].Token.Equals("BARRA"))
                    {
                        this.GerarMipsMoverValores(mips, exprIndice[i + 3], "$t9");
                        this.GerarMipsAdicionarOperação(mips, exprIndice[i + 2], "$t8", "$t9");
                        this.GerarMipsAdicionarOperação(mips, exprIndice[i], "$t7", "$t8");
                        i += 4;
                        continue;
                    }
                }

                this.GerarMipsAdicionarOperação(mips, exprIndice[i], "$t7", "$t8");
                i += 2;
            }

            this.GerarMipsAdicionarTesteFaixa(mips, vetFaixa, vetK);

            if (this.Identificador.Variável.Tipo == VariávelTipo.Inteiro || this.Identificador.Variável.Tipo == VariávelTipo.Real)
            {
                mips.SectionText.Adicionar(new MipsText("mul", "$t6,$t6,4"));  // Utiliza múltiplos de quatro para acessar cada registro do array
            }
            if (this.Identificador.Variável.Tipo == VariávelTipo.Caracter)
            {
                mips.SectionText.Adicionar(new MipsText("mul", "$t6,$t6,34"));  // Utiliza múltiplos de 34 para acessar cada registro do array
            }
            
            this.GerarMipsSetaRegistro(mips);
        }

        /// <summary>
        /// Move o valor da variável ou constante, para o registrador informado.
        /// Método utilizado para calcular a posição do registro no vetor.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        /// <param name="elemSem">Token contendo o valor.</param>
        /// <param name="regInteiro">Registrador a receber o valor.</param>
        private void GerarMipsMoverValores(MipsClass mips, SemanticoToken elemSem, string regInteiro)
        {
            if (elemSem.Token.Equals("CONST_INT"))
            {
                mips.SectionText.Adicionar(new MipsText("li", $"{regInteiro},{elemSem.Lexema}"));  // Carrega o valor da constante no registrador.
                return;
            }
            else if (elemSem.Token.Equals("ID"))
            {
                if (elemSem.Variável.Global)
                    mips.SectionText.Adicionar(new MipsText("lw", $"{regInteiro},varInt_{elemSem.Lexema}"));  // Carrega o valor da variável no registrador
                else
                    mips.SectionText.Adicionar(new MipsText("lw", $"{regInteiro},varInt_{elemSem.Variável.Token.Index}_{elemSem.Lexema}"));  // Carrega o valor da variável no registrador
            }
            else
            {
                throw new NotImplementedException($"Não é suportada a geração de código do tipo '{elemSem.Variável.Tipo}' em argumentos de vetor.");
            }
        }

        /// <summary>
        /// Adiciona uma operação aritmetica entre os registradores informados..
        /// Método utilizado para calcular a posição do registro no vetor.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        /// <param name="elemSem">Token contendo o operador.</param>
        /// <param name="regInteiro1">Registrador da esquerda.</param>
        /// <param name="regInteiro2">Registrador da direita.</param>
        private void GerarMipsAdicionarOperação(MipsClass mips, SemanticoToken elemSem, string regInteiro1, string regInteiro2)
        {
            switch (elemSem.Token)
            {
                case "MAIS":
                    mips.SectionText.Adicionar(new MipsText("add", $"{regInteiro1},{regInteiro1},{regInteiro2}"));
                    break;

                case "MENOS":
                    mips.SectionText.Adicionar(new MipsText("sub", $"{regInteiro1},{regInteiro1},{regInteiro2}"));
                    break;

                case "ASTERISTICO":
                    mips.SectionText.Adicionar(new MipsText("mul", $"{regInteiro1},{regInteiro1},{regInteiro2}"));
                    break;

                case "BARRA":
                    mips.SectionText.Adicionar(new MipsText("div", $"{regInteiro1},{regInteiro1},{regInteiro2}"));
                    break;

                default:
                    throw new NotImplementedException($"Não é suportada a geração de código do tipo '{elemSem.Variável.Tipo}' em argumentos de vetor.");
            }
        }

        /// <summary>
        /// Adiciona o teste de indice dentro da faixa da declaração do vetor.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        /// <param name="vetFaixa">Faixa da dimensão atual a ser testada.</param>
        /// <param name="vetK">Fator de correção da dimensão atual do registro do array.</param>
        private void GerarMipsAdicionarTesteFaixa(MipsClass mips, int vetFaixa, int vetK)
        {
            // Testa se a posição informada está dentro dos limites do array:
            mips.SectionText.Adicionar(new MipsText("li", $"$t8,{this.FaixasVetor[vetFaixa].Fim}"));    // Carrega o valor máximo do array no registrador $t8
            mips.SectionText.Adicionar(new MipsText("bgt", "$t7,$t8,fimErroArray"));                    // Testa e se $t7 > $t8 = erro
            mips.SectionText.Adicionar(new MipsText("li", $"$t8,{this.FaixasVetor[vetFaixa].Inicio}")); // Carrega o valor mínimo do array no registrador $t8
            mips.SectionText.Adicionar(new MipsText("blt", "$t7,$t8,fimErroArray"));                    // Testa e se $t7 < $t8 = erro

            mips.SectionText.Adicionar(new MipsText("sub", $"$t7,$t7,{this.FaixasVetor[0].Inicio}"));  // Ajusta o offset para indice do vetor.

            if (vetK > 1)
                mips.SectionText.Adicionar(new MipsText("mul", $"$t7,$t7,{vetK}"));  // Ajusta o offset da posição interna usando o fator de correção.

            mips.SectionText.Adicionar(new MipsText("add", "$t6,$t6,$t7"));   // Acumula a posição
        }

        /// <summary>
        /// Seta o registro do array, conforme valor calculado no registrador $t6.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        private void GerarMipsSetaRegistro(MipsClass mips)
        {
            string id = this.Identificador.Lexema;
            if (!this.Identificador.Variável.Global)
                id = $"{this.Identificador.Variável.Token.Index}_{id}";

            switch (this.Identificador.Variável.Tipo)
            {
                case VariávelTipo.Lógico:
                    mips.HandlerLogico.SetaVetorPosição($"vetLog_{id}");
                    return;

                case VariávelTipo.Inteiro:
                    mips.SectionText.Adicionar(new MipsText("la", $"$t5,vetInt_{id}"));   // Endereço do vetor em memória
                    break;

                case VariávelTipo.Real:
                    mips.SectionText.Adicionar(new MipsText("la", $"$t5,vetReal_{id}"));  // Endereço do vetor em memória
                    break;

                case VariávelTipo.Caracter:
                    mips.SectionText.Adicionar(new MipsText("la", $"$t5,vetCar_{id}"));
                    break;
            }

            mips.SectionText.Adicionar(new MipsText("add", "$t5,$t5,$t6"));   // Soma o endereço do array com a posição calculada
        }

        #endregion

        #region Geração da Árvore

        /// <summary>
        /// Geração de um nó da árvore de análise semântica.
        /// </summary>
        /// <param name="treeNode">Um nó da árvore para adicionar o nó do comando.</param>
        public override void GerarArvore(TreeNode treeNode)
        {
            // Cria o Nó do Comando
            treeNode.Nodes.Add(this.ToString());
        }

        #endregion

        /// <summary>
        /// Retorna um texto que representa o objeto atual.
        /// </summary>
        public override string ToString()
        {
            string tmp = String.Empty;
            for (int i = 0; i < this.FaixasVetor.Length; i++)
            {
                tmp += this.FaixasVetor[i].ToString();
                if (i < this.FaixasVetor.Length - 1)
                    tmp += ", ";
            }

            return $"Declaração: '{this.Identificador.Lexema}: vetor[{tmp}] de {this.TipoDeDados.Lexema}'";
        }

        /// <summary>
        /// Classe interna auxiliar, para armazenamento de uma faixa de dimensão do vetor.
        /// </summary>
        public class FaixaVetor
        {
            /// <summary>
            /// O construtor da classe.
            /// </summary>
            /// <param name="faixa">Token contendo a informação da faixa.</param>
            public FaixaVetor(SemanticoToken faixa)
            {
                this.Elemento = faixa;

                string[] valoresFaixa = faixa.Lexema.Split(new string[] { ".." }, StringSplitOptions.None);
                this.Inicio = int.Parse(valoresFaixa[0]);
                this.Fim = int.Parse(valoresFaixa[1]);
                this.Tamanho = this.Fim - this.Inicio + 1;
            }

            /// <summary>
            /// Obtém o elemento semântico da faixa do da dimensão do vetor.
            /// </summary>
            public SemanticoToken Elemento { get; set; }

            /// <summary>
            /// Obtém o valor inicial da faixa.
            /// </summary>
            public int Inicio { get; }

            /// <summary>
            /// Obtém o valor final da faixa.
            /// </summary>
            public int Fim { get; }

            /// <summary>
            /// Obtém a quantidade de registros na dimensão do vetor.
            /// </summary>
            public int Tamanho { get; }

            /// <summary>
            /// Retorna um texto que representa o objeto atual.
            /// </summary>
            public override string ToString()
            {
                return $"{this.Inicio}..{this.Fim}";
            }
        }
    }
}
