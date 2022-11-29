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
    /// Classe para o comando do tipo 'Leia'.
    /// </summary>
    public class Leia : Tipo
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="elementos">Elementos semânticos que compõem o comando.</param>
        public Leia(List<SemanticoToken> elementos)
        {
            if (elementos == null)
                throw new ArgumentNullException(nameof(elementos));
            if (elementos.Count < 5)
                throw new Exception("Um tipo 'leia' deve receber no mínimo 5 elementos!");

            this.Elementos = elementos.ToArray();

            List<Argumento> argumentos = new List<Argumento>();
            List<SemanticoToken> elem = new List<SemanticoToken>();
            for (int i = 2; i < this.Elementos.Length - 2; i++)
            {
                if (this.Elementos[i].Token.Equals("VIRGULA"))
                {
                    argumentos.Add(new Argumento(elem));
                    elem = new List<SemanticoToken>();
                    continue;
                }

                if (this.Elementos[i].Token.Equals("ABRE_COL"))
                {
                    elem.Add(this.Elementos[i++]);
                    while (!this.Elementos[i].Token.Equals("FECHA_COL"))
                        elem.Add(this.Elementos[i++]);
                    elem.Add(this.Elementos[i]);

                    continue;
                }

                elem.Add(this.Elementos[i]);
            }
            if (elem.Count > 0)
                argumentos.Add(new Argumento(elem));

            this.Argumentos = argumentos.ToArray();
        }

        #region Propriedades

        /// <summary>
        /// Obtém os elementos semânticos que compõem o comando.
        /// </summary>
        public SemanticoToken[] Elementos { get; }

        /// <summary>
        /// Obtém os argumentos para transferir os valores lidos.
        /// </summary>
        public Argumento[] Argumentos { get; }

        #endregion

        #region Validação Semântica

        /// <summary>
        /// Executar a validação semântica das variáveis nos comandos.
        /// </summary>
        /// <param name="listaVariáveisAtribuidas">A lista de variáveis que receberam atribuição de valores (reseta quando retorna do escopo que recebeu a atribuição).</param>
        /// <param name="listaVariáveisGlobal">A lista de variáveis de escopo global.</param>
        /// <param name="listaVariáveisLocal">A lista de variáveis de escopo local.</param>
        /// <param name="listaErrors">A lista de erros encontrados durante a análise.</param>
        /// <param name="debug">Saida de debugação.</param>
        public void VerificarArgumentos(List<Variável> listaVariáveisAtribuidas, List<Variável> listaVariáveisGlobal, List<Variável> listaVariáveisLocal, List<SemanticoError> listaErrors, ListBox debug)
        {
            for (int i = 0; i < this.Argumentos.Length; i++)
            {
                Argumento itemArg = this.Argumentos[i];
                SemanticoToken itemArgElem = itemArg.Elementos[0];

                // Verifica se a variável na operação foi declarada localmente:
                Variável varExpr = listaVariáveisLocal.Find(x => x.Identificador.Equals(itemArgElem.Lexema));
                if (varExpr == null)  // Não encontrada no escopo local, então:
                {
                    // Busca na lista de escopo global:
                    varExpr = listaVariáveisGlobal.Find(x => x.Identificador.Equals(itemArgElem.Lexema));
                    if (varExpr == null)
                    {
                        listaErrors.Add(new SemanticoError(itemArgElem, "A variável usada não está declarada!"));
                        debug.Items.Add($"X > {itemArg.ToString()}");
                        continue;
                    }
                }

                itemArgElem.Variável = varExpr;

                if (varExpr.TipoDeclaração == VariávelDeclaração.Vetor)
                {
                    if (itemArg.Elementos.Length > 1)
                        ((DeclaraçãoVetor)varExpr.Declaração).ValidarIndice(itemArg, listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
                }

                debug.Items.Add($"> {varExpr.ToString()}");

                if (!varExpr.ValorAtribuido)
                    listaVariáveisAtribuidas.Add(varExpr);

                // Variável que recebe a leitura agora possui valor atribuido, então marca:
                varExpr.ValorAtribuido = true;
            }
        }

        #endregion

        #region Geração de Código MIPS Assembly

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para este componente.
        /// </summary>
        /// <param name="mips">Referância para a estrutura de informações Assembly MIPS.</param>
        public override void GerarMips(MipsClass mips)
        {
            foreach (var argumento in this.Argumentos)
            {
                // Se o argumento possuir um elemento, pode ser uma variável simples ou vetor, então:
                if (argumento.Elementos.Length == 1)
                {
                    SemanticoToken elemSem = argumento.Elementos[0];
                    
                    if (elemSem.Variável.TipoDeclaração == VariávelDeclaração.Vetor)
                    {
                        DeclaraçãoVetor decVet = (DeclaraçãoVetor)elemSem.Variável.Declaração;

                        mips.SectionText.Adicionar(new MipsText("li", "$s0,0"));                            // Indice atual do array
                        mips.SectionText.Adicionar(new MipsText("li", $"$s1,{decVet.ContagemRegistros}"));  // Indice máximo

                        mips.SectionText.Adicionar(new MipsText($"loop_{elemSem.Index}:", null));  // Etiqueta da repetição para ler cada elemento do array

                        if (elemSem.Variável.Tipo == VariávelTipo.Inteiro || elemSem.Variável.Tipo == VariávelTipo.Real)
                        {
                            switch (elemSem.Variável.Tipo)
                            {
                                case VariávelTipo.Inteiro:
                                    mips.SectionText.Adicionar(new MipsText("la", $"$t1,{elemSem.ObterEtiqueta()}"));   // Endereço do vetor em memória
                                    break;
                                case VariávelTipo.Real:
                                    mips.SectionText.Adicionar(new MipsText("la", $"$t1,{elemSem.ObterEtiqueta()}"));  // Endereço do vetor em memória
                                    break;
                            }

                            mips.SectionText.Adicionar(new MipsText("mul", "$t2,$s0,4"));    // Utiliza múltiplos de quatro para acessar cada registro do array
                            mips.SectionText.Adicionar(new MipsText("add", "$t1,$t1,$t2"));  // Soma o endereço do array com a posição
                        }
                        if (elemSem.Variável.Tipo == VariávelTipo.Caracter)
                        {
                            mips.SectionText.Adicionar(new MipsText("la", $"$t1,{elemSem.ObterEtiqueta()}"));   // Endereço do vetor em memória
                            mips.SectionText.Adicionar(new MipsText("mul", "$t2,$s0,34"));   // Utiliza múltiplos de 34 para acessar cada registro do array
                            mips.SectionText.Adicionar(new MipsText("add", "$t1,$t1,$t2"));  // Soma o endereço do array com a posição
                        }
                        else
                        {
                            mips.SectionText.Adicionar(new MipsText("move", "$t6,$s0"));   // Move o valor da posição atual à ler, do registrador $s0 para o registrador $t6
                            mips.HandlerLogico.SetaVetorPosição(elemSem.ObterEtiqueta());  // Ajusta o valor no registrador $t6 para corresponder ao registro no array
                        }

                        switch (elemSem.Variável.Tipo)
                        {
                            case VariávelTipo.Lógico:
                                mips.SectionText.Adicionar(new MipsText("li", "$v0,5"));      // Comando para ler um inteiro
                                mips.SectionText.Adicionar(MipsText.Syscall);                 // Executando o comando

                                // Testa se o valor informado está dentro dos limites para um valor lógico:
                                mips.SectionText.Adicionar(new MipsText("bgt", "$v0,1,fimErroEntradaLog"));   // Testa e se $v0 > 1 = erro
                                mips.SectionText.Adicionar(new MipsText("blt", "$v0,0,fimErroEntradaLog"));   // Testa e se $v0 < 0 = erro
                                
                                mips.HandlerLogico.SetaVetorValor("$v0", elemSem.ObterEtiqueta());

                                // Informa utilização de opcionais:
                                mips.HandlerOpcionais.EntradaLógicaUtilizada = true;
                                break;

                            case VariávelTipo.Inteiro:
                                mips.SectionText.Adicionar(new MipsText("li", "$v0,5"));      // Comando para ler um inteiro
                                mips.SectionText.Adicionar(MipsText.Syscall);                 // Executando o comando

                                // Este comando é para mover o inteiro fornecido do registrador $v0 para a posição no array
                                mips.SectionText.Adicionar(new MipsText("sw", "$v0,($t1)"));
                                break;

                            case VariávelTipo.Real:
                                mips.SectionText.Adicionar(new MipsText("li", "$v0,6"));      // Comando para ler um real
                                mips.SectionText.Adicionar(MipsText.Syscall);                 // Executando o comando

                                // Este comando é para mover o real fornecido do coprocessador 1 $f0 para a posição no array
                                mips.SectionText.Adicionar(new MipsText("swc1", "$f0,($t1)"));
                                break;

                            case VariávelTipo.Caracter:
                                mips.SectionText.Adicionar(new MipsText("li", "$v0,8"));      // Comando para ler um texto
                                mips.SectionText.Adicionar(new MipsText("move", "$a0,$t1"));  // Endereço da variável de texto
                                mips.SectionText.Adicionar(new MipsText("li", "$a1,33"));     // Quantia máxima de caracteres a serem lidos (tamanho_variável - 1)
                                mips.SectionText.Adicionar(MipsText.Syscall);                 // Executando o comando

                                // Remove a quebra de linha do final do texto:
                                mips.SectionText.Adicionar(new MipsText($"loop_lf_{elemSem.Index}:", null));
                                mips.SectionText.Adicionar(new MipsText("lb", "$t2,0($t1)"));
                                mips.SectionText.Adicionar(new MipsText("beq", $"$t2,10,jmp_lf_{elemSem.Index}"));
                                mips.SectionText.Adicionar(new MipsText("beq", $"$t2,0,jmp_lf_{elemSem.Index}"));
                                mips.SectionText.Adicionar(new MipsText("add", "$t1,$t1,1"));
                                mips.SectionText.Adicionar(new MipsText("j", $"loop_lf_{elemSem.Index}"));
                                mips.SectionText.Adicionar(new MipsText($"jmp_lf_{elemSem.Index}:", null));
                                mips.SectionText.Adicionar(new MipsText("li", "$t2,0"));
                                mips.SectionText.Adicionar(new MipsText("sb", "$t2,0($t1)"));
                                break;
                        }

                        mips.SectionText.Adicionar(new MipsText("add", "$s0,$s0,1"));   // Pula para o próximo item
                        mips.SectionText.Adicionar(new MipsText("bne", $"$s0,$s1,loop_{elemSem.Index}"));  // Enquanto não chegamos ao último item, continuamos iterando
                        mips.SectionText.Adicionar(MipsText.Blank);
                        continue;
                    }

                    if (elemSem.Variável.Tipo == VariávelTipo.Lógico)
                    {
                        mips.SectionText.Adicionar(new MipsText("li", "$v0,5"));   // Comando para ler um inteiro
                        mips.SectionText.Adicionar(MipsText.Syscall);              // Executando o comando

                        // Testa se o valor informado está dentro dos limites para um valor lógico:
                        mips.SectionText.Adicionar(new MipsText("bgt", "$v0,1,fimErroEntradaLog"));   // Testa e se $v0 > 1 = erro
                        mips.SectionText.Adicionar(new MipsText("blt", "$v0,0,fimErroEntradaLog"));   // Testa e se $v0 < 0 = erro

                        // Move o valor lógico fornecido do registrador $v0 para o segmento de dados da etiqueta (variável).
                        mips.HandlerLogico.SetarVariável("$v0", elemSem.ObterEtiqueta());
                        mips.SectionText.Adicionar(MipsText.Blank);

                        // Informa utilização de opcionais:
                        mips.HandlerOpcionais.EntradaLógicaUtilizada = true;
                    }
                    else if (elemSem.Variável.Tipo == VariávelTipo.Inteiro)
                    {
                        mips.SectionText.Adicionar(new MipsText("li", "$v0,5"));   // Comando para ler um inteiro
                        mips.SectionText.Adicionar(MipsText.Syscall);              // Executando o comando

                        // Move o inteiro fornecido do registrador $v0 para o segmento de dados da etiqueta (variável).
                        mips.SectionText.Adicionar(new MipsText("sw", $"$v0,{elemSem.ObterEtiqueta()}"));
                        mips.SectionText.Adicionar(MipsText.Blank);
                    }
                    else if (elemSem.Variável.Tipo == VariávelTipo.Real)
                    {
                        mips.SectionText.Adicionar(new MipsText("li", "$v0,6"));   // Comando para ler um real
                        mips.SectionText.Adicionar(MipsText.Syscall);              // Executando o comando

                        // Move o real fornecido do coprocessador 1 $f0 para o segmento de dados da etiqueta (variável).
                        mips.SectionText.Adicionar(new MipsText("swc1", $"$f0,{elemSem.ObterEtiqueta()}"));
                        mips.SectionText.Adicionar(MipsText.Blank);
                    }
                    else if (elemSem.Variável.Tipo == VariávelTipo.Caracter)
                    {
                        mips.SectionText.Adicionar(new MipsText("li", "$v0,8"));   // Comando para ler um texto
                        mips.SectionText.Adicionar(new MipsText("la", $"$a0,{elemSem.ObterEtiqueta()}"));  // Endereço da variável de texto
                        mips.SectionText.Adicionar(new MipsText("li", "$a1,33"));  // Quantia máxima de caracteres a serem lidos (tamanho_variável - 1)
                        mips.SectionText.Adicionar(MipsText.Syscall);              // Executando o comando
                        
                        // Adiciona a chamada da função para remoção da quebra de linha.
                        mips.SectionText.Adicionar(new MipsText("jal", "funcTextoRemoveLF"));
                        mips.SectionText.Adicionar(MipsText.Blank);

                        // Informa utilização de opcionais:
                        mips.HandlerOpcionais.EntradaTextoUtilizada = true;
                    }
                    
                    continue;
                }
                
                // No comando 'leia' apenas vetores terão mais argumentos:
                if (argumento.Elementos[0].Variável.TipoDeclaração == VariávelDeclaração.Vetor)
                {
                    SemanticoToken elemSem = argumento.Elementos[0];
                    DeclaraçãoVetor decVet = (DeclaraçãoVetor)elemSem.Variável.Declaração;  // Armazena a declaração do vetor.
                    decVet.GerarMipsValorIndex(mips, argumento.Elementos);   // Posiciona o registro do vetor.

                    switch (elemSem.Variável.Tipo)
                    {
                        case VariávelTipo.Lógico:
                            mips.SectionText.Adicionar(new MipsText("li", "$v0,5"));   // Comando para ler um inteiro
                            mips.SectionText.Adicionar(MipsText.Syscall);              // Executando o comando
                            
                            // Testa se o valor informado está dentro dos limites para um valor lógico:
                            mips.SectionText.Adicionar(new MipsText("bgt", "$v0,1,fimErroEntradaLog"));   // Testa e se $v0 > 1 = erro
                            mips.SectionText.Adicionar(new MipsText("blt", "$v0,0,fimErroEntradaLog"));   // Testa e se $v0 < 0 = erro

                            // Move o valor lógico fornecido do registrador $v0 para a posição no array
                            mips.HandlerLogico.SetaVetorValor("$v0", elemSem.ObterEtiqueta());

                            // Informa utilização de opcionais:
                            mips.HandlerOpcionais.EntradaLógicaUtilizada = true;
                            break;

                        case VariávelTipo.Inteiro:
                            mips.SectionText.Adicionar(new MipsText("li", "$v0,5"));   // Comando para ler um inteiro
                            mips.SectionText.Adicionar(MipsText.Syscall);              // Executando o comando

                            // Este comando é para mover o inteiro fornecido do registrador $v0 para a posição no array
                            mips.SectionText.Adicionar(new MipsText("sw", "$v0,($t5)"));
                            break;

                        case VariávelTipo.Real:
                            mips.SectionText.Adicionar(new MipsText("li", "$v0,6"));   // Comando para ler um real
                            mips.SectionText.Adicionar(MipsText.Syscall);              // Executando o comando

                            // Este comando é para mover o real fornecido do coprocessador 1 $f0 para a posição no array
                            mips.SectionText.Adicionar(new MipsText("swc1", "$f0,($t5)"));
                            break;

                        case VariávelTipo.Caracter:
                            mips.SectionText.Adicionar(new MipsText("li", "$v0,8"));      // Comando para ler um texto
                            mips.SectionText.Adicionar(new MipsText("move", "$a0,$t5"));  // Endereço da variável de texto
                            mips.SectionText.Adicionar(new MipsText("li", "$a1,33"));     // Quantia máxima de caracteres a serem lidos (tamanho_variável - 1)
                            mips.SectionText.Adicionar(MipsText.Syscall);                 // Executando o comando
                            
                            // Adiciona a chamada da função para remoção da quebra de linha.
                            mips.SectionText.Adicionar(new MipsText("jal", "funcTextoRemoveLF"));

                            // Informa utilização de opcionais:
                            mips.HandlerOpcionais.EntradaTextoUtilizada = true;
                            break;
                    }

                    mips.SectionText.Adicionar(MipsText.Blank);
                    continue;
                }
            }
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
            for (int i = 0; i < this.Argumentos.Length; i++)
            {
                tmp += this.Argumentos[i].ToString();
                if (i < this.Argumentos.Length - 1)
                    tmp += ", ";
            }

            return $"Leia: {tmp}";
        }

        /// <summary>
        /// Classe interna auxiliar, para armazenar os argumentos do comando.
        /// </summary>
        public class Argumento
        {
            /// <summary>
            /// O construtor da classe.
            /// </summary>
            /// <param name="elementos">Elementos que compõe um comando na expressão.</param>
            public Argumento(List<SemanticoToken> elementos)
            {
                this.Elementos = elementos.ToArray();
            }

            /// <summary>
            /// Elementos que compõe um comando na expressão.
            /// </summary>
            public SemanticoToken[] Elementos { get; set; }

            /// <summary>
            /// Retorna um texto que representa o objeto atual.
            /// </summary>
            public override string ToString()
            {
                string tmp = String.Empty;
                foreach (var item in this.Elementos)
                    tmp += item.Lexema;

                return tmp;
            }
        }
    }
}
