using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Compilador.GeradorCódigo.MIPS;
using Compilador.GeradorCódigo.MIPS.MipsData;

namespace Compilador.Semantico.Tipos.Auxiliar
{
    public partial class Expressão
    {
        #region Validação Semântica da Expressão Lógica

        /// <summary>
        /// Executa a validação semântica de uma expressão aritmética.
        /// </summary>
        /// <param name="tipoResultado">Tipo de resultado esperado da expressão.</param>
        /// <param name="listaVariáveisGlobal">A lista de variáveis de escopo global.</param>
        /// <param name="listaVariáveisLocal">A lista de variáveis de escopo local.</param>
        /// <param name="listaErrors">A lista de erros encontrados durante a análise.</param>
        /// <param name="debug">Saida de debugação.</param>
        private void ValidarExpressãoAritmética(VariávelTipo tipoResultado, List<Variável> listaVariáveisGlobal, List<Variável> listaVariáveisLocal, List<SemanticoError> listaErrors, ListBox debug)
        {
            // Testa as variáveis na expressão de atribuição, se estão declaradas, possuem valor atribuido e o tipo de dados é compativel com a operação:
            for (int i = 0; i < this.Elementos.Length; i++)
            {
                SemanticoToken itemExpr = this.Elementos[i];

                if (itemExpr.Token.Equals("ID"))
                {
                    // Verifica se a variável na operação foi declarada localmente:
                    Variável varExpr = listaVariáveisLocal.Find(x => x.Identificador.Equals(itemExpr.Lexema));
                    if (varExpr == null)  // Não encontrada no escopo local, então:
                    {
                        // Busca na lista de escopo global:
                        varExpr = listaVariáveisGlobal.Find(x => x.Identificador.Equals(itemExpr.Lexema));
                        if (varExpr == null)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, "A variável usada não está declarada!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            continue;
                        }
                    }

                    itemExpr.Variável = varExpr;

                    if (varExpr.TipoDeclaração == VariávelDeclaração.Vetor)
                    {
                        if (tipoResultado == VariávelTipo.Inteiro && varExpr.Tipo == VariávelTipo.Real)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo inteiro não pode receber o valor do vetor '{itemExpr.Lexema}' do tipo real!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                        }

                        ((DeclaraçãoVetor)varExpr.Declaração).ValidarIndice(ref i, this.Elementos, listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
                        varExpr.Utilizado = true;
                        continue;
                    }

                    if (varExpr.TipoDeclaração == VariávelDeclaração.Procedimento)
                    {
                        listaErrors.Add(new SemanticoError(itemExpr, "Um procedimento não pode ser utilizado como variável em uma atribuição!"));
                        debug.Items.Add($"X > {itemExpr.ToString()}");
                        break;
                    }

                    if (varExpr.TipoDeclaração == VariávelDeclaração.Função)
                    {
                        ((Função)varExpr.Declaração).ValidarArgumentos(ref i, this.Elementos, listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);

                        if (tipoResultado == VariávelTipo.Inteiro && varExpr.Tipo == VariávelTipo.Real)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo inteiro não pode receber o retorno de uma função do tipo real!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                        }

                        varExpr.Utilizado = true;
                        continue;
                    }

                    if (!varExpr.ValorAtribuido)
                    {
                        listaErrors.Add(new SemanticoError(itemExpr, "A variável usada não possui valor atribuido!"));
                        debug.Items.Add($"X > {itemExpr.ToString()}");
                        continue;
                    }

                    if (tipoResultado == VariávelTipo.Inteiro)
                    {
                        if (varExpr.Tipo == VariávelTipo.Lógico)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo inteiro não pode receber a variável '{varExpr.Identificador}' do tipo lógico!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            continue;
                        }

                        if (varExpr.Tipo == VariávelTipo.Real)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo inteiro não pode receber a variável '{varExpr.Identificador}' do tipo real!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            continue;
                        }

                        if (varExpr.Tipo == VariávelTipo.Caracter)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo inteiro não pode receber a variável '{varExpr.Identificador}' do tipo caracter!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            continue;
                        }
                    }

                    if (tipoResultado == VariávelTipo.Real)
                    {
                        if (varExpr.Tipo == VariávelTipo.Lógico)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo real não pode receber a variável '{varExpr.Identificador}' do tipo lógico!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            continue;
                        }

                        if (varExpr.Tipo == VariávelTipo.Caracter)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo real não pode receber a variável '{varExpr.Identificador}' do tipo caracter!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            continue;
                        }
                    }

                    if (tipoResultado == VariávelTipo.Caracter)
                    {
                        if (varExpr.Tipo == VariávelTipo.Lógico)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo caracter não pode receber a variável '{varExpr.Identificador}' do tipo lógico!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            continue;
                        }

                        if (varExpr.Tipo == VariávelTipo.Inteiro)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo caracter não pode receber a variável '{varExpr.Identificador}' do tipo inteiro!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            continue;
                        }

                        if (varExpr.Tipo == VariávelTipo.Real)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo caracter não pode receber a variável '{varExpr.Identificador}' do tipo real!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            continue;
                        }
                    }

                    debug.Items.Add($"> {varExpr.ToString()}");
                }
                else if (itemExpr.Token.Equals("CONST_LOGICA"))
                {
                    if (tipoResultado == VariávelTipo.Inteiro)
                    {
                        listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo inteiro não pode receber a constante '{itemExpr.Lexema}' do tipo lógico!"));
                    }
                    else if (tipoResultado == VariávelTipo.Real)
                    {
                        listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo real não pode receber a constante '{itemExpr.Lexema}' do tipo lógico!"));
                    }
                    else if (tipoResultado == VariávelTipo.Caracter)
                    {
                        listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo caracter não pode receber a constante '{itemExpr.Lexema}' do tipo lógico!"));
                    }

                    debug.Items.Add($"X > {itemExpr.ToString()}");
                    continue;
                }
                else if (itemExpr.Token.Equals("CONST_TEXTO"))
                {
                    if (tipoResultado == VariávelTipo.Inteiro)
                    {
                        listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo inteiro não pode receber a constante '{itemExpr.Lexema}' do tipo caracter!"));
                        debug.Items.Add($"X > {itemExpr.ToString()}");
                        continue;
                    }
                    else if (tipoResultado == VariávelTipo.Real)
                    {
                        listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo real não pode receber a constante '{itemExpr.Lexema}' do tipo caracter!"));
                        debug.Items.Add($"X > {itemExpr.ToString()}");
                        continue;
                    }

                    debug.Items.Add($"> {itemExpr.Lexema} : {itemExpr.Token}");
                }
                else if (itemExpr.Token.Equals("CONST_INT"))
                {
                    if (tipoResultado == VariávelTipo.Caracter)
                    {
                        listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo caracter não pode receber a variável '{itemExpr.Lexema}' do tipo inteiro!"));
                        debug.Items.Add($"X > {itemExpr.ToString()}");
                        continue;
                    }

                    debug.Items.Add($"> {itemExpr.Lexema} : {itemExpr.Token}");
                }
                else if (itemExpr.Token.Equals("CONST_REAL"))
                {
                    if (tipoResultado == VariávelTipo.Inteiro)
                    {
                        listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo inteiro não pode receber uma constante '{itemExpr.Lexema}' do tipo real!"));
                        debug.Items.Add($"X > {itemExpr.ToString()}");
                        continue;
                    }
                    else if (tipoResultado == VariávelTipo.Caracter)
                    {
                        listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo caracter não pode receber uma constante '{itemExpr.Lexema}' do tipo real!"));
                        debug.Items.Add($"X > {itemExpr.ToString()}");
                        continue;
                    }

                    debug.Items.Add($"> {itemExpr.Lexema} : {itemExpr.Token}");
                }
                else if (itemExpr.Token.Equals("MOD") || itemExpr.Token.Equals("DIV"))
                {
                    if (tipoResultado == VariávelTipo.Real)
                    {
                        listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo real não pode utilizar o operadores MOD ou DIV!"));
                        debug.Items.Add($"X > {itemExpr.ToString()}");
                        continue;
                    }

                    debug.Items.Add($"> {itemExpr.Lexema} : {itemExpr.Token}");
                }
                else if (itemExpr.Token.Equals("E") || itemExpr.Token.Equals("OU"))
                {
                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma atribuição matemática não pode utilizar o operadores lógicos E ou OU!"));
                    debug.Items.Add($"X > {itemExpr.ToString()}");
                }
                else if (itemExpr.Token.Equals("MAIOR") || itemExpr.Token.Equals("MENOR") || itemExpr.Token.Equals("MAIOR_IGUAL") || itemExpr.Token.Equals("MENOR_IGUAL") || itemExpr.Token.Equals("IGUAL") || itemExpr.Token.Equals("DIFERENTE"))
                {
                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma atribuição matemática não pode utilizar o operadores lógicos relacionais!"));
                    debug.Items.Add($"X > {itemExpr.ToString()}");
                }
            }
        }

        #endregion

        #region Geração de Código MIPS Assembly

        private static string[] memRegSP = new string[2];

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para expressões aritméticas.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        /// <param name="tipoResultado">Tipo de resultado esperado da expressão.</param>
        private void GerarMipsAritmético(MipsClass mips, VariávelTipo tipoResultado)
        {
            // Inicia o primeiro elemento no registrador de resultado:
            int r = this.GerarMipsMoverValores(mips, tipoResultado, 0, 0, "$t0", "$f4", memRegSP);

            // Agora adiciona os demais elementos:
            int i = r + 1;
            while (i < this.Elementos.Length)
            {
                if (this.Elementos[i].Token.Equals("ASTERISTICO") || this.Elementos[i].Token.Equals("BARRA"))
                {
                    r = this.GerarMipsMoverValores(mips, tipoResultado, i, 1, "$t1", "$f5", memRegSP);
                    this.GerarMipsAdicionarOperação(mips, tipoResultado, this.Elementos[i], "$t0", "$t1", "$f4", "$f5");
                    i = r + 2;
                    continue;
                }

                r = this.GerarMipsMoverValores(mips, tipoResultado, i, 1, "$t1", "$f5", memRegSP);

                if (i < this.Elementos.Length - 3)
                {
                    if (this.Elementos[i + 2].Token.Equals("ASTERISTICO") || this.Elementos[i + 2].Token.Equals("BARRA"))
                    {
                        r = this.GerarMipsMoverValores(mips, tipoResultado, i, 3, "$t2", "$f6", memRegSP);
                        this.GerarMipsAdicionarOperação(mips, tipoResultado, this.Elementos[i + 2], "$t1", "$t2", "$f5", "$f6");
                        this.GerarMipsAdicionarOperação(mips, tipoResultado, this.Elementos[i], "$t0", "$t1", "$f4", "$f5");
                        i = r + 4;
                        continue;
                    }
                }

                this.GerarMipsAdicionarOperação(mips, tipoResultado, this.Elementos[i], "$t0", "$t1", "$f4", "$f5");
                i = r + 2;
            }
        }

        /// <summary>
        /// Move o valor de uma variável ou constante, para o registrador especificado.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        /// <param name="tipoResultado">Tipo de resultado da expressão.</param>
        /// <param name="i">Indice do elemento atual analisado na expressão.</param>
        /// <param name="offset">Offset de indice do elemento atual analisado na expressão.</param>
        /// <param name="regInteiro">Registrador para receber o valor inteiro.</param>
        /// <param name="regReal">Registrador para receber o valor real.</param>
        /// <param name="memRegSP">Memória de uso dos registradores.</param>
        /// <returns>O novo indice do elemento atual a ser analisado na expressão.</returns>
        private int GerarMipsMoverValores(MipsClass mips, VariávelTipo tipoResultado, int i, int offset, string regInteiro, string regReal, string[] memRegSP = null)
        {
            SemanticoToken elemSem = this.Elementos[i + offset];

            if (elemSem.Token.Equals("CONST_LOGICA"))
            {
                if (tipoResultado != VariávelTipo.Lógico)
                    throw new Exception($"Uma constante lógica não pode ser utilizada utilizada em uma operação '{tipoResultado}'!");

                // Carrega o valor da constante lógica no registrador inteiro.
                mips.SectionText.Adicionar(new MipsText("li", $"{regInteiro},{(elemSem.Lexema.Equals("falso") ? 0 : 1)}"));
            }
            else if (elemSem.Token.Equals("CONST_INT"))
            {
                if (tipoResultado == VariávelTipo.Inteiro)
                {
                    mips.SectionText.Adicionar(new MipsText("li", $"{regInteiro},{elemSem.Lexema}"));  // Carrega o valor da constante no registrador inteiro.

                    this.ControleMemoriaSP(mips, offset, regInteiro, memRegSP);
                }
                else if (tipoResultado == VariávelTipo.Real)
                {
                    /*mips.SectionText.Adicionar(new MipsText("li", $"{regInteiro},{elemSem.Lexema}"));  // Carrega o valor da constante no registrador inteiro.
                    mips.SectionText.Adicionar(new MipsText("mtc1", $"{regInteiro}, {regReal}"));
                    mips.SectionText.Adicionar(new MipsText("cvt.s.w", $"{regReal}, {regReal}"));*/

                    /* Não funciona, Runtime exception at 0x00400094: fetch address not aligned on word boundary 0x0000000a
                    mips.SectionText.Adicionar(new MipsText("lwc1", $"{regReal},{elemSem.Lexema}")); // Carrega o valor da constante no registrador de ponto flutuante.
                    mips.SectionText.Adicionar(new MipsText("cvt.s.w", $"{regReal}, {regReal}"));*/

                    // Adiciona o valor da constante real como uma constante no segmento de dados.
                    mips.SectionData.Adicionar(new MipsDataFloat(elemSem.ObterEtiqueta(), new float[] { float.Parse(elemSem.Lexema, CultureInfo.InvariantCulture) }));
                    
                    mips.SectionText.Adicionar(new MipsText("lwc1", $"{regReal},{elemSem.ObterEtiqueta()}")); // Carrega o valor da constante no registrador de ponto flutuante.

                    this.ControleMemoriaSP(mips, offset, regReal, memRegSP);
                }
            }
            else if (elemSem.Token.Equals("CONST_REAL"))
            {
                // Adiciona o valor da constante real como uma constante no segmento de dados.
                mips.SectionData.Adicionar(new MipsDataFloat(elemSem.ObterEtiqueta(), new float[] { float.Parse(elemSem.Lexema, CultureInfo.InvariantCulture) }));

                //mips.SectionText.Adicionar(new MipsText("l.s", $"{regReal},{elemSem.ObterEtiqueta()}")); // Carrega o valor da constante no registrador de ponto flutuante.
                mips.SectionText.Adicionar(new MipsText("lwc1", $"{regReal},{elemSem.ObterEtiqueta()}")); // Carrega o valor da constante no registrador de ponto flutuante.

                this.ControleMemoriaSP(mips, offset, regReal, memRegSP);
            }
            else if (elemSem.Token.Equals("ID"))
            {
                if (elemSem.Variável.TipoDeclaração == VariávelDeclaração.Vetor)
                {
                    DeclaraçãoVetor decVet = (DeclaraçãoVetor)elemSem.Variável.Declaração;  // Armazena a declaração do vetor.
                    decVet.GerarMipsValorIndex(mips, this.Elementos, ref i, offset);        // Posiciona o registro do vetor.

                    if (elemSem.Variável.Tipo == VariávelTipo.Lógico)
                    {
                        mips.HandlerLogico.ObterVetorValor($"{regInteiro}", elemSem.ObterEtiqueta());
                    }
                    else if (elemSem.Variável.Tipo == VariávelTipo.Inteiro)
                    {
                        mips.SectionText.Adicionar(new MipsText("lw", $"{regInteiro},($t5)"));  // Carrega o valor no registro do array para o registrador

                        if (tipoResultado == VariávelTipo.Real)
                        {
                            mips.SectionText.Adicionar(new MipsText("mtc1", $"{regInteiro},{regReal}"));  // Move o valor do registrador temporário $t para o registrador de ponto flutuante
                            mips.SectionText.Adicionar(new MipsText("cvt.s.w", $"{regReal},{regReal}"));  // Converte o valor armazenado no registrador $f de inteiro para ponto flutuante

                            this.ControleMemoriaSP(mips, offset, regReal, memRegSP);
                        }
                        else
                        {
                            this.ControleMemoriaSP(mips, offset, regInteiro, memRegSP);
                        }
                    }
                    else if (elemSem.Variável.Tipo == VariávelTipo.Real)
                    {
                        mips.SectionText.Adicionar(new MipsText("lwc1", $"{regReal},($t5)"));  // Carrega o valor no registro do array para o registrador

                        this.ControleMemoriaSP(mips, offset, regReal, memRegSP);
                    }

                    return i;
                }
                else if (elemSem.Variável.TipoDeclaração == VariávelDeclaração.Função)
                {
                    Função decFunc = (Função)elemSem.Variável.Declaração;           // Armazena a declaração da função.
                    decFunc.GerarMipsChamada(mips, this.Elementos, ref i, offset);  // Passa os argumetos para os parâmetros da função.
                    
                    if (elemSem.Variável.Tipo == VariávelTipo.Lógico)
                    {
                        mips.SectionText.Adicionar(new MipsText("move", $"{regInteiro},$s0"));  // Carrega o valor do retorno da função para o registrador
                    }
                    else if (elemSem.Variável.Tipo == VariávelTipo.Inteiro)
                    {
                        mips.SectionText.Adicionar(new MipsText("move", $"{regInteiro},$s0"));  // Carrega o valor do retorno da função para o registrador

                        if (tipoResultado == VariávelTipo.Real)
                        {
                            mips.SectionText.Adicionar(new MipsText("mtc1", $"{regInteiro},{regReal}"));  // Move o valor do registrador temporário $t para o registrador de ponto flutuante
                            mips.SectionText.Adicionar(new MipsText("cvt.s.w", $"{regReal},{regReal}"));  // Converte o valor armazenado no registrador $f de inteiro para ponto flutuante

                            this.ControleMemoriaSP(mips, offset, regReal, memRegSP);
                        }
                        else
                        {
                            this.ControleMemoriaSP(mips, offset, regInteiro, memRegSP);
                        }
                    }
                    else if (elemSem.Variável.Tipo == VariávelTipo.Real)
                    {
                        mips.SectionText.Adicionar(new MipsText("mov.s", $"{regReal},$f20"));  // Carrega o valor do retorno da função para o registrador de ponto flutuante

                        this.ControleMemoriaSP(mips, offset, regReal, memRegSP);
                    }

                    mips.SectionText.Adicionar(MipsText.Blank);

                    return i;
                }

                if (elemSem.Variável.Tipo == VariávelTipo.Lógico)
                {
                    mips.HandlerLogico.ObterVariável(regInteiro, elemSem.ObterEtiqueta());
                }
                else if (elemSem.Variável.Tipo == VariávelTipo.Inteiro)
                {
                    mips.SectionText.Adicionar(new MipsText("lw", $"{regInteiro},{elemSem.ObterEtiqueta()}"));  // Carrega o valor da variável no registrador inteiro

                    if (tipoResultado == VariávelTipo.Real)
                    {
                        mips.SectionText.Adicionar(new MipsText("mtc1", $"{regInteiro},{regReal}"));  // Move o valor do registrador temporário $t3 para o registrador de ponto flutuante
                        mips.SectionText.Adicionar(new MipsText("cvt.s.w", $"{regReal},{regReal}"));  // Converte o valor armazenado no registrador $f0 de inteiro para ponto flutuante

                        this.ControleMemoriaSP(mips, offset, regReal, memRegSP);
                    }
                    else
                    {
                        this.ControleMemoriaSP(mips, offset, regInteiro, memRegSP);
                    }
                }
                else if (elemSem.Variável.Tipo == VariávelTipo.Real)
                {
                    mips.SectionText.Adicionar(new MipsText("lwc1", $"{regReal},{elemSem.ObterEtiqueta()}"));  // Carrega o valor da variável no registrador $f0

                    this.ControleMemoriaSP(mips, offset, regReal, memRegSP);
                }
            }
            else if (elemSem.Token.Equals("ABRE_PAR"))
            {
                if (elemSem.Variável.Tipo == VariávelTipo.Lógico)
                {
                    mips.HandlerLogico.ObterVariável(regInteiro, elemSem.ObterEtiqueta());
                }
                else if (elemSem.Variável.Tipo == VariávelTipo.Inteiro)
                {
                    mips.SectionText.Adicionar(new MipsText("lw", $"{regInteiro},{elemSem.ObterEtiqueta()}"));  // Carrega o valor da variável no registrador $t3

                    if (tipoResultado == VariávelTipo.Real)
                    {
                        mips.SectionText.Adicionar(new MipsText("mtc1", $"{regInteiro},{regReal}"));  // Move o valor do registrador temporário $t3 para o registrador de ponto flutuante
                        mips.SectionText.Adicionar(new MipsText("cvt.s.w", $"{regReal},{regReal}"));  // Converte o valor armazenado no registrador $f0 de inteiro para ponto flutuante

                        this.ControleMemoriaSP(mips, offset, regReal, memRegSP);
                    }
                    else
                    {
                        this.ControleMemoriaSP(mips, offset, regInteiro, memRegSP);
                    }
                }
                else if (elemSem.Variável.Tipo == VariávelTipo.Real)
                {
                    mips.SectionText.Adicionar(new MipsText("lwc1", $"{regReal},{elemSem.ObterEtiqueta()}"));  // Carrega o valor da variável no registrador de ponto flutuante

                    this.ControleMemoriaSP(mips, offset, regReal, memRegSP);
                }
            }
            else if (elemSem.Token.Equals("NAO"))
            {
                if (this.Elementos.Length == 2)
                {
                    mips.HandlerLogico.ObterVariável($"{regInteiro}", this.Elementos[1].ObterEtiqueta());
                    mips.SectionText.Adicionar(new MipsText("xori", $"{regInteiro},{regInteiro},0x01"));
                    return i + 1;
                }
                
                throw new NotImplementedException("Erro durante a geração de código do comando lógico 'não', a geração resultou em muitos elementos.");
            }
            else
            {
                if (elemSem.Variável == null)
                    throw new NotImplementedException($"O elemento '{elemSem.Lexema}' não é reconhecido como uma variável para a geração de código.");

                throw new NotImplementedException($"Não é suportada a geração de código para variáveis do tipo '{elemSem.Variável.Tipo}'.");
            }

            return i;
        }

        /// <summary>
        /// Controla a memória de uso dos registradores utilizados durante a geração da expressão.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        /// <param name="offset">Offset de indice do elemento atual analisado na expressão.</param>
        /// <param name="registrador">Registrador utilizado.</param>
        /// <param name="memRegSP">Memória de uso dos registradores.</param>
        private void ControleMemoriaSP(MipsClass mips, int offset, string registrador, string[] memRegSP)
        {
            if (memRegSP == null)
                return;

            if (offset == 0)
            {
                mips.HandlerStackPointer.Remover(memRegSP[0]);
                mips.HandlerStackPointer.Remover(memRegSP[1]);
                mips.HandlerStackPointer.Adicionar(registrador);

                memRegSP[0] = registrador;
                memRegSP[1] = null;
            }

            if (offset == 1)
            {
                mips.HandlerStackPointer.Remover(memRegSP[1]);
                mips.HandlerStackPointer.Adicionar(registrador);

                memRegSP[1] = registrador;
            }
        }

        /// <summary>
        /// Adiciona a instrução MIPS relativo ao token de operador especificado.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        /// <param name="tipoResultado">Tipo de resultado esperado da expressão.</param>
        /// <param name="elemSem">Token contendo o operador.</param>
        /// <param name="regInteiro1">Registrador inteiro da esquerda na operação.</param>
        /// <param name="regInteiro2">Registrador inteiro da direita na operação.</param>
        /// <param name="regReal1">Registrador ponto flutuante da esquerda na operação.</param>
        /// <param name="regReal2">Registrador ponto flutuante da direita na operação.</param>
        private void GerarMipsAdicionarOperação(MipsClass mips, VariávelTipo tipoResultado, SemanticoToken elemSem, string regInteiro1, string regInteiro2, string regReal1, string regReal2)
        {
            if (tipoResultado == VariávelTipo.Inteiro)
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
                    case "DIV":
                        mips.SectionText.Adicionar(new MipsText("div", $"{regInteiro1},{regInteiro1},{regInteiro2}"));
                        break;

                    case "MOD":
                        mips.SectionText.Adicionar(new MipsText("div", $"{regInteiro1},{regInteiro1},{regInteiro2}"));
                        mips.SectionText.Adicionar(new MipsText("mfhi", regInteiro1));
                        break;

                    default:
                        throw new NotImplementedException($"Não é suportada a geração de código para operações do tipo '{elemSem.Lexema}'.");
                }
            }
            else if (tipoResultado == VariávelTipo.Real)
            {
                switch (elemSem.Token)
                {
                    case "MAIS":
                        mips.SectionText.Adicionar(new MipsText("add.s", $"{regReal1},{regReal1},{regReal2}"));
                        break;

                    case "MENOS":
                        mips.SectionText.Adicionar(new MipsText("sub.s", $"{regReal1},{regReal1},{regReal2}"));
                        break;

                    case "ASTERISTICO":
                        mips.SectionText.Adicionar(new MipsText("mul.s", $"{regReal1},{regReal1},{regReal2}"));
                        break;

                    case "BARRA":
                        mips.SectionText.Adicionar(new MipsText("div.s", $"{regReal1},{regReal1},{regReal2}"));
                        break;

                    default:
                        throw new NotImplementedException($"Não é suportada a geração de código para operações do tipo '{elemSem.Lexema}'.");
                }
            }
            else
            {
                throw new NotImplementedException($"Não é suportada a geração de código para variáveis do tipo '{elemSem.Variável.Tipo}'.");
            }
        }

        #endregion
    }
}
