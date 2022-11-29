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
    /// <summary>
    /// Classe para armazenamento de uma expressão aritmética, lógica ou texto, utilizado em atribuições e outros comando que utilizem a expressão para controle de ações.
    /// </summary>
    public partial class Expressão
    {
        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="elementos">Elementos semânticos que compõem o comando.</param>
        public Expressão(List<SemanticoToken> elementos)
        {
            this.Elementos = elementos.ToArray();
        }

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="elementos">Elementos semânticos que compõem o comando.</param>
        public Expressão(SemanticoToken[] elementos)
        {
            this.Elementos = elementos;
        }

        #region Propriedades

        /// <summary>
        /// Obtém os elementos semânticos que compõem o comando.
        /// </summary>
        public SemanticoToken[] Elementos { get; }

        /// <summary>
        /// Obtém ou define o tipo de resultado que a expressão irá gerar.
        /// </summary>
        public VariávelTipo TipoResultado { get; set; } = VariávelTipo.Desconhecido;

        #endregion

        #region Validação Semântica

        /// <summary>
        /// Executa a validação semântica de uma expressão (aritmética, lógica ou texto).
        /// </summary>
        /// <param name="tipoResultado">Tipo de resultado esperado da expressão.</param>
        /// <param name="listaVariáveisGlobal">A lista de variáveis de escopo global.</param>
        /// <param name="listaVariáveisLocal">A lista de variáveis de escopo local.</param>
        /// <param name="listaErrors">A lista de erros encontrados durante a análise.</param>
        /// <param name="debug">Saida de debugação.</param>
        public void ValidarExpressão(VariávelTipo tipoResultado, List<Variável> listaVariáveisGlobal, List<Variável> listaVariáveisLocal, List<SemanticoError> listaErrors, ListBox debug)
        {
            switch (tipoResultado)
            {
                case VariávelTipo.Desconhecido:
                    break;
                case VariávelTipo.Lógico:
                    this.ValidarExpressãoLógica(listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
                    break;

                case VariávelTipo.Inteiro:
                case VariávelTipo.Real:
                        this.ValidarExpressãoAritmética(tipoResultado, listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
                        break;

                case VariávelTipo.Caracter:
                    this.ValidarExpressãoTexto(listaVariáveisGlobal, listaVariáveisLocal, listaErrors, debug);
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Geração de Código MIPS Assembly

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para expressões.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        public void GerarMips(MipsClass mips)
        {
            if (this.TipoResultado != VariávelTipo.Desconhecido)
            {
                this.GerarMips(mips, this.TipoResultado);
                return;
            }

            VariávelTipo tipoResultado = VariávelTipo.Lógico;

            // Tenta identificar o tipo de resultado da expressão:
            for (int i = 0; i < this.Elementos.Length; i++)
            {
                if (this.Elementos[i].Variável == null)
                {
                    if (this.Elementos[i].Token.Equals("MAIOR") || this.Elementos[i].Token.Equals("MENOR") || this.Elementos[i].Token.Equals("MAIOR_IGUAL") ||
                        this.Elementos[i].Token.Equals("MENOR_IGUAL") || this.Elementos[i].Token.Equals("IGUAL") || this.Elementos[i].Token.Equals("DIFERENTE") ||
                        this.Elementos[i].Token.Equals("E") || this.Elementos[i].Token.Equals("OU") || this.Elementos[i].Token.Equals("CONST_LOGICA"))
                    {
                        tipoResultado = VariávelTipo.Lógico;
                        break;
                    }

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
                            if (this.Elementos[i].Token.Equals("FECHA_COL"))
                                break;
                        }
                        continue;
                    }

                    if (this.Elementos[i].Token.Equals("ABRE_PAR"))
                    {
                        for (; i < this.Elementos.Length; i++)
                        {
                            if (this.Elementos[i].Token.Equals("FECHA_PAR"))
                                break;
                        }
                        continue;
                    }
                    
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
                else if (this.Elementos[i].Variável.Tipo == VariávelTipo.Lógico)
                {
                    tipoResultado = VariávelTipo.Lógico;
                    break;
                }
                else if (this.Elementos[i].Variável.Tipo == VariávelTipo.Caracter)
                {
                    tipoResultado = VariávelTipo.Caracter;
                    break;
                }
            }

            this.GerarMips(mips, tipoResultado);
            this.TipoResultado = tipoResultado;
        }

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para expressões.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        /// <param name="tipoResultado">Tipo de resultado esperado da expressão.</param>
        public void GerarMips(MipsClass mips, VariávelTipo tipoResultado)
        {
            // Procura pelo abre parênteses, se encontrado, testa se foi atribuido uma variável, e se não, é necessário processar os agrupamentos:
            SemanticoToken semTokenAP = Array.Find(this.Elementos, x => x.Token.Equals("ABRE_PAR"));
            if (semTokenAP != null && semTokenAP.Variável == null)
            {
                this.GerarMipsAgrupamento(mips, tipoResultado);
                return;
            }

            switch (tipoResultado)
            {
                case VariávelTipo.Desconhecido:
                    break;

                case VariávelTipo.Lógico:
                    this.GerarMipsLógico(mips);
                    return;

                case VariávelTipo.Inteiro:
                    this.GerarMipsAritmético(mips, tipoResultado);
                    return;

                case VariávelTipo.Real:
                    this.GerarMipsAritmético(mips, tipoResultado);
                    return;

                case VariávelTipo.Caracter:
                    this.GerarMipsTexto(mips);
                    return;

                default:
                    break;
            }

            throw new NotImplementedException($"Não é suportada a geração de código para expressões do tipo '{tipoResultado}'.");
        }

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para expressões com agrupamentos por parênteses.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        /// <param name="tipoResultado">Tipo de resultado esperado da expressão.</param>
        private void GerarMipsAgrupamento(MipsClass mips, VariávelTipo tipoResultado)
        {
            List<SemanticoToken> elemExpr = new List<SemanticoToken>();
            for (int i = 0; i < this.Elementos.Length; i++)
            {
                // Se encontrou uma chamada de função, então:
                if (this.Elementos[i].Token.Equals("ID") && this.Elementos[i].Variável.TipoDeclaração == VariávelDeclaração.Função)
                {
                    elemExpr.Add(this.Elementos[i++]);

                    this.Elementos[i].Variável = Variável.Dummy;
                    elemExpr.Add(this.Elementos[i++]);

                    while (i < this.Elementos.Length)
                    {
                        elemExpr.Add(this.Elementos[i]);

                        if (this.Elementos[i].Token.Equals("FECHA_PAR"))
                            break;

                        i++;
                    }
                    continue;
                }

                // Se encontrou o abre parânteses, então:
                if (this.Elementos[i].Token.Equals("ABRE_PAR"))
                {
                    if (i + 2 < this.Elementos.Length)
                    {
                        if (this.Elementos[i + 2].Token.Equals("FECHA_PAR"))
                        {
                            elemExpr.Add(this.Elementos[i + 1]);
                            i += 2;
                            continue;
                        }
                        else if (i == 0) // Se inicia com abertura de parenteses, então:
                        {
                            if (this.Elementos.Last().Token.Equals("FECHA_PAR")) // Se termina com fechamento de parenteses, então:
                            {
                                // Cria uma expressão sem os parenteses no inicio e fim da expressão:
                                i = 1;
                                while (i < this.Elementos.Length - 1)
                                    elemExpr.Add(this.Elementos[i++]);

                                break;
                            }
                        }
                    }

                    // Define o abre parânteses como uma variável, para armazenar o resultado temporário da expressão agrupada:
                    this.Elementos[i].Variável = new Variável(this.Elementos[i], VariávelTipo.Desconhecido, VariávelDeclaração.Agrupamento, true);
                    SemanticoToken semVarExpr = this.Elementos[i++];
                    semVarExpr.Variável.Tipo = this.GerarMipsAgrupamento(mips, ref i);
                    elemExpr.Add(semVarExpr);
                    
                    // Salva a variável temporária criada.
                    this.SalvarVarTemp(mips, semVarExpr);
                    continue;
                }

                elemExpr.Add(this.Elementos[i]);
            }

            Expressão expr = new Expressão(elemExpr);
            expr.GerarMips(mips, tipoResultado);
        }

        /// <summary>
        /// Executa a geração dos comandos Assembly MIPS para expressões com agrupamentos por parênteses.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        /// <param name="i">Indice do elemento atual analisado na expressão.</param>
        /// <returns>O tipo de valor resultante do agrupamento.</returns>
        private VariávelTipo GerarMipsAgrupamento(MipsClass mips, ref int i)
        {
            List<SemanticoToken> elemExpr = new List<SemanticoToken>();
            for (; i < this.Elementos.Length; i++)
            {
                // Se encontrou o abre parânteses, então:
                if (this.Elementos[i].Token.Equals("ABRE_PAR"))
                {
                    if (i + 2 < this.Elementos.Length)
                    {
                        if (this.Elementos[i + 2].Token.Equals("FECHA_PAR"))
                        {
                            elemExpr.Add(this.Elementos[i + 1]);
                            i += 2;
                            continue;
                        }
                    }

                    // Define o abre parânteses como uma variável, para armazenar o resultado temporário da expressão agrupada:
                    this.Elementos[i].Variável = new Variável(this.Elementos[i], VariávelTipo.Desconhecido, VariávelDeclaração.Agrupamento, true);
                    SemanticoToken semVarExpr = this.Elementos[i++];
                    semVarExpr.Variável.Tipo = this.GerarMipsAgrupamento(mips, ref i);
                    elemExpr.Add(semVarExpr);

                    // Salva a variável temporária criada.
                    this.SalvarVarTemp(mips, semVarExpr);
                    continue;
                }

                if (this.Elementos[i].Token.Equals("FECHA_PAR"))
                    break;

                elemExpr.Add(this.Elementos[i]);
            }
            
            Expressão expr = new Expressão(elemExpr);
            expr.TipoResultado = this.ObterTipo(elemExpr);
            expr.GerarMips(mips);

            return expr.TipoResultado;
        }

        /// <summary>
        /// Salva a variável temporária resultante do agrupamento.
        /// </summary>
        /// <param name="mips">Referência para a estrutura de informações Assembly MIPS.</param>
        /// <param name="semVarExpr">Token relacionado ao valor de resultado do agrupamento.</param>
        private void SalvarVarTemp(MipsClass mips, SemanticoToken semVarExpr)
        {
            if (semVarExpr.Variável.Tipo == VariávelTipo.Lógico)
            {
                mips.HandlerLogico.AdicionarVariável(semVarExpr.ObterEtiqueta());

                // Este comando é para mover o resultado da expressão lógica do registrador $t0 para o segmento de dados da etiqueta (variável).
                mips.HandlerLogico.SetarVariável("$t4", semVarExpr.ObterEtiqueta());
            }
            else if (semVarExpr.Variável.Tipo == VariávelTipo.Inteiro)
            {
                mips.SectionData.Adicionar(new MipsDataWord(semVarExpr.ObterEtiqueta()));

                // Este comando é para mover o resultado da expressão inteiro do registrador $t0 para o segmento de dados da etiqueta (variável).
                mips.SectionText.Adicionar(new MipsText("sw", $"$t0,{semVarExpr.ObterEtiqueta()}"));
            }
            else if (semVarExpr.Variável.Tipo == VariávelTipo.Real)
            {
                mips.SectionData.Adicionar(new MipsDataFloat(semVarExpr.ObterEtiqueta()));

                // Este comando é para mover o resultado da expressão real fornecido do coprocessador 1 $f4 para o segmento de dados da etiqueta (variável).
                mips.SectionText.Adicionar(new MipsText("swc1", $"$f4,{semVarExpr.ObterEtiqueta()}"));
            }
        }

        /// <summary>
        /// Obtém o tipo de resultado possivel de uma expressão.
        /// </summary>
        /// <param name="elementos">Os elementos da expressão.</param>
        /// <returns>O tipo de valor resultante da expressão.</returns>
        private VariávelTipo ObterTipo(List<SemanticoToken> elementos)
        {
            VariávelTipo tipoResultado = VariávelTipo.Lógico;

            // Tenta identificar o tipo de resultado da expressão:
            for (int i = 0; i < elementos.Count; i++)
            {
                if (elementos[i].Variável == null)
                {
                    if (elementos[i].Token.Equals("MAIOR") || elementos[i].Token.Equals("MENOR") || elementos[i].Token.Equals("MAIOR_IGUAL") ||
                        elementos[i].Token.Equals("MENOR_IGUAL") || elementos[i].Token.Equals("IGUAL") || elementos[i].Token.Equals("DIFERENTE") ||
                        elementos[i].Token.Equals("E") || elementos[i].Token.Equals("OU") || elementos[i].Token.Equals("CONST_LOGICA"))
                    {
                        tipoResultado = VariávelTipo.Lógico;
                        break;
                    }

                    if (elementos[i].Token.Equals("CONST_INT"))
                    {
                        if (tipoResultado != VariávelTipo.Real)
                            tipoResultado = VariávelTipo.Inteiro;
                    }
                    else if (elementos[i].Token.Equals("CONST_REAL"))
                    {
                        tipoResultado = VariávelTipo.Real;
                    }

                    if (elementos[i].Token.Equals("ABRE_COL"))
                    {
                        for (; i < elementos.Count; i++)
                        {
                            if (elementos[i].Token.Equals("FECHA_COL"))
                                break;
                        }
                        continue;
                    }

                    if (elementos[i].Token.Equals("ABRE_PAR"))
                    {
                        for (; i < elementos.Count; i++)
                        {
                            if (elementos[i].Token.Equals("FECHA_PAR"))
                                break;
                        }
                        continue;
                    }

                    continue;
                }

                if (elementos[i].Variável.Tipo == VariávelTipo.Inteiro)
                {
                    if (tipoResultado != VariávelTipo.Real)
                        tipoResultado = VariávelTipo.Inteiro;
                }
                else if (elementos[i].Variável.Tipo == VariávelTipo.Real)
                {
                    tipoResultado = VariávelTipo.Real;
                }
                else if (elementos[i].Variável.Tipo == VariávelTipo.Lógico)
                {
                    tipoResultado = VariávelTipo.Lógico;
                    break;
                }
            }

            return tipoResultado;
        }

        #endregion

        /// <summary>
        /// Retorna um texto que representa o objeto atual.
        /// </summary>
        public override string ToString()
        {
            string tmp = String.Empty;
            foreach (var item in this.Elementos)
                tmp += item.Lexema;

            return $"Expressão: '{tmp}'";
        }
    }
}
