using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Classe para o comando do tipo 'Escreva'.
    /// </summary>
    public class Escreva : Tipo
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="elementos">Elementos semânticos que compõem o comando.</param>
        public Escreva(List<SemanticoToken> elementos)
        {
            if (elementos == null)
                throw new ArgumentNullException(nameof(elementos));
            if (elementos.Count < 5)
                throw new Exception("Um tipo 'escreva' deve receber no mínimo 5 elementos!");

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
        /// Obtém os argumentos para escrever na saída do sistema.
        /// </summary>
        public Argumento[] Argumentos { get; }

        #endregion

        #region Validação Semântica

        /// <summary>
        /// Executar a validação semântica das variáveis nos comandos.
        /// </summary>
        /// <param name="listaVariáveisGlobal">A lista de variáveis de escopo global.</param>
        /// <param name="listaVariáveisLocal">A lista de variáveis de escopo local.</param>
        /// <param name="listaErrors">A lista de erros encontrados durante a análise.</param>
        /// <param name="debug">Saida de debugação.</param>
        public void VerificarArgumentos(List<Variável> listaVariáveisGlobal, List<Variável> listaVariáveisLocal, List<SemanticoError> listaErrors, ListBox debug)
        {

            foreach (Argumento itemArg in this.Argumentos)
            {
                for (int i = 0; i < itemArg.Elementos.Length; i++)
                {
                    SemanticoToken itemArgElem = itemArg.Elementos[i];

                    if (itemArgElem.Token.Equals("ID"))
                    {
                        // Verifica se a variável na operação foi declarada localmente:
                        Variável varExpr = listaVariáveisLocal.Find(x => x.Identificador.Equals(itemArgElem.Lexema));
                        if (varExpr == null)  // Não encontrada no escopo local, então:
                        {
                            // Busca na lista de escopo global:
                            varExpr = listaVariáveisGlobal.Find(x => x.Identificador.Equals(itemArgElem.Lexema));
                            if (varExpr == null)
                            {
                                listaErrors.Add(new SemanticoError(itemArgElem, "A variável usada não está declarada!"));
                                debug.Items.Add($"X > {itemArgElem.ToString()}");
                                continue;
                            }
                        }

                        itemArgElem.Variável = varExpr;

                        if (varExpr.TipoDeclaração == VariávelDeclaração.Vetor)
                        {
                            if (itemArg.Elementos.Length > 1)
                                ((DeclaraçãoVetor)varExpr.Declaração).ValidarIndice(ref i, itemArg.Elementos, listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);

                            varExpr.Utilizado = true;
                            continue;
                        }

                        if (varExpr.TipoDeclaração == VariávelDeclaração.Procedimento)
                        {
                            listaErrors.Add(new SemanticoError(itemArgElem, "Um procedimento não pode ser utilizado como variável em uma comando 'escreva'!"));
                            debug.Items.Add($"X > {itemArgElem.ToString()}");
                            break;
                        }

                        if (varExpr.TipoDeclaração == VariávelDeclaração.Função)
                        {
                            ((Função)varExpr.Declaração).ValidarArgumentos(ref i, itemArg.Elementos, listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
                            varExpr.Utilizado = true;
                            continue;
                        }

                        if (!varExpr.ValorAtribuido)
                        {
                            listaErrors.Add(new SemanticoError(itemArgElem, "A variável usada não possui valor atribuido!"));
                            debug.Items.Add($"X > {itemArgElem.ToString()}");
                            continue;
                        }

                        debug.Items.Add($"> {varExpr.ToString()}");
                    }
                }
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
                if (argumento.Elementos.Length == 1)
                {
                    SemanticoToken elemSem = argumento.Elementos[0];
                    if (elemSem.Token.Equals("CONST_TEXTO"))
                    {
                        if (elemSem.Lexema.Equals("\"\\n\""))
                        {
                            mips.SectionText.Adicionar(new MipsText("li", "$v0,4"));     // Comando para imprimir um texto
                            mips.SectionText.Adicionar(new MipsText("la", "$a0,lfeed")); // Carregando o texto de quebra de linha no argumento para habilitar a impressão
                            mips.HandlerOpcionais.QuebraLinha = true;
                        }
                        else if (elemSem.Lexema.Equals("\" \""))
                        {
                            mips.SectionText.Adicionar(new MipsText("li", "$v0,4"));     // Comando para imprimir um texto
                            mips.SectionText.Adicionar(new MipsText("la", "$a0,space")); // Carregando o texto de quebra de linha no argumento para habilitar a impressão
                            mips.HandlerOpcionais.EspaçoBranco = true;
                        }
                        else
                        {
                            mips.SectionData.Adicionar(new MipsDataAsciiz($"escreva_{elemSem.Index}", elemSem.Lexema));

                            mips.SectionText.Adicionar(new MipsText("li", "$v0,4"));    // Comando para imprimir um texto
                            mips.SectionText.Adicionar(new MipsText("la", $"$a0,escreva_{elemSem.Index}")); // Carregando o texto no argumento para habilitar a impressão
                        }

                        mips.SectionText.Adicionar(MipsText.Syscall);               // Executando o comando
                        mips.SectionText.Adicionar(MipsText.Blank);
                        continue;
                    }

                    if (elemSem.Token.Equals("CONST_INT"))
                    {
                        mips.SectionText.Adicionar(new MipsText("li", "$v0,1"));    // Comando para escrever um inteiro
                        mips.SectionText.Adicionar(new MipsText("la", $"$a0,{elemSem.Lexema}")); // Carregando o número no argumento para habilitar a impressão
                        mips.SectionText.Adicionar(MipsText.Syscall);               // Executando o comando
                        mips.SectionText.Adicionar(MipsText.Blank);
                        continue;
                    }

                    if (elemSem.Token.Equals("CONST_REAL"))
                    {
                        // Adiciona o valor da constante real como uma constante no segmento de dados.
                        mips.SectionData.Adicionar(new MipsDataFloat($"constReal_{elemSem.Index}", new float[] { float.Parse(elemSem.Lexema, CultureInfo.InvariantCulture) }));

                        mips.SectionText.Adicionar(new MipsText("l.s", $"$f12,constReal_{elemSem.Index}")); // Carregando o número no argumento para habilitar a impressão
                        mips.SectionText.Adicionar(new MipsText("li", "$v0,2"));    // Comando para escrever um real
                        mips.SectionText.Adicionar(MipsText.Syscall);               // Executando o comando
                        mips.SectionText.Adicionar(MipsText.Blank);
                        continue;
                    }

                    if (elemSem.Variável.TipoDeclaração == VariávelDeclaração.Vetor)
                    {
                        DeclaraçãoVetor decVet = (DeclaraçãoVetor)elemSem.Variável.Declaração;

                        mips.SectionText.Adicionar(new MipsText("li", "$s0,0"));                            // Indice atual do array
                        mips.SectionText.Adicionar(new MipsText("li", $"$s1,{decVet.ContagemRegistros}"));  // Indice máximo

                        // Adiciona o abre chaves inicial:
                        mips.SectionText.Adicionar(new MipsText("li", "$v0,4"));       // Comando para imprimir um texto
                        mips.SectionText.Adicionar(new MipsText("la", "$a0,obraces")); // Carregando o texto no argumento para habilitar a impressão
                        mips.SectionText.Adicionar(MipsText.Syscall);                  // Executando o comando

                        mips.SectionText.Adicionar(new MipsText($"loop_{elemSem.Index}:", null));  // Etiqueta da repetição para escrever cada elemento do array

                        // Se o vetor possuir mais de uma dimensão, então será necessário abre chaves adicionais para separar as dimensões:
                        if (decVet.FaixasVetor.Length > 1)
                        {
                            int vetK = 1;
                            for (int i = 0; i < decVet.FaixasVetor.Length - 1; i++)
                            {
                                vetK *= decVet.FaixasVetor[i].Tamanho;
                                
                                mips.SectionText.Adicionar(new MipsText("div", $"$t3,$s0,{vetK}"));
                                mips.SectionText.Adicionar(new MipsText("mfhi", "$t3"));

                                mips.SectionText.Adicionar(new MipsText("bne", $"$t3,0,loop_{elemSem.Index}_obraces_d{i}"));  // Enquanto não chegamos ao último item, continuamos inserindo espaços
                                mips.SectionText.Adicionar(new MipsText("li", "$v0,4"));       // Comando para imprimir um texto
                                mips.SectionText.Adicionar(new MipsText("la", "$a0,obraces")); // Carregando o texto no argumento para habilitar a impressão
                                mips.SectionText.Adicionar(MipsText.Syscall);                  // Executando o comando
                                mips.SectionText.Adicionar(new MipsText($"loop_{elemSem.Index}_obraces_d{i}:", null));
                            }
                        }

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
                            mips.SectionText.Adicionar(new MipsText("move", "$t6,$s0"));  // Move o valor da posição atual à escrever, do registrador $s0 para o registrador $t6
                            
                            mips.HandlerLogico.SetaVetorPosição(elemSem.ObterEtiqueta());        // Ajusta o valor no registrador $t6 para corresponder ao registro no array
                            mips.HandlerLogico.ObterVetorValor("$t4", elemSem.ObterEtiqueta());  // Obtém o valor do registro no array
                        }

                        switch (elemSem.Variável.Tipo)
                        {
                            case VariávelTipo.Lógico:
                                mips.SectionText.Adicionar(new MipsText("li", "$v0,1"));      // Comando para escrever um inteiro
                                mips.SectionText.Adicionar(new MipsText("move", "$a0,$t4"));  // Carrega o valor da posição atual para o argumento para o argumento $a0
                                break;

                            case VariávelTipo.Inteiro:
                                mips.SectionText.Adicionar(new MipsText("li", "$v0,1"));      // Comando para escrever um inteiro
                                mips.SectionText.Adicionar(new MipsText("lw", $"$a0,($t1)")); // Carrega o valor da posição atual para o argumento para o argumento $a0
                                break;

                            case VariávelTipo.Real:
                                mips.SectionText.Adicionar(new MipsText("li", "$v0,2"));         // Comando para escrever um real
                                mips.SectionText.Adicionar(new MipsText("lwc1", $"$f12,($t1)")); // Carrega o valor da posição atual para o argumento para o argumento $f12
                                break;

                            case VariávelTipo.Caracter:
                                mips.SectionText.Adicionar(new MipsText("li", "$v0,4"));        // Comando para imprimir um texto
                                mips.SectionText.Adicionar(new MipsText("la", $"$a0,dquotes")); // Carregando o texto no argumento para habilitar a impressão
                                mips.SectionText.Adicionar(MipsText.Syscall);                   // Executando o comando

                                mips.SectionText.Adicionar(new MipsText("li", "$v0,4"));     // Comando para imprimir um texto
                                mips.SectionText.Adicionar(new MipsText("move", "$a0,$t1")); // Carregando o endereço do registro de texto no argumento para habilitar a impressão
                                break;
                        }
                        mips.SectionText.Adicionar(MipsText.Syscall);   // Executando o comando

                        if (elemSem.Variável.Tipo == VariávelTipo.Caracter)
                        {
                            mips.SectionText.Adicionar(new MipsText("li", "$v0,4"));        // Comando para imprimir um texto
                            mips.SectionText.Adicionar(new MipsText("la", $"$a0,dquotes")); // Carregando o texto no argumento para habilitar a impressão
                            mips.SectionText.Adicionar(MipsText.Syscall);                   // Executando o comando

                            // Informa utilização de opcionais:
                            mips.HandlerOpcionais.AspasDuplas = true;
                        }

                        mips.SectionText.Adicionar(new MipsText("add", "$s0,$s0,1"));  // Pula para o próximo item

                        // Se o vetor possuir mais de uma dimensão, então será necessário fecha chaves adicionais para separar as dimensões:
                        if (decVet.FaixasVetor.Length > 1)
                        {
                            int vetK = 1;
                            for (int i = 0; i < decVet.FaixasVetor.Length - 1; i++)
                            {
                                vetK *= decVet.FaixasVetor[i].Tamanho;
                                
                                // Calcula o resto da divisão da posição atual pela dimensão do array:
                                mips.SectionText.Adicionar(new MipsText("div", $"$t3,$s0,{vetK}"));
                                mips.SectionText.Adicionar(new MipsText("mfhi", "$t3"));

                                mips.SectionText.Adicionar(new MipsText("bne", $"$t3,0,loop_{elemSem.Index}_cbraces_d{i}"));  // Se o resto não for zero, não insere o fecha chaves
                                mips.SectionText.Adicionar(new MipsText("li", "$v0,4"));       // Comando para imprimir um texto
                                mips.SectionText.Adicionar(new MipsText("la", "$a0,cbraces")); // Carregando o texto no argumento para habilitar a impressão
                                mips.SectionText.Adicionar(MipsText.Syscall);                  // Executando o comando
                                mips.SectionText.Adicionar(new MipsText($"loop_{elemSem.Index}_cbraces_d{i}:", null));
                            }
                        }

                        mips.SectionText.Adicionar(new MipsText("beq", $"$s0,$s1,loop_{elemSem.Index}_space"));  // Enquanto não chegamos ao último item, continuamos inserindo espaços
                        mips.SectionText.Adicionar(new MipsText("li", "$v0,4"));      // Comando para imprimir um texto
                        mips.SectionText.Adicionar(new MipsText("la", $"$a0,space")); // Carregando o texto no argumento para habilitar a impressão
                        mips.SectionText.Adicionar(MipsText.Syscall);                 // Executando o comando
                        mips.SectionText.Adicionar(new MipsText($"loop_{elemSem.Index}_space:", null));

                        mips.SectionText.Adicionar(new MipsText("bne", $"$s0,$s1,loop_{elemSem.Index}"));  // Enquanto não chegamos ao último item, continuamos iterando

                        // Adiciona o fecha chaves final:
                        mips.SectionText.Adicionar(new MipsText("li", "$v0,4"));       // Comando para imprimir um texto
                        mips.SectionText.Adicionar(new MipsText("la", "$a0,cbraces")); // Carregando o texto no argumento para habilitar a impressão
                        mips.SectionText.Adicionar(MipsText.Syscall);                  // Executando o comando
                        mips.SectionText.Adicionar(MipsText.Blank);

                        // Informa utilização de opcionais:
                        mips.HandlerOpcionais.EspaçoBranco = true;
                        mips.HandlerOpcionais.Chaves = true;
                        continue;
                    }
                    
                    if (elemSem.Variável.Tipo == VariávelTipo.Lógico)
                    {
                        mips.SectionText.Adicionar(new MipsText("li", "$v0,1"));    // Comando para escrever um inteiro
                        mips.HandlerLogico.ObterVariável("$a0", elemSem.ObterEtiqueta());
                        mips.SectionText.Adicionar(MipsText.Syscall);   // Executando o comando
                        mips.SectionText.Adicionar(MipsText.Blank);
                    }
                    else if(elemSem.Variável.Tipo == VariávelTipo.Inteiro)
                    {
                        mips.SectionText.Adicionar(new MipsText("li", "$v0,1"));    // Comando para escrever um inteiro
                        mips.SectionText.Adicionar(new MipsText("lw", $"$a0,{elemSem.ObterEtiqueta()}"));
                        mips.SectionText.Adicionar(MipsText.Syscall);   // Executando o comando
                        mips.SectionText.Adicionar(MipsText.Blank);
                    }
                    else if(elemSem.Variável.Tipo == VariávelTipo.Real)
                    {
                        mips.SectionText.Adicionar(new MipsText("li", "$v0,2"));    // Comando para escrever um real
                        mips.SectionText.Adicionar(new MipsText("lwc1", $"$f12,{elemSem.ObterEtiqueta()}"));
                        mips.SectionText.Adicionar(MipsText.Syscall);   // Executando o comando
                        mips.SectionText.Adicionar(MipsText.Blank);
                    }
                    else if (elemSem.Variável.Tipo == VariávelTipo.Caracter)
                    {
                        mips.SectionText.Adicionar(new MipsText("li", "$v0,4"));    // Comando para imprimir um texto
                        mips.SectionText.Adicionar(new MipsText("la", $"$a0,{elemSem.ObterEtiqueta()}")); // Carregando o texto no argumento para habilitar a impressão
                        mips.SectionText.Adicionar(MipsText.Syscall);   // Executando o comando
                        mips.SectionText.Adicionar(MipsText.Blank);
                    }

                    continue;
                }
                
                Expressão expr = new Expressão(argumento.Elementos);
                expr.GerarMips(mips);     // Adiciona os comandos da expressão.

                //switch (tipoResultado)
                switch (expr.TipoResultado)
                {
                    case VariávelTipo.Lógico:
                        mips.SectionText.Adicionar(new MipsText("li", "$v0,1"));     // Comando para escrever um inteiro
                        mips.SectionText.Adicionar(new MipsText("move", "$a0,$t4")); // Carrega o valor da posição atual para o argumento para o argumento $a0
                        break;

                    case VariávelTipo.Inteiro:
                        mips.SectionText.Adicionar(new MipsText("li", "$v0,1"));     // Comando para escrever um inteiro
                        mips.SectionText.Adicionar(new MipsText("move", "$a0,$t0")); // Move o resultado da expressão armazenado no registrador $t0 para o argumento $a0
                        break;

                    case VariávelTipo.Real:
                        mips.SectionText.Adicionar(new MipsText("li", "$v0,2"));       // Comando para escrever um real
                        mips.SectionText.Adicionar(new MipsText("mov.s", "$f12,$f4")); // Move o resultado da expressão armazenado no registrador $f4 para o argumento $f12
                        break;

                    case VariávelTipo.Caracter:
                        mips.SectionText.Adicionar(new MipsText("li", "$v0,4"));       // Comando para imprimir um texto
                        break;
                }

                mips.SectionText.Adicionar(MipsText.Syscall);   // Executando o comando
                mips.SectionText.Adicionar(MipsText.Blank);
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

            return $"Escreva: {tmp}";
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
