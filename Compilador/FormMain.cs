using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Compilador.Exceptions;
using Compilador.GeradorCódigo.MIPS;
using Compilador.Lexico;
using Compilador.Semantico;
using Compilador.Sintatico.Ascendente.SLR;

namespace Compilador
{
    public partial class FormMain : Form
    {
        private string _codeFilePath = null;

        /// <summary>
        /// The class constructor.
        /// </summary>
        public FormMain()
        {
            //
            // Required for Windows Form Designer support
            //
            this.InitializeComponent();
            //
            // Add any constructor code after InitializeComponent call
            //

            this.txtCodeTextBox.TextChanged += new System.EventHandler(this.txtCodeTextBox_TextChanged);
        }

        /// <summary>
        /// Evento 'Load' da janela principal do sistema.
        /// </summary>
        private void FormMain_Load(object sender, EventArgs e)
        {
            // Carrega as configurações de sistema.
            AppConfig.LoadAppConfig();

            // Inicializa as dimensões da Janela Principal.
            this.Size = AppConfig.MyFormMainSize;

            // Inicializa a localização da Janela Principal.
            this.Location = AppConfig.MyFormMainLocation;

            // Atualiza mensagem na barra de status.
            this.ssplStatus.Text = "Pronto";

            // Atualiza o título do formulário.
            this.AtualizarTítuloFormulário();
        }

        /// <summary>
        /// Evento 'FormClosing' da janela principal do sistema.
        /// </summary>
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.mnuiFileSave.Enabled)
            {
                DialogResult result = MessageBox.Show("Existem alterações não salvas, deseja salvar as alterações antes de fechar?", "Salvar?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (!this.SalvarArquivo())
                        e.Cancel = true;
                }
                else if (result == DialogResult.Cancel)
                    e.Cancel = true;
            }

            if (this.WindowState != FormWindowState.Maximized)
            {
                // Salva as dimensões da Janela Principal.
                AppConfig.MyFormMainSize = this.Size;

                // Salva a localização da Janela Principal.
                AppConfig.MyFormMainLocation = this.Location;
            }

            // Salva as configurações de sistema.
            AppConfig.SaveAppConfig();
        }

        #region Menu do sistema

        private void mnuiFileNew_Click(object sender, EventArgs e)
        {
            if (this.mnuiFileSave.Enabled)
            {
                DialogResult result = MessageBox.Show("Deseja salvar as alterações?", "Salvar?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (!this.SalvarArquivo())
                        return;
                }
                else if (result == DialogResult.Cancel)
                    return;
            }

            this.LimparListasInformações();
            this.txtCodeTextBox.Text = String.Empty;
            this._codeFilePath = null;
            this.LimparAlteraçõesNãoSalvas();
            this.AtualizarTítuloFormulário();
        }

        private void mnuiFileOpen_Click(object sender, EventArgs e)
        {
            // Cria e configura a janela de diálogo da abertura de arquivos:
            OpenFileDialog dlgOpenFile = new OpenFileDialog();
            if (this._codeFilePath != null)
                dlgOpenFile.FileName = Path.GetFileNameWithoutExtension(this._codeFilePath);   // Nome padrão do arquivo.
            dlgOpenFile.DefaultExt = ".txt";  // Extensão padrão do arquivo.
            dlgOpenFile.Filter = "Algoritmo Code (*.alg)|*.alg|All Files (*.*)|*.*";   // Filtra arquivos por extensão.

            // Verifica a existência do diretório do último arquivo carregado:
            if (Directory.Exists(AppConfig.MyFormLastSearchDirectory))
                dlgOpenFile.InitialDirectory = AppConfig.MyFormLastSearchDirectory;  // Diretório do último arquivo carregado.
            else
                dlgOpenFile.InitialDirectory = AppConfig.AppPath; // Diretório padrão.

            // Mostra a janela de diálogo da abertura de arquivos, e se foi pressionado o botão OK:
            if (dlgOpenFile.ShowDialog() == DialogResult.OK)
            {
                // Memoriza o diretório de onde foi carregado o arquivo.
                AppConfig.MyFormLastSearchDirectory = Path.GetDirectoryName(@dlgOpenFile.FileName);

                // Salva o caminho para o arquivo.
                this._codeFilePath = @dlgOpenFile.FileName;

                // Carrega o arquivo na caixa de código.
                this.txtCodeTextBox.Text = File.ReadAllText(@dlgOpenFile.FileName);

                // Atualiza mensagem na barra de status.
                this.ssplStatus.Text = $"Arquivo '{Path.GetFileName(@dlgOpenFile.FileName)}' aberto com sucesso!";

                // Executa a limpeza:
                this.LimparListasInformações();
                this.LimparAlteraçõesNãoSalvas();
                this.AtualizarTítuloFormulário();

                // Seleciona a guia de código fonte.
                this.tctrlMain.SelectedIndex = 0;
            }

            dlgOpenFile.Dispose();
        }

        private void mnuiFileSave_Click(object sender, EventArgs e)
        {
            this.SalvarArquivo();
        }

        private void mnuiFileSaveAs_Click(object sender, EventArgs e)
        {
            this.SalvarArquivoComo();
        }

        private void mnuiExit_Click(object sender, EventArgs e)
        {
            // Requisita a saída do programa.
            Application.Exit();
        }

        private void mnuiExecuteSyntaxCheck_Click(object sender, EventArgs e)
        {
            if (this.ChecarSintaxe(true))
            {
                // Atualiza mensagem na barra de status.
                this.ssplStatus.Text = "Verificação completada com sucesso!";

                MessageBox.Show("Verificação completada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void mnuiVerificarCriar_Click(object sender, EventArgs e)
        {
            if (!this.ChecarSintaxe(false))
            {
                MessageBox.Show($"Erro durante validação do código!{Environment.NewLine}Compilação abortada.", "Abortado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Atualiza mensagem na barra de status.
            this.ssplStatus.Text = "Geração completada com sucesso!";

            MessageBox.Show("Geração código MIPS completado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void mnuiExecuteClean_Click(object sender, EventArgs e)
        {
            this.LimparListasInformações();

            // Atualiza mensagem na barra de status.
            this.ssplStatus.Text = "Pronto";
        }

        #endregion

        private void txtCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            this.MarcarAlteraçõesNãoSalvas();
        }

        private bool SalvarArquivo()
        {
            if (this._codeFilePath == null)
                return this.SalvarArquivoComo();

            try
            {
                // Testa se o arquivo existe e tenta apagar o arquivo:
                if (File.Exists(this._codeFilePath))
                    File.Delete(this._codeFilePath);

                // Salva o texto do analisador sintático para o arquivo de texto especificado.
                File.WriteAllText(this._codeFilePath, this.txtCodeTextBox.Text);

                this.LimparAlteraçõesNãoSalvas();
                this.AtualizarTítuloFormulário();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool SalvarArquivoComo()
        {
            // Cria e configura a janela de diálogo para salvamento de arquivos:
            SaveFileDialog dlgSaveFile = new SaveFileDialog();
            dlgSaveFile.CreatePrompt = false;
            dlgSaveFile.OverwritePrompt = true;
            dlgSaveFile.FileName = this._codeFilePath == null ? "Novo Algoritmo" : Path.GetFileNameWithoutExtension(this._codeFilePath);   // Nome padrão do arquivo.
            dlgSaveFile.DefaultExt = ".alg";
            dlgSaveFile.Filter = "Algoritmo Code (*.alg)|*.alg|All Files (*.*)|*.*";

            try
            {
                if (dlgSaveFile.ShowDialog() == DialogResult.OK)
                {
                    // Carrega o diretório e o nome do arquivo.
                    string saveFileName = dlgSaveFile.FileName;

                    // Testa se o arquivo existe e tenta apagar o arquivo:
                    if (File.Exists(@saveFileName))
                        File.Delete(@saveFileName);

                    // Salva o texto do analisador sintático para o arquivo de texto especificado.
                    File.WriteAllText(@saveFileName, this.txtCodeTextBox.Text);

                    // Salva o caminho para o arquivo.
                    this._codeFilePath = @dlgSaveFile.FileName;

                    this.LimparAlteraçõesNãoSalvas();
                    this.AtualizarTítuloFormulário();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                dlgSaveFile.Dispose();
            }

            return true;
        }

        private bool ChecarSintaxe(bool verificarApenas)
        {
            this.LimparListasInformações();

            // Declara o analisador léxico.
            LexicoClass lexico;
            SemanticoClass semantico = new SemanticoClass();
            MipsClass mips;

            try
            {
                this.AdicionarInfoSaida("Análise léxica iniciada.");

                // Tenta criar uma instância do analisador léxico
                lexico = new LexicoClass(this.txtCodeTextBox.Text, marcadorFinal: "$");

                this.AdicionarInfoSaida("Análise léxica concluida com sucesso.");

                // Adiciona à lista os novos tokens encontrados, se existirem:
                foreach (var token in lexico.ListaTokens)
                    this.lstInfoLexico.Items.Add(token.ToString());
                
                this.AdicionarInfoSaida("Análise sintática iniciada.");

                // Executa a análise sintática usando a lista de tokens encontrados
                AscSLR.Verificar(lexico, this.lstInfoSintatico, semantico);

                this.AdicionarInfoSaida("Análise sintática concluida com sucesso.");

                // Adiciona à lista de saída sintática/semântica os tokens atualizados:
                foreach (var token in semantico.ListaTokens)
                    this.lstInfoSintaticoSemantico.Items.Add(token.ToString());

                this.AdicionarInfoSaida("Análise semântica iniciada.");

                semantico.Processar(this.lstInfoSemantico, this.treeViewAAS);
                
                this.AdicionarInfoSaida("Análise semântica concluida com sucesso.");

                if (verificarApenas)
                    return true;

                this.AdicionarInfoSaida("Geração de código iniciada.");
                mips = new MipsClass(semantico.Algoritmo);
                this.txtInfoSaidaCódigo.Text = mips.ObterCódigo();
                this.AdicionarInfoSaida("Geração de código concluida com sucesso.");
            }
            catch (LexicoListException ex)
            {
                // Atualiza mensagem na barra de status.
                this.ssplStatus.Text = $"Erro: {ex.Message}";

                this.AdicionarInfoSaida($"Erro: {ex.Message}");

                foreach (var erro in ex.ListaErros)
                    this.lstInfoOutputs.Items.Add($"--> {erro.ToString()}");

                this.txtCodeTextBox.Select(ex.ListaErros);
                
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
            catch (SintaticoException ex)
            {
                // Atualiza mensagem na barra de status.
                this.ssplStatus.Text = $"Erro: {ex.Message}";

                this.AdicionarInfoSaida($"Erro: {ex.Message}");

                this.txtCodeTextBox.Select(ex);

                this.lstInfoSintatico.Items.Add("--> Erro sintático <---");

                if (this.lstInfoSintatico.Items.Count > 0)
                    this.lstInfoSintatico.SelectedIndex = this.lstInfoSintatico.Items.Count - 1;
                
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
            catch (SemanticoListException ex)
            {
                // Atualiza mensagem na barra de status.
                this.ssplStatus.Text = $"Erro: {ex.Message}";

                this.AdicionarInfoSaida($"Erro: {ex.Message}");

                foreach (var erro in ex.ListaErros)
                    this.lstInfoOutputs.Items.Add($"--> {erro.ToString()}");

                this.txtCodeTextBox.Select(ex.ListaErros);
                
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
            catch (Exception ex)
            {
                // Atualiza mensagem na barra de status.
                this.ssplStatus.Text = $"Erro: {ex.Message}";

                this.AdicionarInfoSaida($"Erro: {ex.Message}");
                
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        private void MarcarAlteraçõesNãoSalvas()
        {
            // Habilita a opção 'Salvar'.
            this.mnuiFileSave.Enabled = true;

            if (!this.Text.StartsWith("*"))
                this.Text = $"*{this.Text}";
        }

        private void LimparAlteraçõesNãoSalvas()
        {
            // Desabilita a opção 'Salvar'.
            this.mnuiFileSave.Enabled = false;

            if (this.Text.StartsWith("*"))
                this.Text = this.Text.Remove(0, 1);
        }

        private void AtualizarTítuloFormulário()
        {
            this.Text = $"{(this._codeFilePath == null ? "Novo Algoritmo" : Path.GetFileName(this._codeFilePath))} - Compilador";
        }

        private void LimparListasInformações()
        {
            this.lstInfoOutputs.Items.Clear();
            this.lstInfoLexico.Items.Clear();
            this.lstInfoSintatico.Items.Clear();
            this.lstInfoSintaticoSemantico.Items.Clear();
            this.lstInfoSemantico.Items.Clear();
            this.treeViewAAS.Nodes.Clear();
            this.txtInfoSaidaCódigo.Text = String.Empty;
        }

        private void AdicionarInfoSaida(string mensagem)
        {
            this.lstInfoOutputs.Items.Add($"{DateTime.Now:HH:mm:ss.fff}: {mensagem}");
        }
    }
}
