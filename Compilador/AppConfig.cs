using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compilador
{
    /// <summary>
    /// Classe estática encarregada por disponibilizar toda informação da configuração do sistema.
    /// </summary>
    public static class AppConfig
    {
        /// <summary>
        /// Informa a quantidade de linhas do arquivo de configurações. </summary>
        private const int FILE_APP_CONFIG_LINES = 4;

        /// <summary>
        /// Diretório onde a aplicação se encontra. </summary>
        private static string _appPath = Application.StartupPath;   // Get the application directory.

        /// <summary>
        /// Caminho do arquivo de configuração. </summary>
        private static string filePathAppConfig = AppPath + "\\Compilador.ini";

        // Parâmetros da Janela Principal.
        private static Point _myFormMainLocation = new Point(0, 0);
        private static Size _myFormMainSize = new Size(380, 400);
        private static string _myFormLastSearchDirectory = String.Empty;

        /// <summary>
        /// Gets a value indicating the path of the application.
        /// </summary>
        /// <value>
        /// A string value with the path. </value>
        public static string AppPath
        {
            get { return _appPath; }
        }

        #region 'Gets e Sets das váriaveis de Configuração do Aplicativo'
        /// <summary>
        /// Obtém ou Define uma classe 'Point' com as posisões das coordenadas X e Y da Janela Principal.
        /// </summary>
        /// <value>
        /// Os valores minímos para as coordenadas X e Y é 0, valores abaixo deste serão automáticamente ajustados. </value>
        public static Point MyFormMainLocation
        {
            get
            {
                return _myFormMainLocation;
            }

            set
            {
                _myFormMainLocation.X = Math.Max(0, value.X);
                _myFormMainLocation.Y = Math.Max(0, value.Y);
            }
        }

        /// <summary>
        /// Obtém ou Define uma estrutura 'Size' com as dimensões da Janela Principal.
        /// </summary>
        /// <value>
        /// A Largura miníma é de 480 e a Altura miníma é de 360, valores abaixo destes serão automáticamente ajustados para os seus respectivos minímos. </value>
        public static Size MyFormMainSize
        {
            get
            {
                return _myFormMainSize;
            }

            set
            {
                _myFormMainSize.Width = Math.Max(380, value.Width);
                _myFormMainSize.Height = Math.Max(400, value.Height);
            }
        }

        /// <summary>
        /// Gets or Sets a value indicating the directory to the last search file.
        /// </summary>
        /// <value>
        /// A string value with the directory to the last search file. </value>
        public static string MyFormLastSearchDirectory
        {
            get
            {
                if (_myFormLastSearchDirectory.Contains(":\\"))
                {
                    return _myFormLastSearchDirectory;
                }
                else
                {
                    return AppPath + "\\" + _myFormLastSearchDirectory;
                }
            }

            set
            {
                string directory = value;

                if (directory.StartsWith(AppPath))
                {
                    directory = directory.Remove(0, AppPath.Length);

                    if (directory.StartsWith("\\"))
                    {
                        directory = directory.Remove(0, 1);
                        _myFormLastSearchDirectory = directory;
                    }
                    else
                    {
                        _myFormLastSearchDirectory = value;
                    }
                }
                else
                {
                    _myFormLastSearchDirectory = value;
                }
            }
        }

        #endregion

        #region "Métodos privados para conversão de dados"

        /// <summary>
        /// Converte uma estrutura de localização para uma string com as coordenadas no formato 'X;Y'.
        /// </summary>
        /// <param name="location"> A variável contendo as coordenadas a serem convertidas. </param>
        /// <returns>
        /// As coordenadas convertidas no texto em formato 'X;Y'. </returns>
        private static string LocationToStr(Point location)
        {
            return location.X.ToString() + ";" + location.Y.ToString();
        }

        /// <summary>
        /// Converte uma string com as coordenadas no formato 'X,Y' para uma estrutura de localização.
        /// Caso não seja possível executar a conversão, retorna as coordenadas 'X=0, Y=0'.
        /// </summary>
        /// <param name="location"> A string contendo as coordenadas a serem convertidas.</param>
        /// <returns>
        /// As coordenadas convertidas para uma estrutura de localização. </returns>
        private static Point LocationFromStr(string location)
        {
            string[] separators = { "=", ";", " " };
            string[] tmpStr = location.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            int x, y;
            if (int.TryParse(tmpStr[1], out x) && int.TryParse(tmpStr[2], out y))
                return new Point(x, y);

            return new Point(0, 0);
        }

        /// <summary>
        /// Converte uma estrutura de dimensão para uma string com as tamanhos no formato 'Width;Height'. </summary>
        /// <param name="size"> A estrutura contendo as dimensões a serem convertidas. </param>
        /// <returns>
        /// As dimensões convertidas no texto em formato 'Width;Height'. </returns>
        private static string SizeToStr(Size size)
        {
            return size.Width.ToString() + ";" + size.Height.ToString();
        }

        /// <summary>
        /// Converte uma string com os tamanhos no formato 'Width,Height' para uma estrutura de dimensão.
        /// Caso não seja possível executar a conversão, retorna as dimensões 'Width=0, Height=0'. </summary>
        /// <param name="size"> A string contendo as dimensões a serem convertidas. </param>
        /// <returns>
        /// As tamanhos convertidos para uma propriedade de estrutura. </returns>
        private static Size SizeFromStr(string size)
        {
            string[] separators = { "=", ";", " " };
            string[] tmpStr = size.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            int width, height;
            if (tmpStr.Length >= 3)
                if (int.TryParse(tmpStr[1], out width) && int.TryParse(tmpStr[2], out height))
                    return new Size(width, height);

            return new Size(0, 0);
        }

        /// <summary>
        /// Captura e retorna uma string com a informação do valor do parâmetro.
        /// Caso não seja possível identificar o parâmetro, retorna uma string vazia.
        /// </summary>
        /// <param name="line"> A string contendo as informações do parâmetro a capturar. </param>
        /// <returns>
        /// A string com o valor do parâmetro. </returns>
        private static string GetParameterValueStr(string line)
        {
            string[] separators = { "=", " " };
            string[] tmpStr = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            if (tmpStr.Length > 1)
                return tmpStr[1];

            return "";
        }

        /// <summary>
        /// Captura e retorna uma string com a informação do valor de um parâmetro do tipo caminho/diretório.
        /// Caso não seja possível identificar o parâmetro, retorna uma string vazia.
        /// </summary>
        /// <param name="line"> A string contendo as informações do parâmetro a capturar. </param>
        /// <returns>
        /// A string com o valor do parâmetro. </returns>
        private static string GetParameterValuePath(string line)
        {
            string[] separators = { "=" };
            string[] tmpStr = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            if (tmpStr.Length > 1)
                return tmpStr[1];

            return "";
        }

        #endregion

        #region 'Métodos para Salvar e Carregar as Configurações do Sistema'
        /// <summary>
        /// Salva os parâmetros de configuração no arquivo 'AppConfig.ini'.
        /// </summary>
        public static void SaveAppConfig()
        {
            if (!File.Exists(AppConfig.filePathAppConfig))
                MessageBox.Show("O arquivo de configurações do sistema não foi encontrado.\nSerá criado um novo arquivo com as configurações atuais.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            AppConfig.WriteAppConfigFile();
        }

        /// <summary>
        /// Carrega os parâmetros de configuração salvas no arquivo 'AppConfig.ini'.
        /// </summary>
        public static void LoadAppConfig()
        {
            if (File.Exists(AppConfig.filePathAppConfig))
            {
                try
                {
                    string[] fileReadLines = File.ReadAllLines(AppConfig.filePathAppConfig);

                    if (fileReadLines.Length == AppConfig.FILE_APP_CONFIG_LINES)
                    {
                        // Carrega os parâmetros da Janela Principal.
                        AppConfig.MyFormMainLocation = AppConfig.LocationFromStr(fileReadLines[1]);
                        AppConfig.MyFormMainSize = AppConfig.SizeFromStr(fileReadLines[2]);
                        AppConfig.MyFormLastSearchDirectory = AppConfig.GetParameterValuePath(fileReadLines[3]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Houve um erro na leitura do arquivo de configurações do sistema.\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Se o arquivo de configurações do sistema não foi encontrado, cria um novo arquivo com as configurações padrão.
                AppConfig.WriteAppConfigFile();
            }
        }

        /// <summary>
        /// Método que efetua a manipulação e a criação do arquivo de parâmetros de configuração do arquivo 'AppConfig.ini'.
        /// </summary>
        private static void WriteAppConfigFile()
        {
            try
            {
                string[] fileWriteLines = new string[AppConfig.FILE_APP_CONFIG_LINES];

                fileWriteLines[0] = "[FormMain]";
                fileWriteLines[1] = "Location=" + AppConfig.LocationToStr(AppConfig.MyFormMainLocation);
                fileWriteLines[2] = "Size=" + AppConfig.SizeToStr(AppConfig.MyFormMainSize);
                fileWriteLines[3] = "SearchDir=" + AppConfig._myFormLastSearchDirectory;

                File.WriteAllLines(filePathAppConfig, fileWriteLines);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Houve um erro na escrita do arquivo de configurações do sistema.\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
