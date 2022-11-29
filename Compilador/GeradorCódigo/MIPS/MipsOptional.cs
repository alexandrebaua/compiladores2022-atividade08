using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Compilador.Exceptions;
using Compilador.GeradorCódigo.MIPS.MipsData;
using Compilador.Semantico.Tipos.Auxiliar;

namespace Compilador.GeradorCódigo.MIPS
{
    /// <summary>
    /// Classe do manipulador auxiliar para elementos opcionais utilizadas pelo gerador.
    /// </summary>
    public class MipsOptional
    {
        #region Variáveis Privadas
        private MipsClass _mips;
        private bool _arraysUtilizados = false;
        #endregion

        /// <summary>
        /// O construtor da classe.
        /// </summary>
        /// <param name="mips"></param>
        public MipsOptional(MipsClass mips)
        {
            this._mips = mips;
        }

        /// <summary>
        /// Obtém ou define a opção da quebra de linha.
        /// </summary>
        public bool QuebraLinha { get; set; } = false;

        /// <summary>
        /// Obtém ou define a opção do espaço em branco.
        /// </summary>
        public bool EspaçoBranco { get; set; } = false;

        /// <summary>
        /// Obtém ou define a opção de abre chaves e fecha chaves.
        /// </summary>
        public bool Chaves { get; set; } = false;

        /// <summary>
        /// Obtém ou define a opção de aspas duplas.
        /// </summary>
        public bool AspasDuplas { get; set; } = false;

        /// <summary>
        /// Obtém ou define a opção de utilização de arrays.
        /// </summary>
        public bool ArraysUtilizados
        {
            get { return this._arraysUtilizados; }

            set
            {
                this._arraysUtilizados = value;
                this.EspaçoBranco = true;
                this.Chaves = true;
            }
        }

        /// <summary>
        /// Obtém ou define a opção de utilização da entrada de valores lógicos.
        /// </summary>
        public bool EntradaLógicaUtilizada { get; set; } = false;

        /// <summary>
        /// Obtém ou define a opção de utilização da entrada de valores de texto.
        /// </summary>
        public bool EntradaTextoUtilizada { get; set; } = false;

        /// <summary>
        /// Obtém ou define a opção de utilização de valores de texto.
        /// </summary>
        public bool VariáveisTextoUtilizado { get; set; } = false;

        /// <summary>
        /// Obtém ou define a opção de utilização do ponteiro de pilha ($sp).
        /// </summary>
        public bool StackPointerUtilizado { get; set; } = false;

        /// <summary>
        /// Inclui os comandos opcionais necessários nos segmentos 'data' e 'text':
        /// </summary>
        public void IncluirOpcionais()
        {
            this.IncluirSectionData();
            this.IncluirSectionText();
        }

        /// <summary>
        /// Inclui os dados opcionais necessários no segmento de dados.
        /// </summary>
        private void IncluirSectionData()
        {
            if (this.QuebraLinha)
                this._mips.SectionData.Adicionar(new MipsDataAsciiz("lfeed", "\\n"));  // Adiciona a quebra de linha ao segmento de dados

            if (this.EspaçoBranco)
                this._mips.SectionData.Adicionar(new MipsDataAsciiz("space", " "));    // Adiciona um espaço em branco

            if (this.Chaves)
            {
                this._mips.SectionData.Adicionar(new MipsDataAsciiz("obraces", "{"));  // Adiciona o símbolo abre colchetes
                this._mips.SectionData.Adicionar(new MipsDataAsciiz("cbraces", "}"));  // Adiciona o símbolo fecha colchetes
            }

            if (this.AspasDuplas)
                this._mips.SectionData.Adicionar(new MipsDataAsciiz("dquotes", "\"\\\"\"")); // Adiciona o símbolo aspas duplas

            // Se existem arrays utilizados, então adiciona a mensagem de erro para matrizes:
            if (this._arraysUtilizados)
                this._mips.SectionData.Adicionar(new MipsDataAsciiz("msgErroArray", "\\nÍndice fora dos limites da matriz.\\n"));

            // Se foi utilizado entrada de valores lógicos, então adiciona a mensagem de erro para valor inválido:
            if (this.EntradaLógicaUtilizada)
                this._mips.SectionData.Adicionar(new MipsDataAsciiz("msgErroEntrLog", "\\nO valor lógico informado não está dentro da faixa válida (0~1).\\n"));

            // Se foi utilizado o Stack Pointer, então adiciona a mensagem de erro para fim do SP:
            if (this.StackPointerUtilizado)
                this._mips.SectionData.Adicionar(new MipsDataAsciiz("msgErroSP", "\\nEstouro do ponteiro de pilha.\\n"));

            // Se existem variáveis de texto utilizadas, então adiciona a variável temporária:
            if (this.VariáveisTextoUtilizado)
                this._mips.SectionData.Adicionar(new MipsDataSpace("tmpCar", 34));

        }

        /// <summary>
        /// Inclui os dados opcionais necessários no segmento de texto (instruções).
        /// </summary>
        private void IncluirSectionText()
        {
            // Se foi utilizado valores de texto, então adiciona a função interna para mover textos:
            if (this.VariáveisTextoUtilizado)
                this.FunçãoTextoMove();

            // Se foi utilizado entrada de valores de texto pelo usuário, então adiciona a função interna para remover a quebra de linha no final da entrada:
            if (this.EntradaTextoUtilizada)
                this.FunçãoTextoRemoveLF();

            // Se existem arrays utilizados, então:
            if (this._arraysUtilizados)
            {
                this._mips.SectionText.Adicionar(MipsText.Blank);
                this._mips.SectionText.Adicionar(new MipsText("#", "Finaliza o programa com erro de acesso no índice do array"));
                this._mips.SectionText.Adicionar(new MipsText("fimErroArray:", null));
                this._mips.SectionText.Adicionar(new MipsText("li", "$v0,4"));            // Comando para imprimir um texto
                this._mips.SectionText.Adicionar(new MipsText("la", "$a0,msgErroArray")); // Carregando o texto no argumento para habilitar a impressão
                this._mips.SectionText.Adicionar(MipsText.Syscall);                       // Executando o comando

                this._mips.SectionText.Adicionar(new MipsText("li", "$a0,1"));  // Adiciona o código de erro ao registrador $a0
                this._mips.SectionText.Adicionar(new MipsText("li", "$v0,17")); // Adiciona o fim de programa com erro
                this._mips.SectionText.Adicionar(MipsText.Syscall);             // Executando o comando
            }
            
            // Se foi utilizado entrada de valores lógicos, então:
            if (this.EntradaLógicaUtilizada)
            {
                this._mips.SectionText.Adicionar(MipsText.Blank);
                this._mips.SectionText.Adicionar(new MipsText("#", "Finaliza o programa com erro na entrada de valores lógicos"));
                this._mips.SectionText.Adicionar(new MipsText("fimErroEntradaLog:", null));
                this._mips.SectionText.Adicionar(new MipsText("li", "$v0,4"));              // Comando para imprimir um texto
                this._mips.SectionText.Adicionar(new MipsText("la", "$a0,msgErroEntrLog")); // Carregando o texto no argumento para habilitar a impressão
                this._mips.SectionText.Adicionar(MipsText.Syscall);                         // Executando o comando

                this._mips.SectionText.Adicionar(new MipsText("li", "$a0,2"));  // Adiciona o código de erro ao registrador $a0
                this._mips.SectionText.Adicionar(new MipsText("li", "$v0,17")); // Adiciona o fim de programa com erro
                this._mips.SectionText.Adicionar(MipsText.Syscall);             // Executando o comando
            }

            // Se foi utilizado o Stack Pointer, então:
            if (this.StackPointerUtilizado)
            {
                this._mips.SectionText.Adicionar(MipsText.Blank);
                this._mips.SectionText.Adicionar(new MipsText("#", "Finaliza o programa com erro de estouro no ponterio da pilha"));
                this._mips.SectionText.Adicionar(new MipsText("fimErroStackPointer:", null));
                this._mips.SectionText.Adicionar(new MipsText("li", "$v0,4"));          // Comando para imprimir um texto
                this._mips.SectionText.Adicionar(new MipsText("la", "$a0,msgErroSP"));  // Carregando o texto no argumento para habilitar a impressão
                this._mips.SectionText.Adicionar(MipsText.Syscall);                     // Executando o comando

                this._mips.SectionText.Adicionar(new MipsText("li", "$a0,3"));  // Adiciona o código de erro ao registrador $a0
                this._mips.SectionText.Adicionar(new MipsText("li", "$v0,17")); // Adiciona o fim de programa com erro
                this._mips.SectionText.Adicionar(MipsText.Syscall);             // Executando o comando
            }
        }

        /// <summary>
        /// Função interna para mover caracteres do texto de origem para o texto de destino.
        /// </summary>
        private void FunçãoTextoMove()
        {
            this._mips.SectionText.Adicionar(MipsText.Blank);
            this._mips.SectionText.Adicionar(new MipsText("#", "Função Interna - Move os caracteres de um texto para outro."));
            this._mips.SectionText.Adicionar(new MipsText("#", "$a0 = Endereço Texto de Origem"));
            this._mips.SectionText.Adicionar(new MipsText("#", "$a1 = Endereço Texto de Destino"));
            this._mips.SectionText.Adicionar(new MipsText("#", "$a2 = Contador de Caracteres"));

            // Adiciona a etiqueta para a chamada da função.
            this._mips.SectionText.Adicionar(new MipsText("funcTextoMove:", null));

            // Salva o enderço de retono no stack pointer:
            this._mips.SectionText.Adicionar(new MipsText("addiu", "$sp,$sp,-4"));
            this._mips.SectionText.Adicionar(new MipsText("sw", "$ra,0($sp)"));
            
            this._mips.SectionText.Adicionar(new MipsText("loopFuncTxtMov_Inicio:", null));

            // Move o caracter do texto da expressão para o registrador $t2.
            this._mips.SectionText.Adicionar(new MipsText("lb", "$t0,0($a0)"));

            // Move o caracter do registrador $t2 para o texto da variável atribuida.
            this._mips.SectionText.Adicionar(new MipsText("sb", "$t0,0($a1)"));

            // Se chegou ao final do texto, então finaliza.
            this._mips.SectionText.Adicionar(new MipsText("beq", "$t0,$zero,jmpFuncTxtMov_Fim"));

            // Move para o próximo caracter.
            this._mips.SectionText.Adicionar(new MipsText("addi", "$a0,$a0,1"));
            this._mips.SectionText.Adicionar(new MipsText("addi", "$a1,$a1,1"));
            this._mips.SectionText.Adicionar(new MipsText("addi", "$a2,$a2,1"));

            // Se chegou no limite da capacidade de caracteres do texto, então finaliza.
            this._mips.SectionText.Adicionar(new MipsText("bge", $"$a2,32,jmpFuncTxtMov_Fim"));

            this._mips.SectionText.Adicionar(new MipsText("j", "loopFuncTxtMov_Inicio"));
            this._mips.SectionText.Adicionar(new MipsText("jmpFuncTxtMov_Fim:", null));

            // Garante que o último caracter do texto na variável atribuida seja 0
            this._mips.SectionText.Adicionar(new MipsText("li", "$t0,0"));
            this._mips.SectionText.Adicionar(new MipsText("sb", "$t0,0($a1)"));

            // Restaura o enderço de retono do stack pointer:
            this._mips.SectionText.Adicionar(new MipsText("lw", "$ra,0($sp)"));
            this._mips.SectionText.Adicionar(new MipsText("addiu", "$sp,$sp,4"));

            this._mips.SectionText.Adicionar(new MipsText("jr", "$ra"));  // Volta para o lugar de onde foi chamado.
        }

        /// <summary>
        /// Função interna para remover a quebra de linha no final da entrada de texto pelo terminal do usuário.
        /// </summary>
        private void FunçãoTextoRemoveLF()
        {
            this._mips.SectionText.Adicionar(MipsText.Blank);
            this._mips.SectionText.Adicionar(new MipsText("#", "Função Interna - Remove o caracter de quebra de linha do texto."));
            this._mips.SectionText.Adicionar(new MipsText("#", "$a0 = Endereço do Texto"));

            // Adiciona a etiqueta para a chamada da função.
            this._mips.SectionText.Adicionar(new MipsText("funcTextoRemoveLF:", null));

            // Salva o enderço de retono no stack pointer:
            this._mips.SectionText.Adicionar(new MipsText("addiu", "$sp,$sp,-4"));
            this._mips.SectionText.Adicionar(new MipsText("sw", "$ra,0($sp)"));

            // Remove a quebra de linha do final do texto:
            this._mips.SectionText.Adicionar(new MipsText("loopFuncTxtRemLF_Inicio:", null));
            this._mips.SectionText.Adicionar(new MipsText("lb", "$t0,0($a0)"));
            this._mips.SectionText.Adicionar(new MipsText("beq", "$t0,10,jmpFuncTxtRemLF_Fim"));
            this._mips.SectionText.Adicionar(new MipsText("beq", "$t0,0,jmpFuncTxtRemLF_Fim"));
            this._mips.SectionText.Adicionar(new MipsText("add", "$a0,$a0,1"));
            this._mips.SectionText.Adicionar(new MipsText("j", "loopFuncTxtRemLF_Inicio"));
            this._mips.SectionText.Adicionar(new MipsText("jmpFuncTxtRemLF_Fim:", null));
            this._mips.SectionText.Adicionar(new MipsText("li", "$t0,0"));
            this._mips.SectionText.Adicionar(new MipsText("sb", "$t0,0($a0)"));

            // Restaura o enderço de retono do stack pointer:
            this._mips.SectionText.Adicionar(new MipsText("lw", "$ra,0($sp)"));
            this._mips.SectionText.Adicionar(new MipsText("addiu", "$sp,$sp,4"));

            this._mips.SectionText.Adicionar(new MipsText("jr", "$ra"));  // Volta para o lugar de onde foi chamado.
        }
    }
}
