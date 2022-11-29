using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Compilador.GeradorCódigo.MIPS.MipsData;

namespace Compilador.GeradorCódigo.MIPS
{
    /// <summary>
    /// Classe do manipulador auxiliar para variáveis lógicas booleanas.
    /// </summary>
    public class MipsLógico
    {
        #region Variáveis Privadas
        private Dictionary<string, int> _dicVariáveis = new Dictionary<string, int>();
        private Dictionary<string, int> _dicVetor = new Dictionary<string, int>();
        private int _index = 0, _jumpCount = 0;
        private MipsClass _mips;
        #endregion

        /// <summary>
        /// O construtor da classe.
        /// </summary>
        /// <param name="mips"></param>
        public MipsLógico(MipsClass mips)
        {
            this._mips = mips;
        }

        /// <summary>
        /// Obtém o indicador de utilização de variáveis lógicas.
        /// </summary>
        public bool Utilizado { get { return this._dicVariáveis.Count != 0 || this._dicVetor.Count != 0; } }

        #region Variáveis

        /// <summary>
        /// Adiciona uma nova variável lógica.
        /// </summary>
        /// <param name="nome">O nome para a variável.</param>
        public void AdicionarVariável(string nome)
        {
            this._dicVariáveis.Add(nome, this._index++);
        }

        /// <summary>
        /// Seta ou reseta o valor lógico da variável, conforme o valor (0~1) armazenado no registrador informado.
        /// </summary>
        /// <param name="registrador">O registrador contendo o valor lógico.</param>
        /// <param name="nome">O nome da variável.</param>
        public void SetarVariável(string registrador, string nome)
        {
            if (!this._dicVariáveis.ContainsKey(nome))
                return;

            // Obtém o índice onde a variável está armazenada.
            int index = this._dicVariáveis[nome];

            // Se foram utilizadas menos de 8 variáveis lógicas, foi consumido apenas 1 byte de memória para armazenar, então:
            if (this._index < 8)
            {
                //mips.SectionText.Adicionar(new MipsText("li", "$t6,0"));
                this._mips.SectionText.Adicionar(new MipsText("lb", "$t7,varsLog"));

                this.SetIndex(registrador, index);

                // Salva na memória
                this._mips.SectionText.Adicionar(new MipsText("sb", $"{registrador},varsLog"));

                this._jumpCount++;
                return;
            }

            // Foram utilizadas 8 ou mais variáveis lógicas, foi consumido múltiplos bytes de memória para armazenar, então está armazenado em um array de bytes:
            int bitIndex = index % 8;
            int byteIndex = (index - bitIndex) / 8;

            this._mips.SectionText.Adicionar(new MipsText("la", "$t5,varsLog"));   // Endereço do vetor em memória

            if (byteIndex == 0)
                this._mips.SectionText.Adicionar(new MipsText("lb", "$t7,($t5)"));  // Carrega o valor no registro do array para o registrador $t6
            else
                this._mips.SectionText.Adicionar(new MipsText("lb", $"$t7,{byteIndex}($t5)"));  // Carrega o valor no registro do array para o registrador $t6

            this.SetIndex(registrador, bitIndex);

            if (byteIndex == 0)
                this._mips.SectionText.Adicionar(new MipsText("sb", $"{registrador},($t5)"));  // Move o resultado do registrador para o registro no array
            else
                this._mips.SectionText.Adicionar(new MipsText("sb", $"{registrador},{byteIndex}($t5)"));  // Move o resultado do registrador para o registro no array

            this._jumpCount++;
        }

        /// <summary>
        /// Seta a indexação da posição do bit onde está armazenado a variável lógica.
        /// </summary>
        /// <param name="registrador">O registrador contendo o valor lógico.</param>
        /// <param name="index">A posição do bit dentro do byte.</param>
        private void SetIndex(string registrador, int index)
        {
            // Se for necessário resetar o bit
            this._mips.SectionText.Adicionar(new MipsText("beqz", $"{registrador},jmpLogZero{index}_{this._jumpCount}"));

            if (index != 0)
                this._mips.SectionText.Adicionar(new MipsText("sll", $"{registrador},{registrador},{index}"));

            // Seta o bit
            this._mips.SectionText.Adicionar(new MipsText("or", $"{registrador},{registrador},$t7"));
            this._mips.SectionText.Adicionar(new MipsText("j", $"jmpLog{index}_{this._jumpCount}"));

            // Reseta o bit
            this._mips.SectionText.Adicionar(new MipsText($"jmpLogZero{index}_{this._jumpCount}:", null));

            this._mips.SectionText.Adicionar(new MipsText("li", $"{registrador},1"));

            if (index != 0)
                this._mips.SectionText.Adicionar(new MipsText("sll", $"{registrador},{registrador},{index}"));

            this._mips.SectionText.Adicionar(new MipsText("not", $"{registrador},{registrador}"));
            this._mips.SectionText.Adicionar(new MipsText("and", $"{registrador},{registrador},$t7"));

            this._mips.SectionText.Adicionar(new MipsText($"jmpLog{index}_{this._jumpCount}:", null));
        }

        /// <summary>
        /// Obtém o valor lógico (0~1) da variável, sendo armazenado no registrador informado.
        /// </summary>
        /// <param name="registrador">O registrador contendo o valor lógico.</param>
        /// <param name="nome">O nome da variável.</param>
        public void ObterVariável(string registrador, string nome)
        {
            if (!this._dicVariáveis.ContainsKey(nome))
                return;

            // Obtém o índice onde a variável está armazenada.
            int index = this._dicVariáveis[nome];

            // Se foram utilizadas menos de 8 variáveis lógicas, foi consumido apenas 1 byte de memória para armazenar, então:
            if (this._index < 8)
            {
                this._mips.SectionText.Adicionar(new MipsText("lb", $"{registrador},varsLog"));
                if (index != 0)
                    this._mips.SectionText.Adicionar(new MipsText("srl", $"{registrador},{registrador},{index}"));

                this._mips.SectionText.Adicionar(new MipsText("andi", $"{registrador},{registrador},0x01"));
                return;
            }

            // Foram utilizadas 8 ou mais variáveis lógicas, foi consumido múltiplos bytes de memória para armazenar, então está armazenado em um array de bytes:
            int byteIndex = index / 8;
            int bitIndex = index % 8;

            this._mips.SectionText.Adicionar(new MipsText("la", "$t5,varsLog"));   // Endereço do vetor em memória

            if (byteIndex == 0)
                this._mips.SectionText.Adicionar(new MipsText("lb", $"{registrador},($t5)"));  // Carrega o valor no registro do array para o registrador
            else
                this._mips.SectionText.Adicionar(new MipsText("lb", $"{registrador},{byteIndex}($t5)"));  // Carrega o valor no registro do array para o registrador

            if (bitIndex != 0)
                this._mips.SectionText.Adicionar(new MipsText("srl", $"{registrador},{registrador},{bitIndex}"));

            this._mips.SectionText.Adicionar(new MipsText("andi", $"{registrador},{registrador},0x01"));
        }

        #endregion

        #region Vetores

        /// <summary>
        /// Adiciona um novo vetor lógico.
        /// </summary>
        /// <param name="nome">O nome para o vetor.</param>
        /// <param name="tamanho">A quantia de registros do vetor.</param>
        public void AdicionarVetor(string nome, int tamanho)
        {
            this._dicVetor.Add(nome, tamanho);
        }

        /// <summary>
        /// Seta o endereço do byte do registro aonde se encontra o valor lógico (bit) armazenado.
        /// </summary>
        /// <param name="nome">O nome do vetor.</param>
        public void SetaVetorPosição(string nome)
        {
            if (!this._dicVetor.ContainsKey(nome))
                return;

            // Obtém o tamanho do vetor.
            int tamanho = this._dicVetor[nome];

            // Se o vetor possuir possuir menos que 8 registros, não necessita tratamentos extras, então retorna:
            if (tamanho < 8)
                return;
            
            this._mips.SectionText.Adicionar(new MipsText("div", "$t6,$t6,8"));    // Cada registro armazena armazena 1 byte (8 bits), acessa o registro do array correspondente ao bit requerido 

            this._mips.SectionText.Adicionar(new MipsText("la", $"$t5,{nome}"));   // Endereço do vetor em memória
            this._mips.SectionText.Adicionar(new MipsText("add", "$t5,$t5,$t6"));  // Soma o endereço do array com a posição do registro calculado

            this._mips.SectionText.Adicionar(new MipsText("mfhi", "$t6"));         // Armazena o resto da divisão, este valor é a posição do bit
        }

        /// <summary>
        /// Seta ou reseta o valor lógico do registro no vetor, conforme o valor (0~1) armazenado no registrador informado.
        /// </summary>
        /// <param name="registrador">O registrador contendo o valor lógico.</param>
        /// <param name="nome">O nome do vetor.</param>
        public void SetaVetorValor(string registrador, string nome)
        {
            if (!this._dicVetor.ContainsKey(nome))
                return;

            // Obtém o tamanho do vetor.
            int tamanho = this._dicVetor[nome];

            // Se foram utilizadas menos de 8 variáveis lógicas, foi consumido apenas 1 byte de memória para armazenar, então:
            if (tamanho < 8)
            {
                this._mips.SectionText.Adicionar(new MipsText("lb", $"$t7,{nome}"));

                this.SetRegistrador(registrador);

                // Salva na memória
                this._mips.SectionText.Adicionar(new MipsText("sb", $"{registrador},{nome}"));

                this._jumpCount++;
                return;
            }

            // Foram utilizadas 8 ou mais variáveis lógicas, foi consumido múltiplos bytes de memória para armazenar, então está armazenado em um array de bytes:

            this._mips.SectionText.Adicionar(new MipsText("lb", "$t7,($t5)"));  // Carrega o valor no registro do array para o registrador $t7

            this.SetRegistrador(registrador);

            this._mips.SectionText.Adicionar(new MipsText("sb", $"{registrador},($t5)"));  // Move o resultado do registrador para o registro no array

            this._jumpCount++;
        }

        /// <summary>
        /// Obtém o valor lógico (0~1) do registro no vetor, sendo armazenado no registrador informado.
        /// </summary>
        /// <param name="registrador">O registrador contendo o valor lógico.</param>
        /// <param name="nome">O nome do vetor.</param>
        public void ObterVetorValor(string registrador, string nome)
        {
            if (!this._dicVetor.ContainsKey(nome))
                return;

            // Obtém o tamanho do vetor.
            int tamanho = this._dicVetor[nome];

            // Se foram utilizadas menos de 8 variáveis lógicas, foi consumido apenas 1 byte de memória para armazenar, então:
            if (tamanho < 8)
            {
                this._mips.SectionText.Adicionar(new MipsText("lb", $"{registrador},{nome}"));
                this._mips.SectionText.Adicionar(new MipsText("srlv", $"{registrador},{registrador},$t6"));
                this._mips.SectionText.Adicionar(new MipsText("andi", $"{registrador},{registrador},0x01"));
                return;
            }

            // Foram utilizadas 8 ou mais variáveis lógicas, foi consumido múltiplos bytes de memória para armazenar, então está armazenado em um array de bytes:

            this._mips.SectionText.Adicionar(new MipsText("lb", $"{registrador},($t5)"));  // Carrega o valor no registro do array para o registrador
            this._mips.SectionText.Adicionar(new MipsText("srlv", $"{registrador},{registrador},$t6"));
            this._mips.SectionText.Adicionar(new MipsText("andi", $"{registrador},{registrador},0x01"));
        }

        /// <summary>
        /// Seta ou reseta o valor lógico selecionado, conforme o valor (0~1) armazenado no registrador informado.
        /// </summary>
        /// <param name="registrador">O registrador contendo o valor lógico.</param>
        private void SetRegistrador(string registrador)
        {
            // Se for necessário resetar o bit
            this._mips.SectionText.Adicionar(new MipsText("beqz", $"{registrador},jmpLogZero_{this._jumpCount}"));

            this._mips.SectionText.Adicionar(new MipsText("sllv", $"{registrador},{registrador},$t6"));

            // Seta o bit
            this._mips.SectionText.Adicionar(new MipsText("or", $"{registrador},{registrador},$t7"));
            this._mips.SectionText.Adicionar(new MipsText("j", $"jmpLog_{this._jumpCount}"));

            // Reseta o bit
            this._mips.SectionText.Adicionar(new MipsText($"jmpLogZero_{this._jumpCount}:", null));

            this._mips.SectionText.Adicionar(new MipsText("li", $"{registrador},1"));

            this._mips.SectionText.Adicionar(new MipsText("sllv", $"{registrador},{registrador},$t6"));

            this._mips.SectionText.Adicionar(new MipsText("not", $"{registrador},{registrador}"));
            this._mips.SectionText.Adicionar(new MipsText("and", $"{registrador},{registrador},$t7"));

            this._mips.SectionText.Adicionar(new MipsText($"jmpLog_{this._jumpCount}:", null));
        }

        #endregion

        /// <summary>
        /// Obtém o código Assembly MIPS que representa o objeto atual.
        /// </summary>
        public string ObterCódigo()
        {
            if (this._dicVariáveis.Count == 0 && this._dicVetor.Count == 0)
                return null;

            List<MipsDataByte> memLog = new List<MipsDataByte>();
            if (this._dicVetor.Count > 0)
            {
                foreach (var vetor in this._dicVetor)
                {
                    memLog.Add(new MipsDataByte(vetor.Key, this.ObterQuantiaBytes(vetor.Value)));
                }
            }

            if (this._dicVariáveis.Count > 0)
            {
                memLog.Add(new MipsDataByte($"varsLog", this.ObterQuantiaBytes(this._index)));
            }

            string strData = String.Empty;
            foreach (var vetor in memLog)
                strData += $"  {vetor}{Environment.NewLine}";

            return strData;
        }

        /// <summary>
        /// Obtém a quantidade de bytes necessários para armazenar a quantia de registros informado.
        /// </summary>
        /// <param name="registros">A quantia de registros lógicos a serem armazenados.</param>
        /// <returns>A quantia de bytes necessário.</returns>
        private int ObterQuantiaBytes(int registros)
        {
            int bitLen = registros % 8;
            int byteLen = (registros - bitLen) / 8;
            if (bitLen != 0)
                byteLen++;

            return byteLen;
        }
    }
}
