namespace Compilador.Componentes
{
    partial class CodeTextBox
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picLineNumbers = new System.Windows.Forms.PictureBox();
            this.rtxtCodeText = new Compilador.Componentes.RichTextBoxEx();
            ((System.ComponentModel.ISupportInitialize)(this.picLineNumbers)).BeginInit();
            this.SuspendLayout();
            // 
            // picLineNumbers
            // 
            this.picLineNumbers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.picLineNumbers.BackColor = System.Drawing.Color.Silver;
            this.picLineNumbers.Location = new System.Drawing.Point(0, 0);
            this.picLineNumbers.Name = "picLineNumbers";
            this.picLineNumbers.Size = new System.Drawing.Size(35, 142);
            this.picLineNumbers.TabIndex = 0;
            this.picLineNumbers.TabStop = false;
            this.picLineNumbers.Paint += new System.Windows.Forms.PaintEventHandler(this.picLineNumbers_Paint);
            // 
            // rtxtCodeText
            // 
            this.rtxtCodeText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtCodeText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtCodeText.DetectUrls = false;
            this.rtxtCodeText.Location = new System.Drawing.Point(38, 2);
            this.rtxtCodeText.Name = "rtxtCodeText";
            this.rtxtCodeText.Size = new System.Drawing.Size(105, 140);
            this.rtxtCodeText.TabIndex = 1;
            this.rtxtCodeText.Text = "";
            this.rtxtCodeText.WordWrap = false;
            this.rtxtCodeText.VScroll += new System.EventHandler(this.rtxtCodeText_VScroll);
            this.rtxtCodeText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.rtxtCodeText_KeyUp);
            // 
            // CodeTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.rtxtCodeText);
            this.Controls.Add(this.picLineNumbers);
            this.Name = "CodeTextBox";
            this.Size = new System.Drawing.Size(146, 146);
            ((System.ComponentModel.ISupportInitialize)(this.picLineNumbers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picLineNumbers;
        private RichTextBoxEx rtxtCodeText;
    }
}
