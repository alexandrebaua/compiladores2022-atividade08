namespace Compilador
{
    partial class FormMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuiFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiExecutar = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiExecuteSyntaxCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuiVerificarCriar = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuiExecuteClean = new System.Windows.Forms.ToolStripMenuItem();
            this.sspMain = new System.Windows.Forms.StatusStrip();
            this.ssplStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tctrlMain = new System.Windows.Forms.TabControl();
            this.tpgMainCode = new System.Windows.Forms.TabPage();
            this.txtCodeTextBox = new Compilador.Componentes.CodeTextBox();
            this.tpgMainInfo = new System.Windows.Forms.TabPage();
            this.tctrlInfo = new System.Windows.Forms.TabControl();
            this.tpgInfoOutputs = new System.Windows.Forms.TabPage();
            this.lstInfoOutputs = new System.Windows.Forms.ListBox();
            this.tpgInfoLexico = new System.Windows.Forms.TabPage();
            this.lstInfoLexico = new System.Windows.Forms.ListBox();
            this.tpgInfoSintatico = new System.Windows.Forms.TabPage();
            this.lstInfoSintatico = new System.Windows.Forms.ListBox();
            this.tpgInfoSintaticoSemantico = new System.Windows.Forms.TabPage();
            this.lstInfoSintaticoSemantico = new System.Windows.Forms.ListBox();
            this.tpgInfoSemantico = new System.Windows.Forms.TabPage();
            this.lstInfoSemantico = new System.Windows.Forms.ListBox();
            this.tpgInfoAAS = new System.Windows.Forms.TabPage();
            this.treeViewAAS = new System.Windows.Forms.TreeView();
            this.tpgInfoCodigo = new System.Windows.Forms.TabPage();
            this.txtInfoSaidaCódigo = new Compilador.Componentes.CodeTextBox();
            this.mnuMain.SuspendLayout();
            this.sspMain.SuspendLayout();
            this.tctrlMain.SuspendLayout();
            this.tpgMainCode.SuspendLayout();
            this.tpgMainInfo.SuspendLayout();
            this.tctrlInfo.SuspendLayout();
            this.tpgInfoOutputs.SuspendLayout();
            this.tpgInfoLexico.SuspendLayout();
            this.tpgInfoSintatico.SuspendLayout();
            this.tpgInfoSintaticoSemantico.SuspendLayout();
            this.tpgInfoSemantico.SuspendLayout();
            this.tpgInfoAAS.SuspendLayout();
            this.tpgInfoCodigo.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuiFile,
            this.mnuiExecutar});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(484, 24);
            this.mnuMain.TabIndex = 0;
            // 
            // mnuiFile
            // 
            this.mnuiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuiFileNew,
            this.mnuiFileOpen,
            this.mnuiFileSave,
            this.mnuiFileSaveAs,
            this.toolStripSeparator1,
            this.mnuiExit});
            this.mnuiFile.Name = "mnuiFile";
            this.mnuiFile.Size = new System.Drawing.Size(61, 20);
            this.mnuiFile.Text = "Arquivo";
            // 
            // mnuiFileNew
            // 
            this.mnuiFileNew.Name = "mnuiFileNew";
            this.mnuiFileNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.mnuiFileNew.Size = new System.Drawing.Size(222, 22);
            this.mnuiFileNew.Text = "Novo";
            this.mnuiFileNew.Click += new System.EventHandler(this.mnuiFileNew_Click);
            // 
            // mnuiFileOpen
            // 
            this.mnuiFileOpen.Name = "mnuiFileOpen";
            this.mnuiFileOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mnuiFileOpen.Size = new System.Drawing.Size(222, 22);
            this.mnuiFileOpen.Text = "Abrir";
            this.mnuiFileOpen.Click += new System.EventHandler(this.mnuiFileOpen_Click);
            // 
            // mnuiFileSave
            // 
            this.mnuiFileSave.Enabled = false;
            this.mnuiFileSave.Name = "mnuiFileSave";
            this.mnuiFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mnuiFileSave.Size = new System.Drawing.Size(222, 22);
            this.mnuiFileSave.Text = "Salvar";
            this.mnuiFileSave.Click += new System.EventHandler(this.mnuiFileSave_Click);
            // 
            // mnuiFileSaveAs
            // 
            this.mnuiFileSaveAs.Name = "mnuiFileSaveAs";
            this.mnuiFileSaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.mnuiFileSaveAs.Size = new System.Drawing.Size(222, 22);
            this.mnuiFileSaveAs.Text = "Salvar Como...";
            this.mnuiFileSaveAs.Click += new System.EventHandler(this.mnuiFileSaveAs_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(219, 6);
            // 
            // mnuiExit
            // 
            this.mnuiExit.Name = "mnuiExit";
            this.mnuiExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.mnuiExit.Size = new System.Drawing.Size(222, 22);
            this.mnuiExit.Text = "Sair";
            this.mnuiExit.Click += new System.EventHandler(this.mnuiExit_Click);
            // 
            // mnuiExecutar
            // 
            this.mnuiExecutar.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuiExecuteSyntaxCheck,
            this.mnuiVerificarCriar,
            this.toolStripSeparator2,
            this.mnuiExecuteClean});
            this.mnuiExecutar.Name = "mnuiExecutar";
            this.mnuiExecutar.Size = new System.Drawing.Size(64, 20);
            this.mnuiExecutar.Text = "Executar";
            // 
            // mnuiExecuteSyntaxCheck
            // 
            this.mnuiExecuteSyntaxCheck.Name = "mnuiExecuteSyntaxCheck";
            this.mnuiExecuteSyntaxCheck.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.mnuiExecuteSyntaxCheck.Size = new System.Drawing.Size(175, 22);
            this.mnuiExecuteSyntaxCheck.Text = "Checar Sintaxe";
            this.mnuiExecuteSyntaxCheck.Click += new System.EventHandler(this.mnuiExecuteSyntaxCheck_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(172, 6);
            // 
            // mnuiVerificarCriar
            // 
            this.mnuiVerificarCriar.Name = "mnuiVerificarCriar";
            this.mnuiVerificarCriar.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.mnuiVerificarCriar.Size = new System.Drawing.Size(175, 22);
            this.mnuiVerificarCriar.Text = "Verificar e Gerar";
            this.mnuiVerificarCriar.Click += new System.EventHandler(this.mnuiVerificarCriar_Click);
            // 
            // mnuiExecuteClean
            // 
            this.mnuiExecuteClean.Name = "mnuiExecuteClean";
            this.mnuiExecuteClean.Size = new System.Drawing.Size(175, 22);
            this.mnuiExecuteClean.Text = "Limpar";
            this.mnuiExecuteClean.Click += new System.EventHandler(this.mnuiExecuteClean_Click);
            // 
            // sspMain
            // 
            this.sspMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ssplStatus});
            this.sspMain.Location = new System.Drawing.Point(0, 419);
            this.sspMain.Name = "sspMain";
            this.sspMain.Size = new System.Drawing.Size(484, 22);
            this.sspMain.TabIndex = 1;
            this.sspMain.Text = "statusStrip1";
            // 
            // ssplStatus
            // 
            this.ssplStatus.Name = "ssplStatus";
            this.ssplStatus.Size = new System.Drawing.Size(39, 17);
            this.ssplStatus.Text = "Status";
            // 
            // tctrlMain
            // 
            this.tctrlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tctrlMain.Controls.Add(this.tpgMainCode);
            this.tctrlMain.Controls.Add(this.tpgMainInfo);
            this.tctrlMain.Location = new System.Drawing.Point(12, 27);
            this.tctrlMain.Name = "tctrlMain";
            this.tctrlMain.SelectedIndex = 0;
            this.tctrlMain.Size = new System.Drawing.Size(460, 389);
            this.tctrlMain.TabIndex = 3;
            // 
            // tpgMainCode
            // 
            this.tpgMainCode.Controls.Add(this.txtCodeTextBox);
            this.tpgMainCode.Location = new System.Drawing.Point(4, 22);
            this.tpgMainCode.Name = "tpgMainCode";
            this.tpgMainCode.Padding = new System.Windows.Forms.Padding(3);
            this.tpgMainCode.Size = new System.Drawing.Size(452, 363);
            this.tpgMainCode.TabIndex = 0;
            this.tpgMainCode.Text = "Código";
            this.tpgMainCode.UseVisualStyleBackColor = true;
            // 
            // txtCodeTextBox
            // 
            this.txtCodeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCodeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtCodeTextBox.EsquemaCores = Compilador.Componentes.ColorSchemaType.Algoritmo;
            this.txtCodeTextBox.Location = new System.Drawing.Point(6, 6);
            this.txtCodeTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.txtCodeTextBox.Name = "txtCodeTextBox";
            this.txtCodeTextBox.Size = new System.Drawing.Size(440, 351);
            this.txtCodeTextBox.TabIndex = 2;
            // 
            // tpgMainInfo
            // 
            this.tpgMainInfo.Controls.Add(this.tctrlInfo);
            this.tpgMainInfo.Location = new System.Drawing.Point(4, 22);
            this.tpgMainInfo.Name = "tpgMainInfo";
            this.tpgMainInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpgMainInfo.Size = new System.Drawing.Size(452, 363);
            this.tpgMainInfo.TabIndex = 1;
            this.tpgMainInfo.Text = "Informações";
            this.tpgMainInfo.UseVisualStyleBackColor = true;
            // 
            // tctrlInfo
            // 
            this.tctrlInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tctrlInfo.Controls.Add(this.tpgInfoOutputs);
            this.tctrlInfo.Controls.Add(this.tpgInfoLexico);
            this.tctrlInfo.Controls.Add(this.tpgInfoSintatico);
            this.tctrlInfo.Controls.Add(this.tpgInfoSintaticoSemantico);
            this.tctrlInfo.Controls.Add(this.tpgInfoSemantico);
            this.tctrlInfo.Controls.Add(this.tpgInfoAAS);
            this.tctrlInfo.Controls.Add(this.tpgInfoCodigo);
            this.tctrlInfo.Location = new System.Drawing.Point(6, 6);
            this.tctrlInfo.Name = "tctrlInfo";
            this.tctrlInfo.SelectedIndex = 0;
            this.tctrlInfo.Size = new System.Drawing.Size(440, 351);
            this.tctrlInfo.TabIndex = 0;
            // 
            // tpgInfoOutputs
            // 
            this.tpgInfoOutputs.Controls.Add(this.lstInfoOutputs);
            this.tpgInfoOutputs.Location = new System.Drawing.Point(4, 22);
            this.tpgInfoOutputs.Name = "tpgInfoOutputs";
            this.tpgInfoOutputs.Padding = new System.Windows.Forms.Padding(3);
            this.tpgInfoOutputs.Size = new System.Drawing.Size(432, 325);
            this.tpgInfoOutputs.TabIndex = 0;
            this.tpgInfoOutputs.Text = "Saida";
            this.tpgInfoOutputs.UseVisualStyleBackColor = true;
            // 
            // lstInfoOutputs
            // 
            this.lstInfoOutputs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstInfoOutputs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstInfoOutputs.FormattingEnabled = true;
            this.lstInfoOutputs.HorizontalScrollbar = true;
            this.lstInfoOutputs.IntegralHeight = false;
            this.lstInfoOutputs.ItemHeight = 15;
            this.lstInfoOutputs.Location = new System.Drawing.Point(6, 6);
            this.lstInfoOutputs.Name = "lstInfoOutputs";
            this.lstInfoOutputs.Size = new System.Drawing.Size(420, 313);
            this.lstInfoOutputs.TabIndex = 0;
            // 
            // tpgInfoLexico
            // 
            this.tpgInfoLexico.Controls.Add(this.lstInfoLexico);
            this.tpgInfoLexico.Location = new System.Drawing.Point(4, 22);
            this.tpgInfoLexico.Name = "tpgInfoLexico";
            this.tpgInfoLexico.Padding = new System.Windows.Forms.Padding(3);
            this.tpgInfoLexico.Size = new System.Drawing.Size(432, 325);
            this.tpgInfoLexico.TabIndex = 1;
            this.tpgInfoLexico.Text = "Léxico";
            this.tpgInfoLexico.UseVisualStyleBackColor = true;
            // 
            // lstInfoLexico
            // 
            this.lstInfoLexico.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstInfoLexico.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstInfoLexico.FormattingEnabled = true;
            this.lstInfoLexico.HorizontalScrollbar = true;
            this.lstInfoLexico.IntegralHeight = false;
            this.lstInfoLexico.ItemHeight = 15;
            this.lstInfoLexico.Location = new System.Drawing.Point(6, 6);
            this.lstInfoLexico.Name = "lstInfoLexico";
            this.lstInfoLexico.Size = new System.Drawing.Size(420, 313);
            this.lstInfoLexico.TabIndex = 0;
            // 
            // tpgInfoSintatico
            // 
            this.tpgInfoSintatico.Controls.Add(this.lstInfoSintatico);
            this.tpgInfoSintatico.Location = new System.Drawing.Point(4, 22);
            this.tpgInfoSintatico.Name = "tpgInfoSintatico";
            this.tpgInfoSintatico.Padding = new System.Windows.Forms.Padding(3);
            this.tpgInfoSintatico.Size = new System.Drawing.Size(432, 325);
            this.tpgInfoSintatico.TabIndex = 2;
            this.tpgInfoSintatico.Text = "Sintático";
            this.tpgInfoSintatico.UseVisualStyleBackColor = true;
            // 
            // lstInfoSintatico
            // 
            this.lstInfoSintatico.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstInfoSintatico.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstInfoSintatico.FormattingEnabled = true;
            this.lstInfoSintatico.HorizontalScrollbar = true;
            this.lstInfoSintatico.IntegralHeight = false;
            this.lstInfoSintatico.ItemHeight = 15;
            this.lstInfoSintatico.Location = new System.Drawing.Point(6, 6);
            this.lstInfoSintatico.Name = "lstInfoSintatico";
            this.lstInfoSintatico.Size = new System.Drawing.Size(420, 313);
            this.lstInfoSintatico.TabIndex = 0;
            // 
            // tpgInfoSintaticoSemantico
            // 
            this.tpgInfoSintaticoSemantico.Controls.Add(this.lstInfoSintaticoSemantico);
            this.tpgInfoSintaticoSemantico.Location = new System.Drawing.Point(4, 22);
            this.tpgInfoSintaticoSemantico.Name = "tpgInfoSintaticoSemantico";
            this.tpgInfoSintaticoSemantico.Size = new System.Drawing.Size(432, 325);
            this.tpgInfoSintaticoSemantico.TabIndex = 3;
            this.tpgInfoSintaticoSemantico.Text = "Sintático/Semântico";
            this.tpgInfoSintaticoSemantico.UseVisualStyleBackColor = true;
            // 
            // lstInfoSintaticoSemantico
            // 
            this.lstInfoSintaticoSemantico.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstInfoSintaticoSemantico.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstInfoSintaticoSemantico.FormattingEnabled = true;
            this.lstInfoSintaticoSemantico.HorizontalScrollbar = true;
            this.lstInfoSintaticoSemantico.IntegralHeight = false;
            this.lstInfoSintaticoSemantico.ItemHeight = 15;
            this.lstInfoSintaticoSemantico.Location = new System.Drawing.Point(6, 6);
            this.lstInfoSintaticoSemantico.Name = "lstInfoSintaticoSemantico";
            this.lstInfoSintaticoSemantico.Size = new System.Drawing.Size(420, 313);
            this.lstInfoSintaticoSemantico.TabIndex = 1;
            // 
            // tpgInfoSemantico
            // 
            this.tpgInfoSemantico.Controls.Add(this.lstInfoSemantico);
            this.tpgInfoSemantico.Location = new System.Drawing.Point(4, 22);
            this.tpgInfoSemantico.Name = "tpgInfoSemantico";
            this.tpgInfoSemantico.Size = new System.Drawing.Size(432, 325);
            this.tpgInfoSemantico.TabIndex = 4;
            this.tpgInfoSemantico.Text = "Semântico";
            this.tpgInfoSemantico.UseVisualStyleBackColor = true;
            // 
            // lstInfoSemantico
            // 
            this.lstInfoSemantico.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstInfoSemantico.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstInfoSemantico.FormattingEnabled = true;
            this.lstInfoSemantico.HorizontalScrollbar = true;
            this.lstInfoSemantico.IntegralHeight = false;
            this.lstInfoSemantico.ItemHeight = 15;
            this.lstInfoSemantico.Location = new System.Drawing.Point(6, 6);
            this.lstInfoSemantico.Name = "lstInfoSemantico";
            this.lstInfoSemantico.Size = new System.Drawing.Size(420, 313);
            this.lstInfoSemantico.TabIndex = 2;
            // 
            // tpgInfoAAS
            // 
            this.tpgInfoAAS.Controls.Add(this.treeViewAAS);
            this.tpgInfoAAS.Location = new System.Drawing.Point(4, 22);
            this.tpgInfoAAS.Name = "tpgInfoAAS";
            this.tpgInfoAAS.Size = new System.Drawing.Size(432, 325);
            this.tpgInfoAAS.TabIndex = 6;
            this.tpgInfoAAS.Text = "AAS";
            this.tpgInfoAAS.UseVisualStyleBackColor = true;
            // 
            // treeViewAAS
            // 
            this.treeViewAAS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewAAS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeViewAAS.Location = new System.Drawing.Point(6, 6);
            this.treeViewAAS.Name = "treeViewAAS";
            this.treeViewAAS.Size = new System.Drawing.Size(420, 313);
            this.treeViewAAS.TabIndex = 0;
            // 
            // tpgInfoCodigo
            // 
            this.tpgInfoCodigo.Controls.Add(this.txtInfoSaidaCódigo);
            this.tpgInfoCodigo.Location = new System.Drawing.Point(4, 22);
            this.tpgInfoCodigo.Name = "tpgInfoCodigo";
            this.tpgInfoCodigo.Size = new System.Drawing.Size(432, 325);
            this.tpgInfoCodigo.TabIndex = 5;
            this.tpgInfoCodigo.Text = "Código";
            this.tpgInfoCodigo.UseVisualStyleBackColor = true;
            // 
            // txtInfoSaidaCódigo
            // 
            this.txtInfoSaidaCódigo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInfoSaidaCódigo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtInfoSaidaCódigo.EsquemaCores = Compilador.Componentes.ColorSchemaType.MIPS;
            this.txtInfoSaidaCódigo.Location = new System.Drawing.Point(6, 6);
            this.txtInfoSaidaCódigo.Name = "txtInfoSaidaCódigo";
            this.txtInfoSaidaCódigo.Size = new System.Drawing.Size(420, 313);
            this.txtInfoSaidaCódigo.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 441);
            this.Controls.Add(this.tctrlMain);
            this.Controls.Add(this.sspMain);
            this.Controls.Add(this.mnuMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnuMain;
            this.MinimumSize = new System.Drawing.Size(420, 430);
            this.Name = "FormMain";
            this.Text = "Compilador";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.sspMain.ResumeLayout(false);
            this.sspMain.PerformLayout();
            this.tctrlMain.ResumeLayout(false);
            this.tpgMainCode.ResumeLayout(false);
            this.tpgMainInfo.ResumeLayout(false);
            this.tctrlInfo.ResumeLayout(false);
            this.tpgInfoOutputs.ResumeLayout(false);
            this.tpgInfoLexico.ResumeLayout(false);
            this.tpgInfoSintatico.ResumeLayout(false);
            this.tpgInfoSintaticoSemantico.ResumeLayout(false);
            this.tpgInfoSemantico.ResumeLayout(false);
            this.tpgInfoAAS.ResumeLayout(false);
            this.tpgInfoCodigo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.StatusStrip sspMain;
        private System.Windows.Forms.ToolStripMenuItem mnuiFile;
        private System.Windows.Forms.ToolStripMenuItem mnuiFileOpen;
        private System.Windows.Forms.ToolStripMenuItem mnuiFileSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuiExit;
        private System.Windows.Forms.ToolStripStatusLabel ssplStatus;
        private Componentes.CodeTextBox txtCodeTextBox;
        private System.Windows.Forms.ToolStripMenuItem mnuiExecutar;
        private System.Windows.Forms.ToolStripMenuItem mnuiExecuteSyntaxCheck;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuiVerificarCriar;
        private System.Windows.Forms.ToolStripMenuItem mnuiExecuteClean;
        private System.Windows.Forms.TabControl tctrlMain;
        private System.Windows.Forms.TabPage tpgMainCode;
        private System.Windows.Forms.TabPage tpgMainInfo;
        private System.Windows.Forms.TabControl tctrlInfo;
        private System.Windows.Forms.TabPage tpgInfoOutputs;
        private System.Windows.Forms.ListBox lstInfoOutputs;
        private System.Windows.Forms.TabPage tpgInfoLexico;
        private System.Windows.Forms.ListBox lstInfoLexico;
        private System.Windows.Forms.TabPage tpgInfoSintatico;
        private System.Windows.Forms.ListBox lstInfoSintatico;
        private System.Windows.Forms.ToolStripMenuItem mnuiFileSaveAs;
        private System.Windows.Forms.ToolStripMenuItem mnuiFileNew;
        private System.Windows.Forms.TabPage tpgInfoSintaticoSemantico;
        private System.Windows.Forms.ListBox lstInfoSintaticoSemantico;
        private System.Windows.Forms.TabPage tpgInfoSemantico;
        private System.Windows.Forms.ListBox lstInfoSemantico;
        private System.Windows.Forms.TabPage tpgInfoCodigo;
        private Componentes.CodeTextBox txtInfoSaidaCódigo;
        private System.Windows.Forms.TabPage tpgInfoAAS;
        private System.Windows.Forms.TreeView treeViewAAS;
    }
}

