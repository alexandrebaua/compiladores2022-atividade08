using System;
using System.Collections.Generic;
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
        /// Executa a validação semântica de uma expressão lógica.
        /// </summary>
        /// <param name="listaVariáveisGlobal">A lista de variáveis de escopo global.</param>
        /// <param name="listaVariáveisLocal">A lista de variáveis de escopo local.</param>
        /// <param name="listaErrors">A lista de erros encontrados durante a análise.</param>
        /// <param name="debug">Saida de debugação.</param>
        public void ValidarExpressãoLógica(List<Variável> listaVariáveisGlobal, List<Variável> listaVariáveisLocal, List<SemanticoError> listaErrors, ListBox debug)
        {
            int errosInicio = listaErrors.Count;
            int tipoOp = 0;

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
                        ((DeclaraçãoVetor)varExpr.Declaração).ValidarIndice(ref i, this.Elementos, listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);

                        if (tipoOp == 0)
                        {
                            // Se não existe mais itens na expressão, então:
                            if (i + 1 >= this.Elementos.Length)
                            {
                                // Se o tipo do vetor não for lógico, então erro:
                                if (varExpr.Tipo == VariávelTipo.Inteiro)
                                {
                                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo lógico não pode receber o valor do vetor '{itemExpr.Lexema}' do tipo inteiro!"));
                                    debug.Items.Add($"X > {itemExpr.ToString()}");
                                }

                                if (varExpr.Tipo == VariávelTipo.Real)
                                {
                                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo lógico não pode receber o valor do vetor '{itemExpr.Lexema}' do tipo real!"));
                                    debug.Items.Add($"X > {itemExpr.ToString()}");
                                }

                                if (varExpr.Tipo == VariávelTipo.Caracter)
                                {
                                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo lógico não pode receber o valor do vetor '{itemExpr.Lexema}' do tipo caracter!"));
                                    debug.Items.Add($"X > {itemExpr.ToString()}");
                                }
                            }
                        }

                        if (tipoOp == 0 || tipoOp == 4)
                        {
                            if (varExpr.Tipo == VariávelTipo.Lógico)
                                tipoOp = -1;
                        }

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

                        if (tipoOp == 0)
                        {
                            // Se não existe mais itens na expressão, então:
                            if (i + 1 >= this.Elementos.Length)
                            {
                                // Se o tipo do retorno da função não for lógico, então erro:
                                if (varExpr.Tipo == VariávelTipo.Inteiro)
                                {
                                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo lógico não pode receber o retorno de uma função do tipo inteiro!"));
                                    debug.Items.Add($"X > {itemExpr.ToString()}");
                                }

                                if (varExpr.Tipo == VariávelTipo.Real)
                                {
                                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo lógico não pode receber o retorno de uma função do tipo real!"));
                                    debug.Items.Add($"X > {itemExpr.ToString()}");
                                }

                                if (varExpr.Tipo == VariávelTipo.Caracter)
                                {
                                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo lógico não pode receber o retorno de uma função do tipo caracter!"));
                                    debug.Items.Add($"X > {itemExpr.ToString()}");
                                }
                            }
                        }

                        if (tipoOp == 0 || tipoOp == 4)
                        {
                            if (varExpr.Tipo == VariávelTipo.Lógico)
                                tipoOp = -1;
                        }

                        varExpr.Utilizado = true;
                        continue;
                    }

                    if (tipoOp == 0)
                    {
                        // Se não existe mais itens na expressão, então:
                        if (i + 1 >= this.Elementos.Length)
                        {
                            // Se o tipo do identificador não for lógico, então erro:
                            if (varExpr.Tipo == VariávelTipo.Inteiro)
                            {
                                listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo lógico não pode receber o retorno da função '{varExpr.Identificador}' do tipo inteiro!"));
                                debug.Items.Add($"X > {itemExpr.ToString()}");
                            }

                            if (varExpr.Tipo == VariávelTipo.Real)
                            {
                                listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo lógico não pode receber o retorno da função '{varExpr.Identificador}' do tipo real!"));
                                debug.Items.Add($"X > {itemExpr.ToString()}");
                            }

                            if (varExpr.Tipo == VariávelTipo.Caracter)
                            {
                                listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo lógico não pode receber o retorno da função '{varExpr.Identificador}' do tipo caracter!"));
                                debug.Items.Add($"X > {itemExpr.ToString()}");
                            }
                        }

                    }
                    else if (tipoOp == 1 || tipoOp == 2 || tipoOp == 3 || tipoOp == 5 || tipoOp == 6 || tipoOp == 7)
                    {
                        // Se durante uma operação matemática, o tipo do identificador for lógico, então erro:
                        if (varExpr.Tipo == VariávelTipo.Lógico)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo lógico não pode ser utilizada em uma operação matemática!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                        }
                    }

                    if (tipoOp == 0 || tipoOp == 4)
                    {
                        // Se for uma variável lógica, então espera um operador lógico:
                        if (varExpr.Tipo == VariávelTipo.Lógico)
                            tipoOp = -1;
                    }

                    if (!varExpr.ValorAtribuido)
                    {
                        listaErrors.Add(new SemanticoError(itemExpr, "A variável usada não possui valor atribuido!"));
                        debug.Items.Add($"X > {itemExpr.ToString()}");
                    }
                    
                    debug.Items.Add($"> {varExpr.ToString()}");
                }
                else if (itemExpr.Token.Equals("MENOS") || itemExpr.Token.Equals("MAIS") || itemExpr.Token.Equals("ASTERISTICO") || itemExpr.Token.Equals("BARRA") || itemExpr.Token.Equals("MOD") || itemExpr.Token.Equals("DIV"))
                {
                    if (tipoOp == 0 || tipoOp == 1)
                    {
                        tipoOp = 1;
                    }
                    else if (tipoOp == 2 || tipoOp == 3)
                    {
                        tipoOp = 3;
                    }
                    else if (tipoOp == 4 || tipoOp == 5)
                    {
                        tipoOp = 5;
                    }
                    else if (tipoOp == 6 || tipoOp == 7)
                    {
                        tipoOp = 7;
                    }
                    else
                    {
                        listaErrors.Add(new SemanticoError(itemExpr, "A atribuição para a variável lógica não pode ser validada! Era esperado um operador lógico (E, OU), ou o final da atribuição..."));
                        debug.Items.Add($"X > {itemExpr.ToString()}");
                        break; ;
                    }

                    debug.Items.Add($"> {itemExpr.Lexema} : {itemExpr.Token}");
                }
                else if (itemExpr.Token.Equals("MAIOR") || itemExpr.Token.Equals("MENOR") || itemExpr.Token.Equals("MAIOR_IGUAL") || itemExpr.Token.Equals("MENOR_IGUAL") || itemExpr.Token.Equals("IGUAL") || itemExpr.Token.Equals("DIFERENTE"))
                {
                    if (tipoOp == 0 || tipoOp == 1)
                    {
                        tipoOp = 2;
                    }
                    else if (tipoOp == 4 || tipoOp == 5)
                    {
                        tipoOp = 6;
                    }
                    else
                    {
                        listaErrors.Add(new SemanticoError(itemExpr, "A atribuição para a variável lógica não pode ser validada! Era esperado um operador relacional (>, <, >=, <=, =, <>), ou um operador aritmético (+, -, *, /)..."));
                        debug.Items.Add($"X > {itemExpr.ToString()}");
                        break;
                    }
                    
                    debug.Items.Add($"> {itemExpr.Lexema} : {itemExpr.Token}");
                }
                else if (itemExpr.Token.Equals("E") || itemExpr.Token.Equals("OU"))
                {
                    if (tipoOp == -1 || tipoOp == 2 || tipoOp == 3 || tipoOp == 6 || tipoOp == 7)
                    {
                        tipoOp = 4;
                    }
                    else
                    {
                        listaErrors.Add(new SemanticoError(itemExpr, "A atribuição para a variável lógica não pode ser validada! Era esperado um operador relacional (>, <, >=, <=, =, <>), ou um operador aritmético (+, -, *, /)..."));
                        debug.Items.Add($"X > {itemExpr.ToString()}");
                        break;
                    }
                    
                    debug.Items.Add($"> {itemExpr.Lexema} : {itemExpr.Token}");
                }
                else if (itemExpr.Token.Equals("CONST_INT") || itemExpr.Token.Equals("CONST_REAL"))
                {
                    debug.Items.Add($"> {itemExpr.Lexema} : {itemExpr.Token}");
                }
                else if (itemExpr.Token.Equals("CONST_LOGICA"))
                {
                    // TODO: Testes se pode ser uma constante lógica nesse ponto...
                    tipoOp = -1;

                    debug.Items.Add($"> {itemExpr.Lexema} : {itemExpr.Token}");
                }
                else if (itemExpr.Token.Equals("CONST_TEXTO"))
                {
                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma operação do tipo lógico não pode receber uma constante '{itemExpr.Lexema}' do tipo caracter!"));
                    debug.Items.Add($"X > {itemExpr.ToString()}");
                }
            }
            
            if (listaErrors.Count == errosInicio && (tipoOp == 0 || tipoOp == 1 || tipoOp == 4 || tipoOp == 5))
            {
                int comprimento = this.Elementos.Last().Index + this.Elementos.Last().Lexema.Length - this.Elementos[0].Index;

                string strExpr = String.Empty;
                foreach (var item in this.Elementos)
                    strExpr += $"{item.Lexema} ";

                listaErrors.Add(new SemanticoError(strExpr.Trim(), this.Elementos[0].Linha, this.Elementos[0].Inicio, this.Elementos[0].Index, comprimento, "A expressão lógica não pode ser validada! O resultado da não gera um valor lógico..."));
            }
        }

        #endregion

        #region Geração de Código MIPS Assembly

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para expressões lógicas.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        private void GerarMipsLógico(MipsClass mips)
        {
            VariávelTipo tipoResultado = VariávelTipo.Lógico;
            VariávelTipo tipoResultadoAnterior = tipoResultado;
            SemanticoToken opLogSem = null, opRelSem = null;
            bool aritIni = false, logIni = false;

            // Percorre os elementos da expressão lógica:
            int i = 0;
            while (i < this.Elementos.Length)
            {
                tipoResultadoAnterior = tipoResultado;
                i = this.GerarMipsControleExpressãoLógica(mips, i, ref aritIni, ref logIni, ref tipoResultado);
                if (i >= this.Elementos.Length)
                    break;

                if (this.Elementos[i].Token.Equals("E") || this.Elementos[i].Token.Equals("OU"))
                {
                    tipoResultado = VariávelTipo.Lógico;

                    if (opRelSem != null)
                    {
                        this.GerarMipsAdicionarOperadorRelacional(mips, opRelSem, tipoResultadoAnterior, tipoResultado);
                        opRelSem = null;
                        aritIni = false;

                        if (!logIni)
                        {
                            mips.SectionText.Adicionar(new MipsText("move", "$t4,$t0"));
                            logIni = true;
                        }
                    }

                    if (opLogSem != null)
                    {
                        this.GerarMipsAdicionarOperadorLógico(mips, opLogSem);
                        opLogSem = null;
                    }

                    opLogSem = this.Elementos[i];
                    i++;
                    continue;
                }

                if (this.Elementos[i].Token.Equals("MAIOR") || this.Elementos[i].Token.Equals("MENOR") || this.Elementos[i].Token.Equals("MAIOR_IGUAL") ||
                    this.Elementos[i].Token.Equals("MENOR_IGUAL") || this.Elementos[i].Token.Equals("IGUAL") || this.Elementos[i].Token.Equals("DIFERENTE"))
                {
                    opRelSem = this.Elementos[i];
                    i++;
                    continue;
                }
            }

            if (opRelSem != null)
                this.GerarMipsAdicionarOperadorRelacional(mips, opRelSem, tipoResultadoAnterior, tipoResultado);

            if (opLogSem != null)
                this.GerarMipsAdicionarOperadorLógico(mips, opLogSem);

            if (!logIni)
                mips.SectionText.Adicionar(new MipsText("move", "$t4,$t0"));
        }

        /// <summary>
        /// Adiciona o operador lógico em uma expressão lógica.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        /// <param name="elemSem">O elemento contendo o operador lógido.</param>
        private void GerarMipsAdicionarOperadorLógico(MipsClass mips, SemanticoToken elemSem)
        {
            switch (elemSem.Token)
            {
                case "E":
                    mips.SectionText.Adicionar(new MipsText("and", "$t4,$t4,$t0"));
                    break;

                case "OU":
                    mips.SectionText.Adicionar(new MipsText("or", "$t4,$t4,$t0"));
                    break;
                    
                default:
                    throw new NotImplementedException($"Operador lógico '{elemSem.Lexema}' não reconhecido!");
            }
        }

        /// <summary>
        /// Adiciona o operador relacional em uma expressão lógica.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        /// <param name="elemSem">O elemento contendo o operador relacional.</param>
        /// <param name="tipoResultadoEsquerda">Tipo do valor à esquerda do operador relacional.</param>
        /// <param name="tipoResultadoDireita">Tipo do valor à direita do operador relacional.</param>
        private void GerarMipsAdicionarOperadorRelacional(MipsClass mips, SemanticoToken elemSem, VariávelTipo tipoResultadoEsquerda, VariávelTipo tipoResultadoDireita)
        {
            // Se for uma operação relacional entre valores de ponto flutuante, então:
            if (tipoResultadoEsquerda == VariávelTipo.Real || tipoResultadoDireita == VariávelTipo.Real)
            {
                if (tipoResultadoEsquerda == VariávelTipo.Inteiro)
                {
                    mips.SectionText.Adicionar(new MipsText("mtc1", "$t3,$f7"));     // Move o valor do registrador temporário $t3 para o registrador de ponto flutuante $f7
                    mips.SectionText.Adicionar(new MipsText("cvt.s.w", "$f7,$f7"));  // Converte o valor armazenado no registrador $f7 de inteiro para ponto flutuante
                }
                else if (tipoResultadoDireita == VariávelTipo.Inteiro)
                {
                    mips.SectionText.Adicionar(new MipsText("mtc1", "$t0,$f4"));     // Move o valor do registrador temporário $t0 para o registrador de ponto flutuante $f4
                    mips.SectionText.Adicionar(new MipsText("cvt.s.w", "$f4,$f4"));  // Converte o valor armazenado no registrador $f4 de inteiro para ponto flutuante
                }

                // Senão, operação relacional entre valores inteiros:
                switch (elemSem.Token)
                {
                    case "MAIOR":
                        mips.SectionText.Adicionar(new MipsText("c.le.s", "$f7,$f4"));
                        mips.SectionText.Adicionar(new MipsText("bc1f", $"jumpFalse_{elemSem.Index}"));
                        mips.SectionText.Adicionar(new MipsText("li", "$t0,0"));
                        mips.SectionText.Adicionar(new MipsText("j", $"jump_{elemSem.Index}"));
                        mips.SectionText.Adicionar(new MipsText($"jumpFalse_{elemSem.Index}:", null));
                        mips.SectionText.Adicionar(new MipsText("li", "$t0,1"));
                        mips.SectionText.Adicionar(new MipsText($"jump_{elemSem.Index}:", null));
                        break;

                    case "MENOR":
                        mips.SectionText.Adicionar(new MipsText("c.lt.s", "$f7,$f4"));
                        mips.SectionText.Adicionar(new MipsText("bc1t", $"jumpTrue_{elemSem.Index}"));
                        mips.SectionText.Adicionar(new MipsText("li", "$t0,0"));
                        mips.SectionText.Adicionar(new MipsText("j", $"jump_{elemSem.Index}"));
                        mips.SectionText.Adicionar(new MipsText($"jumpTrue_{elemSem.Index}:", null));
                        mips.SectionText.Adicionar(new MipsText("li", "$t0,1"));
                        mips.SectionText.Adicionar(new MipsText($"jump_{elemSem.Index}:", null));
                        break;

                    case "MAIOR_IGUAL":
                        mips.SectionText.Adicionar(new MipsText("c.lt.s", "$f7,$f4"));
                        mips.SectionText.Adicionar(new MipsText("bc1f", $"jumpFalse_{elemSem.Index}"));
                        mips.SectionText.Adicionar(new MipsText("li", "$t0,0"));
                        mips.SectionText.Adicionar(new MipsText("j", $"jump_{elemSem.Index}"));
                        mips.SectionText.Adicionar(new MipsText($"jumpFalse_{elemSem.Index}:", null));
                        mips.SectionText.Adicionar(new MipsText("li", "$t0,1"));
                        mips.SectionText.Adicionar(new MipsText($"jump_{elemSem.Index}:", null));
                        break;

                    case "MENOR_IGUAL":
                        mips.SectionText.Adicionar(new MipsText("c.le.s", "$f7,$f4"));
                        mips.SectionText.Adicionar(new MipsText("bc1t", $"jumpTrue_{elemSem.Index}"));
                        mips.SectionText.Adicionar(new MipsText("li", "$t0,0"));
                        mips.SectionText.Adicionar(new MipsText("j", $"jump_{elemSem.Index}"));
                        mips.SectionText.Adicionar(new MipsText($"jumpTrue_{elemSem.Index}:", null));
                        mips.SectionText.Adicionar(new MipsText("li", "$t0,1"));
                        mips.SectionText.Adicionar(new MipsText($"jump_{elemSem.Index}:", null));
                        break;

                    case "IGUAL":
                        mips.SectionText.Adicionar(new MipsText("c.eq.s", "$f7,$f4"));
                        mips.SectionText.Adicionar(new MipsText("bc1t", $"jumpTrue_{elemSem.Index}"));
                        mips.SectionText.Adicionar(new MipsText("li", "$t0,0"));
                        mips.SectionText.Adicionar(new MipsText("j", $"jump_{elemSem.Index}"));
                        mips.SectionText.Adicionar(new MipsText($"jumpTrue_{elemSem.Index}:", null));
                        mips.SectionText.Adicionar(new MipsText("li", "$t0,1"));
                        mips.SectionText.Adicionar(new MipsText($"jump_{elemSem.Index}:", null));
                        break;

                    case "DIFERENTE":
                        mips.SectionText.Adicionar(new MipsText("c.eq.s", "$f7,$f4"));
                        mips.SectionText.Adicionar(new MipsText("bc1f", $"jumpFalse_{elemSem.Index}"));
                        mips.SectionText.Adicionar(new MipsText("li", "$t0,0"));
                        mips.SectionText.Adicionar(new MipsText("j", $"jump_{elemSem.Index}"));
                        mips.SectionText.Adicionar(new MipsText($"jumpFalse_{elemSem.Index}:", null));
                        mips.SectionText.Adicionar(new MipsText("li", "$t0,1"));
                        mips.SectionText.Adicionar(new MipsText($"jump_{elemSem.Index}:", null));
                        break;

                    default:
                        throw new NotImplementedException($"Operador relacional '{elemSem.Lexema}' não reconhecido!");
                }
                return;
            }

            // Senão, operação relacional entre valores inteiros:
            switch (elemSem.Token)
            {
                case "MAIOR":
                    mips.SectionText.Adicionar(new MipsText("sgt", "$t0,$t3,$t0"));
                    break;

                case "MENOR":
                    mips.SectionText.Adicionar(new MipsText("slt", "$t0,$t3,$t0"));
                    break;

                case "MAIOR_IGUAL":
                    mips.SectionText.Adicionar(new MipsText("sge", "$t0,$t3,$t0"));
                    break;

                case "MENOR_IGUAL":
                    mips.SectionText.Adicionar(new MipsText("sle", "$t0,$t3,$t0"));
                    break;

                case "IGUAL":
                    mips.SectionText.Adicionar(new MipsText("seq", "$t0,$t3,$t0"));
                    break;

                case "DIFERENTE":
                    mips.SectionText.Adicionar(new MipsText("sne", "$t0,$t3,$t0"));
                    break;

                default:
                    throw new NotImplementedException($"Operador relacional '{elemSem.Lexema}' não reconhecido!");
            }
        }

        /// <summary>
        /// Controla a movimentação dos valores armazenados nos registradores.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        /// <param name="i">Indice do elemento atual analisado na expressão.</param>
        /// <param name="aritIni">Registrador aritmético inicializado.</param>
        /// <param name="logIni">Registrador lógico inicializado.</param>
        /// <param name="tipoResultado">Tipo de resultado atual da expressão.</param>
        /// <returns>O novo indice do elemento atual a ser analisado na expressão.</returns>
        private int GerarMipsControleExpressãoLógica(MipsClass mips, int i, ref bool aritIni, ref bool logIni, ref VariávelTipo tipoResultado)
        {
            SemanticoToken itemExpr = this.Elementos[i];
            VariávelTipo tipoResultadoAnterior = tipoResultado;
            int r = i;

            Expressão exprArit = this.GerarMipsLógicoObterExpressãoAritmérica(ref r, ref tipoResultado);
            if (tipoResultado == VariávelTipo.Lógico)
            {
                if (logIni)
                {
                    this.GerarMipsMoverValores(mips, VariávelTipo.Lógico, i, 0, "$t0", null);
                }
                else
                {
                    this.GerarMipsMoverValores(mips, VariávelTipo.Lógico, i, 0, "$t4", null);
                    logIni = true;
                }
            }
            else
            {
                exprArit.GerarMips(mips, tipoResultado);

                if (!aritIni)
                {
                    if (tipoResultado == VariávelTipo.Inteiro)
                        mips.SectionText.Adicionar(new MipsText("move", "$t3,$t0"));
                    else
                        mips.SectionText.Adicionar(new MipsText("mov.s", "$f7,$f4"));

                    aritIni = true;
                }
            }

            return r;
        }

        /// <summary>
        /// Extrai uma expressão aritmética de uma expressão lógica.
        /// </summary>
        /// <param name="i">Indice do elemento atual analisado na expressão.</param>
        /// <param name="tipoResultado">Tipo de resultado atual da expressão.</param>
        /// <returns>Uma expressão aritmética extraida da expressão lógica.</returns>
        private Expressão GerarMipsLógicoObterExpressãoAritmérica(ref int i, ref VariávelTipo tipoResultado)
        {
            List<SemanticoToken> exprArit = new List<SemanticoToken>();

            for (; i < this.Elementos.Length; i++)
            {
                if (this.Elementos[i].Variável == null)
                {
                    if (this.Elementos[i].Token.Equals("MAIOR") || this.Elementos[i].Token.Equals("MENOR") || this.Elementos[i].Token.Equals("MAIOR_IGUAL") ||
                        this.Elementos[i].Token.Equals("MENOR_IGUAL") || this.Elementos[i].Token.Equals("IGUAL") || this.Elementos[i].Token.Equals("DIFERENTE") ||
                        this.Elementos[i].Token.Equals("E") || this.Elementos[i].Token.Equals("OU"))
                        break;

                    if (this.Elementos[i].Token.Equals("CONST_INT"))
                    {
                        if (tipoResultado != VariávelTipo.Real)
                            tipoResultado = VariávelTipo.Inteiro;
                    }
                    else if (this.Elementos[i].Token.Equals("CONST_REAL"))
                    {
                        tipoResultado = VariávelTipo.Real;
                    }

                    if (this.Elementos[i].Token.Equals("ABRE_COL"))
                    {
                        for (; i < this.Elementos.Length; i++)
                        {
                            exprArit.Add(this.Elementos[i]);
                            if (this.Elementos[i].Token.Equals("FECHA_COL"))
                                break;
                        }
                        continue;
                    }

                    if (this.Elementos[i].Token.Equals("ABRE_PAR"))
                    {
                        for (; i < this.Elementos.Length; i++)
                        {
                            exprArit.Add(this.Elementos[i]);
                            if (this.Elementos[i].Token.Equals("FECHA_PAR"))
                                break;
                        }
                        continue;
                    }

                    exprArit.Add(this.Elementos[i]);
                    continue;
                }

                if (this.Elementos[i].Variável.Tipo == VariávelTipo.Inteiro)
                {
                    if (tipoResultado != VariávelTipo.Real)
                        tipoResultado = VariávelTipo.Inteiro;
                }
                else if (this.Elementos[i].Variável.Tipo == VariávelTipo.Real)
                {
                    tipoResultado = VariávelTipo.Real;
                }

                exprArit.Add(this.Elementos[i]);
            }

            return new Expressão(exprArit);
        }

        #endregion
    }
}
