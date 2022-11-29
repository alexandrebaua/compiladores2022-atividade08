using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Compilador.Semantico;

namespace Compilador.GeradorCódigo.MIPS
{
    /// <summary>
    /// Classe do manipulador auxiliar para controlar o stack pointer utilizado pelo gerador.
    /// Utilizado para a troca de contexto nas chamadas de procedimentos e funções.
    /// </summary>
    public class MipsStackPointer
    {
        #region Variáveis Privadas
        private MipsClass _mips;
        private Stack<Contexto> _contexto = new Stack<Contexto>();
        private Contexto _contextoAtual = new Contexto();
        #endregion

        /// <summary>
        /// O construtor da classe.
        /// </summary>
        /// <param name="mips"></param>
        public MipsStackPointer(MipsClass mips)
        {
            this._mips = mips;
        }

        /// <summary>
        /// Adiciona uma variável na memória de contexto.
        /// </summary>
        /// <param name="var">O token da variável.</param>
        public void Adicionar(SemanticoToken var)
        {
            if (var.Variável == null)
                return;

            if (this._contexto.Count == 0 && !var.Variável.Global)
                return;

            this._contextoAtual.Adicionar(var.ObterEtiqueta());
        }

        /// <summary>
        /// Adiciona uma variável ou registrador na memória de contexto.
        /// </summary>
        /// <param name="var">A variável ou registrador.</param>
        public void Adicionar(string var)
        {
            if (String.IsNullOrWhiteSpace(var))
                return;

            this._contextoAtual.Adicionar(var);
        }

        /// <summary>
        /// Remove uma variável ou registrador na memória de contexto.
        /// </summary>
        /// <param name="var">A variável ou registrador à ser removido.</param>
        public void Remover(string var)
        {
            if (String.IsNullOrWhiteSpace(var))
                return;

            this._contextoAtual.Remover(var);
        }

        /// <summary>
        /// Incrementa o contexto, insere os valores do contexto atual na pilha, e cria um novo contexto.
        /// </summary>
        public void IncrementaContexto()
        {
            this._contexto.Push(this._contextoAtual);
            this._contextoAtual.Push(this._mips);
            this._contextoAtual = new Contexto();
        }

        /// <summary>
        /// Decrementa o contexto, e recupera o contexto e os valores da pilha.
        /// </summary>
        public void DecrementaContexto()
        {
            this._contextoAtual = this._contexto.Pop();
            this._contextoAtual.Pop(this._mips);
        }

        /// <summary>
        /// Descarta o contexto atual.
        /// </summary>
        public void DescartaContexto()
        {
            this._contextoAtual = new Contexto();
        }

        /// <summary>
        /// Classe auxiliar para armazenamento e manipulação de valores na pilha, e do ponteiro da pilha.
        /// </summary>
        private class Contexto
        {
            /// <summary>
            /// Obtém ou define as variáveis e registradores a serem gerenciados na pilha.
            /// </summary>
            public List<string> VarSP { get; } = new List<string>();

            /// <summary>
            /// Adiciona uma variável ou registrador na memória de contexto.
            /// </summary>
            /// <param name="var">A variável ou registrador.</param>
            public void Adicionar(string var)
            {
                if (this.VarSP.Contains(var))
                    return;

                if (var.StartsWith("$") && this.VarSP.Count > 0)
                {
                    int i = 0;
                    while (i < this.VarSP.Count)
                    {
                        if (!this.VarSP[i].StartsWith("$"))
                            break;

                        i++;
                    }

                    if (i < this.VarSP.Count)
                    {
                        this.VarSP.Insert(i, var);
                        return;
                    }
                }

                this.VarSP.Add(var);
            }

            /// <summary>
            /// Remove uma variável ou registrador na memória de contexto.
            /// </summary>
            /// <param name="var">A variável ou registrador à ser removido.</param>
            public void Remover(string var)
            {
                if (this.VarSP.Contains(var))
                    this.VarSP.Remove(var);
            }

            /// <summary>
            /// Insere as variáveis e registradores do contexto na pilha, e atualiza o ponteiro de pilha.
            /// </summary>
            /// <param name="mips"></param>
            public void Push(MipsClass mips)
            {
                if (this.VarSP.Count == 0)
                    return;

                mips.HandlerOpcionais.StackPointerUtilizado = true;

                // Adiciona o teste de estouro de pilha:
                mips.SectionText.Adicionar(new MipsText("li", $"$t5,{(this.VarSP.Count * 4 + 10)}"));
                mips.SectionText.Adicionar(new MipsText("blt", "$sp,$t5,fimErroStackPointer"));

                // Desloca o ponteiro para adicionar os valores.
                mips.SectionText.Adicionar(new MipsText("addiu", $"$sp,$sp,-{(this.VarSP.Count * 4)}"));

                for (int i = 0; i < this.VarSP.Count; i++)
                {
                    if (this.VarSP[i].StartsWith("$"))
                    {
                        mips.SectionText.Adicionar(new MipsText("sw", $"{this.VarSP[i]},{(i * 4)}($sp)"));
                        continue;
                    }

                    mips.SectionText.Adicionar(new MipsText("lw", $"$t5,{this.VarSP[i]}"));
                    mips.SectionText.Adicionar(new MipsText("sw", $"$t5,{(i * 4)}($sp)"));
                }
            }

            /// <summary>
            /// Recupera as variáveis e registradores memorizados na pilha, e atualiza o ponteiro de pilha.
            /// </summary>
            /// <param name="mips"></param>
            public void Pop(MipsClass mips)
            {
                if (this.VarSP.Count == 0)
                    return;

                for (int i = 0; i < this.VarSP.Count; i++)
                {
                    if (this.VarSP[i].StartsWith("$"))
                    {
                        mips.SectionText.Adicionar(new MipsText("lw", $"{this.VarSP[i]},{(i * 4)}($sp)"));
                        continue;
                    }

                    mips.SectionText.Adicionar(new MipsText("lw", $"$t5,{(i * 4)}($sp)"));
                    mips.SectionText.Adicionar(new MipsText("sw", $"$t5,{this.VarSP[i]}"));
                }

                mips.SectionText.Adicionar(new MipsText("addiu", $"$sp,$sp,{(this.VarSP.Count * 4)}"));
            }
        }
    }
}
