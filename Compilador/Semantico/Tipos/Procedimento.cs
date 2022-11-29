using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Compilador.Exceptions;
using Compilador.GeradorCódigo.MIPS;
using Compilador.GeradorCódigo.MIPS.MipsData;
using Compilador.Semantico.Tipos.Auxiliar;

namespace Compilador.Semantico.Tipos
{
    /// <summary>
    /// Classe para o comando do tipo 'Procedimento'.
    /// </summary>
    public class Procedimento : Tipo
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="elementos">Elementos semânticos que compõem o comando.</param>
        /// <param name="listaTiposEscopoMaior">Elementos do tipo comando em escopos seguintes à este comando.</param>
        public Procedimento(List<SemanticoToken> elementos, List<ITipo> listaTiposEscopoMaior)
        {
            if (elementos == null)
                throw new ArgumentNullException(nameof(elementos));
            if (elementos.Count < 5)
                throw new Exception("Um tipo 'procedimento' deve receber no mínimo 5 elementos!");

            this.Elementos = elementos.ToArray();
            this.ListaTiposEscopoMaior = listaTiposEscopoMaior.ToArray();

            this.Nome = this.Elementos[1];

            if (this.Elementos.Length >= 8)
            {
                List<ParâmetroProcFunc> parametros = new List<ParâmetroProcFunc>();
                List<SemanticoToken> elem = new List<SemanticoToken>();
                for (int i = 3; i < this.Elementos.Length - 2; i++)
                {
                    if (this.Elementos[i].Token.Equals("PONTO_VIRGULA"))
                    {
                        parametros.Add(new ParâmetroProcFunc(elem));
                        elem = new List<SemanticoToken>();
                        continue;
                    }

                    if (this.Elementos[i].Token.Equals("FECHA_PAR"))
                        break;

                    elem.Add(this.Elementos[i]);
                }
                if (elem.Count > 0)
                    parametros.Add(new ParâmetroProcFunc(elem));

                if (parametros.Count > 0)
                    this.Parametros = parametros.ToArray();
            }

            List<ITipo> variáveis = new List<ITipo>();
            List<ITipo> comandos = new List<ITipo>();
            foreach (var item in listaTiposEscopoMaior)
            {
                if (item is DeclaraçãoSimples || item is DeclaraçãoVetor)
                    variáveis.Add(item);
                else
                    comandos.Add(item);
            }

            if (variáveis.Count > 0)
                this.Variáveis = variáveis.ToArray();

            this.Comandos = comandos.ToArray();
        }

        #region Propriedades

        /// <summary>
        /// Obtém os elementos semânticos que compõem o comando.
        /// </summary>
        public SemanticoToken[] Elementos { get; }

        /// <summary>
        /// Obtém os elementos do tipo comando em escopos seguintes à este comando.
        /// </summary>
        public ITipo[] ListaTiposEscopoMaior { get; }

        /// <summary>
        /// Obtém o token do nome do procedimento.
        /// </summary>
        public SemanticoToken Nome { get; }

        /// <summary>
        /// Obtém os parâmetros do procedimento.
        /// </summary>
        public ParâmetroProcFunc[] Parametros { get; } = null;

        /// <summary>
        /// Obtém as variáveis declaradas no escopo do procedimento.
        /// </summary>
        public ITipo[] Variáveis { get; } = null;

        /// <summary>
        /// Obtém os comandos que compõe o procedimento.
        /// </summary>
        public ITipo[] Comandos { get; }

        #endregion

        /// <summary>
        /// Obtém a variável referênte à esta declaração.
        /// </summary>
        public Variável ObterVariável()
        {
            return new Variável(this);
        }

        #region Validação Semântica

        /// <summary>
        /// Executar a validação semântica das variáveis nos comandos.
        /// </summary>
        /// <param name="listaVariáveisGlobal">A lista de variáveis de escopo global.</param>
        /// <param name="listaErrors">A lista de erros encontrados durante a análise.</param>
        /// <param name="debug">Saida de debugação.</param>
        public void VerificarVariáveis(List<Variável> listaVariáveisGlobal, List<SemanticoError> listaErrors, ListBox debug)
        {
            base.VerificarVariáveis(listaVariáveisGlobal, this.Parametros, this.Variáveis, this.Comandos, listaErrors, debug);
        }

        /// <summary>
        /// Executa a validação dos argumentos da chamada do procedimento.
        /// </summary>
        /// <param name="chamaProcedimento">Chamada do procedimento.</param>
        /// <param name="listaVariáveisGlobal">A lista de variáveis de escopo global.</param>
        /// <param name="listaVariáveisLocal">A lista de variáveis de escopo local.</param>
        /// <param name="listaErrors">A lista de erros encontrados durante a análise.</param>
        /// <param name="debug">Saida de debugação.</param>
        public void ValidarArgumentos(ChamaProcedimento chamaProcedimento, List<Variável> listaVariáveisGlobal, List<Variável> listaVariáveisLocal, List<SemanticoError> listaErrors, ListBox debug)
        {
            debug.Items.Add($">>> {this.Nome.Lexema}(?)");

            int index = 0;
            int argumento = 0;
            int identificador = -1;
            VariávelTipo parametroTipo = Variável.ConverterTipo(this.Parametros[0].TipoDeDados);
            bool parametroPonteiro = this.Parametros[0].Ponteiro;

            while (index < chamaProcedimento.Argumentos.Length)
            {
                identificador++;
                if (identificador >= this.Parametros[argumento].Identificadores.Length)
                {
                    argumento++;
                    identificador = 0;
                    if (argumento < this.Parametros.Length)
                    {
                        parametroTipo = Variável.ConverterTipo(this.Parametros[argumento].TipoDeDados);
                        parametroPonteiro = this.Parametros[argumento].Ponteiro;
                    }
                    else
                    {
                        parametroTipo = VariávelTipo.Desconhecido;
                        parametroPonteiro = false;
                    }
                }

                SemanticoToken itemExpr = chamaProcedimento.Argumentos[index];

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
                            listaErrors.Add(new SemanticoError(itemExpr, "A variável usada como argumento no procedimento não está declarada!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            index++;
                            continue;
                        }
                    }

                    // Armazena a referência da variável de argumento para o parâmetro do procedimento.
                    itemExpr.Variável = elementsExpr;

                    if (elementsExpr.TipoDeclaração == VariávelDeclaração.Vetor)
                    {
                        ((DeclaraçãoVetor)elementsExpr.Declaração).ValidarIndice(ref index, chamaProcedimento.Argumentos, listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
                        continue;
                    }

                    if (!elementsExpr.ValorAtribuido)
                    {
                        listaErrors.Add(new SemanticoError(itemExpr, "A variável usada no argumento no procedimento não possui valor atribuido!"));
                        debug.Items.Add($"X > {itemExpr.ToString()}");
                        index++;
                        continue;
                    }

                    if (elementsExpr.Tipo == VariávelTipo.Real)
                    {
                        if (parametroTipo == VariávelTipo.Lógico || parametroTipo == VariávelTipo.Inteiro || parametroTipo == VariávelTipo.Caracter)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"O parâmetro '{this.Parametros[argumento].Identificadores[identificador].Lexema}' do procedimento não pode receber uma variável do tipo real!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            index++;
                            continue;
                        }

                    }
                    else if (elementsExpr.Tipo == VariávelTipo.Inteiro)
                    {
                        if (parametroTipo == VariávelTipo.Lógico || parametroTipo == VariávelTipo.Caracter)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"O parâmetro '{this.Parametros[argumento].Identificadores[identificador].Lexema}' do procedimento não pode receber uma variável do tipo inteiro!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            index++;
                            continue;
                        }

                        if (parametroTipo == VariávelTipo.Real && parametroPonteiro)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"O parâmetro '{this.Parametros[argumento].Identificadores[identificador].Lexema}' do procedimento não pode receber uma variável do tipo inteiro por ponteiro!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            index++;
                            continue;
                        }

                    }
                    else if (elementsExpr.Tipo == VariávelTipo.Caracter)
                    {
                        if (parametroTipo != VariávelTipo.Caracter && parametroTipo != VariávelTipo.Desconhecido)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"O parâmetro '{this.Parametros[argumento].Identificadores[identificador].Lexema}' do procedimento não pode receber uma variável do tipo caracter!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            index++;
                            continue;
                        }

                    }
                    else if (elementsExpr.Tipo == VariávelTipo.Lógico)
                    {
                        if (parametroTipo != VariávelTipo.Lógico && parametroTipo != VariávelTipo.Desconhecido)
                        {
                            listaErrors.Add(new SemanticoError(itemExpr, $"O parâmetro '{this.Parametros[argumento].Identificadores[identificador].Lexema}' do procedimento não pode receber uma variável do tipo lógico!"));
                            debug.Items.Add($"X > {itemExpr.ToString()}");
                            index++;
                            continue;
                        }

                    }
                    
                    debug.Items.Add($">> {elementsExpr.ToString()}");
                }

                index++;
            }
            
            int parametros = 0;
            for (int i = 0; i < this.Parametros.Length; i++)
                parametros += this.Parametros[i].Identificadores.Length;

            if (chamaProcedimento.Argumentos.Length != parametros)
            {
                int comprimento = this.Parametros.Last().Elementos.Last().Index + this.Parametros.Last().Elementos.Last().Lexema.Length - this.Parametros[0].Elementos[0].Index;
                listaErrors.Add(new SemanticoError(chamaProcedimento.Nome.Lexema, chamaProcedimento.Nome.Linha, chamaProcedimento.Nome.Inicio, chamaProcedimento.Nome.Index, comprimento, $"Foram passados {chamaProcedimento.Argumentos.Length} argumentos, mas o procedimento possui {parametros} parâmetros."));
                debug.Items.Add($">>> {this.Nome.Lexema}(erro...)");
                return;
            }

            string tmp = "ok";
            for (int i = 1; i < index; i++)
                tmp += ", ok";
            debug.Items.Add($">>> {this.Nome.Lexema}({tmp})");
        }

        #endregion

        #region Geração de Código MIPS Assembly

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para este componente.
        /// </summary>
        /// <param name="mips">Referância para a estrutura de informações Assembly MIPS.</param>
        public override void GerarMips(MipsClass mips)
        {
            if (!this.Nome.Variável.Utilizado)
                return;

            foreach (var parametro in this.Parametros)
            {
                foreach (var id in parametro.Identificadores)
                {
                    switch (id.Variável.Tipo)
                    {
                        case VariávelTipo.Lógico:
                            mips.HandlerLogico.AdicionarVariável(id.ObterEtiqueta());
                            break;
                        case VariávelTipo.Inteiro:
                            mips.SectionData.Adicionar(new MipsDataWord(id.ObterEtiqueta()));
                            break;
                        case VariávelTipo.Real:
                            mips.SectionData.Adicionar(new MipsDataFloat(id.ObterEtiqueta()));
                            break;
                        case VariávelTipo.Caracter:
                            if (id.Variável.Ponteiro)
                                mips.SectionData.Adicionar(new MipsDataWord(id.ObterEtiqueta()));
                            else
                                mips.SectionData.Adicionar(new MipsDataSpace(id.ObterEtiqueta(), 34));
                            break;
                    }
                }
            }

            mips.SectionText.Adicionar(new MipsText("#", $"Procedimento: '{this.InfoDeclaração()}'"));
            mips.SectionText.Adicionar(new MipsText($"{this.Nome.Lexema}:", null)); // Adiciona a etiqueta para a chamada do procedimento.

            // Salva o enderço de retono no stack pointer:
            mips.SectionText.Adicionar(new MipsText("addiu", "$sp,$sp,-4"));
            mips.SectionText.Adicionar(new MipsText("sw", "$ra,0($sp)"));
            mips.SectionText.Adicionar(MipsText.Blank);

            if (this.Variáveis != null)
            {
                foreach (var variavel in this.Variáveis)
                    variavel.GerarMips(mips);
            }

            foreach (var comando in this.Comandos)
                comando.GerarMips(mips);

            // Restaura o enderço de retono do stack pointer:
            mips.SectionText.Adicionar(new MipsText("lw", "$ra,0($sp)"));
            mips.SectionText.Adicionar(new MipsText("addiu", "$sp,$sp,4"));

            mips.SectionText.Adicionar(new MipsText("jr", "$ra"));  // Volta para o lugar de onde foi chamado.
            mips.SectionText.Adicionar(MipsText.Blank);
        }

        #endregion

        #region Geração da Árvore

        /// <summary>
        /// Geração de um nó da árvore de análise semântica.
        /// </summary>
        /// <param name="treeNode">Um nó da árvore para adicionar os nós subsequentes.</param>
        public override void GerarArvore(TreeNode treeNode)
        {
            // Cria o Nó do Procedimento
            TreeNode node = treeNode.Nodes.Add(this.ToString());

            if (this.Variáveis != null)
            {
                TreeNode varLocal = node.Nodes.Add("Variáveis Locais");
                foreach (var item in this.Variáveis)
                    varLocal.Nodes.Add(item.ToString());
            }

            if (this.Comandos != null)
            {
                foreach (var comando in this.Comandos)
                    comando.GerarArvore(node);
            }
        }

        #endregion

        /// <summary>
        /// Retorna um texto com a informação básica do procedimento, para utilizar nos comantários inseridos no código Assembly MPIS.
        /// </summary>
        public string InfoDeclaração()
        {
            string tmp = String.Empty;
            if (this.Parametros != null)
            {
                for (int i = 0; i < this.Parametros.Length; i++)
                {
                    tmp += this.Parametros[i].ToString();
                    if (i < this.Parametros.Length - 1)
                        tmp += "; ";
                }
            }

            return $"{this.Nome.Lexema} ({tmp})";
        }

        /// <summary>
        /// Retorna um texto que representa o objeto atual.
        /// </summary>
        public override string ToString()
        {
            int parametros = 0;
            string tmp = String.Empty;
            if (this.Parametros != null)
            {
                for (int i = 0; i < this.Parametros.Length; i++)
                {
                    parametros += this.Parametros[i].Identificadores.Length;
                    tmp += this.Parametros[i].ToString();
                    if (i < this.Parametros.Length - 1)
                        tmp += ", ";
                }
            }

            int var = 0, dec = 0;
            if (this.Variáveis != null)
            {
                foreach (var item in this.Variáveis)
                {
                    if (item is DeclaraçãoVetor)
                        var += 1;
                    else if (item is DeclaraçãoSimples)
                        var += ((DeclaraçãoSimples)item).Identificadores.Length;
                }

                dec = this.Variáveis.Count();
            }

            return $"Procedimento: '{this.Nome.Lexema} ({tmp})', {parametros} parâmetro, {var} variável local ({dec} declaração), {(this.Comandos == null ? 0 : this.Comandos.Count())} comandos";
        }
    }
}
