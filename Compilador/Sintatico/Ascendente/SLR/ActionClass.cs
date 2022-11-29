using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Sintatico.Ascendente.SLR
{
    /// <summary>
    /// Enumerador contendo os tipos de ações possíveis executadas pelo analisador sintático.
    /// </summary>
    public enum ActionType : byte
    {
        /// <summary>
        /// Indica que deverá ser empilhado o simbolo atual na pilha.
        /// </summary>
        Shift = 0x01,

        /// <summary>
        /// Indica que deverá ser executado uma ação de redução da pilha.
        /// </summary>
        Reduce = 0x02,

        /// <summary>
        /// Indica que o analisador alcançou o estado de aceitação.
        /// </summary>
        Accept = 0x03
    }

    /// <summary>
    /// Classe de armazenamento de uma ação do analisador sintático SLR.
    /// </summary>
    public class ActionClass
    {
        /// <summary>
        /// Construtor da classe.
        /// Esse construtor é exclusivo para criação do estado de aceitação.
        /// </summary>
        /// <param name="actionType">O tipo de ação à armazenar.</param>
        public ActionClass(ActionType actionType) : this(actionType, 0) { }

        /// <summary>
        /// Construtor da classe.
        /// Esse construtor é exclusivo para criação do estado de empilhar.
        /// </summary>
        /// <param name="actionType">O tipo de ação à armazenar.</param>
        /// <param name="estado">O estado a ser empilhado.</param>
        public ActionClass(ActionType actionType, int estado) : this(actionType, estado, null) { }

        /// <summary>
        /// Construtor da classe.
        /// Esse construtor é exclusivo para criação do estado de redução.
        /// </summary>
        /// <param name="actionType">O tipo de ação à armazenar.</param>
        /// <param name="reducao">A redução à armazenar.</param>
        public ActionClass(ActionType actionType, ReducaoClass reducao) : this(actionType, 0, reducao) { }

        /// <summary>
        /// Construtor da classe.
        /// Esse construtor é recomendado para criação do estado de redução.
        /// </summary>
        /// <param name="actionType">O tipo de ação à armazenar.</param>
        /// <param name="estado">O estado a ser empilhado, OU, o indice da redução armazenada (apenas estético).</param>
        /// <param name="reducao">A redução à armazenar (nulo se for empilhar).</param>
        public ActionClass(ActionType actionType, int estado, ReducaoClass reducao)
        {
            if (actionType == ActionType.Shift)
            {
                if (estado <= 0)
                    throw new Exception("Para a ação de 'empilhar' é necessário informar um 'estado' com valor maior que 0 (zero).");
                else if (reducao != null)
                    throw new Exception("Para a ação de 'empilhar' não deverá ser informado a 'redução'.");
            }
            else if (actionType == ActionType.Reduce)
            {
                if (estado > 0)
                    throw new Exception("Para a ação de 'reduzir' manter o valor do 'estado' em  0 (zero).");
                else if (reducao == null)
                    throw new Exception("Para a ação de 'reduzir' deverá ser informado a 'redução'.");
            }

            this.TipoAcao = actionType;
            this.Estado = estado;
            this.Reducao = reducao;
        }

        /// <summary>
        /// Obtém ou define o tipo da ação a ser executada.
        /// </summary>
        public ActionType TipoAcao { get; set; }

        /// <summary>
        /// Obtém ou define o estado a ser empilhado.
        /// Para reduções o valor é utilizado apenas para facilitar o registro de eventos apresentado.
        /// </summary>
        public int Estado { get; set; }

        /// <summary>
        /// Obtém ou define uma redução.
        /// Para ações de empilhar esse valor é nulo.
        /// </summary>
        public ReducaoClass Reducao { get; set; }

        public static ActionClass CreateShift(int estado)
        {
            return new ActionClass(ActionType.Shift, estado);
        }

        /// <summary>
        /// Retorna um texto contendo as informações da ação armazenada.
        /// </summary>
        public override string ToString()
        {
            switch (TipoAcao)
            {
                case ActionType.Shift:
                    return $"s{this.Estado}";

                case ActionType.Reduce:
                    return $"{this.Reducao}";

                case ActionType.Accept:
                    return "Aceito";

                default:
                    return "Desconhecida";
            }
        }
    }
}
