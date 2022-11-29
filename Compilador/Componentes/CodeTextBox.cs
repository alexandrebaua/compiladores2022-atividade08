using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Compilador.Exceptions;
using Compilador.Lexico;
using Compilador.Semantico;

namespace Compilador.Componentes
{
    /// <summary>
    /// Enumerador para os tipos de esquemas de definições das linguagens de programação.
    /// </summary>
    public enum ColorSchemaType
    {
        /// <summary>
        /// Sem cores.
        /// </summary>
        Nenhum = 0,

        /// <summary>
        /// Algoritmo
        /// </summary>
        Algoritmo = 1,

        /// <summary>
        /// MIPS Assembly Language
        /// </summary>
        MIPS = 2,

        /// <summary>
        /// C#
        /// </summary>
        CSharp = 3
    }

    public partial class CodeTextBox : UserControl
    {
        private int textHash = 0;

        #region Eventos
        
        /// <summary>
        /// Occurs when the <seealso cref="Control.Text"/> property value changes.
        /// </summary>
        public new event EventHandler TextChanged;

        /// <summary>
        /// Raises the <seealso cref="Control.TextChanged"/> event.
        /// </summary>
        /// <param name="e">An System.EventArgs that contains the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnTextChanged(object sender, EventArgs e)
        {
            if (this.rtxtCodeText.Suspenso)
                return;

            if (this.TextChanged != null)
                this.TextChanged(this, e);
        }
        
        #endregion

        public CodeTextBox()
        {
            //
            // Required for Windows Form Designer support
            //
            this.InitializeComponent();
            //
            // Add any constructor code after InitializeComponent call
            //

            this.rtxtCodeText.TextChanged += new System.EventHandler(this.OnTextChanged);
        }
        
        private void rtxtCodeText_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.rtxtCodeText.Focused)
            {
                this.FormatSyntaticText();
                this.picLineNumbers.Invalidate();
            }
        }

        /// <summary>
        /// Obtém ou define a aparência do texto de código fonte e o comportamento do analisador léxico.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DisplayName("Esquema de Cores")]
        [Description("Define a aparência do texto de código fonte.")]
        public ColorSchemaType EsquemaCores { get; set; }

        /// <summary>
        /// Obtém ou define o texto atual na caixa de texto de código.
        /// </summary>
        /// <returns>O texto mostrado na caixa de código.</returns>
        public override string Text
        {
            get { return this.rtxtCodeText.Text; }

            set
            {
                this.rtxtCodeText.Text = value;

                // Cria um código hash oposto ao hash do texto informado, para garantir a formatação.
                this.textHash = ~value.GetHashCode();

                // Aplica a formatação no texto carregado.
                this.FormatSyntaticText();
                this.picLineNumbers.Invalidate();
            }
        }

        /// <summary>
        /// Seleciona um intervalo de texto na caixa de texto.
        /// </summary>
        /// <param name="inicio">A posição do primeiro caractere na seleção de texto atual na caixa de texto.</param>
        /// <param name="comprimento">O número de caracteres a serem selecionados.</param>
        /// <exception cref="ArgumentOutOfRangeException">O valor do parâmetro inicial é menor que zero.</exception>
        public void Select(int inicio, int comprimento)
        {
            this.rtxtCodeText.Select(inicio, comprimento);
        }

        public void Select(LexicoError[] listaErros)
        {
            // Suspende os controles da caixa de texto.
            this.rtxtCodeText.SuspendPainting();

            // Armazena a posição atual do cursor.
            int cursor = this.rtxtCodeText.SelectionStart;

            foreach (var erro in listaErros)
            {
                this.rtxtCodeText.SelectionStart = erro.Index;
                this.rtxtCodeText.SelectionLength = erro.Lexema.Length;
                this.rtxtCodeText.SelectionBackColor = Color.Pink;
            }

            // Re-posiciona o cursor para a posição memorizada:
            this.rtxtCodeText.SelectionStart = cursor;
            this.rtxtCodeText.SelectionLength = 0;

            // Recupera os controles da caixa de texto.
            this.rtxtCodeText.ResumePainting();
        }

        public void Select(SintaticoException exception)
        {
            if (exception == null)
                return;

            // Suspende os controles da caixa de texto.
            this.rtxtCodeText.SuspendPainting();

            // Armazena a posição atual do cursor.
            int cursor = this.rtxtCodeText.SelectionStart;

            this.rtxtCodeText.SelectionStart = exception.Token.Index;
            this.rtxtCodeText.SelectionLength = exception.Token.Lexema.Length;
            this.rtxtCodeText.SelectionBackColor = Color.Pink;

            // Re-posiciona o cursor para a posição memorizada:
            this.rtxtCodeText.SelectionStart = cursor;
            this.rtxtCodeText.SelectionLength = 0;

            // Recupera os controles da caixa de texto.
            this.rtxtCodeText.ResumePainting();
        }

        public void Select(SemanticoError[] listaErros)
        {
            // Suspende os controles da caixa de texto.
            this.rtxtCodeText.SuspendPainting();

            // Armazena a posição atual do cursor.
            int cursor = this.rtxtCodeText.SelectionStart;

            foreach (var erro in listaErros)
            {
                this.rtxtCodeText.SelectionStart = erro.Index;
                //this.rtxtCodeText.SelectionLength = erro.Lexema.Length;
                this.rtxtCodeText.SelectionLength = erro.Comprimento;
                this.rtxtCodeText.SelectionBackColor = Color.Pink;
            }

            // Re-posiciona o cursor para a posição memorizada:
            this.rtxtCodeText.SelectionStart = cursor;
            this.rtxtCodeText.SelectionLength = 0;

            // Recupera os controles da caixa de texto.
            this.rtxtCodeText.ResumePainting();
        }

        /// <summary>
        /// Executa a formatação automática do texto sintático.
        /// </summary>
        private void FormatSyntaticText()
        {
            if (this.EsquemaCores == ColorSchemaType.Nenhum)
                return;

            if (this.rtxtCodeText.Text.GetHashCode() == this.textHash)
                return;
            this.textHash = this.rtxtCodeText.Text.GetHashCode();

            // Fontes padrões da caixa de texto.
            Font fontDefault = new Font("Lucida Sans Typewriter", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            Font fontBold = new Font(fontDefault, FontStyle.Bold);

            // Armazena a posição atual do cursor.
            int cursor = this.rtxtCodeText.SelectionStart;

            // Suspende os controles da caixa de texto.
            this.rtxtCodeText.SuspendPainting();

            // Aplica a fonte padrão na caixa de texto.
            this.rtxtCodeText.Font = fontDefault;

            // Limpa toda a formatação do texto:
            this.rtxtCodeText.SelectAll();
            this.rtxtCodeText.SelectionFont = fontDefault;
            this.rtxtCodeText.SelectionColor = Color.Black;
            this.rtxtCodeText.SelectionBackColor = this.rtxtCodeText.BackColor;

            switch (this.EsquemaCores)
            {
                case ColorSchemaType.Algoritmo:
                    this.EsquemaCoresAlgoritmo(fontDefault, fontBold);
                    break;

                case ColorSchemaType.MIPS:
                    this.EsquemaCoresMIPS(fontDefault, fontBold);
                    break;

                case ColorSchemaType.CSharp:
                    this.EsquemaCoresCSharp(fontDefault, fontBold);
                    break;
            }

            // Reseta as cores e fonte para seleção do texto:
            this.rtxtCodeText.SelectionLength = 0;
            this.rtxtCodeText.SelectionFont = fontDefault;
            this.rtxtCodeText.SelectionColor = Color.Black;
            this.rtxtCodeText.SelectionBackColor = this.rtxtCodeText.BackColor;

            // Re-posiciona o cursor para a posição memorizada:
            this.rtxtCodeText.SelectionStart = cursor;
            this.rtxtCodeText.SelectionLength = 0;

            // Recupera os controles da caixa de texto.
            this.rtxtCodeText.ResumePainting();
        }

        private void EsquemaCoresAlgoritmo(Font fontDefault, Font fontBold)
        {

            // Realça os elementos numéricos no texto:
            //MatchCollection matches = Regex.Matches(this.rtxtCodeText.Text, @"\b(\d+([.|,]\d+)?)\b");
            MatchCollection matches = Regex.Matches(this.rtxtCodeText.Text, @"\b(\d+([.]\d+)?)\b");
            foreach (Match match in matches)
            {
                this.rtxtCodeText.SelectionStart = match.Index;
                this.rtxtCodeText.SelectionLength = match.Length;
                this.rtxtCodeText.SelectionColor = Color.DarkOrange;
            }

            // Realça as palavras reservadas no texto:
            //matches = Regex.Matches(this.rtxtCodeText.Text, @"\b(int|real|string|void)\b");
            matches = Regex.Matches(this.rtxtCodeText.Text, @"\b(caracter|inteiro|lógico|real|vetor)\b");
            foreach (Match match in matches)
            {
                this.rtxtCodeText.SelectionStart = match.Index;
                this.rtxtCodeText.SelectionLength = match.Length;
                this.rtxtCodeText.SelectionFont = fontBold;
                this.rtxtCodeText.SelectionColor = Color.Purple;
            }

            matches = Regex.Matches(this.rtxtCodeText.Text, @"\b(de|div|e|mod|não|ou)\b");
            foreach (Match match in matches)
            {
                this.rtxtCodeText.SelectionStart = match.Index;
                this.rtxtCodeText.SelectionLength = match.Length;
                this.rtxtCodeText.SelectionFont = fontBold;
                this.rtxtCodeText.SelectionColor = Color.RoyalBlue;
            }

            //matches = Regex.Matches(this.rtxtCodeText.Text, @"\b(if|else|do|while|return)\b");
            matches = Regex.Matches(this.rtxtCodeText.Text, @"\b(algoritmo|até|caso|decresente|enquanto|então|escreva|escrevaln|faça|falso|fim|fim_enquanto|fim_faça|fim_para|fim_se|função|inicio|leia|para|procedimento|que|repita|retorne|se|senão|var|variáveis|verdadeiro)\b");
            foreach (Match match in matches)
            {
                this.rtxtCodeText.SelectionStart = match.Index;
                this.rtxtCodeText.SelectionLength = match.Length;
                this.rtxtCodeText.SelectionFont = fontBold;
                this.rtxtCodeText.SelectionColor = Color.Blue;
            }
            
            // Realça os comentários de linha simples:
            //matches = Regex.Matches(this.rtxtCodeText.Text, @"//(.*?)(\n|$)");
            matches = Regex.Matches(this.rtxtCodeText.Text, @"//.*$", RegexOptions.Multiline);
            foreach (Match match in matches)
            {
                this.rtxtCodeText.SelectionStart = match.Index;
                this.rtxtCodeText.SelectionLength = match.Length;
                this.rtxtCodeText.SelectionFont = fontDefault;
                this.rtxtCodeText.SelectionColor = Color.Green;
            }

            // Realça os comentários de múltiplas linhas:
            //matches = Regex.Matches(this.rtxtCodeText.Text, @"/\*(.*?|\n)(\* /|$)");
            matches = Regex.Matches(this.rtxtCodeText.Text, @"(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline);
            //matches = Regex.Matches(this.rtxtCodeText.Text, @"(/\*.*?\*/)|(.*\*/)", RegexOptions.Singleline | RegexOptions.RightToLeft);
            foreach (Match match in matches)
            {
                this.rtxtCodeText.SelectionStart = match.Index;
                this.rtxtCodeText.SelectionLength = match.Length;
                this.rtxtCodeText.SelectionFont = fontDefault;
                this.rtxtCodeText.SelectionColor = Color.Green;
            }

            // Realça os textos:
            //matches = Regex.Matches(this.rtxtCodeText.Text, @"""(.*?)(""|\n|$)");
            //matches = Regex.Matches(this.rtxtCodeText.Text, @"("".*?""|\n|$)");
            //matches = Regex.Matches(this.rtxtCodeText.Text, @""".*?(""|\n|$)");
            //matches = Regex.Matches(this.rtxtCodeText.Text, @""".*?(""|$)", RegexOptions.Multiline);
            //matches = Regex.Matches(this.rtxtCodeText.Text, @"("".*?"")|("".*)", RegexOptions.Singleline);
            matches = Regex.Matches(this.rtxtCodeText.Text, @"("".*?"")|("".*)", RegexOptions.Multiline);
            foreach (Match match in matches)
            {
                this.rtxtCodeText.SelectionStart = match.Index;
                this.rtxtCodeText.SelectionLength = 0;

                // Apenas altera a formatação se ainda não foi alterada:
                if (this.rtxtCodeText.SelectionColor == Color.Black)
                {
                    this.rtxtCodeText.SelectionLength = match.Length;
                    this.rtxtCodeText.SelectionFont = fontBold;
                    this.rtxtCodeText.SelectionColor = Color.DarkGray;
                }
            }
        }

        private void EsquemaCoresMIPS(Font fontDefault, Font fontBold)
        {
            // Realça as diretrivas:
            //MatchCollection matches = Regex.Matches(this.rtxtCodeText.Text, @"([.]\w+)(\b|\n|$)");
            //MatchCollection matches = Regex.Matches(this.rtxtCodeText.Text, @"([.]\D\w+)(\b|\n|$)");
            MatchCollection matches = Regex.Matches(this.rtxtCodeText.Text, @"([.]\D\w+)(\b|\n|$)", RegexOptions.Multiline);
            foreach (Match match in matches)
            {
                this.rtxtCodeText.SelectionStart = match.Index;
                this.rtxtCodeText.SelectionLength = match.Length;
                this.rtxtCodeText.SelectionFont = fontDefault;
                this.rtxtCodeText.SelectionColor = Color.DeepPink;
            }

            // Realça as palavras reservadas no texto:
            matches = Regex.Matches(this.rtxtCodeText.Text, @"\b(syscall|add|addi|addu|sub|subi|subu|addiu|mul|mult|div|and|andi|or|ori|xori|not|sll|sllv|srl|srlv|lw|sw|lb|sb|lui|la|li|mfhi|mflo|move|mov|beq|bne|bgt|bge|blt|ble|beqz|sgt|slt|slti|sge|sle|seq|sne|j|jr|jal|lwc1|swc1|l|mtc1|cvt|.s|.w)\b");
            foreach (Match match in matches)
            {
                this.rtxtCodeText.SelectionStart = match.Index;
                this.rtxtCodeText.SelectionLength = match.Length;
                this.rtxtCodeText.SelectionFont = fontBold;
                this.rtxtCodeText.SelectionColor = Color.Blue;
            }

            // Realça as registradores:
            matches = Regex.Matches(this.rtxtCodeText.Text, @"([$]\w+)(\b|\n|$)");
            foreach (Match match in matches)
            {
                this.rtxtCodeText.SelectionStart = match.Index;
                this.rtxtCodeText.SelectionLength = match.Length;
                this.rtxtCodeText.SelectionFont = fontDefault;
                this.rtxtCodeText.SelectionColor = Color.Red;
            }

            // Realça os comentários:
            matches = Regex.Matches(this.rtxtCodeText.Text, @"#.*$", RegexOptions.Multiline);
            foreach (Match match in matches)
            {
                this.rtxtCodeText.SelectionStart = match.Index;
                this.rtxtCodeText.SelectionLength = match.Length;
                this.rtxtCodeText.SelectionFont = fontDefault;
                this.rtxtCodeText.SelectionColor = Color.ForestGreen;
            }

            // Realça os textos:
            matches = Regex.Matches(this.rtxtCodeText.Text, @"("".*?"")|("".*)", RegexOptions.Multiline);
            foreach (Match match in matches)
            {
                this.rtxtCodeText.SelectionStart = match.Index;
                this.rtxtCodeText.SelectionLength = match.Length;
                this.rtxtCodeText.SelectionFont = fontDefault;
                this.rtxtCodeText.SelectionColor = Color.SeaGreen;
            }
        }

        private void EsquemaCoresCSharp(Font fontDefault, Font fontBold)
        {

        }
        
        #region Enumeração linhas caixa de código

        private void DrawRichTextBoxLineNumbers(Graphics g)
        {
            // calculate font heigth as the difference in Y coordinate between line 2 and line 1
            float font_height = this.rtxtCodeText.Font.Height + 2;
            if (this.rtxtCodeText.Lines.Length > 1)
                font_height = this.rtxtCodeText.GetPositionFromCharIndex(this.rtxtCodeText.GetFirstCharIndexFromLine(1)).Y - this.rtxtCodeText.GetPositionFromCharIndex(this.rtxtCodeText.GetFirstCharIndexFromLine(0)).Y;

            if (font_height <= 0)
                return;

            // Get the first line index and location
            int firstIndex = this.rtxtCodeText.GetCharIndexFromPosition(new Point(0, (int)(g.VisibleClipBounds.Y + font_height / 3)));
            int firstLine = this.rtxtCodeText.GetLineFromCharIndex(firstIndex);
            int firstLineY = this.rtxtCodeText.GetPositionFromCharIndex(firstIndex).Y;

            // Get the last line count
            int lastLine = this.rtxtCodeText.Lines.Length - firstLine;
            if (lastLine <= 0)
                lastLine = 1;

            // Print on the PictureBox the visible line numbers of the RichTextBox
            //g.Clear(Control.DefaultBackColor);
            int i = firstLine;
            float y = 0;
            while (y < g.VisibleClipBounds.Y + g.VisibleClipBounds.Height && lastLine-- > 0)
            {
                y = firstLineY + 4 + font_height * (i - firstLine);
                i++;
                g.DrawString(i.ToString(), this.rtxtCodeText.Font, Brushes.DarkBlue, this.picLineNumbers.Width - g.MeasureString(i.ToString(), this.rtxtCodeText.Font).Width - 3, y);
            }
        }

        private void picLineNumbers_Paint(object sender, PaintEventArgs e)
        {
            this.DrawRichTextBoxLineNumbers(e.Graphics);
        }

        private void rtxtCodeText_VScroll(object sender, EventArgs e)
        {
            this.picLineNumbers.Invalidate();
        }

        #endregion
    }
}
