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
        #region Validação Semântica

        /// <summary>
        /// Executa a validação semântica em uma expressão de texto.
        /// </summary>
        /// <param name="listaVariáveisGlobal">A lista de variáveis de escopo global.</param>
        /// <param name="listaVariáveisLocal">A lista de variáveis de escopo local.</param>
        /// <param name="listaErrors">A lista de erros encontrados durante a análise.</param>
        /// <param name="debug">Saida de debugação.</param>
        private void ValidarExpressãoTexto(List<Variável> listaVariáveisGlobal, List<Variável> listaVariáveisLocal, List<SemanticoError> listaErrors, ListBox debug)
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
                        if (varExpr.Tipo != VariávelTipo.Caracter)
                        {
                            switch (varExpr.Tipo)
                            {
                                case VariávelTipo.Lógico:
                                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo caracter não pode receber o valor do vetor '{itemExpr.Lexema}' do tipo lógico!"));
                                    break;
                                case VariávelTipo.Inteiro:
                                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo caracter não pode receber o valor do vetor '{itemExpr.Lexema}' do tipo inteiro!"));
                                    break;
                                case VariávelTipo.Real:
                                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo caracter não pode receber o valor do vetor '{itemExpr.Lexema}' do tipo real!"));
                                    break;
                            }
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

                        if (varExpr.Tipo != VariávelTipo.Real)
                        {
                            switch (varExpr.Tipo)
                            {
                                case VariávelTipo.Lógico:
                                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo caracter não pode receber o retorno de uma função do tipo lógico!"));
                                    break;
                                case VariávelTipo.Inteiro:
                                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo caracter não pode receber o retorno de uma função do tipo inteiro!"));
                                    break;
                                case VariávelTipo.Real:
                                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo caracter não pode receber o retorno de uma função do tipo real!"));
                                    break;
                            }
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

                    if (varExpr.Tipo != VariávelTipo.Caracter)
                    {
                        switch (varExpr.Tipo)
                        {
                            case VariávelTipo.Lógico:
                                listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo caracter não pode receber a variável '{varExpr.Identificador}' do tipo lógico!"));
                                break;
                            case VariávelTipo.Inteiro:
                                listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo caracter não pode receber a variável '{varExpr.Identificador}' do tipo inteiro!"));
                                break;
                            case VariávelTipo.Real:
                                listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo caracter não pode receber a variável '{varExpr.Identificador}' do tipo real!"));
                                break;
                        }

                        debug.Items.Add($"X > {itemExpr.ToString()}");
                        continue;
                    }
                    
                    debug.Items.Add($"> {varExpr.ToString()}");
                }
                else if (itemExpr.Token.Equals("CONST_TEXTO") || itemExpr.Token.Equals("MAIS"))
                {
                    debug.Items.Add($"> {itemExpr.Lexema} : {itemExpr.Token}");
                }
                else if (itemExpr.Token.Equals("CONST_LOGICA"))
                {
                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo caracter não pode receber a constante '{itemExpr.Lexema}' do tipo lógico!"));
                    debug.Items.Add($"X > {itemExpr.ToString()}");
                }
                else if (itemExpr.Token.Equals("CONST_INT"))
                {
                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo caracter não pode receber a constante '{itemExpr.Lexema}' do tipo inteiro!"));
                    debug.Items.Add($"X > {itemExpr.ToString()}");
                }
                else if (itemExpr.Token.Equals("CONST_REAL"))
                {
                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo caracter não pode receber uma constante '{itemExpr.Lexema}' do tipo real!"));
                    debug.Items.Add($"X > {itemExpr.ToString()}");
                }
                else if (itemExpr.Token.Equals("MOD") || itemExpr.Token.Equals("DIV"))
                {
                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma variável do tipo caracter não pode utilizar o operadores MOD ou DIV!"));
                    debug.Items.Add($"X > {itemExpr.ToString()}");
                }
                else if (itemExpr.Token.Equals("E") || itemExpr.Token.Equals("OU"))
                {
                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma atribuição de caracteres não pode utilizar o operadores lógicos E ou OU!"));
                    debug.Items.Add($"X > {itemExpr.ToString()}");
                }
                else if (itemExpr.Token.Equals("MAIOR") || itemExpr.Token.Equals("MENOR") || itemExpr.Token.Equals("MAIOR_IGUAL") || itemExpr.Token.Equals("MENOR_IGUAL") || itemExpr.Token.Equals("IGUAL") || itemExpr.Token.Equals("DIFERENTE"))
                {
                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma atribuição de caracteres não pode utilizar o operadores lógicos relacionais!"));
                    debug.Items.Add($"X > {itemExpr.ToString()}");
                }
                else if (itemExpr.Token.Equals("MENOS") || itemExpr.Token.Equals("ASTERISTICO") || itemExpr.Token.Equals("BARRA"))
                {
                    listaErrors.Add(new SemanticoError(itemExpr, $"Uma atribuição de caracteres não pode utilizar o operadores matemáticos!"));
                    debug.Items.Add($"X > {itemExpr.ToString()}");
                }
            }
        }

        #endregion

        #region Geração de Código MIPS Assembly

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para este expressões de texto.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        private void GerarMipsTexto(MipsClass mips)
        {

            if (this.Elementos.Length == 1)
            {
                if (this.Elementos[0].Token.Equals("CONST_TEXTO"))
                {
                    // Adiciona o texto na seção de dados MIPS.
                    mips.SectionData.Adicionar(new MipsDataAsciiz(this.Elementos[0].ObterEtiqueta(), this.Elementos[0].Lexema));
                    
                    // Carrega o endereço da constante de caracteres no registrador $a0.
                    mips.SectionText.Adicionar(new MipsText("la", $"$a0,{this.Elementos[0].ObterEtiqueta()}"));
                }
                else if (this.Elementos[0].Token.Equals("ID"))
                {
                    if (this.Elementos[0].Variável.Ponteiro)
                    {
                        // Carrega o endereço da variável de caracteres no registrador $a0.
                        mips.SectionText.Adicionar(new MipsText("lw", $"$a0,{this.Elementos[0].ObterEtiqueta()}"));
                    }
                    else
                    {
                        // Carrega o endereço da variável de caracteres no registrador $a0.
                        mips.SectionText.Adicionar(new MipsText("la", $"$a0,{this.Elementos[0].ObterEtiqueta()}"));
                    }
                }

                return;
            }

            MipsText[] tempVar = new MipsText[2];
            tempVar[0] = new MipsText("li", "$a2,0");
            tempVar[1] = new MipsText("la", "$a1,tmpCar");
            mips.SectionText.Adicionar(tempVar);

            // Percorre os elementos da expressão:
            for (int i = 0; i < this.Elementos.Length; i++)
            {
                SemanticoToken elemSem = this.Elementos[i];

                if (this.Elementos[i].Token.Equals("CONST_TEXTO"))
                {
                    // Adiciona o texto na seção de dados MIPS.
                    mips.SectionData.Adicionar(new MipsDataAsciiz(this.Elementos[i].ObterEtiqueta(), this.Elementos[i].Lexema));

                    // Carrega o endereço da constante de caracteres no registrador $a0.
                    mips.SectionText.Adicionar(new MipsText("la", $"$a0,{this.Elementos[i].ObterEtiqueta()}"));

                    // Será necessário utilizar a variável temporária, então limpa a memória de registro das instruções.
                    tempVar = null;
                }
                else if (this.Elementos[i].Token.Equals("ID"))
                {
                    if (elemSem.Variável.TipoDeclaração == VariávelDeclaração.Vetor)
                    {
                        DeclaraçãoVetor decVet = (DeclaraçãoVetor)elemSem.Variável.Declaração;  // Armazena a declaração do vetor.
                        decVet.GerarMipsValorIndex(mips, this.Elementos, ref i);                // Posiciona o registro do vetor.

                        // Se a memória de registro das instruções da variável temporária ainda não foi limpa, então:
                        if (tempVar != null)
                        {
                            // Se existe mais elementos na expressão, então:
                            if (i < this.Elementos.Length - 1)
                            {
                                // Foi necessário utilizar a variável temporária, limpa a memória de registro das instruções.
                                tempVar = null;
                            }
                            else
                            {
                                // Senão, não existe mais elementos na expressão, e não será utilizado a variável temporária, então remove as instruções.
                                mips.SectionText.Remover(tempVar);

                                // Carrega o endereço do registro do array de caracteres no registrador $a0.
                                mips.SectionText.Adicionar(new MipsText("move", "$a0,$t5"));

                                break;
                            }
                        }

                        // Move o endereço do registro selecionado, do registrador $t5 para o registrador $a0.
                        mips.SectionText.Adicionar(new MipsText("move", "$a0,$t5"));
                    }
                    else if (elemSem.Variável.TipoDeclaração == VariávelDeclaração.Função)
                    {
                        Função decFunc = (Função)elemSem.Variável.Declaração;  // Armazena a declaração da função.
                        
                        MipsText[] argTxtMoveSP = null;
                        foreach (var pr in decFunc.Parametros)
                        {
                            if (pr.TipoDeDados.Token.Equals("CARACTER") && !pr.Ponteiro && decFunc.Retorne.Pai.Tipo == VariávelTipo.Caracter)
                            {
                                argTxtMoveSP = new MipsText[3];
                                break;
                            }
                        }
                        
                        if (argTxtMoveSP != null)
                        {
                            // Salva os argumentos da função move texto no stack pointer:
                            argTxtMoveSP[0] = new MipsText("addiu", "$sp,$sp,-8");
                            argTxtMoveSP[1] = new MipsText("sw", "$a1,0($sp)");
                            argTxtMoveSP[2] = new MipsText("sw", "$a2,4($sp)");
                            mips.SectionText.Adicionar(argTxtMoveSP);
                        }

                        decFunc.GerarMipsChamada(mips, this.Elementos, ref i, 0);

                        if (argTxtMoveSP != null)
                        {
                            // Se existe mais elementos na expressão, então:
                            if (i < this.Elementos.Length - 1)
                            {
                                // restaura os argumentos da função move texto no stack pointer:
                                mips.SectionText.Adicionar(new MipsText("lw", "$a1,0($sp)"));
                                mips.SectionText.Adicionar(new MipsText("lw", "$a2,4($sp)"));
                                mips.SectionText.Adicionar(new MipsText("addiu", "$sp,$sp,8"));
                            }
                            else
                            {
                                // Senão, não existe mais elementos na expressão, e não será utilizado os argumentos da função mmove texto, então remove as instruções.
                                mips.SectionText.Remover(argTxtMoveSP);
                            }
                        }

                        // Se a memória de registro das instruções da variável temporária ainda não foi limpa, então.
                        if (tempVar != null)
                        {
                            // Se existe mais elementos na expressão, então:
                            if (i < this.Elementos.Length - 1)
                            {
                                // Foi necessário utilizar a variável temporária, limpa a memória de registro das instruções.
                                tempVar = null;
                            }
                            else
                            {
                                // Senão, não existe mais elementos na expressão, e não será utilizado a variável temporária, então remove as instruções.
                                mips.SectionText.Remover(tempVar);

                                // Move o endereço do texto no retorno da função para o registrador $a0.
                                mips.SectionText.Adicionar(new MipsText("move", "$a0,$s0"));

                                break;
                            }
                        }

                        // Move o endereço do texto no retorno da função para o registrador $a0.
                        mips.SectionText.Adicionar(new MipsText("move", "$a0,$s0"));
                    }
                    else
                    {
                        if (this.Elementos[i].Variável.Ponteiro)
                        {
                            // Carrega o endereço da variável de caracteres no registrador $a0.
                            mips.SectionText.Adicionar(new MipsText("lw", $"$a0,{this.Elementos[i].ObterEtiqueta()}"));
                        }
                        else
                        {
                            // Carrega o endereço da variável de caracteres no registrador $a0.
                            mips.SectionText.Adicionar(new MipsText("la", $"$a0,{this.Elementos[i].ObterEtiqueta()}"));
                        }

                        // Será necessário utilizar a variável temporária, então limpa a memória de registro das instruções.
                        tempVar = null;
                    }
                }
                else
                {
                    continue;
                }
                
                // Adiciona a chamada da função para mover o texto.
                mips.SectionText.Adicionar(new MipsText("jal", "funcTextoMove"));

                // Se existe mais elementos na expressão, então:
                if (i < this.Elementos.Length - 1)
                {
                    // Se chegou no limite da capacidade de caracteres do texto, então finaliza.
                    mips.SectionText.Adicionar(new MipsText("bge", $"$a2,32,jmpExprCarFimCap_{this.Elementos[0].Index}"));
                }
            }

            // Se não utilizou a variável temporária, então o texto já está correto.
            if (tempVar != null)
                return;

            mips.SectionText.Adicionar(new MipsText($"jmpExprCarFimCap_{this.Elementos[0].Index}:", null));
            
            // Carrega o endereço da variável temporária de caracteres no registrador $a0.
            mips.SectionText.Adicionar(new MipsText("la", "$a0,tmpCar"));
        }

        #endregion
    }
}
