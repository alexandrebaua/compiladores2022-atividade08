using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Sintatico.Ascendente.SLR
{
    /// <summary>
    /// Tabelas de ações e desvio (GOTO) do analisador sintatico ascendente SLR.
    /// </summary>
    public static class Tabelas
    {
        /// <summary>
        /// Vetor para armazenamento das tabelas de ação.
        /// </summary>
        private static Dictionary<string, ActionClass>[] tabelaAcao = null;

        /// <summary>
        /// Vetor para armazenamento das tabelas de desvio.
        /// </summary>
        private static Dictionary<string, int>[] tabelaGOTO = null;

        #region Constantes internas

        /// <summary>
        /// Indica o tamanho máximo das tabelas de ação e de desvio.
        /// </summary>
        private const int TAMANHO_TABELAS = 731;
        
        #endregion
        
        /// <summary>
        /// Obtém a tabela de ação.
        /// </summary>
        public static Dictionary<string, ActionClass>[] TabelaAcao
        {
            get
            {
                if (Tabelas.tabelaAcao == null)
                    Tabelas.InitTableActionAndGoto();

                return Tabelas.tabelaAcao;
            }
        }

        /// <summary>
        /// Obtém a tabela de desvio (GOTO).
        /// </summary>
        public static Dictionary<string, int>[] TabelaGOTO
        {
            get
            {
                if (Tabelas.tabelaGOTO == null)
                    Tabelas.InitTableActionAndGoto();

                return Tabelas.tabelaGOTO;
            }
        }

        /// <summary>
        /// Busca uma ação associada à tupla informada.
        /// </summary>
        /// <param name="s">O estado no topo da pilha.</param>
        /// <param name="a">O simbolo atual.</param>
        /// <returns>A ação encontrada, ou nulo se não existir nenhuma ação associada à tupla.</returns>
        public static ActionClass BuscaAcao(int s, string a)
        {
            if (Tabelas.TabelaAcao[s].ContainsKey(a))
                return Tabelas.TabelaAcao[s][a];

            return null;
        }

        /// <summary>
        /// Busca um desvio associado à tupla informada.
        /// </summary>
        /// <param name="t">O estado no topo da pilha.</param>
        /// <param name="A">O simbolo não terminal.</param>
        /// <returns>O devio encontrado, ou -1 se não existir nenhuma ação associada à tupla.</returns>
        public static int BuscaGOTO(int t, string A)
        {
            /*if (Tabelas.TabelaGOTO[t] == null)
                throw new Exception($"Desvio nulo!{Environment.NewLine}t: {t}");*/

            if (Tabelas.TabelaGOTO[t].ContainsKey(A))
                return Tabelas.TabelaGOTO[t][A];

            return -1;
        }

        /// <summary>
        /// Inicializa as tabelas de ação e de desvio (GOTO).
        /// </summary>
        private static void InitTableActionAndGoto()
        {
            // Define localmente todas as reduções da gramática, para serem utilizadas durante a criação dos estados:
            ReducaoClass r1 = new ReducaoClass(1, "<ALGORITMO>", new string[] { "ALGORITMO", "CONST_TEXTO", "<BLOCO_LISTA_DECL_VAR>", "<BLOCO_LISTA_PROC_FUNC>", "<BLOCO_PRINCIPAL>", "PONTO" }, ReduceTypeEnum.Algoritmo);
            ReducaoClass r2 = new ReducaoClass(2, "<BLOCO_LISTA_DECL_VAR>", new string[] { "VARIAVEIS", "<LISTA_DECL_VAR>" });
            ReducaoClass r3 = new ReducaoClass(3, "<BLOCO_LISTA_DECL_VAR>", new string[] { });
            ReducaoClass r4 = new ReducaoClass(4, "<LISTA_DECL_VAR>", new string[] { "<DECL_VAR>" });
            ReducaoClass r5 = new ReducaoClass(5, "<LISTA_DECL_VAR>", new string[] { "<LISTA_DECL_VAR>", "<DECL_VAR>" });
            ReducaoClass r6 = new ReducaoClass(6, "<DECL_VAR>", new string[] { "<LISTA_ID>", "DOIS_PONTO", "<TIPO_SIMPLES>", "PONTO_VIRGULA" }, ReduceTypeEnum.DeclaraçãoSimples);
            ReducaoClass r7 = new ReducaoClass(7, "<LISTA_ID>", new string[] { "ID" });
            ReducaoClass r8 = new ReducaoClass(8, "<LISTA_ID>", new string[] { "<LISTA_ID>", "VIRGULA", "ID" });
            ReducaoClass r9 = new ReducaoClass(9, "<DECL_VAR>", new string[] { "ID", "DOIS_PONTO", "<TIPO_SIMPLES>", "PONTO_VIRGULA" }, ReduceTypeEnum.DeclaraçãoSimples);
            ReducaoClass r10 = new ReducaoClass(10, "<TIPO_DADOS>", new string[] { "<TIPO_SIMPLES>" });
            ReducaoClass r11 = new ReducaoClass(11, "<TIPO_SIMPLES>", new string[] { "CARACTER | INTEIRO | REAL | LOGICO" });
            //ReducaoClass r12 = new ReducaoClass(12, "<TIPO_DADOS>", new string[] { "<TIPO_VETOR>" });
            ReducaoClass r12 = new ReducaoClass(12, "<DECL_VAR>", new string[] { "ID", "DOIS_PONTO", "VETOR", "ABRE_COL", "<LISTA_VETOR_FAIXA>", "FECHA_COL", "DE", "<TIPO_SIMPLES>", "PONTO_VIRGULA" }, ReduceTypeEnum.DeclaraçãoVetor);
            //ReducaoClass r13 = new ReducaoClass(13, "<TIPO_VETOR>", new string[] { "VETOR", "ABRE_COL", "<LISTA_VETOR_FAIXA>", "FECHA_COL", "DE", "<TIPO_SIMPLES>" });
            ReducaoClass r14 = new ReducaoClass(14, "<LISTA_VETOR_FAIXA>", new string[] { "CONST_FAIXA_VETOR" });
            ReducaoClass r15 = new ReducaoClass(15, "<LISTA_VETOR_FAIXA>", new string[] { "<LISTA_VETOR_FAIXA>", "PONTO_VIRGULA", "CONST_FAIXA_VETOR" });
            //ReducaoClass r16 = new ReducaoClass(16, "<TIPO_VETOR_FAIXA>", new string[] { "CONST_INT", "PONTO", "PONTO", "CONST_INT" });
            ReducaoClass r17 = new ReducaoClass(17, "<BLOCO_PROCEDIMENTO>", new string[] { "PROCEDIMENTO", "ID", "ABRE_PAR", "<LISTA_PARAMETROS>", "FECHA_PAR", "<BLOCO_LISTA_DECL_VAR>", "INICIO", "<LISTA_COMANDOS>", "FIM", "PONTO_VIRGULA" }, ReduceTypeEnum.Procedimento);
            ReducaoClass r18 = new ReducaoClass(18, "<BLOCO_LISTA_PROC_FUNC>", new string[] { });
            ReducaoClass r19 = new ReducaoClass(19, "<BLOCO_LISTA_PROC_FUNC>", new string[] { "<BLOCO_PROCEDIMENTO>" });
            ReducaoClass r20 = new ReducaoClass(20, "<BLOCO_LISTA_PROC_FUNC>", new string[] { "<BLOCO_LISTA_PROC_FUNC>", "<BLOCO_PROCEDIMENTO>" });
            ReducaoClass r21 = new ReducaoClass(21, "<BLOCO_FUNCAO>", new string[] { "FUNCAO", "ID", "ABRE_PAR", "<LISTA_PARAMETROS>", "FECHA_PAR", "DOIS_PONTO", "<TIPO_DADOS>", "<BLOCO_LISTA_DECL_VAR>", "INICIO", "<LISTA_COMANDOS>", "<FUNC_RETORNE>", "FIM", "PONTO_VIRGULA" }, ReduceTypeEnum.Função);
            ReducaoClass r22 = new ReducaoClass(22, "<BLOCO_LISTA_PROC_FUNC>", new string[] { "<BLOCO_FUNCAO>" });
            ReducaoClass r23 = new ReducaoClass(23, "<BLOCO_LISTA_PROC_FUNC>", new string[] { "<BLOCO_LISTA_PROC_FUNC>", "<BLOCO_FUNCAO>" });
            ReducaoClass r24 = new ReducaoClass(24, "<LISTA_PARAMETROS>", new string[] { });
            ReducaoClass r25 = new ReducaoClass(25, "<LISTA_PARAMETROS>", new string[] { "<DECL_PARAMETRO>" });
            ReducaoClass r26 = new ReducaoClass(26, "<LISTA_PARAMETROS>", new string[] { "<LISTA_PARAMETROS>", "PONTO_VIRGULA", "<DECL_PARAMETRO>" });
            ReducaoClass r27 = new ReducaoClass(27, "<DECL_PARAMETRO>", new string[] { "<LISTA_PR_ID_VAR>", "DOIS_PONTO", "<TIPO_SIMPLES_PARAM>" });
            ReducaoClass r28 = new ReducaoClass(28, "<LISTA_PR_ID_VAR>", new string[] { "<LISTA_PR_ID_VAR>", "VIRGULA", "ID" });
            ReducaoClass r29 = new ReducaoClass(29, "<LISTA_PR_ID_VAR>", new string[] { "VAR", "ID" });
            ReducaoClass r30 = new ReducaoClass(30, "<LISTA_PR_ID_VAR>", new string[] { "ID" });
            ReducaoClass r31 = new ReducaoClass(31, "<TIPO_SIMPLES_PARAM>", new string[] { "CARACTER | INTEIRO | REAL | LOGICO" });
            ReducaoClass r32 = new ReducaoClass(32, "<TIPO_DADOS_FUNC_RET>", new string[] { "<TIPO_SIMPLES_FUNC_RET>" });
            ReducaoClass r33 = new ReducaoClass(33, "<TIPO_SIMPLES_FUNC_RET>", new string[] { "CARACTER | INTEIRO | REAL | LOGICO" });
            ReducaoClass r34 = new ReducaoClass(34, "<TIPO_DADOS_FUNC_RET>", new string[] { "<TIPO_VETOR_FUNC_RET>" });
            ReducaoClass r35 = new ReducaoClass(35, "<TIPO_VETOR_FUNC_RET>", new string[] { "VETOR", "ABRE_COL", "<LISTA_VETOR_FAIXA>", "FECHA_COL", "DE", "<TIPO_SIMPLES_FUNC_RET>" });
            ReducaoClass r36 = new ReducaoClass(36, "<FUNC_RETORNE>", new string[] { "RETORNE", "<FUNC_RET_VAR>", "PONTO_VIRGULA" }, ReduceTypeEnum.FunçãoRetorne);
            ReducaoClass r37 = new ReducaoClass(37, "<FUNC_RET_VAR>", new string[] { "<FUNC_RET_VAR>", "<FUNC_RET_OP>", "<FUNC_RET_VAR>" });
            ReducaoClass r38 = new ReducaoClass(38, "<FUNC_RET_VAR>", new string[] { "ID | CONST_INT | CONST_REAL | CONST_TEXTO" });
            ReducaoClass r39 = new ReducaoClass(39, "<FUNC_RET_OP>", new string[] { "MENOS | MAIS | ASTERISTICO | BARRA" });
            ReducaoClass r40 = new ReducaoClass(40, "<BLOCO_PRINCIPAL>", new string[] { "<BLOCO_LISTA_DECL_VAR>", "INICIO", "<LISTA_COMANDOS>", "FIM" }, ReduceTypeEnum.Principal);

            ReducaoClass r130 = new ReducaoClass(130, "<LISTA_COMANDOS>", new string[] { "<CMD_ATRIBUICAO>" });
            ReducaoClass r131 = new ReducaoClass(131, "<LISTA_COMANDOS>", new string[] { "<LISTA_COMANDOS>", "<CMD_ATRIBUICAO>" });
            ReducaoClass r132 = new ReducaoClass(132, "<CMD_ATRIBUICAO>", new string[] { "<CMD_ATRIB_VAR_REC>", "ATRIBUICAO", "<LISTA_CMD_ATRIB_VAL>", "PONTO_VIRGULA" }, ReduceTypeEnum.AtribuiçãoNumérica);
            ReducaoClass r133 = new ReducaoClass(133, "<CMD_ATRIB_VAR_REC>", new string[] { "ID" });
            ReducaoClass r134 = new ReducaoClass(134, "<CMD_ATRIB_VAR_REC>", new string[] { "ID", "ABRE_COL", "<LST_ATRIB_VET_INDEX>", "FECHA_COL" });
            ReducaoClass r135 = new ReducaoClass(135, "<LISTA_CMD_ATRIB_VAL>", new string[] { "<CMD_ATRIB_VAL>" });
            ReducaoClass r136 = new ReducaoClass(136, "<LISTA_CMD_ATRIB_VAL>", new string[] { "<LISTA_CMD_ATRIB_VAL>", "<CMD_ATRIB_OP>", "<CMD_ATRIB_VAL>" });
            ReducaoClass r137 = new ReducaoClass(137, "<CMD_ATRIB_VAL>", new string[] { "ID" });
            ReducaoClass r138 = new ReducaoClass(138, "<CMD_ATRIB_VAL>", new string[] { "<CMD_ATRIB_CONST>" });
            ReducaoClass r139 = new ReducaoClass(139, "<CMD_ATRIB_VAL>", new string[] { "MENOS", "<CMD_ATRIB_CONST>" });
            ReducaoClass r140 = new ReducaoClass(140, "<CMD_ATRIB_VAL>", new string[] { "ABRE_PAR", "<LISTA_CMD_ATRIB_VAL>", "FECHA_PAR" });
            ReducaoClass r141 = new ReducaoClass(141, "<CMD_ATRIB_VAL>", new string[] { "<CMD_ATRIB_VET>" });
            ReducaoClass r142 = new ReducaoClass(142, "<CMD_ATRIB_VAL>", new string[] { "<CMD_ATRIB_FUNC>" });
            ReducaoClass r143 = new ReducaoClass(143, "<CMD_ATRIB_VAL>", new string[] { "CONST_TEXTO" });
            ReducaoClass r144 = new ReducaoClass(144, "<CMD_ATRIB_CONST>", new string[] { "CONST_INT | CONST_REAL" });
            ReducaoClass r145 = new ReducaoClass(145, "<CMD_ATRIB_OP>", new string[] { "MENOS | MAIS | ASTERISTICO | BARRA" });
            ReducaoClass r146 = new ReducaoClass(146, "<CMD_ATRIB_FUNC>", new string[] { "ID", "ABRE_PAR", "<LST_ATRIB_FUNC_PARAM>", "FECHA_PAR" });
            ReducaoClass r147 = new ReducaoClass(147, "<LST_ATRIB_FUNC_PARAM>", new string[] { });
            ReducaoClass r148 = new ReducaoClass(148, "<LST_ATRIB_FUNC_PARAM>", new string[] { "ID" });
            ReducaoClass r149 = new ReducaoClass(149, "<LST_ATRIB_FUNC_PARAM>", new string[] { "<LST_ATRIB_FUNC_PARAM>", "VIRGULA", "ID" });
            ReducaoClass r150 = new ReducaoClass(150, "<CMD_ATRIB_VET>", new string[] { "ID", "ABRE_COL", "<LST_ATRIB_VET_INDEX>", "FECHA_COL" });
            ReducaoClass r151 = new ReducaoClass(151, "<LST_ATRIB_VET_INDEX>", new string[] { "<ATRIB_VET_INDEX_VAL>" });
            ReducaoClass r152 = new ReducaoClass(152, "<LST_ATRIB_VET_INDEX>", new string[] { "<LST_ATRIB_VET_INDEX>", "VIRGULA", "<ATRIB_VET_INDEX_VAL>" });
            ReducaoClass r153 = new ReducaoClass(153, "<LST_ATRIB_VET_INDEX>", new string[] { "<LST_ATRIB_VET_INDEX>", "<ATRIB_VET_INDEX_OP>", "<ATRIB_VET_INDEX_VAL>" });
            ReducaoClass r154 = new ReducaoClass(154, "<ATRIB_VET_INDEX_VAL>", new string[] { "ID | CONST_INT" });
            ReducaoClass r155 = new ReducaoClass(155, "<ATRIB_VET_INDEX_OP>", new string[] { "MENOS | MAIS | ASTERISTICO | BARRA" });

            ReducaoClass r160 = new ReducaoClass(160, "<LISTA_COMANDOS>", new string[] { "<CMD_CHAMA_PROC>" });
            ReducaoClass r161 = new ReducaoClass(161, "<LISTA_COMANDOS>", new string[] { "<LISTA_COMANDOS>", "<CMD_CHAMA_PROC>" });
            ReducaoClass r162 = new ReducaoClass(162, "<CMD_CHAMA_PROC>", new string[] { "ID", "ABRE_PAR", "<LST_CHAMA_PROC_PARAM>", "FECHA_PAR", "PONTO_VIRGULA" }, ReduceTypeEnum.ChamaProcedimento);
            ReducaoClass r163 = new ReducaoClass(163, "<LST_CHAMA_PROC_PARAM>", new string[] { "ID" });
            ReducaoClass r164 = new ReducaoClass(164, "<LST_CHAMA_PROC_PARAM>", new string[] { "<LST_CHAMA_PROC_PARAM>", "VIRGULA", "ID" });

            ReducaoClass r180 = new ReducaoClass(180, "<ARG_LOG_VAR>", new string[] { "<LISTA_CMD_ATRIB_VAL>" });
            ReducaoClass r181 = new ReducaoClass(181, "<CMD_ATRIBUICAO>", new string[] { "<CMD_ATRIB_VAR_REC>", "ATRIBUICAO", "<ARGUMENTO_LOGICO>", "PONTO_VIRGULA" }, ReduceTypeEnum.AtribuiçãoLogica);


            ReducaoClass r250 = new ReducaoClass(250, "<ARGUMENTO_LOGICO>", new string[] { "<LST_ARG_LOG_COND>" });
            ReducaoClass r251 = new ReducaoClass(251, "<ARGUMENTO_LOGICO>", new string[] { "<ARGUMENTO_LOGICO>", "<ARG_LOG_OP_LOG>", "<LST_ARG_LOG_COND>" });
            ReducaoClass r252 = new ReducaoClass(252, "<LST_ARG_LOG_COND>", new string[] { "ABRE_PAR", "<ARGUMENTO_LOGICO>", "FECHA_PAR" });
            ReducaoClass r253 = new ReducaoClass(253, "<LST_ARG_LOG_COND>", new string[] { "NAO", "ABRE_PAR", "<ARGUMENTO_LOGICO>", "FECHA_PAR" });
            ReducaoClass r254 = new ReducaoClass(254, "<LST_ARG_LOG_COND>", new string[] { "CONST_LOGICA" });
            ReducaoClass r255 = new ReducaoClass(255, "<LST_ARG_LOG_COND>", new string[] { "<ARG_LOG_VAR>" });
            ReducaoClass r256 = new ReducaoClass(256, "<LST_ARG_LOG_COND>", new string[] { "<ARG_LOG_VAR>", "<ARG_LOG_OP_REL>", "<LST_ARG_LOG_VAR2>" });
            ReducaoClass r257 = new ReducaoClass(257, "<LST_ARG_LOG_VAR1>", new string[] { "<ARG_LOG_VAR>", "<ARG_LOG_OP_MAT>", "<ARG_LOG_VAR_EX>" });
            ReducaoClass r258 = new ReducaoClass(258, "<LST_ARG_LOG_COND>", new string[] { "<LST_ARG_LOG_VAR1>", "<ARG_LOG_OP_REL>", "<LST_ARG_LOG_VAR2>" });
            ReducaoClass r259 = new ReducaoClass(259, "<LST_ARG_LOG_VAR1>", new string[] { "<LST_ARG_LOG_VAR1>", "<ARG_LOG_OP_MAT>", "<ARG_LOG_VAR_EX>" });
            ReducaoClass r260 = new ReducaoClass(260, "<LST_ARG_LOG_VAR1>", new string[] { "<ARG_LOG_VAR_CONST>" });
            ReducaoClass r261 = new ReducaoClass(261, "<LST_ARG_LOG_VAR1>", new string[] { "MENOS", "<ARG_LOG_VAR_CONST>" });
            ReducaoClass r262 = new ReducaoClass(262, "<LST_ARG_LOG_VAR2>", new string[] { "<LST_ARG_LOG_VAR2>", "<ARG_LOG_OP_MAT>", "<ARG_LOG_VAR_EX>" });
            ReducaoClass r263 = new ReducaoClass(263, "<LST_ARG_LOG_VAR2>", new string[] { "<ARG_LOG_VAR_CONST>" });
            ReducaoClass r264 = new ReducaoClass(264, "<LST_ARG_LOG_VAR2>", new string[] { "MENOS", "<ARG_LOG_VAR_CONST>" });
            ReducaoClass r265 = new ReducaoClass(265, "<LST_ARG_LOG_VAR2>", new string[] { "<ARG_LOG_VAR>" });
            ReducaoClass r266 = new ReducaoClass(266, "<LST_ARG_LOG_VAR2>", new string[] { "<LST_ARG_LOG_VAR2>", "<ARG_LOG_OP_MAT>", "<ARG_LOG_VAR_EX>" });
            ReducaoClass r267 = new ReducaoClass(267, "<LST_ARG_LOG_VAR2>", new string[] { "<LST_ARG_LOG_VAR2>", "<ARG_LOG_OP_MAT>", "ABRE_PAR", "<LST_ARG_LOG_VAR2>", "FECHA_PAR" });
            ReducaoClass r268 = new ReducaoClass(268, "<LST_ARG_LOG_VAR2>", new string[] { "ABRE_PAR", "<LST_ARG_LOG_VAR2>", "FECHA_PAR" });
            ReducaoClass r269 = new ReducaoClass(269, "<ARG_LOG_VAR_EX>", new string[] { "<ARG_LOG_VAR>" });
            ReducaoClass r270 = new ReducaoClass(270, "<ARG_LOG_VAR_EX>", new string[] { "<ARG_LOG_VAR_CONST>" });
            ReducaoClass r271 = new ReducaoClass(271, "<ARG_LOG_VAR_EX>", new string[] { "MENOS", "<ARG_LOG_VAR_CONST>" });

            ReducaoClass r280 = new ReducaoClass(280, "<ARG_LOG_VAR>", new string[] { "ID" });
            ReducaoClass r281 = new ReducaoClass(281, "<ARG_LOG_VAR>", new string[] { "ID", "ABRE_COL", "<LST_ATRIB_VET_INDEX>", "FECHA_COL" });
            ReducaoClass r282 = new ReducaoClass(282, "<ARG_LOG_VAR>", new string[] { "ID", "ABRE_PAR", "<LST_ATRIB_FUNC_PARAM>", "FECHA_PAR" });
            ReducaoClass r283 = new ReducaoClass(283, "<ARG_LOG_VAR_CONST>", new string[] { "CONST_INT | CONST_REAL" });
            ReducaoClass r284 = new ReducaoClass(284, "<ARG_LOG_OP_REL>", new string[] { "MAIOR | MENOR | MAIOR_IGUAL | MENOR_IGUAL | IGUAL | DIFERENTE" });
            ReducaoClass r285 = new ReducaoClass(285, "<ARG_LOG_OP_MAT>", new string[] { "MENOS | MAIS | ASTERISTICO | BARRA | MOD | DIV" });
            ReducaoClass r286 = new ReducaoClass(286, "<ARG_LOG_OP_LOG>", new string[] { "E | OU" });


            ReducaoClass r350 = new ReducaoClass(350, "<LISTA_COMANDOS>", new string[] { "<CMD_SEL_IF>" });
            ReducaoClass r351 = new ReducaoClass(351, "<LISTA_COMANDOS>", new string[] { "<LISTA_COMANDOS>", "<CMD_SEL_IF>" });
            ReducaoClass r352 = new ReducaoClass(352, "<CMD_SEL_IF>", new string[] { "SE", "<ARGUMENTO_LOGICO>", "ENTAO", "<LISTA_COMANDOS>", "<CMD_SEL_IF_BLOCO_ELSE>", "FIM_SE", "PONTO_VIRGULA" }, ReduceTypeEnum.SeleçãoIf);
            ReducaoClass r353 = new ReducaoClass(353, "<CMD_SEL_IF_BLOCO_ELSE>", new string[] { });
            ReducaoClass r354 = new ReducaoClass(354, "<CMD_SEL_IF_BLOCO_ELSE>", new string[] { "SENAO", "<LISTA_COMANDOS>" });

            ReducaoClass r375 = new ReducaoClass(375, "<LISTA_COMANDOS>", new string[] { "<CMD_REP_ENQUANTO>" });
            ReducaoClass r376 = new ReducaoClass(376, "<LISTA_COMANDOS>", new string[] { "<LISTA_COMANDOS>", "<CMD_REP_ENQUANTO>" });
            ReducaoClass r377 = new ReducaoClass(377, "<CMD_REP_ENQUANTO>", new string[] { "ENQUANTO", "<ARGUMENTO_LOGICO>", "FACA", "<LISTA_COMANDOS>", "FIM_ENQUANTO", "PONTO_VIRGULA" }, ReduceTypeEnum.Enquanto);

            ReducaoClass r400 = new ReducaoClass(400, "<LISTA_COMANDOS>", new string[] { "<CMD_REP_REPITA>" });
            ReducaoClass r401 = new ReducaoClass(401, "<LISTA_COMANDOS>", new string[] { "<LISTA_COMANDOS>", "<CMD_REP_REPITA>" });
            ReducaoClass r402 = new ReducaoClass(402, "<CMD_REP_REPITA>", new string[] { "REPITA", "<LISTA_COMANDOS>", "ATE", "QUE", "<ARGUMENTO_LOGICO>", "PONTO_VIRGULA" }, ReduceTypeEnum.Repita);

            ReducaoClass r425 = new ReducaoClass(425, "<LISTA_COMANDOS>", new string[] { "<CMD_REP_PARA>" });
            ReducaoClass r426 = new ReducaoClass(426, "<LISTA_COMANDOS>", new string[] { "<LISTA_COMANDOS>", "<CMD_REP_PARA>" });
            ReducaoClass r427 = new ReducaoClass(427, "<CMD_REP_PARA>", new string[] { "PARA", "ID", "ATRIBUICAO", "<LISTA_CMD_ATRIB_VAL>", "<CMD_REP_PARA_MODO>", "<LISTA_CMD_ATRIB_VAL>", "FACA", "<LISTA_COMANDOS>", "FIM_PARA", "PONTO_VIRGULA" }, ReduceTypeEnum.ParaFaça);
            ReducaoClass r428 = new ReducaoClass(428, "<CMD_REP_PARA_MODO>", new string[] { "ATE" });
            ReducaoClass r429 = new ReducaoClass(429, "<CMD_REP_PARA_MODO>", new string[] { "DECRESCENTE", "ATE" });

            ReducaoClass r500 = new ReducaoClass(500, "<LISTA_COMANDOS>", new string[] { "<CMD_LEIA>" });
            ReducaoClass r501 = new ReducaoClass(501, "<LISTA_COMANDOS>", new string[] { "<LISTA_COMANDOS>", "<CMD_LEIA>" });
            ReducaoClass r502 = new ReducaoClass(502, "<CMD_LEIA>", new string[] { "LEIA", "ABRE_PAR", "<LISTA_CMD_LEIA_VAR>", "FECHA_PAR", "PONTO_VIRGULA" }, ReduceTypeEnum.Leia);
            ReducaoClass r503 = new ReducaoClass(503, "<LISTA_CMD_LEIA_VAR>", new string[] { "<CMD_LEIA_VAR>" });
            ReducaoClass r504 = new ReducaoClass(504, "<LISTA_CMD_LEIA_VAR>", new string[] { "<LISTA_CMD_LEIA_VAR>", "VIRGULA", "<CMD_LEIA_VAR>" });
            ReducaoClass r505 = new ReducaoClass(505, "<CMD_LEIA_VAR>", new string[] { "ID" });
            ReducaoClass r506 = new ReducaoClass(506, "<CMD_LEIA_VAR>", new string[] { "ID", "ABRE_COL", "<LST_ATRIB_VET_INDEX>", "FECHA_COL" });

            ReducaoClass r525 = new ReducaoClass(525, "<LISTA_COMANDOS>", new string[] { "<CMD_ESCREVA>" });
            ReducaoClass r526 = new ReducaoClass(526, "<LISTA_COMANDOS>", new string[] { "<LISTA_COMANDOS>", "<CMD_ESCREVA>" });
            ReducaoClass r527 = new ReducaoClass(527, "<CMD_ESCREVA>", new string[] { "ESCREVA", "ABRE_PAR", "<LISTA_IMP_CMD_ESCREVA>", "FECHA_PAR", "PONTO_VIRGULA" }, ReduceTypeEnum.Escreva);

            ReducaoClass r528 = new ReducaoClass(528, "<LISTA_COMANDOS>", new string[] { "<CMD_ESCREVALN>" });
            ReducaoClass r529 = new ReducaoClass(529, "<LISTA_COMANDOS>", new string[] { "<LISTA_COMANDOS>", "<CMD_ESCREVALN>" });
            ReducaoClass r530 = new ReducaoClass(530, "<CMD_ESCREVALN>", new string[] { "ESCREVALN", "ABRE_PAR", "<LISTA_IMP_CMD_ESCREVA>", "FECHA_PAR", "PONTO_VIRGULA" }, ReduceTypeEnum.Escrevaln);

            ReducaoClass r531 = new ReducaoClass(531, "<LISTA_IMP_CMD_ESCREVA>", new string[] { "CONST_TEXTO" });
            ReducaoClass r532 = new ReducaoClass(532, "<LISTA_IMP_CMD_ESCREVA>", new string[] { "<LISTA_CMD_ESCREVA_VAR>" });
            ReducaoClass r533 = new ReducaoClass(533, "<LISTA_IMP_CMD_ESCREVA>", new string[] { "<LISTA_IMP_CMD_ESCREVA>", "VIRGULA", "CONST_TEXTO" });
            ReducaoClass r534 = new ReducaoClass(534, "<LISTA_IMP_CMD_ESCREVA>", new string[] { "<LISTA_IMP_CMD_ESCREVA>", "VIRGULA", "<LISTA_CMD_ESCREVA_VAR>" });
            ReducaoClass r535 = new ReducaoClass(535, "<LISTA_CMD_ESCREVA_VAR>", new string[] { "<CMD_ESCREVA_VAR>" });
            ReducaoClass r536 = new ReducaoClass(536, "<LISTA_CMD_ESCREVA_VAR>", new string[] { "<LISTA_CMD_ESCREVA_VAR>", "<CMD_ESCREVA_OP>", "<CMD_ESCREVA_VAR>" });
            ReducaoClass r537 = new ReducaoClass(537, "<CMD_ESCREVA_VAR>", new string[] { "ID | CONST_INT | CONST_REAL | CONST_TEXTO" });
            ReducaoClass r538 = new ReducaoClass(538, "<CMD_ESCREVA_VAR>", new string[] { "ID", "ABRE_COL", "<LST_ATRIB_VET_INDEX>", "FECHA_COL" });
            ReducaoClass r539 = new ReducaoClass(539, "<CMD_ESCREVA_VAR>", new string[] { "ID", "ABRE_PAR", "<LST_ATRIB_FUNC_PARAM>", "FECHA_PAR" });
            ReducaoClass r540 = new ReducaoClass(540, "<CMD_ESCREVA_OP>", new string[] { "MENOS | MAIS | ASTERISTICO | BARRA" });

            ReducaoClass r = new ReducaoClass(4, "<>", new string[] { "" });
            //ReducaoClass r = new ReducaoClass(, "<>", new string[] { "" });

            // Criação dos estados das tabelas de ação e de desvio...
            Tabelas.tabelaAcao = new Dictionary<string, ActionClass>[Tabelas.TAMANHO_TABELAS];
            Tabelas.tabelaGOTO = new Dictionary<string, int>[Tabelas.TAMANHO_TABELAS];

            // <ALGORITMO>  -> . ALGORITMO CONST_TEXTO <BLOCO_LISTA_DECL_VAR> <BLOCO_LISTA_PROC_FUNC> <BLOCO_PRINCIPAL> PONTO
            Tabelas.tabelaAcao[0] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[0].Add("ALGORITMO", new ActionClass(ActionType.Shift, 2));

            Tabelas.tabelaGOTO[0] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[0].Add("<ALGORITMO>", 1);

            // <ALGORITMO>' -> <ALGORITMO> . $
            Tabelas.tabelaAcao[1] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[1].Add("$", new ActionClass(ActionType.Accept));

            Tabelas.tabelaGOTO[1] = null;

            //  <ALGORITMO> -> ALGORITMO . CONST_TEXTO <BLOCO_LISTA_DECL_VAR> <BLOCO_LISTA_PROC_FUNC> <BLOCO_PRINCIPAL> PONTO
            Tabelas.tabelaAcao[2] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[2].Add("CONST_TEXTO", new ActionClass(ActionType.Shift, 3));

            Tabelas.tabelaGOTO[2] = null;

            // <ALGORITMO> -> ALGORITMO CONST_TEXTO . <BLOCO_LISTA_DECL_VAR> <BLOCO_LISTA_PROC_FUNC> <BLOCO_PRINCIPAL> PONTO
            Tabelas.tabelaAcao[3] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[3].Add("VARIAVEIS", new ActionClass(ActionType.Shift, 8));
            Tabelas.tabelaAcao[3].Add("PROCEDIMENTO", new ActionClass(ActionType.Reduce, r3));
            Tabelas.tabelaAcao[3].Add("FUNCAO", new ActionClass(ActionType.Reduce, r3));
            Tabelas.tabelaAcao[3].Add("INICIO", new ActionClass(ActionType.Reduce, r3));

            Tabelas.tabelaGOTO[3] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[3].Add("<BLOCO_LISTA_DECL_VAR>", 4);

            // <ALGORITMO> -> ALGORITMO CONST_TEXTO <BLOCO_LISTA_DECL_VAR> . <BLOCO_LISTA_PROC_FUNC> <BLOCO_PRINCIPAL> PONTO
            Tabelas.tabelaAcao[4] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[4].Add("PROCEDIMENTO", new ActionClass(ActionType.Shift, 38));
            Tabelas.tabelaAcao[4].Add("FUNCAO", new ActionClass(ActionType.Shift, 50));
            Tabelas.tabelaAcao[4].Add("INICIO", new ActionClass(ActionType.Reduce, r18));

            Tabelas.tabelaGOTO[4] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[4].Add("<BLOCO_LISTA_PROC_FUNC>", 5);
            Tabelas.tabelaGOTO[4].Add("<BLOCO_PROCEDIMENTO>", 48);
            Tabelas.tabelaGOTO[4].Add("<BLOCO_FUNCAO>", 63);

            // <ALGORITMO> -> ALGORITMO CONST_TEXTO <BLOCO_LISTA_DECL_VAR> <BLOCO_LISTA_PROC_FUNC> . <BLOCO_PRINCIPAL> PONTO
            Tabelas.tabelaAcao[5] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[5].Add("VARIAVEIS", new ActionClass(ActionType.Shift, 8));
            Tabelas.tabelaAcao[5].Add("PROCEDIMENTO", new ActionClass(ActionType.Shift, 38));
            Tabelas.tabelaAcao[5].Add("FUNCAO", new ActionClass(ActionType.Shift, 50));
            Tabelas.tabelaAcao[5].Add("INICIO", new ActionClass(ActionType.Reduce, r3));

            Tabelas.tabelaGOTO[5] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[5].Add("<BLOCO_PROCEDIMENTO>", 49);
            Tabelas.tabelaGOTO[5].Add("<BLOCO_FUNCAO>", 64);
            Tabelas.tabelaGOTO[5].Add("<BLOCO_LISTA_DECL_VAR>", 93);
            Tabelas.tabelaGOTO[5].Add("<BLOCO_PRINCIPAL>", 6);

            // <ALGORITMO> -> ALGORITMO CONST_TEXTO <BLOCO_LISTA_DECL_VAR> <BLOCO_LISTA_PROC_FUNC> <BLOCO_PRINCIPAL> . PONTO
            Tabelas.tabelaAcao[6] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[6].Add("PONTO", new ActionClass(ActionType.Shift, 7));

            // <ALGORITMO> -> ALGORITMO CONST_TEXTO <BLOCO_LISTA_DECL_VAR> <BLOCO_LISTA_PROC_FUNC> <BLOCO_PRINCIPAL> PONTO .
            Tabelas.tabelaAcao[7] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[7].Add("$", new ActionClass(ActionType.Reduce, r1));

            // <BLOCO_LISTA_DECL_VAR> -> VARIAVEIS . <LISTA_DECL_VAR>
            Tabelas.tabelaAcao[8] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[8].Add("ID", new ActionClass(ActionType.Shift, 16));

            Tabelas.tabelaGOTO[8] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[8].Add("<LISTA_DECL_VAR>", 9);
            Tabelas.tabelaGOTO[8].Add("<DECL_VAR>", 10);
            Tabelas.tabelaGOTO[8].Add("<LISTA_ID>", 12);

            // <BLOCO_LISTA_DECL_VAR> -> VARIAVEIS <LISTA_DECL_VAR> .
            Tabelas.tabelaAcao[9] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[9].Add("ID", new ActionClass(ActionType.Shift, 16));
            Tabelas.tabelaAcao[9].Add("PROCEDIMENTO", new ActionClass(ActionType.Reduce, r2));
            Tabelas.tabelaAcao[9].Add("FUNCAO", new ActionClass(ActionType.Reduce, r2));
            Tabelas.tabelaAcao[9].Add("INICIO", new ActionClass(ActionType.Reduce, r2));

            Tabelas.tabelaGOTO[9] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[9].Add("<DECL_VAR>", 11);
            Tabelas.tabelaGOTO[9].Add("<LISTA_ID>", 12);

            // <LISTA_DECL_VAR> -> <DECL_VAR> .
            Tabelas.tabelaAcao[10] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[10].Add("ID", new ActionClass(ActionType.Reduce, r4));
            Tabelas.tabelaAcao[10].Add("PROCEDIMENTO", new ActionClass(ActionType.Reduce, r4));
            Tabelas.tabelaAcao[10].Add("FUNCAO", new ActionClass(ActionType.Reduce, r4));
            Tabelas.tabelaAcao[10].Add("INICIO", new ActionClass(ActionType.Reduce, r4));

            // <LISTA_DECL_VAR> -> <LISTA_DECL_VAR> <DECL_VAR> .
            Tabelas.tabelaAcao[11] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[11].Add("ID", new ActionClass(ActionType.Reduce, r5));
            Tabelas.tabelaAcao[11].Add("PROCEDIMENTO", new ActionClass(ActionType.Reduce, r5));
            Tabelas.tabelaAcao[11].Add("FUNCAO", new ActionClass(ActionType.Reduce, r5));
            Tabelas.tabelaAcao[11].Add("INICIO", new ActionClass(ActionType.Reduce, r5));

            // <DECL_VAR> -> <LISTA_ID> . DOIS_PONTO <TIPO_SIMPLES> PONTO_VIRGULA
            Tabelas.tabelaAcao[12] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[12].Add("DOIS_PONTO", new ActionClass(ActionType.Shift, 13));
            Tabelas.tabelaAcao[12].Add("VIRGULA", new ActionClass(ActionType.Shift, 20));

            // <DECL_VAR> -> <LISTA_ID> DOIS_PONTO . <TIPO_SIMPLES> PONTO_VIRGULA
            Tabelas.tabelaAcao[13] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[13].Add("CARACTER", new ActionClass(ActionType.Shift, 23));
            Tabelas.tabelaAcao[13].Add("INTEIRO", new ActionClass(ActionType.Shift, 23));
            Tabelas.tabelaAcao[13].Add("REAL", new ActionClass(ActionType.Shift, 23));
            Tabelas.tabelaAcao[13].Add("LOGICO", new ActionClass(ActionType.Shift, 23));

            Tabelas.tabelaGOTO[13] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[13].Add("<TIPO_SIMPLES>", 14);

            // <DECL_VAR> -> <LISTA_ID> DOIS_PONTO <TIPO_SIMPLES> . PONTO_VIRGULA
            Tabelas.tabelaAcao[14] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[14].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 15));

            // <DECL_VAR> -> <LISTA_ID> DOIS_PONTO <TIPO_SIMPLES> PONTO_VIRGULA .
            Tabelas.tabelaAcao[15] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[15].Add("ID", new ActionClass(ActionType.Reduce, r6));
            Tabelas.tabelaAcao[15].Add("PROCEDIMENTO", new ActionClass(ActionType.Reduce, r6));
            Tabelas.tabelaAcao[15].Add("FUNCAO", new ActionClass(ActionType.Reduce, r6));
            Tabelas.tabelaAcao[15].Add("INICIO", new ActionClass(ActionType.Reduce, r6));

            // <DECL_VAR> -> ID . DOIS_PONTO <TIPO_DADOS> PONTO_VIRGULA
            Tabelas.tabelaAcao[16] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[16].Add("DOIS_PONTO", new ActionClass(ActionType.Shift, 17));
            Tabelas.tabelaAcao[16].Add("VIRGULA", new ActionClass(ActionType.Reduce, r7));

            // <DECL_VAR> -> ID DOIS_PONTO . <TIPO_DADOS> PONTO_VIRGULA
            Tabelas.tabelaAcao[17] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[17].Add("CARACTER", new ActionClass(ActionType.Shift, 23));
            Tabelas.tabelaAcao[17].Add("INTEIRO", new ActionClass(ActionType.Shift, 23));
            Tabelas.tabelaAcao[17].Add("REAL", new ActionClass(ActionType.Shift, 23));
            Tabelas.tabelaAcao[17].Add("LOGICO", new ActionClass(ActionType.Shift, 23));
            Tabelas.tabelaAcao[17].Add("VETOR", new ActionClass(ActionType.Shift, 24));

            Tabelas.tabelaGOTO[17] = new Dictionary<string, int>();
            /*Tabelas.tabelaGOTO[17].Add("<TIPO_DADOS>", 18);
            Tabelas.tabelaGOTO[17].Add("<TIPO_SIMPLES>", 22);
            Tabelas.tabelaGOTO[17].Add("<TIPO_VETOR>", 24);*/
            Tabelas.tabelaGOTO[17].Add("<TIPO_SIMPLES>", 18);

            // <DECL_VAR> -> ID DOIS_PONTO <TIPO_DADOS> . PONTO_VIRGULA
            Tabelas.tabelaAcao[18] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[18].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 19));

            // <DECL_VAR> -> ID DOIS_PONTO <TIPO_DADOS> PONTO_VIRGULA .
            Tabelas.tabelaAcao[19] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[19].Add("ID", new ActionClass(ActionType.Reduce, r9));
            Tabelas.tabelaAcao[19].Add("PROCEDIMENTO", new ActionClass(ActionType.Reduce, r9));
            Tabelas.tabelaAcao[19].Add("FUNCAO", new ActionClass(ActionType.Reduce, r9));
            Tabelas.tabelaAcao[19].Add("INICIO", new ActionClass(ActionType.Reduce, r9));

            // <LISTA_ID> -> <LISTA_ID> VIRGULA . ID
            Tabelas.tabelaAcao[20] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[20].Add("ID", new ActionClass(ActionType.Shift, 21));

            // <LISTA_ID> -> <LISTA_ID> VIRGULA ID .
            Tabelas.tabelaAcao[21] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[21].Add("DOIS_PONTO", new ActionClass(ActionType.Reduce, r8));
            Tabelas.tabelaAcao[21].Add("VIRGULA", new ActionClass(ActionType.Reduce, r8));

            // <TIPO_DADOS> -> <TIPO_SIMPLES> .
            Tabelas.tabelaAcao[22] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[22].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r10));

            // <TIPO_SIMPLES> -> CARACTER .
            // <TIPO_SIMPLES> -> INTEIRO .
            // <TIPO_SIMPLES> -> REAL .
            // <TIPO_SIMPLES> -> LOGICO .
            Tabelas.tabelaAcao[23] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[23].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r11));

            //  <DECL_VAR> -> ID DOIS_PONTO VETOR . ABRE_COL <LISTA_VETOR_FAIXA> FECHA_COL DE <TIPO_SIMPLES> PONTO_VIRGULA
            Tabelas.tabelaAcao[24] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[24].Add("ABRE_COL", new ActionClass(ActionType.Shift, 25));

            // <DECL_VAR> -> ID DOIS_PONTO VETOR ABRE_COL . <LISTA_VETOR_FAIXA> FECHA_COL DE <TIPO_SIMPLES> PONTO_VIRGULA
            Tabelas.tabelaAcao[25] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[25].Add("CONST_FAIXA_VETOR", new ActionClass(ActionType.Shift, 31));

            Tabelas.tabelaGOTO[25] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[25].Add("<LISTA_VETOR_FAIXA>", 26);

            // <DECL_VAR> -> ID DOIS_PONTO VETOR ABRE_COL <LISTA_VETOR_FAIXA> . FECHA_COL DE <TIPO_SIMPLES> PONTO_VIRGULA
            Tabelas.tabelaAcao[26] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[26].Add("VIRGULA", new ActionClass(ActionType.Shift, 32));
            Tabelas.tabelaAcao[26].Add("FECHA_COL", new ActionClass(ActionType.Shift, 27));

            // <DECL_VAR> -> ID DOIS_PONTO VETOR ABRE_COL <LISTA_VETOR_FAIXA> FECHA_COL . DE <TIPO_SIMPLES> PONTO_VIRGULA
            Tabelas.tabelaAcao[27] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[27].Add("DE", new ActionClass(ActionType.Shift, 28));

            // <DECL_VAR> -> ID DOIS_PONTO VETOR ABRE_COL <LISTA_VETOR_FAIXA> FECHA_COL DE . <TIPO_SIMPLES> PONTO_VIRGULA
            Tabelas.tabelaAcao[28] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[28].Add("CARACTER", new ActionClass(ActionType.Shift, 23));
            Tabelas.tabelaAcao[28].Add("INTEIRO", new ActionClass(ActionType.Shift, 23));
            Tabelas.tabelaAcao[28].Add("REAL", new ActionClass(ActionType.Shift, 23));
            Tabelas.tabelaAcao[28].Add("LOGICO", new ActionClass(ActionType.Shift, 23));

            Tabelas.tabelaGOTO[28] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[28].Add("<TIPO_SIMPLES>", 29);

            // <DECL_VAR> -> ID DOIS_PONTO VETOR ABRE_COL <LISTA_VETOR_FAIXA> FECHA_COL DE <TIPO_SIMPLES> . PONTO_VIRGULA
            Tabelas.tabelaAcao[29] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[29].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 30));

            // <DECL_VAR> -> ID DOIS_PONTO VETOR ABRE_COL <LISTA_VETOR_FAIXA> FECHA_COL DE <TIPO_SIMPLES> PONTO_VIRGULA .
            Tabelas.tabelaAcao[30] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[30].Add("ID", new ActionClass(ActionType.Reduce, r12));
            Tabelas.tabelaAcao[30].Add("PROCEDIMENTO", new ActionClass(ActionType.Reduce, r12));
            Tabelas.tabelaAcao[30].Add("FUNCAO", new ActionClass(ActionType.Reduce, r12));
            Tabelas.tabelaAcao[30].Add("INICIO", new ActionClass(ActionType.Reduce, r12));

            // <LISTA_VETOR_FAIXA> -> CONST_FAIXA_VETOR .
            Tabelas.tabelaAcao[31] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[31].Add("VIRGULA", new ActionClass(ActionType.Reduce, r14));
            Tabelas.tabelaAcao[31].Add("FECHA_COL", new ActionClass(ActionType.Reduce, r14));

            // <LISTA_VETOR_FAIXA> -> <LISTA_VETOR_FAIXA> VIRGULA . CONST_FAIXA_VETOR
            Tabelas.tabelaAcao[32] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[32].Add("CONST_FAIXA_VETOR", new ActionClass(ActionType.Shift, 33));

            // <LISTA_VETOR_FAIXA> -> <LISTA_VETOR_FAIXA> VIRGULA CONST_FAIXA_VETOR .
            Tabelas.tabelaAcao[33] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[33].Add("VIRGULA", new ActionClass(ActionType.Reduce, r15));
            Tabelas.tabelaAcao[33].Add("FECHA_COL", new ActionClass(ActionType.Reduce, r15));
            


            // <BLOCO_PROCEDIMENTO> -> PROCEDIMENTO . ID ABRE_PAR <LISTA_PARAMETROS> FECHA_PAR <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> FIM PONTO_VIRGULA
            Tabelas.tabelaAcao[38] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[38].Add("ID", new ActionClass(ActionType.Shift, 39));

            // <BLOCO_PROCEDIMENTO> -> PROCEDIMENTO ID . ABRE_PAR <LISTA_PARAMETROS> FECHA_PAR <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> FIM PONTO_VIRGULA
            Tabelas.tabelaAcao[39] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[39].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 40));

            // <BLOCO_PROCEDIMENTO> -> PROCEDIMENTO ID ABRE_PAR . <LISTA_PARAMETROS> FECHA_PAR <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> FIM PONTO_VIRGULA
            Tabelas.tabelaAcao[40] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[40].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r24));
            Tabelas.tabelaAcao[40].Add("VAR", new ActionClass(ActionType.Shift, 73));
            Tabelas.tabelaAcao[40].Add("ID", new ActionClass(ActionType.Shift, 75));

            Tabelas.tabelaGOTO[40] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[40].Add("<LISTA_PARAMETROS>", 41);
            Tabelas.tabelaGOTO[40].Add("<DECL_PARAMETRO>", 65);
            Tabelas.tabelaGOTO[40].Add("<LISTA_PR_ID_VAR>", 68);

            // <BLOCO_PROCEDIMENTO> -> PROCEDIMENTO ID ABRE_PAR <LISTA_PARAMETROS> . FECHA_PAR <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> FIM PONTO_VIRGULA
            Tabelas.tabelaAcao[41] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[41].Add("FECHA_PAR", new ActionClass(ActionType.Shift, 42));
            Tabelas.tabelaAcao[41].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 66));

            Tabelas.tabelaGOTO[41] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[41].Add("<LISTA_PR_ID_VAR>", 68);

            // <BLOCO_PROCEDIMENTO> -> PROCEDIMENTO ID ABRE_PAR <LISTA_PARAMETROS> FECHA_PAR . <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> FIM PONTO_VIRGULA
            Tabelas.tabelaAcao[42] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[42].Add("VARIAVEIS", new ActionClass(ActionType.Shift, 8));
            Tabelas.tabelaAcao[42].Add("INICIO", new ActionClass(ActionType.Reduce, r3));

            Tabelas.tabelaGOTO[42] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[42].Add("<BLOCO_LISTA_DECL_VAR>", 43);

            // <BLOCO_PROCEDIMENTO> -> PROCEDIMENTO ID ABRE_PAR <LISTA_PARAMETROS> FECHA_PAR <BLOCO_LISTA_DECL_VAR> . INICIO <LISTA_COMANDOS> FIM PONTO_VIRGULA
            Tabelas.tabelaAcao[43] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[43].Add("INICIO", new ActionClass(ActionType.Shift, 44));

            // <BLOCO_PROCEDIMENTO> -> PROCEDIMENTO ID ABRE_PAR <LISTA_PARAMETROS> FECHA_PAR <BLOCO_LISTA_DECL_VAR> INICIO . <LISTA_COMANDOS> FIM PONTO_VIRGULA
            Tabelas.tabelaAcao[44] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[44].Add("LEIA", new ActionClass(ActionType.Shift, 652));
            Tabelas.tabelaAcao[44].Add("ESCREVA", new ActionClass(ActionType.Shift, 702));
            Tabelas.tabelaAcao[44].Add("ESCREVALN", new ActionClass(ActionType.Shift, 709));
            Tabelas.tabelaAcao[44].Add("ID", new ActionClass(ActionType.Shift, 202));
            Tabelas.tabelaAcao[44].Add("SE", new ActionClass(ActionType.Shift, 502));
            //Tabelas.tabelaAcao[44].Add("", new ActionClass(ActionType.Shift, ));
            //Tabelas.tabelaAcao[44].Add("", new ActionClass(ActionType.Shift, ));

            Tabelas.tabelaGOTO[44] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[44].Add("<LISTA_COMANDOS>", 45);
            Tabelas.tabelaGOTO[44].Add("<CMD_LEIA>", 650);
            Tabelas.tabelaGOTO[44].Add("<CMD_ESCREVA>", 700);
            Tabelas.tabelaGOTO[44].Add("<CMD_ESCREVALN>", 707);
            Tabelas.tabelaGOTO[44].Add("<CMD_ATRIBUICAO>", 200);
            Tabelas.tabelaGOTO[44].Add("<CMD_ATRIB_VAR_REC>", 203);
            Tabelas.tabelaGOTO[44].Add("<CMD_CHAMA_PROC>", 150);
            Tabelas.tabelaGOTO[44].Add("<CMD_SEL_IF>", 500);
            //Tabelas.tabelaGOTO[44].Add("<>", );
            //Tabelas.tabelaGOTO[44].Add("<>", );

            // <BLOCO_PROCEDIMENTO> -> PROCEDIMENTO ID ABRE_PAR <LISTA_PARAMETROS> FECHA_PAR <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> . FIM PONTO_VIRGULA
            Tabelas.tabelaAcao[45] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[45].Add("LEIA", new ActionClass(ActionType.Shift, 652));
            Tabelas.tabelaAcao[45].Add("ESCREVA", new ActionClass(ActionType.Shift, 702));
            Tabelas.tabelaAcao[45].Add("ESCREVALN", new ActionClass(ActionType.Shift, 709));
            Tabelas.tabelaAcao[45].Add("ID", new ActionClass(ActionType.Shift, 202));
            Tabelas.tabelaAcao[45].Add("SE", new ActionClass(ActionType.Shift, 502));
            //Tabelas.tabelaAcao[45].Add("", new ActionClass(ActionType.Shift, ));
            //Tabelas.tabelaAcao[45].Add("", new ActionClass(ActionType.Shift, ));
            Tabelas.tabelaAcao[45].Add("FIM", new ActionClass(ActionType.Shift, 46));

            Tabelas.tabelaGOTO[45] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[45].Add("<CMD_LEIA>", 651);
            Tabelas.tabelaGOTO[45].Add("<CMD_ESCREVA>", 701);
            Tabelas.tabelaGOTO[45].Add("<CMD_ESCREVALN>", 708);
            Tabelas.tabelaGOTO[45].Add("<CMD_ATRIBUICAO>", 201);
            Tabelas.tabelaGOTO[45].Add("<CMD_ATRIB_VAR_REC>", 203);
            Tabelas.tabelaGOTO[45].Add("<CMD_CHAMA_PROC>", 151);
            Tabelas.tabelaGOTO[45].Add("<CMD_SEL_IF>", 501);
            //Tabelas.tabelaGOTO[45].Add("<>", );
            //Tabelas.tabelaGOTO[45].Add("<>", );

            // <BLOCO_PROCEDIMENTO> -> PROCEDIMENTO ID ABRE_PAR <LISTA_PARAMETROS> FECHA_PAR <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> FIM . PONTO_VIRGULA
            Tabelas.tabelaAcao[46] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[46].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 47));

            // <BLOCO_PROCEDIMENTO> -> PROCEDIMENTO ID ABRE_PAR <LISTA_PARAMETROS> FECHA_PAR <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> FIM PONTO_VIRGULA .
            Tabelas.tabelaAcao[47] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[47].Add("VARIAVEIS", new ActionClass(ActionType.Reduce, r17));
            Tabelas.tabelaAcao[47].Add("PROCEDIMENTO", new ActionClass(ActionType.Reduce, r17));
            Tabelas.tabelaAcao[47].Add("FUNCAO", new ActionClass(ActionType.Reduce, r17));
            Tabelas.tabelaAcao[47].Add("INICIO", new ActionClass(ActionType.Reduce, r17));

            // <BLOCO_LISTA_PROC_FUNC> -> <BLOCO_PROCEDIMENTO> .
            Tabelas.tabelaAcao[48] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[48].Add("VARIAVEIS", new ActionClass(ActionType.Reduce, r19));
            Tabelas.tabelaAcao[48].Add("PROCEDIMENTO", new ActionClass(ActionType.Reduce, r19));
            Tabelas.tabelaAcao[48].Add("FUNCAO", new ActionClass(ActionType.Reduce, r19));
            Tabelas.tabelaAcao[48].Add("INICIO", new ActionClass(ActionType.Reduce, r19));

            // <BLOCO_LISTA_PROC_FUNC> -> <BLOCO_LISTA_PROC_FUNC> <BLOCO_PROCEDIMENTO> .
            Tabelas.tabelaAcao[49] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[49].Add("VARIAVEIS", new ActionClass(ActionType.Reduce, r20));
            Tabelas.tabelaAcao[49].Add("PROCEDIMENTO", new ActionClass(ActionType.Reduce, r20));
            Tabelas.tabelaAcao[49].Add("FUNCAO", new ActionClass(ActionType.Reduce, r20));
            Tabelas.tabelaAcao[49].Add("INICIO", new ActionClass(ActionType.Reduce, r20));

            // <BLOCO_FUNCAO> -> FUNCAO . ID ABRE_PAR <LISTA_PARAMETROS> FECHA_PAR DOIS_PONTO <TIPO_DADOS_FUNC_RET> <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> <FUNC_RETORNE> FIM PONTO_VIRGULA
            Tabelas.tabelaAcao[50] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[50].Add("ID", new ActionClass(ActionType.Shift, 51));

            // <BLOCO_FUNCAO> -> FUNCAO ID . ABRE_PAR <LISTA_PARAMETROS> FECHA_PAR DOIS_PONTO <TIPO_DADOS_FUNC_RET> <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> <FUNC_RETORNE> FIM PONTO_VIRGULA
            Tabelas.tabelaAcao[51] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[51].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 52));

            // <BLOCO_FUNCAO> -> FUNCAO ID ABRE_PAR . <LISTA_PARAMETROS> FECHA_PAR DOIS_PONTO <TIPO_DADOS_FUNC_RET> <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> <FUNC_RETORNE> FIM PONTO_VIRGULA
            Tabelas.tabelaAcao[52] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[52].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r24));
            Tabelas.tabelaAcao[52].Add("VAR", new ActionClass(ActionType.Shift, 73));
            Tabelas.tabelaAcao[52].Add("ID", new ActionClass(ActionType.Shift, 75));

            Tabelas.tabelaGOTO[52] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[52].Add("<LISTA_PARAMETROS>", 53);
            Tabelas.tabelaGOTO[52].Add("<DECL_PARAMETRO>", 65);
            Tabelas.tabelaGOTO[52].Add("<LISTA_PR_ID_VAR>", 68);

            // <BLOCO_FUNCAO> -> FUNCAO ID ABRE_PAR <LISTA_PARAMETROS> . FECHA_PAR DOIS_PONTO <TIPO_DADOS_FUNC_RET> <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> <FUNC_RETORNE> FIM PONTO_VIRGULA
            Tabelas.tabelaAcao[53] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[53].Add("FECHA_PAR", new ActionClass(ActionType.Shift, 54));
            Tabelas.tabelaAcao[53].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 66));

            Tabelas.tabelaGOTO[53] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[53].Add("<LISTA_PR_ID_VAR>", 68);

            // <BLOCO_FUNCAO> -> FUNCAO ID ABRE_PAR <LISTA_PARAMETROS> FECHA_PAR . DOIS_PONTO <TIPO_DADOS_FUNC_RET> <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> <FUNC_RETORNE> FIM PONTO_VIRGULA
            Tabelas.tabelaAcao[54] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[54].Add("DOIS_PONTO", new ActionClass(ActionType.Shift, 55));

            // <BLOCO_FUNCAO> -> FUNCAO ID ABRE_PAR <LISTA_PARAMETROS> FECHA_PAR DOIS_PONTO . <TIPO_DADOS_FUNC_RET> <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> <FUNC_RETORNE> FIM PONTO_VIRGULA
            Tabelas.tabelaAcao[55] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[55].Add("CARACTER", new ActionClass(ActionType.Shift, 78));
            Tabelas.tabelaAcao[55].Add("INTEIRO", new ActionClass(ActionType.Shift, 78));
            Tabelas.tabelaAcao[55].Add("REAL", new ActionClass(ActionType.Shift, 78));
            Tabelas.tabelaAcao[55].Add("LOGICO", new ActionClass(ActionType.Shift, 78));
            Tabelas.tabelaAcao[55].Add("VETOR", new ActionClass(ActionType.Shift, 80));

            Tabelas.tabelaGOTO[55] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[55].Add("<TIPO_DADOS_FUNC_RET>", 56);
            Tabelas.tabelaGOTO[55].Add("<TIPO_SIMPLES_FUNC_RET>", 77);
            Tabelas.tabelaGOTO[55].Add("<TIPO_VETOR_FUNC_RET>", 79);

            // <BLOCO_FUNCAO> -> FUNCAO ID ABRE_PAR <LISTA_PARAMETROS> FECHA_PAR DOIS_PONTO <TIPO_DADOS_FUNC_RET> . <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> <FUNC_RETORNE> FIM PONTO_VIRGULA
            Tabelas.tabelaAcao[56] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[56].Add("VARIAVEIS", new ActionClass(ActionType.Shift, 8));
            Tabelas.tabelaAcao[56].Add("INICIO", new ActionClass(ActionType.Reduce, r3));

            Tabelas.tabelaGOTO[56] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[56].Add("<BLOCO_LISTA_DECL_VAR>", 57);

            // <BLOCO_FUNCAO> -> FUNCAO ID ABRE_PAR <LISTA_PARAMETROS> FECHA_PAR DOIS_PONTO <TIPO_DADOS_FUNC_RET> <BLOCO_LISTA_DECL_VAR> . INICIO <LISTA_COMANDOS> <FUNC_RETORNE> FIM PONTO_VIRGULA
            Tabelas.tabelaAcao[57] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[57].Add("INICIO", new ActionClass(ActionType.Shift, 58));

            // <BLOCO_FUNCAO> -> FUNCAO ID ABRE_PAR <LISTA_PARAMETROS> FECHA_PAR DOIS_PONTO <TIPO_DADOS_FUNC_RET> <BLOCO_LISTA_DECL_VAR> INICIO . <LISTA_COMANDOS> <FUNC_RETORNE> FIM PONTO_VIRGULA
            Tabelas.tabelaAcao[58] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[58].Add("LEIA", new ActionClass(ActionType.Shift, 652));
            Tabelas.tabelaAcao[58].Add("ESCREVA", new ActionClass(ActionType.Shift, 702));
            Tabelas.tabelaAcao[58].Add("ESCREVALN", new ActionClass(ActionType.Shift, 709));
            Tabelas.tabelaAcao[58].Add("ID", new ActionClass(ActionType.Shift, 202));
            Tabelas.tabelaAcao[58].Add("SE", new ActionClass(ActionType.Shift, 502));
            Tabelas.tabelaAcao[58].Add("ENQUANTO", new ActionClass(ActionType.Shift, 532));
            Tabelas.tabelaAcao[58].Add("PARA", new ActionClass(ActionType.Shift, 602));
            //Tabelas.tabelaAcao[58].Add("", new ActionClass(ActionType.Shift, ));
            //Tabelas.tabelaAcao[58].Add("", new ActionClass(ActionType.Shift, ));

            Tabelas.tabelaGOTO[58] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[58].Add("<LISTA_COMANDOS>", 59);
            Tabelas.tabelaGOTO[58].Add("<CMD_LEIA>", 650);
            Tabelas.tabelaGOTO[58].Add("<CMD_ESCREVA>", 700);
            Tabelas.tabelaGOTO[58].Add("<CMD_ESCREVALN>", 707);
            Tabelas.tabelaGOTO[58].Add("<CMD_ATRIBUICAO>", 200);
            Tabelas.tabelaGOTO[58].Add("<CMD_ATRIB_VAR_REC>", 203);
            Tabelas.tabelaGOTO[58].Add("<CMD_CHAMA_PROC>", 150);
            Tabelas.tabelaGOTO[58].Add("<CMD_SEL_IF>", 500);
            Tabelas.tabelaGOTO[58].Add("<CMD_REP_ENQUANTO>", 530);
            Tabelas.tabelaGOTO[58].Add("<CMD_REP_PARA>", 600);
            //Tabelas.tabelaGOTO[58].Add("<>", );
            //Tabelas.tabelaGOTO[58].Add("<>", );

            // <BLOCO_FUNCAO> -> FUNCAO ID ABRE_PAR <LISTA_PARAMETROS> FECHA_PAR DOIS_PONTO <TIPO_DADOS_FUNC_RET> <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> . <FUNC_RETORNE> FIM PONTO_VIRGULA
            Tabelas.tabelaAcao[59] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[59].Add("LEIA", new ActionClass(ActionType.Shift, 652));
            Tabelas.tabelaAcao[59].Add("ESCREVA", new ActionClass(ActionType.Shift, 702));
            Tabelas.tabelaAcao[59].Add("ESCREVALN", new ActionClass(ActionType.Shift, 709));
            Tabelas.tabelaAcao[59].Add("ID", new ActionClass(ActionType.Shift, 202));
            Tabelas.tabelaAcao[59].Add("SE", new ActionClass(ActionType.Shift, 502));
            Tabelas.tabelaAcao[59].Add("ENQUANTO", new ActionClass(ActionType.Shift, 532));
            Tabelas.tabelaAcao[59].Add("PARA", new ActionClass(ActionType.Shift, 602));
            //Tabelas.tabelaAcao[59].Add("", new ActionClass(ActionType.Shift, ));
            //Tabelas.tabelaAcao[59].Add("", new ActionClass(ActionType.Shift, ));
            Tabelas.tabelaAcao[59].Add("RETORNE", new ActionClass(ActionType.Shift, 86));

            Tabelas.tabelaGOTO[59] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[59].Add("<CMD_LEIA>", 651);
            Tabelas.tabelaGOTO[59].Add("<CMD_ESCREVA>", 701);
            Tabelas.tabelaGOTO[59].Add("<CMD_ESCREVALN>", 708);
            Tabelas.tabelaGOTO[59].Add("<CMD_ATRIBUICAO>", 201);
            Tabelas.tabelaGOTO[59].Add("<CMD_ATRIB_VAR_REC>", 203);
            Tabelas.tabelaGOTO[59].Add("<CMD_CHAMA_PROC>", 151);
            Tabelas.tabelaGOTO[59].Add("<CMD_SEL_IF>", 501);
            Tabelas.tabelaGOTO[59].Add("<CMD_REP_ENQUANTO>", 531);
            Tabelas.tabelaGOTO[59].Add("<CMD_REP_PARA>", 601);
            //Tabelas.tabelaGOTO[59].Add("<>", );
            //Tabelas.tabelaGOTO[59].Add("<>", );
            Tabelas.tabelaGOTO[59].Add("<FUNC_RETORNE>", 60);

            // <BLOCO_FUNCAO> -> FUNCAO ID ABRE_PAR <LISTA_PARAMETROS> FECHA_PAR DOIS_PONTO <TIPO_DADOS_FUNC_RET> <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> <FUNC_RETORNE> . FIM PONTO_VIRGULA
            Tabelas.tabelaAcao[60] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[60].Add("FIM", new ActionClass(ActionType.Shift, 61));

            // <BLOCO_FUNCAO> -> FUNCAO ID ABRE_PAR <LISTA_PARAMETROS> FECHA_PAR DOIS_PONTO <TIPO_DADOS_FUNC_RET> <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> <FUNC_RETORNE> FIM . PONTO_VIRGULA
            Tabelas.tabelaAcao[61] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[61].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 62));

            // <BLOCO_FUNCAO> -> FUNCAO ID ABRE_PAR <LISTA_PARAMETROS> FECHA_PAR DOIS_PONTO <TIPO_DADOS_FUNC_RET> <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> <FUNC_RETORNE> FIM PONTO_VIRGULA .
            Tabelas.tabelaAcao[62] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[62].Add("VARIAVEIS", new ActionClass(ActionType.Reduce, r21));
            Tabelas.tabelaAcao[62].Add("PROCEDIMENTO", new ActionClass(ActionType.Reduce, r21));
            Tabelas.tabelaAcao[62].Add("FUNCAO", new ActionClass(ActionType.Reduce, r21));
            Tabelas.tabelaAcao[62].Add("INICIO", new ActionClass(ActionType.Reduce, r21));

            // <BLOCO_LISTA_PROC_FUNC> -> <BLOCO_FUNCAO> .
            Tabelas.tabelaAcao[63] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[63].Add("VARIAVEIS", new ActionClass(ActionType.Reduce, r22));
            Tabelas.tabelaAcao[63].Add("PROCEDIMENTO", new ActionClass(ActionType.Reduce, r22));
            Tabelas.tabelaAcao[63].Add("FUNCAO", new ActionClass(ActionType.Reduce, r22));
            Tabelas.tabelaAcao[63].Add("INICIO", new ActionClass(ActionType.Reduce, r22));

            // <BLOCO_LISTA_PROC_FUNC> -> <BLOCO_LISTA_PROC_FUNC> <BLOCO_FUNCAO> .
            Tabelas.tabelaAcao[64] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[64].Add("VARIAVEIS", new ActionClass(ActionType.Reduce, r23));
            Tabelas.tabelaAcao[64].Add("PROCEDIMENTO", new ActionClass(ActionType.Reduce, r23));
            Tabelas.tabelaAcao[64].Add("FUNCAO", new ActionClass(ActionType.Reduce, r23));
            Tabelas.tabelaAcao[64].Add("INICIO", new ActionClass(ActionType.Reduce, r23));

            // <LISTA_PARAMETROS> -> <DECL_PARAMETRO> .
            Tabelas.tabelaAcao[65] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[65].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r25));
            Tabelas.tabelaAcao[65].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r25));

            // <LISTA_PARAMETROS> -> <LISTA_PARAMETROS> PONTO_VIRGULA . <DECL_PARAMETRO>
            Tabelas.tabelaAcao[66] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[66].Add("VAR", new ActionClass(ActionType.Shift, 73));
            Tabelas.tabelaAcao[66].Add("ID", new ActionClass(ActionType.Shift, 75));

            Tabelas.tabelaGOTO[66] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[66].Add("<LISTA_PARAMETROS>", 41);
            Tabelas.tabelaGOTO[66].Add("<DECL_PARAMETRO>", 67);
            Tabelas.tabelaGOTO[66].Add("<LISTA_PR_ID_VAR>", 68);

            // <LISTA_PARAMETROS> -> <LISTA_PARAMETROS> PONTO_VIRGULA <DECL_PARAMETRO> .
            Tabelas.tabelaAcao[67] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[67].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r26));
            Tabelas.tabelaAcao[67].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r26));

            // <DECL_PARAMETRO> -> <LISTA_PR_ID_VAR> . DOIS_PONTO <TIPO_SIMPLES_PARAM>
            //<LISTA_PR_ID_VAR> -> <LISTA_PR_ID_VAR> . VIRGULA ID
            Tabelas.tabelaAcao[68] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[68].Add("DOIS_PONTO", new ActionClass(ActionType.Shift, 69));
            Tabelas.tabelaAcao[68].Add("VIRGULA", new ActionClass(ActionType.Shift, 71));

            // <DECL_PARAMETRO> -> <LISTA_PR_ID_VAR> DOIS_PONTO . <TIPO_SIMPLES_PARAM>
            Tabelas.tabelaAcao[69] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[69].Add("CARACTER", new ActionClass(ActionType.Shift, 76));
            Tabelas.tabelaAcao[69].Add("INTEIRO", new ActionClass(ActionType.Shift, 76));
            Tabelas.tabelaAcao[69].Add("REAL", new ActionClass(ActionType.Shift, 76));
            Tabelas.tabelaAcao[69].Add("LOGICO", new ActionClass(ActionType.Shift, 76));

            Tabelas.tabelaGOTO[69] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[69].Add("<TIPO_SIMPLES_PARAM>", 70);

            // <DECL_PARAMETRO> -> <LISTA_PR_ID_VAR> DOIS_PONTO <TIPO_SIMPLES_PARAM> .
            Tabelas.tabelaAcao[70] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[70].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r27));
            Tabelas.tabelaAcao[70].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r27));

            // <LISTA_PR_ID_VAR> -> <LISTA_PR_ID_VAR> VIRGULA . ID
            Tabelas.tabelaAcao[71] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[71].Add("ID", new ActionClass(ActionType.Shift, 72));

            // <LISTA_PR_ID_VAR> -> <LISTA_PR_ID_VAR> VIRGULA ID .
            Tabelas.tabelaAcao[72] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[72].Add("DOIS_PONTO", new ActionClass(ActionType.Reduce, r28));
            Tabelas.tabelaAcao[72].Add("VIRGULA", new ActionClass(ActionType.Reduce, r28));

            // <LISTA_PR_ID_VAR> -> VAR . ID
            Tabelas.tabelaAcao[73] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[73].Add("ID", new ActionClass(ActionType.Shift, 74));

            // <LISTA_PR_ID_VAR> -> VAR ID .
            Tabelas.tabelaAcao[74] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[74].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r29));
            Tabelas.tabelaAcao[74].Add("DOIS_PONTO", new ActionClass(ActionType.Reduce, r29));
            Tabelas.tabelaAcao[74].Add("VIRGULA", new ActionClass(ActionType.Reduce, r29));

            // <LISTA_PR_ID_VAR> -> ID .
            Tabelas.tabelaAcao[75] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[75].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r30));
            Tabelas.tabelaAcao[75].Add("DOIS_PONTO", new ActionClass(ActionType.Reduce, r30));
            Tabelas.tabelaAcao[75].Add("VIRGULA", new ActionClass(ActionType.Reduce, r30));

            // <TIPO_SIMPLES_PARAM> -> CARACTER .
            // <TIPO_SIMPLES_PARAM> -> INTEIRO .
            // <TIPO_SIMPLES_PARAM> -> REAL .
            // <TIPO_SIMPLES_PARAM> -> LOGICO .
            Tabelas.tabelaAcao[76] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[76].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r31));
            Tabelas.tabelaAcao[76].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r31));

            // <TIPO_DADOS_FUNC_RET> -> <TIPO_SIMPLES_FUNC_RET> .
            Tabelas.tabelaAcao[77] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[77].Add("VARIAVEIS", new ActionClass(ActionType.Reduce, r32));
            Tabelas.tabelaAcao[77].Add("INICIO", new ActionClass(ActionType.Reduce, r32));

            // <TIPO_SIMPLES_FUNC_RET> -> CARACTER .
            // <TIPO_SIMPLES_FUNC_RET> -> INTEIRO .
            // <TIPO_SIMPLES_FUNC_RET> -> REAL .
            // <TIPO_SIMPLES_FUNC_RET> -> LOGICO .
            Tabelas.tabelaAcao[78] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[78].Add("VARIAVEIS", new ActionClass(ActionType.Reduce, r33));
            Tabelas.tabelaAcao[78].Add("INICIO", new ActionClass(ActionType.Reduce, r33));

            // <TIPO_DADOS_FUNC_RET> -> <TIPO_VETOR_FUNC_RET> .
            Tabelas.tabelaAcao[79] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[79].Add("VARIAVEIS", new ActionClass(ActionType.Reduce, r34));
            Tabelas.tabelaAcao[79].Add("INICIO", new ActionClass(ActionType.Reduce, r34));

            // <TIPO_VETOR_FUNC_RET> -> VETOR . ABRE_COL <LISTA_VETOR_FAIXA> FECHA_COL DE <TIPO_SIMPLES_FUNC_RET>
            Tabelas.tabelaAcao[80] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[80].Add("ABRE_COL", new ActionClass(ActionType.Shift, 81));

            // <TIPO_VETOR_FUNC_RET> -> VETOR ABRE_COL . <LISTA_VETOR_FAIXA> FECHA_COL DE <TIPO_SIMPLES_FUNC_RET>
            Tabelas.tabelaAcao[81] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[81].Add("CONST_INT", new ActionClass(ActionType.Shift, 34));

            Tabelas.tabelaGOTO[81] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[81].Add("<LISTA_VETOR_FAIXA>", 82);
            Tabelas.tabelaGOTO[81].Add("<TIPO_VETOR_FAIXA>", 31);

            // <TIPO_VETOR_FUNC_RET> -> VETOR ABRE_COL <LISTA_VETOR_FAIXA> . FECHA_COL DE <TIPO_SIMPLES_FUNC_RET>
            // <LISTA_VETOR_FAIXA> -> <LISTA_VETOR_FAIXA> . PONTO_VIRGULA <TIPO_VETOR_FAIXA>
            Tabelas.tabelaAcao[82] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[82].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 32));
            Tabelas.tabelaAcao[82].Add("FECHA_COL", new ActionClass(ActionType.Shift, 83));

            // <TIPO_VETOR_FUNC_RET> -> VETOR ABRE_COL <LISTA_VETOR_FAIXA> FECHA_COL . DE <TIPO_SIMPLES_FUNC_RET>
            Tabelas.tabelaAcao[83] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[83].Add("DE", new ActionClass(ActionType.Shift, 84));

            // <TIPO_VETOR_FUNC_RET> -> VETOR ABRE_COL <LISTA_VETOR_FAIXA> FECHA_COL DE . <TIPO_SIMPLES_FUNC_RET>
            Tabelas.tabelaAcao[84] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[84].Add("CARACTER", new ActionClass(ActionType.Shift, 78));
            Tabelas.tabelaAcao[84].Add("INTEIRO", new ActionClass(ActionType.Shift, 78));
            Tabelas.tabelaAcao[84].Add("REAL", new ActionClass(ActionType.Shift, 78));
            Tabelas.tabelaAcao[84].Add("LOGICO", new ActionClass(ActionType.Shift, 78));

            Tabelas.tabelaGOTO[84] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[84].Add("<TIPO_SIMPLES_FUNC_RET>", 85);

            // <TIPO_VETOR_FUNC_RET> -> VETOR ABRE_COL <LISTA_VETOR_FAIXA> FECHA_COL DE <TIPO_SIMPLES_FUNC_RET> .
            Tabelas.tabelaAcao[85] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[85].Add("VARIAVEIS", new ActionClass(ActionType.Reduce, r35));
            Tabelas.tabelaAcao[85].Add("INICIO", new ActionClass(ActionType.Reduce, r35));

            // <FUNC_RETORNE> -> RETORNE . <FUNC_RET_VAR> PONTO_VIRGULA
            Tabelas.tabelaAcao[86] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[86].Add("ID", new ActionClass(ActionType.Shift, 91));
            Tabelas.tabelaAcao[86].Add("CONST_INT", new ActionClass(ActionType.Shift, 91));
            Tabelas.tabelaAcao[86].Add("CONST_REAL", new ActionClass(ActionType.Shift, 91));
            Tabelas.tabelaAcao[86].Add("CONST_TEXTO", new ActionClass(ActionType.Shift, 91));

            Tabelas.tabelaGOTO[86] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[86].Add("<FUNC_RET_VAR>", 87);

            // <FUNC_RETORNE> -> RETORNE <FUNC_RET_VAR> . PONTO_VIRGULA
            Tabelas.tabelaAcao[87] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[87].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 88));
            Tabelas.tabelaAcao[87].Add("MENOS", new ActionClass(ActionType.Shift, 92));
            Tabelas.tabelaAcao[87].Add("MAIS", new ActionClass(ActionType.Shift, 92));
            Tabelas.tabelaAcao[87].Add("ASTERISTICO", new ActionClass(ActionType.Shift, 92));
            Tabelas.tabelaAcao[87].Add("BARRA", new ActionClass(ActionType.Shift, 92));

            Tabelas.tabelaGOTO[87] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[87].Add("<FUNC_RET_OP>", 89);

            // <FUNC_RETORNE> -> RETORNE <FUNC_RET_VAR> PONTO_VIRGULA .
            Tabelas.tabelaAcao[88] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[88].Add("FIM", new ActionClass(ActionType.Reduce, r36));

            // <FUNC_RET_VAR> -> <FUNC_RET_VAR> <FUNC_RET_OP> . <FUNC_RET_VAR>
            Tabelas.tabelaAcao[89] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[89].Add("ID", new ActionClass(ActionType.Shift, 91));
            Tabelas.tabelaAcao[89].Add("CONST_INT", new ActionClass(ActionType.Shift, 91));
            Tabelas.tabelaAcao[89].Add("CONST_REAL", new ActionClass(ActionType.Shift, 91));
            Tabelas.tabelaAcao[89].Add("CONST_TEXTO", new ActionClass(ActionType.Shift, 91));

            Tabelas.tabelaGOTO[89] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[89].Add("<FUNC_RET_VAR>", 90);

            // <FUNC_RET_VAR> -> <FUNC_RET_VAR> <FUNC_RET_OP> <FUNC_RET_VAR> .
            Tabelas.tabelaAcao[90] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[90].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r37));
            Tabelas.tabelaAcao[90].Add("MENOS", new ActionClass(ActionType.Reduce, r37));
            Tabelas.tabelaAcao[90].Add("MAIS", new ActionClass(ActionType.Reduce, r37));
            Tabelas.tabelaAcao[90].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r37));
            Tabelas.tabelaAcao[90].Add("BARRA", new ActionClass(ActionType.Reduce, r37));

            // <FUNC_RET_VAR> -> ID .
            // <FUNC_RET_VAR> -> CONST_INT .
            // <FUNC_RET_VAR> -> CONST_REAL .
            // <FUNC_RET_VAR> -> CONST_TEXTO .
            Tabelas.tabelaAcao[91] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[91].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r38));
            Tabelas.tabelaAcao[91].Add("MENOS", new ActionClass(ActionType.Reduce, r38));
            Tabelas.tabelaAcao[91].Add("MAIS", new ActionClass(ActionType.Reduce, r38));
            Tabelas.tabelaAcao[91].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r38));
            Tabelas.tabelaAcao[91].Add("BARRA", new ActionClass(ActionType.Reduce, r38));

            // <FUNC_RET_OP> -> MENOS .
            // <FUNC_RET_OP> -> MAIS .
            // <FUNC_RET_OP> -> ASTERISTICO .
            // <FUNC_RET_OP> -> BARRA .
            Tabelas.tabelaAcao[92] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[92].Add("ID", new ActionClass(ActionType.Reduce, r39));
            Tabelas.tabelaAcao[92].Add("CONST_INT", new ActionClass(ActionType.Reduce, r39));
            Tabelas.tabelaAcao[92].Add("CONST_REAL", new ActionClass(ActionType.Reduce, r39));
            Tabelas.tabelaAcao[92].Add("CONST_TEXTO", new ActionClass(ActionType.Reduce, r39));

            // <BLOCO_PRINCIPAL> -> <BLOCO_LISTA_DECL_VAR> . INICIO <LISTA_COMANDOS> FIM
            Tabelas.tabelaAcao[93] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[93].Add("INICIO", new ActionClass(ActionType.Shift, 94));

            // <BLOCO_PRINCIPAL> -> <BLOCO_LISTA_DECL_VAR> INICIO . <LISTA_COMANDOS> FIM
            Tabelas.tabelaAcao[94] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[94].Add("LEIA", new ActionClass(ActionType.Shift, 652));
            Tabelas.tabelaAcao[94].Add("ESCREVA", new ActionClass(ActionType.Shift, 702));
            Tabelas.tabelaAcao[94].Add("ESCREVALN", new ActionClass(ActionType.Shift, 709));
            Tabelas.tabelaAcao[94].Add("ID", new ActionClass(ActionType.Shift, 202));
            Tabelas.tabelaAcao[94].Add("SE", new ActionClass(ActionType.Shift, 502));
            Tabelas.tabelaAcao[94].Add("ENQUANTO", new ActionClass(ActionType.Shift, 532));
            Tabelas.tabelaAcao[94].Add("REPITA", new ActionClass(ActionType.Shift, 552));
            Tabelas.tabelaAcao[94].Add("PARA", new ActionClass(ActionType.Shift, 602));
            //Tabelas.tabelaAcao[94].Add("", new ActionClass(ActionType.Shift, ));
            //Tabelas.tabelaAcao[94].Add("", new ActionClass(ActionType.Shift, ));

            Tabelas.tabelaGOTO[94] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[94].Add("<LISTA_COMANDOS>", 95);
            Tabelas.tabelaGOTO[94].Add("<CMD_LEIA>", 650);
            Tabelas.tabelaGOTO[94].Add("<CMD_ESCREVA>", 700);
            Tabelas.tabelaGOTO[94].Add("<CMD_ESCREVALN>", 707);
            Tabelas.tabelaGOTO[94].Add("<CMD_ATRIBUICAO>", 200);
            Tabelas.tabelaGOTO[94].Add("<CMD_ATRIB_VAR_REC>", 203);
            Tabelas.tabelaGOTO[94].Add("<CMD_CHAMA_PROC>", 150);
            Tabelas.tabelaGOTO[94].Add("<CMD_SEL_IF>", 500);
            Tabelas.tabelaGOTO[94].Add("<CMD_REP_ENQUANTO>", 530);
            Tabelas.tabelaGOTO[94].Add("<CMD_REP_REPITA>", 550);
            Tabelas.tabelaGOTO[94].Add("<CMD_REP_PARA>", 600);
            //Tabelas.tabelaGOTO[94].Add("<>", );
            //Tabelas.tabelaGOTO[94].Add("<>", );

            // <BLOCO_PRINCIPAL> -> <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> . FIM
            Tabelas.tabelaAcao[95] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[95].Add("LEIA", new ActionClass(ActionType.Shift, 652));
            Tabelas.tabelaAcao[95].Add("ESCREVA", new ActionClass(ActionType.Shift, 702));
            Tabelas.tabelaAcao[95].Add("ESCREVALN", new ActionClass(ActionType.Shift, 709));
            Tabelas.tabelaAcao[95].Add("ID", new ActionClass(ActionType.Shift, 202));
            Tabelas.tabelaAcao[95].Add("SE", new ActionClass(ActionType.Shift, 502));
            Tabelas.tabelaAcao[95].Add("ENQUANTO", new ActionClass(ActionType.Shift, 532));
            Tabelas.tabelaAcao[95].Add("REPITA", new ActionClass(ActionType.Shift, 552));
            Tabelas.tabelaAcao[95].Add("PARA", new ActionClass(ActionType.Shift, 602));
            //Tabelas.tabelaAcao[95].Add("", new ActionClass(ActionType.Shift, ));
            //Tabelas.tabelaAcao[95].Add("", new ActionClass(ActionType.Shift, ));
            Tabelas.tabelaAcao[95].Add("FIM", new ActionClass(ActionType.Shift, 96));

            Tabelas.tabelaGOTO[95] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[95].Add("<CMD_LEIA>", 651);
            Tabelas.tabelaGOTO[95].Add("<CMD_ESCREVA>", 701);
            Tabelas.tabelaGOTO[95].Add("<CMD_ESCREVALN>", 708);
            Tabelas.tabelaGOTO[95].Add("<CMD_ATRIBUICAO>", 201);
            Tabelas.tabelaGOTO[95].Add("<CMD_ATRIB_VAR_REC>", 203);
            Tabelas.tabelaGOTO[95].Add("<CMD_CHAMA_PROC>", 151);
            Tabelas.tabelaGOTO[95].Add("<CMD_SEL_IF>", 501);
            Tabelas.tabelaGOTO[95].Add("<CMD_REP_ENQUANTO>", 531);
            Tabelas.tabelaGOTO[95].Add("<CMD_REP_REPITA>", 551);
            Tabelas.tabelaGOTO[95].Add("<CMD_REP_PARA>", 601);
            //Tabelas.tabelaGOTO[95].Add("<>", );
            //Tabelas.tabelaGOTO[95].Add("<>", );

            // <BLOCO_PRINCIPAL> -> <BLOCO_LISTA_DECL_VAR> INICIO <LISTA_COMANDOS> FIM .
            Tabelas.tabelaAcao[96] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[96].Add("PONTO", new ActionClass(ActionType.Reduce, r40));


            #region Chamada de Procedimentos
            
            // <LISTA_COMANDOS> -> <CMD_CHAMA_PROC> .
            Tabelas.tabelaAcao[150] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[150].Add("LEIA", new ActionClass(ActionType.Reduce, r160));
            Tabelas.tabelaAcao[150].Add("ESCREVA", new ActionClass(ActionType.Reduce, r160));
            Tabelas.tabelaAcao[150].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r160));
            Tabelas.tabelaAcao[150].Add("ID", new ActionClass(ActionType.Reduce, r160));
            //Tabelas.tabelaAcao[150].Add("", new ActionClass(ActionType.Reduce, r160));
            //Tabelas.tabelaAcao[150].Add("", new ActionClass(ActionType.Reduce, r160));
            Tabelas.tabelaAcao[150].Add("RETORNE", new ActionClass(ActionType.Reduce, r160));
            Tabelas.tabelaAcao[150].Add("FIM", new ActionClass(ActionType.Reduce, r160));

            // <LISTA_COMANDOS> -> <LISTA_COMANDOS> <CMD_CHAMA_PROC> .
            Tabelas.tabelaAcao[151] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[151].Add("LEIA", new ActionClass(ActionType.Reduce, r161));
            Tabelas.tabelaAcao[151].Add("ESCREVA", new ActionClass(ActionType.Reduce, r161));
            Tabelas.tabelaAcao[151].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r161));
            Tabelas.tabelaAcao[151].Add("ID", new ActionClass(ActionType.Reduce, r161));
            //Tabelas.tabelaAcao[151].Add("", new ActionClass(ActionType.Reduce, r161));
            //Tabelas.tabelaAcao[151].Add("", new ActionClass(ActionType.Reduce, r161));
            Tabelas.tabelaAcao[151].Add("RETORNE", new ActionClass(ActionType.Reduce, r161));
            Tabelas.tabelaAcao[151].Add("FIM", new ActionClass(ActionType.Reduce, r161));

            // <CMD_CHAMA_PROC> -> ID ABRE_PAR . <LST_CHAMA_PROC_PARAM> FECHA_PAR PONTO_VIRGULA
            // <LST_CHAMA_PROC_PARAM> -> . <LST_CHAMA_PROC_PARAM> VIRGULA ID
            Tabelas.tabelaAcao[152] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[152].Add("ID", new ActionClass(ActionType.Shift, 156));

            Tabelas.tabelaGOTO[152] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[152].Add("<LST_CHAMA_PROC_PARAM>", 153);

            // <CMD_CHAMA_PROC> -> ID ABRE_PAR <LST_CHAMA_PROC_PARAM> . FECHA_PAR PONTO_VIRGULA
            // <LST_CHAMA_PROC_PARAM> -> <LST_CHAMA_PROC_PARAM> . VIRGULA ID
            Tabelas.tabelaAcao[153] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[153].Add("FECHA_PAR", new ActionClass(ActionType.Shift, 154));
            Tabelas.tabelaAcao[153].Add("VIRGULA", new ActionClass(ActionType.Shift, 157));

            // <CMD_CHAMA_PROC> -> ID ABRE_PAR <LST_CHAMA_PROC_PARAM> FECHA_PAR . PONTO_VIRGULA
            Tabelas.tabelaAcao[154] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[154].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 155));

            // <CMD_CHAMA_PROC> -> ID ABRE_PAR <LST_CHAMA_PROC_PARAM> FECHA_PAR PONTO_VIRGULA .
            Tabelas.tabelaAcao[155] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[155].Add("LEIA", new ActionClass(ActionType.Reduce, r162));
            Tabelas.tabelaAcao[155].Add("ESCREVA", new ActionClass(ActionType.Reduce, r162));
            Tabelas.tabelaAcao[155].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r162));
            Tabelas.tabelaAcao[155].Add("ID", new ActionClass(ActionType.Reduce, r162));
            //Tabelas.tabelaAcao[155].Add("", new ActionClass(ActionType.Reduce, r162));
            //Tabelas.tabelaAcao[155].Add("", new ActionClass(ActionType.Reduce, r162));
            Tabelas.tabelaAcao[155].Add("RETORNE", new ActionClass(ActionType.Reduce, r162));
            Tabelas.tabelaAcao[155].Add("FIM", new ActionClass(ActionType.Reduce, r162));

            // <LST_CHAMA_PROC_PARAM> -> ID .
            Tabelas.tabelaAcao[156] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[156].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r163));
            Tabelas.tabelaAcao[156].Add("VIRGULA", new ActionClass(ActionType.Reduce, r163));

            // <LST_CHAMA_PROC_PARAM> -> <LST_CHAMA_PROC_PARAM> VIRGULA . ID
            Tabelas.tabelaAcao[157] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[157].Add("ID", new ActionClass(ActionType.Shift, 158));

            // <LST_CHAMA_PROC_PARAM> -> <LST_CHAMA_PROC_PARAM> VIRGULA ID .
            Tabelas.tabelaAcao[158] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[158].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r164));
            Tabelas.tabelaAcao[158].Add("VIRGULA", new ActionClass(ActionType.Reduce, r164));

            #endregion

            #region Comandos de Atribuição

            #region Atribuição com Valores Numéricos, Texto, Variáveis, Vetores e Funções

            // <LISTA_COMANDOS> -> <CMD_ATRIBUICAO> .
            Tabelas.tabelaAcao[200] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[200].Add("LEIA", new ActionClass(ActionType.Reduce, r130));
            Tabelas.tabelaAcao[200].Add("ESCREVA", new ActionClass(ActionType.Reduce, r130));
            Tabelas.tabelaAcao[200].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r130));
            Tabelas.tabelaAcao[200].Add("ID", new ActionClass(ActionType.Reduce, r130));
            Tabelas.tabelaAcao[200].Add("SE", new ActionClass(ActionType.Reduce, r130));
            Tabelas.tabelaAcao[200].Add("SENAO", new ActionClass(ActionType.Reduce, r130));
            Tabelas.tabelaAcao[200].Add("FIM_SE", new ActionClass(ActionType.Reduce, r130));
            Tabelas.tabelaAcao[200].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r130));
            Tabelas.tabelaAcao[200].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r130));
            Tabelas.tabelaAcao[200].Add("REPITA", new ActionClass(ActionType.Reduce, r130));
            Tabelas.tabelaAcao[200].Add("ATE", new ActionClass(ActionType.Reduce, r130));
            Tabelas.tabelaAcao[200].Add("PARA", new ActionClass(ActionType.Reduce, r130));
            Tabelas.tabelaAcao[200].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r130));
            //Tabelas.tabelaAcao[200].Add("", new ActionClass(ActionType.Reduce, r130));
            //Tabelas.tabelaAcao[200].Add("", new ActionClass(ActionType.Reduce, r130));
            Tabelas.tabelaAcao[200].Add("RETORNE", new ActionClass(ActionType.Reduce, r130));
            Tabelas.tabelaAcao[200].Add("FIM", new ActionClass(ActionType.Reduce, r130));

            // <LISTA_COMANDOS> -> <LISTA_COMANDOS> <CMD_ATRIBUICAO> .
            Tabelas.tabelaAcao[201] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[201].Add("LEIA", new ActionClass(ActionType.Reduce, r131));
            Tabelas.tabelaAcao[201].Add("ESCREVA", new ActionClass(ActionType.Reduce, r131));
            Tabelas.tabelaAcao[201].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r131));
            Tabelas.tabelaAcao[201].Add("ID", new ActionClass(ActionType.Reduce, r131));
            Tabelas.tabelaAcao[201].Add("SE", new ActionClass(ActionType.Reduce, r131));
            Tabelas.tabelaAcao[201].Add("SENAO", new ActionClass(ActionType.Reduce, r131));
            Tabelas.tabelaAcao[201].Add("FIM_SE", new ActionClass(ActionType.Reduce, r131));
            Tabelas.tabelaAcao[201].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r131));
            Tabelas.tabelaAcao[201].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r131));
            Tabelas.tabelaAcao[201].Add("REPITA", new ActionClass(ActionType.Reduce, r131));
            Tabelas.tabelaAcao[201].Add("ATE", new ActionClass(ActionType.Reduce, r131));
            Tabelas.tabelaAcao[201].Add("PARA", new ActionClass(ActionType.Reduce, r131));
            Tabelas.tabelaAcao[201].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r131));
            //Tabelas.tabelaAcao[201].Add("", new ActionClass(ActionType.Reduce, r131));
            //Tabelas.tabelaAcao[201].Add("", new ActionClass(ActionType.Reduce, r131));
            Tabelas.tabelaAcao[201].Add("RETORNE", new ActionClass(ActionType.Reduce, r131));
            Tabelas.tabelaAcao[201].Add("FIM", new ActionClass(ActionType.Reduce, r131));

            // <CMD_CHAMA_PROC> -> ID . ABRE_PAR <LST_CHAMA_PROC_PARAM> FECHA_PAR
            // <CMD_ATRIBUICAO> -> . <CMD_ATRIB_VAR_REC> ATRIBUICAO <LISTA_CMD_ATRIB_VAL> PONTO_VIRGULA
            // <CMD_ATRIB_VAR_REC> ::= ID | ID ABRE_COL <LST_ATRIB_VET_INDEX> FECHA_COL
            Tabelas.tabelaAcao[202] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[202].Add("ATRIBUICAO", new ActionClass(ActionType.Reduce, r133));
            Tabelas.tabelaAcao[202].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 152));
            Tabelas.tabelaAcao[202].Add("ABRE_COL", new ActionClass(ActionType.Shift, 207));

            // <CMD_ATRIBUICAO> -> <CMD_ATRIB_VAR_REC> . ATRIBUICAO <LISTA_CMD_ATRIB_VAL> PONTO_VIRGULA
            Tabelas.tabelaAcao[203] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[203].Add("ATRIBUICAO", new ActionClass(ActionType.Shift, 204));

            // <CMD_ATRIBUICAO> -> <CMD_ATRIB_VAR_REC> ATRIBUICAO . <LISTA_CMD_ATRIB_VAL> PONTO_VIRGULA
            Tabelas.tabelaAcao[204] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[204].Add("ID", new ActionClass(ActionType.Shift, 213));
            Tabelas.tabelaAcao[204].Add("CONST_INT", new ActionClass(ActionType.Shift, 222));
            Tabelas.tabelaAcao[204].Add("CONST_REAL", new ActionClass(ActionType.Shift, 222));
            Tabelas.tabelaAcao[204].Add("CONST_TEXTO", new ActionClass(ActionType.Shift, 221));
            Tabelas.tabelaAcao[204].Add("CONST_LOGICA", new ActionClass(ActionType.Shift, 361));
            Tabelas.tabelaAcao[204].Add("NAO", new ActionClass(ActionType.Shift, 357));
            Tabelas.tabelaAcao[204].Add("MENOS", new ActionClass(ActionType.Shift, 215));
            Tabelas.tabelaAcao[204].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 217));

            Tabelas.tabelaGOTO[204] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[204].Add("<LISTA_CMD_ATRIB_VAL>", 205);
            Tabelas.tabelaGOTO[204].Add("<CMD_ATRIB_VAL>", 210);
            Tabelas.tabelaGOTO[204].Add("<CMD_ATRIB_VET>", 230);
            Tabelas.tabelaGOTO[204].Add("<CMD_ATRIB_FUNC>", 220);
            Tabelas.tabelaGOTO[204].Add("<CMD_ATRIB_CONST>", 214);
            Tabelas.tabelaGOTO[204].Add("<ARGUMENTO_LOGICO>", 302);
            Tabelas.tabelaGOTO[204].Add("<LST_ARG_LOG_COND>", 350);
            Tabelas.tabelaGOTO[204].Add("<ARG_LOG_VAR>", 362);

            // <CMD_ATRIBUICAO> -> <CMD_ATRIB_VAR_REC> ATRIBUICAO <LISTA_CMD_ATRIB_VAL> . PONTO_VIRGULA
            // <LISTA_CMD_ATRIB_VAL> -> <LISTA_CMD_ATRIB_VAL> . <CMD_ATRIB_OP> <CMD_ATRIB_VAL>
            Tabelas.tabelaAcao[205] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[205].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 206));
            Tabelas.tabelaAcao[205].Add("E", new ActionClass(ActionType.Reduce, r180));
            Tabelas.tabelaAcao[205].Add("OU", new ActionClass(ActionType.Reduce, r180));
            Tabelas.tabelaAcao[205].Add("MAIOR", new ActionClass(ActionType.Reduce, r180));
            Tabelas.tabelaAcao[205].Add("MENOR", new ActionClass(ActionType.Reduce, r180));
            Tabelas.tabelaAcao[205].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r180));
            Tabelas.tabelaAcao[205].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r180));
            Tabelas.tabelaAcao[205].Add("IGUAL", new ActionClass(ActionType.Reduce, r180));
            Tabelas.tabelaAcao[205].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r180));
            Tabelas.tabelaAcao[205].Add("MENOS", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[205].Add("MAIS", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[205].Add("ASTERISTICO", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[205].Add("BARRA", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[205].Add("MOD", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[205].Add("DIV", new ActionClass(ActionType.Shift, 223));

            Tabelas.tabelaGOTO[205] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[205].Add("<CMD_ATRIB_OP>", 211);

            // <CMD_ATRIBUICAO> -> <CMD_ATRIB_VAR_REC> ATRIBUICAO <LISTA_CMD_ATRIB_VAL> PONTO_VIRGULA .
            Tabelas.tabelaAcao[206] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[206].Add("LEIA", new ActionClass(ActionType.Reduce, r132));
            Tabelas.tabelaAcao[206].Add("ESCREVA", new ActionClass(ActionType.Reduce, r132));
            Tabelas.tabelaAcao[206].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r132));
            Tabelas.tabelaAcao[206].Add("ID", new ActionClass(ActionType.Reduce, r132));
            Tabelas.tabelaAcao[206].Add("SE", new ActionClass(ActionType.Reduce, r132));
            Tabelas.tabelaAcao[206].Add("SENAO", new ActionClass(ActionType.Reduce, r132));
            Tabelas.tabelaAcao[206].Add("FIM_SE", new ActionClass(ActionType.Reduce, r132));
            Tabelas.tabelaAcao[206].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r132));
            Tabelas.tabelaAcao[206].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r132));
            Tabelas.tabelaAcao[206].Add("REPITA", new ActionClass(ActionType.Reduce, r132));
            Tabelas.tabelaAcao[206].Add("ATE", new ActionClass(ActionType.Reduce, r132));
            Tabelas.tabelaAcao[206].Add("PARA", new ActionClass(ActionType.Reduce, r132));
            Tabelas.tabelaAcao[206].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r132));
            //Tabelas.tabelaAcao[206].Add("", new ActionClass(ActionType.Reduce, r132));
            //Tabelas.tabelaAcao[206].Add("", new ActionClass(ActionType.Reduce, r132));
            Tabelas.tabelaAcao[206].Add("RETORNE", new ActionClass(ActionType.Reduce, r132));
            Tabelas.tabelaAcao[206].Add("FIM", new ActionClass(ActionType.Reduce, r132));

            // <CMD_ATRIB_VAR_REC> -> ID ABRE_COL . <LST_ATRIB_VET_INDEX> FECHA_COL
            // <LST_ATRIB_VET_INDEX> -> . <LST_ATRIB_VET_INDEX> VIRGULA <ATRIB_VET_INDEX_VAL>
            // <LST_ATRIB_VET_INDEX> -> . <LST_ATRIB_VET_INDEX> <ATRIB_VET_INDEX_OP> <ATRIB_VET_INDEX_VAL>
            Tabelas.tabelaAcao[207] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[207].Add("ID", new ActionClass(ActionType.Shift, 239));
            Tabelas.tabelaAcao[207].Add("CONST_INT", new ActionClass(ActionType.Shift, 239));

            Tabelas.tabelaGOTO[207] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[207].Add("<LST_ATRIB_VET_INDEX>", 208);
            Tabelas.tabelaGOTO[207].Add("<ATRIB_VET_INDEX_VAL>", 234);

            // <CMD_ATRIB_VAR_REC> -> ID ABRE_COL <LST_ATRIB_VET_INDEX> . FECHA_COL
            Tabelas.tabelaAcao[208] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[208].Add("FECHA_COL", new ActionClass(ActionType.Shift, 209));
            Tabelas.tabelaAcao[208].Add("VIRGULA", new ActionClass(ActionType.Shift, 235));
            Tabelas.tabelaAcao[208].Add("MENOS", new ActionClass(ActionType.Shift, 240));
            Tabelas.tabelaAcao[208].Add("MAIS", new ActionClass(ActionType.Shift, 240));
            Tabelas.tabelaAcao[208].Add("ASTERISTICO", new ActionClass(ActionType.Shift, 240));
            Tabelas.tabelaAcao[208].Add("BARRA", new ActionClass(ActionType.Shift, 240));

            Tabelas.tabelaGOTO[208] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[208].Add("<ATRIB_VET_INDEX_OP>", 237);

            // <CMD_ATRIB_VAR_REC> -> ID ABRE_COL <LST_ATRIB_VET_INDEX> FECHA_COL .
            Tabelas.tabelaAcao[209] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[209].Add("ATRIBUICAO", new ActionClass(ActionType.Reduce, r134));
            Tabelas.tabelaAcao[209].Add("MENOS", new ActionClass(ActionType.Reduce, r134));
            Tabelas.tabelaAcao[209].Add("MAIS", new ActionClass(ActionType.Reduce, r134));
            Tabelas.tabelaAcao[209].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r134));
            Tabelas.tabelaAcao[209].Add("BARRA", new ActionClass(ActionType.Reduce, r134));
            Tabelas.tabelaAcao[209].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r134));

            // <LISTA_CMD_ATRIB_VAL> -> <CMD_ATRIB_VAL> .
            Tabelas.tabelaAcao[210] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[210].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r135));
            Tabelas.tabelaAcao[210].Add("E", new ActionClass(ActionType.Reduce, r135));
            Tabelas.tabelaAcao[210].Add("OU", new ActionClass(ActionType.Reduce, r135));
            Tabelas.tabelaAcao[210].Add("MAIOR", new ActionClass(ActionType.Reduce, r135));
            Tabelas.tabelaAcao[210].Add("MENOR", new ActionClass(ActionType.Reduce, r135));
            Tabelas.tabelaAcao[210].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r135));
            Tabelas.tabelaAcao[210].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r135));
            Tabelas.tabelaAcao[210].Add("IGUAL", new ActionClass(ActionType.Reduce, r135));
            Tabelas.tabelaAcao[210].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r135));
            Tabelas.tabelaAcao[210].Add("MENOS", new ActionClass(ActionType.Reduce, r135));
            Tabelas.tabelaAcao[210].Add("MAIS", new ActionClass(ActionType.Reduce, r135));
            Tabelas.tabelaAcao[210].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r135));
            Tabelas.tabelaAcao[210].Add("BARRA", new ActionClass(ActionType.Reduce, r135));
            Tabelas.tabelaAcao[210].Add("MOD", new ActionClass(ActionType.Reduce, r135));
            Tabelas.tabelaAcao[210].Add("DIV", new ActionClass(ActionType.Reduce, r135));
            Tabelas.tabelaAcao[210].Add("ABRE_PAR", new ActionClass(ActionType.Reduce, r135));
            Tabelas.tabelaAcao[210].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r135));
            Tabelas.tabelaAcao[210].Add("ATE", new ActionClass(ActionType.Reduce, r135));
            Tabelas.tabelaAcao[210].Add("DECRESCENTE", new ActionClass(ActionType.Reduce, r135));
            Tabelas.tabelaAcao[210].Add("FACA", new ActionClass(ActionType.Reduce, r135));

            // <LISTA_CMD_ATRIB_VAL> -> <LISTA_CMD_ATRIB_VAL> <CMD_ATRIB_OP> . <CMD_ATRIB_VAL>
            Tabelas.tabelaAcao[211] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[211].Add("ID", new ActionClass(ActionType.Shift, 213));
            Tabelas.tabelaAcao[211].Add("CONST_INT", new ActionClass(ActionType.Shift, 222));
            Tabelas.tabelaAcao[211].Add("CONST_REAL", new ActionClass(ActionType.Shift, 222));
            Tabelas.tabelaAcao[211].Add("CONST_TEXTO", new ActionClass(ActionType.Shift, 221));
            Tabelas.tabelaAcao[211].Add("MENOS", new ActionClass(ActionType.Shift, 215));
            Tabelas.tabelaAcao[211].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 217));

            Tabelas.tabelaGOTO[211] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[211].Add("<CMD_ATRIB_VAL>", 212);
            Tabelas.tabelaGOTO[211].Add("<CMD_ATRIB_VET>", 230);
            Tabelas.tabelaGOTO[211].Add("<CMD_ATRIB_FUNC>", 220);
            Tabelas.tabelaGOTO[211].Add("<CMD_ATRIB_CONST>", 214);

            // <LISTA_CMD_ATRIB_VAL> -> <LISTA_CMD_ATRIB_VAL> <CMD_ATRIB_OP> <CMD_ATRIB_VAL> .
            Tabelas.tabelaAcao[212] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[212].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r136));
            Tabelas.tabelaAcao[212].Add("MAIOR", new ActionClass(ActionType.Reduce, r136));
            Tabelas.tabelaAcao[212].Add("MENOR", new ActionClass(ActionType.Reduce, r136));
            Tabelas.tabelaAcao[212].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r136));
            Tabelas.tabelaAcao[212].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r136));
            Tabelas.tabelaAcao[212].Add("IGUAL", new ActionClass(ActionType.Reduce, r136));
            Tabelas.tabelaAcao[212].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r136));
            Tabelas.tabelaAcao[212].Add("MENOS", new ActionClass(ActionType.Reduce, r136));
            Tabelas.tabelaAcao[212].Add("MAIS", new ActionClass(ActionType.Reduce, r136));
            Tabelas.tabelaAcao[212].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r136));
            Tabelas.tabelaAcao[212].Add("BARRA", new ActionClass(ActionType.Reduce, r136));
            Tabelas.tabelaAcao[212].Add("MOD", new ActionClass(ActionType.Reduce, r136));
            Tabelas.tabelaAcao[212].Add("DIV", new ActionClass(ActionType.Reduce, r136));
            Tabelas.tabelaAcao[212].Add("ABRE_PAR", new ActionClass(ActionType.Reduce, r136));
            Tabelas.tabelaAcao[212].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r136));
            Tabelas.tabelaAcao[212].Add("ATE", new ActionClass(ActionType.Reduce, r136));
            Tabelas.tabelaAcao[212].Add("DECRESCENTE", new ActionClass(ActionType.Reduce, r136));
            Tabelas.tabelaAcao[212].Add("FACA", new ActionClass(ActionType.Reduce, r136));

            // <CMD_ATRIB_VAL> -> ID .
            // <CMD_ATRIB_FUNC> -> ID . ABRE_PAR <LST_ATRIB_FUNC_PARAM> FECHA_PAR
            Tabelas.tabelaAcao[213] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[213].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r137));
            Tabelas.tabelaAcao[213].Add("E", new ActionClass(ActionType.Reduce, r137));
            Tabelas.tabelaAcao[213].Add("OU", new ActionClass(ActionType.Reduce, r137));
            Tabelas.tabelaAcao[213].Add("MAIOR", new ActionClass(ActionType.Reduce, r137));
            Tabelas.tabelaAcao[213].Add("MENOR", new ActionClass(ActionType.Reduce, r137));
            Tabelas.tabelaAcao[213].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r137));
            Tabelas.tabelaAcao[213].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r137));
            Tabelas.tabelaAcao[213].Add("IGUAL", new ActionClass(ActionType.Reduce, r137));
            Tabelas.tabelaAcao[213].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r137));
            Tabelas.tabelaAcao[213].Add("MENOS", new ActionClass(ActionType.Reduce, r137));
            Tabelas.tabelaAcao[213].Add("MAIS", new ActionClass(ActionType.Reduce, r137));
            Tabelas.tabelaAcao[213].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r137));
            Tabelas.tabelaAcao[213].Add("BARRA", new ActionClass(ActionType.Reduce, r137));
            Tabelas.tabelaAcao[213].Add("MOD", new ActionClass(ActionType.Reduce, r137));
            Tabelas.tabelaAcao[213].Add("DIV", new ActionClass(ActionType.Reduce, r137));
            Tabelas.tabelaAcao[213].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 224));
            Tabelas.tabelaAcao[213].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r137));
            Tabelas.tabelaAcao[213].Add("ABRE_COL", new ActionClass(ActionType.Shift, 231));
            Tabelas.tabelaAcao[213].Add("ATE", new ActionClass(ActionType.Reduce, r137));
            Tabelas.tabelaAcao[213].Add("DECRESCENTE", new ActionClass(ActionType.Reduce, r137));
            Tabelas.tabelaAcao[213].Add("FACA", new ActionClass(ActionType.Reduce, r137));

            // <CMD_ATRIB_VAL> -> <CMD_ATRIB_CONST> .
            Tabelas.tabelaAcao[214] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[214].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r138));
            Tabelas.tabelaAcao[214].Add("MAIOR", new ActionClass(ActionType.Reduce, r138));
            Tabelas.tabelaAcao[214].Add("MENOR", new ActionClass(ActionType.Reduce, r138));
            Tabelas.tabelaAcao[214].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r138));
            Tabelas.tabelaAcao[214].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r138));
            Tabelas.tabelaAcao[214].Add("IGUAL", new ActionClass(ActionType.Reduce, r138));
            Tabelas.tabelaAcao[214].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r138));
            Tabelas.tabelaAcao[214].Add("MENOS", new ActionClass(ActionType.Reduce, r138));
            Tabelas.tabelaAcao[214].Add("MAIS", new ActionClass(ActionType.Reduce, r138));
            Tabelas.tabelaAcao[214].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r138));
            Tabelas.tabelaAcao[214].Add("BARRA", new ActionClass(ActionType.Reduce, r138));
            Tabelas.tabelaAcao[214].Add("MOD", new ActionClass(ActionType.Reduce, r138));
            Tabelas.tabelaAcao[214].Add("DIV", new ActionClass(ActionType.Reduce, r138));
            Tabelas.tabelaAcao[214].Add("ABRE_PAR", new ActionClass(ActionType.Reduce, r138));
            Tabelas.tabelaAcao[214].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r138));
            Tabelas.tabelaAcao[214].Add("ATE", new ActionClass(ActionType.Reduce, r138));
            Tabelas.tabelaAcao[214].Add("DECRESCENTE", new ActionClass(ActionType.Reduce, r138));
            Tabelas.tabelaAcao[214].Add("FACA", new ActionClass(ActionType.Reduce, r138));

            // <CMD_ATRIB_VAL> -> MENOS . <CMD_ATRIB_CONST>
            Tabelas.tabelaAcao[215] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[215].Add("CONST_INT", new ActionClass(ActionType.Shift, 222));
            Tabelas.tabelaAcao[215].Add("CONST_REAL", new ActionClass(ActionType.Shift, 222));
            Tabelas.tabelaAcao[215].Add("CONST_TEXTO", new ActionClass(ActionType.Shift, 221));

            Tabelas.tabelaGOTO[215] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[215].Add("<CMD_ATRIB_CONST>", 216);

            // <CMD_ATRIB_VAL> -> MENOS <CMD_ATRIB_CONST> .
            Tabelas.tabelaAcao[216] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[216].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r139));
            Tabelas.tabelaAcao[216].Add("MAIOR", new ActionClass(ActionType.Reduce, r139));
            Tabelas.tabelaAcao[216].Add("MENOR", new ActionClass(ActionType.Reduce, r139));
            Tabelas.tabelaAcao[216].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r139));
            Tabelas.tabelaAcao[216].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r139));
            Tabelas.tabelaAcao[216].Add("IGUAL", new ActionClass(ActionType.Reduce, r139));
            Tabelas.tabelaAcao[216].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r139));
            Tabelas.tabelaAcao[216].Add("MENOS", new ActionClass(ActionType.Reduce, r139));
            Tabelas.tabelaAcao[216].Add("MAIS", new ActionClass(ActionType.Reduce, r139));
            Tabelas.tabelaAcao[216].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r139));
            Tabelas.tabelaAcao[216].Add("BARRA", new ActionClass(ActionType.Reduce, r139));
            Tabelas.tabelaAcao[216].Add("MOD", new ActionClass(ActionType.Reduce, r139));
            Tabelas.tabelaAcao[216].Add("DIV", new ActionClass(ActionType.Reduce, r139));
            Tabelas.tabelaAcao[216].Add("ABRE_PAR", new ActionClass(ActionType.Reduce, r139));
            Tabelas.tabelaAcao[216].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r139));

            // <CMD_ATRIB_VAL> -> ABRE_PAR . <LISTA_CMD_ATRIB_VAL> FECHA_PAR
            Tabelas.tabelaAcao[217] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[217].Add("ID", new ActionClass(ActionType.Shift, 213));
            Tabelas.tabelaAcao[217].Add("CONST_INT", new ActionClass(ActionType.Shift, 222));
            Tabelas.tabelaAcao[217].Add("CONST_REAL", new ActionClass(ActionType.Shift, 222));
            Tabelas.tabelaAcao[217].Add("CONST_TEXTO", new ActionClass(ActionType.Shift, 221));
            Tabelas.tabelaAcao[217].Add("MENOS", new ActionClass(ActionType.Shift, 215));
            Tabelas.tabelaAcao[217].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 217));

            Tabelas.tabelaGOTO[217] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[217].Add("<LISTA_CMD_ATRIB_VAL>", 218);
            Tabelas.tabelaGOTO[217].Add("<CMD_ATRIB_VAL>", 210);
            Tabelas.tabelaGOTO[217].Add("<CMD_ATRIB_VET>", 230);
            Tabelas.tabelaGOTO[217].Add("<CMD_ATRIB_FUNC>", 220);
            Tabelas.tabelaGOTO[217].Add("<CMD_ATRIB_CONST>", 214);
            Tabelas.tabelaGOTO[217].Add("<ARGUMENTO_LOGICO>", 355);
            Tabelas.tabelaGOTO[217].Add("<LST_ARG_LOG_COND>", 350);
            Tabelas.tabelaGOTO[217].Add("<ARG_LOG_VAR>", 362);

            // <CMD_ATRIB_VAL> -> ABRE_PAR <LISTA_CMD_ATRIB_VAL> . FECHA_PAR
            // <LISTA_CMD_ATRIB_VAL> -> <LISTA_CMD_ATRIB_VAL> . <CMD_ATRIB_OP> <CMD_ATRIB_VAL>
            Tabelas.tabelaAcao[218] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[218].Add("MAIOR", new ActionClass(ActionType.Reduce, r180));
            Tabelas.tabelaAcao[218].Add("MENOR", new ActionClass(ActionType.Reduce, r180));
            Tabelas.tabelaAcao[218].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r180));
            Tabelas.tabelaAcao[218].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r180));
            Tabelas.tabelaAcao[218].Add("IGUAL", new ActionClass(ActionType.Reduce, r180));
            Tabelas.tabelaAcao[218].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r180));
            Tabelas.tabelaAcao[218].Add("MENOS", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[218].Add("MAIS", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[218].Add("ASTERISTICO", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[218].Add("BARRA", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[218].Add("MOD", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[218].Add("DIV", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[218].Add("FECHA_PAR", new ActionClass(ActionType.Shift, 219));

            Tabelas.tabelaGOTO[218] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[218].Add("<CMD_ATRIB_OP>", 211);

            // <CMD_ATRIB_VAL> -> ABRE_PAR <LISTA_CMD_ATRIB_VAL> FECHA_PAR .
            Tabelas.tabelaAcao[219] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[219].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r140));
            Tabelas.tabelaAcao[219].Add("MAIOR", new ActionClass(ActionType.Reduce, r140));
            Tabelas.tabelaAcao[219].Add("MENOR", new ActionClass(ActionType.Reduce, r140));
            Tabelas.tabelaAcao[219].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r140));
            Tabelas.tabelaAcao[219].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r140));
            Tabelas.tabelaAcao[219].Add("IGUAL", new ActionClass(ActionType.Reduce, r140));
            Tabelas.tabelaAcao[219].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r140));
            Tabelas.tabelaAcao[219].Add("MENOS", new ActionClass(ActionType.Reduce, r140));
            Tabelas.tabelaAcao[219].Add("MAIS", new ActionClass(ActionType.Reduce, r140));
            Tabelas.tabelaAcao[219].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r140));
            Tabelas.tabelaAcao[219].Add("BARRA", new ActionClass(ActionType.Reduce, r140));
            Tabelas.tabelaAcao[219].Add("MOD", new ActionClass(ActionType.Reduce, r140));
            Tabelas.tabelaAcao[219].Add("DIV", new ActionClass(ActionType.Reduce, r140));
            Tabelas.tabelaAcao[219].Add("ABRE_PAR", new ActionClass(ActionType.Reduce, r140));
            Tabelas.tabelaAcao[219].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r140));
            Tabelas.tabelaAcao[219].Add("ATE", new ActionClass(ActionType.Reduce, r140));
            Tabelas.tabelaAcao[219].Add("DECRESCENTE", new ActionClass(ActionType.Reduce, r140));
            Tabelas.tabelaAcao[219].Add("FACA", new ActionClass(ActionType.Reduce, r140));

            // <CMD_ATRIB_VAL> -> <CMD_ATRIB_FUNC> .
            Tabelas.tabelaAcao[220] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[220].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r142));
            Tabelas.tabelaAcao[220].Add("E", new ActionClass(ActionType.Reduce, r142));
            Tabelas.tabelaAcao[220].Add("OU", new ActionClass(ActionType.Reduce, r142));
            Tabelas.tabelaAcao[220].Add("MENOS", new ActionClass(ActionType.Reduce, r142));
            Tabelas.tabelaAcao[220].Add("MAIS", new ActionClass(ActionType.Reduce, r142));
            Tabelas.tabelaAcao[220].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r142));
            Tabelas.tabelaAcao[220].Add("BARRA", new ActionClass(ActionType.Reduce, r142));
            Tabelas.tabelaAcao[220].Add("MOD", new ActionClass(ActionType.Reduce, r142));
            Tabelas.tabelaAcao[220].Add("DIV", new ActionClass(ActionType.Reduce, r142));
            Tabelas.tabelaAcao[220].Add("ABRE_PAR", new ActionClass(ActionType.Reduce, r142));
            Tabelas.tabelaAcao[220].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r142));
            Tabelas.tabelaAcao[220].Add("ATE", new ActionClass(ActionType.Reduce, r142));
            Tabelas.tabelaAcao[220].Add("DECRESCENTE", new ActionClass(ActionType.Reduce, r142));
            Tabelas.tabelaAcao[220].Add("FACA", new ActionClass(ActionType.Reduce, r142));

            // <CMD_ATRIB_VAL> -> CONST_TEXTO .
            Tabelas.tabelaAcao[221] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[221].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r143));
            Tabelas.tabelaAcao[221].Add("MAIOR", new ActionClass(ActionType.Reduce, r143));
            Tabelas.tabelaAcao[221].Add("MENOR", new ActionClass(ActionType.Reduce, r143));
            Tabelas.tabelaAcao[221].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r143));
            Tabelas.tabelaAcao[221].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r143));
            Tabelas.tabelaAcao[221].Add("IGUAL", new ActionClass(ActionType.Reduce, r143));
            Tabelas.tabelaAcao[221].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r143));
            Tabelas.tabelaAcao[221].Add("MENOS", new ActionClass(ActionType.Reduce, r143));
            Tabelas.tabelaAcao[221].Add("MAIS", new ActionClass(ActionType.Reduce, r143));
            Tabelas.tabelaAcao[221].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r143));
            Tabelas.tabelaAcao[221].Add("BARRA", new ActionClass(ActionType.Reduce, r143));
            Tabelas.tabelaAcao[221].Add("MOD", new ActionClass(ActionType.Reduce, r143));
            Tabelas.tabelaAcao[221].Add("DIV", new ActionClass(ActionType.Reduce, r143));
            Tabelas.tabelaAcao[221].Add("ABRE_PAR", new ActionClass(ActionType.Reduce, r143));
            Tabelas.tabelaAcao[221].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r143));
            Tabelas.tabelaAcao[221].Add("ATE", new ActionClass(ActionType.Reduce, r143));
            Tabelas.tabelaAcao[221].Add("DECRESCENTE", new ActionClass(ActionType.Reduce, r143));
            Tabelas.tabelaAcao[221].Add("FACA", new ActionClass(ActionType.Reduce, r143));

            // <CMD_ATRIB_CONST> -> CONST_INT .
            // <CMD_ATRIB_CONST> -> CONST_REAL .
            Tabelas.tabelaAcao[222] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[222].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r144));
            Tabelas.tabelaAcao[222].Add("MAIOR", new ActionClass(ActionType.Reduce, r144));
            Tabelas.tabelaAcao[222].Add("MENOR", new ActionClass(ActionType.Reduce, r144));
            Tabelas.tabelaAcao[222].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r144));
            Tabelas.tabelaAcao[222].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r144));
            Tabelas.tabelaAcao[222].Add("IGUAL", new ActionClass(ActionType.Reduce, r144));
            Tabelas.tabelaAcao[222].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r144));
            Tabelas.tabelaAcao[222].Add("MENOS", new ActionClass(ActionType.Reduce, r144));
            Tabelas.tabelaAcao[222].Add("MAIS", new ActionClass(ActionType.Reduce, r144));
            Tabelas.tabelaAcao[222].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r144));
            Tabelas.tabelaAcao[222].Add("BARRA", new ActionClass(ActionType.Reduce, r144));
            Tabelas.tabelaAcao[222].Add("MOD", new ActionClass(ActionType.Reduce, r144));
            Tabelas.tabelaAcao[222].Add("DIV", new ActionClass(ActionType.Reduce, r144));
            Tabelas.tabelaAcao[222].Add("ABRE_PAR", new ActionClass(ActionType.Reduce, r144));
            Tabelas.tabelaAcao[222].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r144));
            Tabelas.tabelaAcao[222].Add("ATE", new ActionClass(ActionType.Reduce, r144));
            Tabelas.tabelaAcao[222].Add("DECRESCENTE", new ActionClass(ActionType.Reduce, r144));
            Tabelas.tabelaAcao[222].Add("FACA", new ActionClass(ActionType.Reduce, r144));

            // <CMD_ATRIB_OP> -> MENOS .
            // <CMD_ATRIB_OP> -> MAIS .
            // <CMD_ATRIB_OP> -> ASTERISTICO .
            // <CMD_ATRIB_OP> -> BARRA .
            // <CMD_ATRIB_OP> -> MOD .
            // <CMD_ATRIB_OP> -> DIV .
            Tabelas.tabelaAcao[223] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[223].Add("ID", new ActionClass(ActionType.Reduce, r145));
            Tabelas.tabelaAcao[223].Add("CONST_INT", new ActionClass(ActionType.Reduce, r145));
            Tabelas.tabelaAcao[223].Add("CONST_REAL", new ActionClass(ActionType.Reduce, r145));
            Tabelas.tabelaAcao[223].Add("CONST_TEXTO", new ActionClass(ActionType.Reduce, r145));
            Tabelas.tabelaAcao[223].Add("MENOS", new ActionClass(ActionType.Reduce, r145));
            Tabelas.tabelaAcao[223].Add("ABRE_PAR", new ActionClass(ActionType.Reduce, r145));

            // <CMD_ATRIB_FUNC> -> ID ABRE_PAR . <LST_ATRIB_FUNC_PARAM> FECHA_PAR
            // <LST_ATRIB_FUNC_PARAM> -> . <LST_ATRIB_FUNC_PARAM> VIRGULA ID
            Tabelas.tabelaAcao[224] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[224].Add("ID", new ActionClass(ActionType.Shift, 227));
            Tabelas.tabelaAcao[224].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r147));

            Tabelas.tabelaGOTO[224] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[224].Add("<LST_ATRIB_FUNC_PARAM>", 225);

            // <CMD_ATRIB_FUNC> -> ID ABRE_PAR <LST_ATRIB_FUNC_PARAM> . FECHA_PAR
            // <LST_ATRIB_FUNC_PARAM> -> <LST_ATRIB_FUNC_PARAM> . VIRGULA ID
            Tabelas.tabelaAcao[225] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[225].Add("FECHA_PAR", new ActionClass(ActionType.Shift, 226));
            Tabelas.tabelaAcao[225].Add("VIRGULA", new ActionClass(ActionType.Shift, 228));

            Tabelas.tabelaGOTO[225] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[225].Add("<LST_ATRIB_FUNC_PARAM>", 224);

            // <CMD_ATRIB_FUNC> -> ID ABRE_PAR <LST_ATRIB_FUNC_PARAM> FECHA_PAR .
            Tabelas.tabelaAcao[226] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[226].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r146));
            Tabelas.tabelaAcao[226].Add("E", new ActionClass(ActionType.Reduce, r146));
            Tabelas.tabelaAcao[226].Add("OU", new ActionClass(ActionType.Reduce, r146));
            Tabelas.tabelaAcao[226].Add("MENOS", new ActionClass(ActionType.Reduce, r146));
            Tabelas.tabelaAcao[226].Add("MAIS", new ActionClass(ActionType.Reduce, r146));
            Tabelas.tabelaAcao[226].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r146));
            Tabelas.tabelaAcao[226].Add("BARRA", new ActionClass(ActionType.Reduce, r146));
            Tabelas.tabelaAcao[226].Add("MOD", new ActionClass(ActionType.Reduce, r146));
            Tabelas.tabelaAcao[226].Add("DIV", new ActionClass(ActionType.Reduce, r146));
            Tabelas.tabelaAcao[226].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r146));
            Tabelas.tabelaAcao[226].Add("ATE", new ActionClass(ActionType.Reduce, r146));
            Tabelas.tabelaAcao[226].Add("DECRESCENTE", new ActionClass(ActionType.Reduce, r146));
            Tabelas.tabelaAcao[226].Add("FACA", new ActionClass(ActionType.Reduce, r146));

            // <LST_ATRIB_FUNC_PARAM> -> ID .
            Tabelas.tabelaAcao[227] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[227].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r148));
            Tabelas.tabelaAcao[227].Add("VIRGULA", new ActionClass(ActionType.Reduce, r148));

            // <LST_ATRIB_FUNC_PARAM> -> <LST_ATRIB_FUNC_PARAM> VIRGULA . ID
            Tabelas.tabelaAcao[228] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[228].Add("ID", new ActionClass(ActionType.Shift, 229));

            // <LST_ATRIB_FUNC_PARAM> -> <LST_ATRIB_FUNC_PARAM> VIRGULA ID .
            Tabelas.tabelaAcao[229] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[229].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r149));
            Tabelas.tabelaAcao[229].Add("VIRGULA", new ActionClass(ActionType.Reduce, r149));

            // <CMD_ATRIB_VAL> -> <CMD_ATRIB_VET> .
            Tabelas.tabelaAcao[230] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[230].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r141));
            Tabelas.tabelaAcao[230].Add("E", new ActionClass(ActionType.Reduce, r141));
            Tabelas.tabelaAcao[230].Add("OU", new ActionClass(ActionType.Reduce, r141));
            Tabelas.tabelaAcao[230].Add("MENOS", new ActionClass(ActionType.Reduce, r141));
            Tabelas.tabelaAcao[230].Add("MAIS", new ActionClass(ActionType.Reduce, r141));
            Tabelas.tabelaAcao[230].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r141));
            Tabelas.tabelaAcao[230].Add("BARRA", new ActionClass(ActionType.Reduce, r141));
            Tabelas.tabelaAcao[230].Add("MOD", new ActionClass(ActionType.Reduce, r141));
            Tabelas.tabelaAcao[230].Add("DIV", new ActionClass(ActionType.Reduce, r141));
            Tabelas.tabelaAcao[230].Add("ABRE_PAR", new ActionClass(ActionType.Reduce, r141));
            Tabelas.tabelaAcao[230].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r141));
            Tabelas.tabelaAcao[230].Add("ATE", new ActionClass(ActionType.Reduce, r141));
            Tabelas.tabelaAcao[230].Add("DECRESCENTE", new ActionClass(ActionType.Reduce, r141));
            Tabelas.tabelaAcao[230].Add("FACA", new ActionClass(ActionType.Reduce, r141));

            // <CMD_ATRIB_VET> -> ID ABRE_COL . <LST_ATRIB_VET_INDEX> FECHA_COL
            // <LST_ATRIB_VET_INDEX> -> . <LST_ATRIB_VET_INDEX> VIRGULA <ATRIB_VET_INDEX_VAL>
            // <LST_ATRIB_VET_INDEX> -> . <LST_ATRIB_VET_INDEX> <ATRIB_VET_INDEX_OP> <ATRIB_VET_INDEX_VAL>
            Tabelas.tabelaAcao[231] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[231].Add("ID", new ActionClass(ActionType.Shift, 239));
            Tabelas.tabelaAcao[231].Add("CONST_INT", new ActionClass(ActionType.Shift, 239));

            Tabelas.tabelaGOTO[231] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[231].Add("<LST_ATRIB_VET_INDEX>", 232);
            Tabelas.tabelaGOTO[231].Add("<ATRIB_VET_INDEX_VAL>", 234);

            // <CMD_ATRIB_VET> -> ID ABRE_COL <LST_ATRIB_VET_INDEX> . FECHA_COL
            // <LST_ATRIB_VET_INDEX> -> <LST_ATRIB_VET_INDEX> . VIRGULA <ATRIB_VET_INDEX_VAL>
            // <LST_ATRIB_VET_INDEX> -> <LST_ATRIB_VET_INDEX> . <ATRIB_VET_INDEX_OP> <ATRIB_VET_INDEX_VAL>
            Tabelas.tabelaAcao[232] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[232].Add("FECHA_COL", new ActionClass(ActionType.Shift, 233));
            Tabelas.tabelaAcao[232].Add("VIRGULA", new ActionClass(ActionType.Shift, 235));
            Tabelas.tabelaAcao[232].Add("MENOS", new ActionClass(ActionType.Shift, 240));
            Tabelas.tabelaAcao[232].Add("MAIS", new ActionClass(ActionType.Shift, 240));
            Tabelas.tabelaAcao[232].Add("ASTERISTICO", new ActionClass(ActionType.Shift, 240));
            Tabelas.tabelaAcao[232].Add("BARRA", new ActionClass(ActionType.Shift, 240));

            Tabelas.tabelaGOTO[232] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[232].Add("<ATRIB_VET_INDEX_OP>", 237);

            // <CMD_ATRIB_VET> -> ID ABRE_COL <LST_ATRIB_VET_INDEX> FECHA_COL .
            Tabelas.tabelaAcao[233] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[233].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r150));
            Tabelas.tabelaAcao[233].Add("E", new ActionClass(ActionType.Reduce, r150));
            Tabelas.tabelaAcao[233].Add("OU", new ActionClass(ActionType.Reduce, r150));
            Tabelas.tabelaAcao[233].Add("MENOS", new ActionClass(ActionType.Reduce, r150));
            Tabelas.tabelaAcao[233].Add("MAIS", new ActionClass(ActionType.Reduce, r150));
            Tabelas.tabelaAcao[233].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r150));
            Tabelas.tabelaAcao[233].Add("BARRA", new ActionClass(ActionType.Reduce, r150));
            Tabelas.tabelaAcao[233].Add("MOD", new ActionClass(ActionType.Reduce, r150));
            Tabelas.tabelaAcao[233].Add("DIV", new ActionClass(ActionType.Reduce, r150));
            Tabelas.tabelaAcao[233].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r150));
            Tabelas.tabelaAcao[233].Add("ATE", new ActionClass(ActionType.Reduce, r150));
            Tabelas.tabelaAcao[233].Add("DECRESCENTE", new ActionClass(ActionType.Reduce, r150));
            Tabelas.tabelaAcao[233].Add("FACA", new ActionClass(ActionType.Reduce, r150));

            // <LST_ATRIB_VET_INDEX> -> <ATRIB_VET_INDEX_VAL> .
            Tabelas.tabelaAcao[234] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[234].Add("FECHA_COL", new ActionClass(ActionType.Reduce, r151));
            Tabelas.tabelaAcao[234].Add("VIRGULA", new ActionClass(ActionType.Reduce, r151));
            Tabelas.tabelaAcao[234].Add("MENOS", new ActionClass(ActionType.Reduce, r151));
            Tabelas.tabelaAcao[234].Add("MAIS", new ActionClass(ActionType.Reduce, r151));
            Tabelas.tabelaAcao[234].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r151));
            Tabelas.tabelaAcao[234].Add("BARRA", new ActionClass(ActionType.Reduce, r151));

            // <LST_ATRIB_VET_INDEX> -> <LST_ATRIB_VET_INDEX> VIRGULA . <ATRIB_VET_INDEX_VAL>
            Tabelas.tabelaAcao[235] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[235].Add("ID", new ActionClass(ActionType.Shift, 239));
            Tabelas.tabelaAcao[235].Add("CONST_INT", new ActionClass(ActionType.Shift, 239));

            Tabelas.tabelaGOTO[235] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[235].Add("<ATRIB_VET_INDEX_VAL>", 236);

            // <LST_ATRIB_VET_INDEX> -> <LST_ATRIB_VET_INDEX> VIRGULA <ATRIB_VET_INDEX_VAL> .
            Tabelas.tabelaAcao[236] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[236].Add("FECHA_COL", new ActionClass(ActionType.Reduce, r152));
            Tabelas.tabelaAcao[236].Add("VIRGULA", new ActionClass(ActionType.Reduce, r152));
            Tabelas.tabelaAcao[236].Add("MENOS", new ActionClass(ActionType.Reduce, r152));
            Tabelas.tabelaAcao[236].Add("MAIS", new ActionClass(ActionType.Reduce, r152));
            Tabelas.tabelaAcao[236].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r152));
            Tabelas.tabelaAcao[236].Add("BARRA", new ActionClass(ActionType.Reduce, r152));

            // <LST_ATRIB_VET_INDEX> -> <LST_ATRIB_VET_INDEX> <ATRIB_VET_INDEX_OP> . <ATRIB_VET_INDEX_VAL>
            Tabelas.tabelaAcao[237] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[237].Add("ID", new ActionClass(ActionType.Shift, 239));
            Tabelas.tabelaAcao[237].Add("CONST_INT", new ActionClass(ActionType.Shift, 239));

            Tabelas.tabelaGOTO[237] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[237].Add("<ATRIB_VET_INDEX_VAL>", 238);

            // <LST_ATRIB_VET_INDEX> -> <LST_ATRIB_VET_INDEX> <ATRIB_VET_INDEX_OP> <ATRIB_VET_INDEX_VAL> .
            Tabelas.tabelaAcao[238] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[238].Add("FECHA_COL", new ActionClass(ActionType.Reduce, r153));
            Tabelas.tabelaAcao[238].Add("VIRGULA", new ActionClass(ActionType.Reduce, r153));
            Tabelas.tabelaAcao[238].Add("MENOS", new ActionClass(ActionType.Reduce, r153));
            Tabelas.tabelaAcao[238].Add("MAIS", new ActionClass(ActionType.Reduce, r153));
            Tabelas.tabelaAcao[238].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r153));
            Tabelas.tabelaAcao[238].Add("BARRA", new ActionClass(ActionType.Reduce, r153));

            // <ATRIB_VET_INDEX_VAL> -> ID .
            // <ATRIB_VET_INDEX_VAL> -> CONST_INT .
            Tabelas.tabelaAcao[239] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[239].Add("FECHA_COL", new ActionClass(ActionType.Reduce, r154));
            Tabelas.tabelaAcao[239].Add("VIRGULA", new ActionClass(ActionType.Reduce, r154));
            Tabelas.tabelaAcao[239].Add("MENOS", new ActionClass(ActionType.Reduce, r154));
            Tabelas.tabelaAcao[239].Add("MAIS", new ActionClass(ActionType.Reduce, r154));
            Tabelas.tabelaAcao[239].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r154));
            Tabelas.tabelaAcao[239].Add("BARRA", new ActionClass(ActionType.Reduce, r154));

            // <ATRIB_VET_INDEX_OP> -> MENOS .
            // <ATRIB_VET_INDEX_OP> -> MAIS .
            // <ATRIB_VET_INDEX_OP> -> ASTERISTICO .
            // <ATRIB_VET_INDEX_OP> -> BARRA .
            Tabelas.tabelaAcao[240] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[240].Add("ID", new ActionClass(ActionType.Reduce, r155));
            Tabelas.tabelaAcao[240].Add("CONST_INT", new ActionClass(ActionType.Reduce, r155));

            #endregion

            #region Atribuição com Argumentos Lógicos

            // <ARG_LOG_VAR> -> <CMD_ATRIB_VAL> . | <LISTA_CMD_ATRIB_VAL> .
            Tabelas.tabelaAcao[300] = new Dictionary<string, ActionClass>();
            //Tabelas.tabelaAcao[300].Add("", new ActionClass(ActionType.Shift, ));

            // <CMD_ATRIBUICAO> -> <CMD_ATRIB_VAR_REC> ATRIBUICAO . <ARGUMENTO_LOGICO> PONTO_VIRGULA 
            Tabelas.tabelaAcao[301] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[301].Add("CONST_INT", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[301].Add("CONST_REAL", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[301].Add("CONST_LOGICA", new ActionClass(ActionType.Shift, 361));
            Tabelas.tabelaAcao[301].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 354));
            Tabelas.tabelaAcao[301].Add("NAO", new ActionClass(ActionType.Shift, 357));
            Tabelas.tabelaAcao[301].Add("ID", new ActionClass(ActionType.Shift, 400));

            Tabelas.tabelaGOTO[301] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[301].Add("<ARGUMENTO_LOGICO>", 302);
            Tabelas.tabelaGOTO[301].Add("<LST_ARG_LOG_COND>", 350);
            Tabelas.tabelaGOTO[301].Add("<LST_ARG_LOG_VAR1>", 367);
            Tabelas.tabelaGOTO[301].Add("<ARG_LOG_VAR>", 362);
            Tabelas.tabelaGOTO[301].Add("<ARG_LOG_VAR_CONST>", 372);

            // <CMD_ATRIBUICAO> -> <CMD_ATRIB_VAR_REC> ATRIBUICAO <ARGUMENTO_LOGICO> . PONTO_VIRGULA 
            // <ARGUMENTO_LOGICO> -> <ARGUMENTO_LOGICO> . <ARG_LOG_OP_LOG> <LST_ARG_LOG_COND>
            Tabelas.tabelaAcao[302] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[302].Add("E", new ActionClass(ActionType.Shift, 410));
            Tabelas.tabelaAcao[302].Add("OU", new ActionClass(ActionType.Shift, 410));
            Tabelas.tabelaAcao[302].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 303));

            Tabelas.tabelaGOTO[302] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[302].Add("<ARG_LOG_OP_LOG>", 352);

            // <CMD_ATRIBUICAO> -> <CMD_ATRIB_VAR_REC> ATRIBUICAO <ARGUMENTO_LOGICO> PONTO_VIRGULA .
            Tabelas.tabelaAcao[303] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[303].Add("LEIA", new ActionClass(ActionType.Reduce, r181));
            Tabelas.tabelaAcao[303].Add("ESCREVA", new ActionClass(ActionType.Reduce, r181));
            Tabelas.tabelaAcao[303].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r181));
            Tabelas.tabelaAcao[303].Add("ID", new ActionClass(ActionType.Reduce, r181));
            Tabelas.tabelaAcao[303].Add("SE", new ActionClass(ActionType.Reduce, r181));
            Tabelas.tabelaAcao[303].Add("SENAO", new ActionClass(ActionType.Reduce, r181));
            Tabelas.tabelaAcao[303].Add("FIM_SE", new ActionClass(ActionType.Reduce, r181));
            Tabelas.tabelaAcao[303].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r181));
            Tabelas.tabelaAcao[303].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r181));
            Tabelas.tabelaAcao[303].Add("REPITA", new ActionClass(ActionType.Reduce, r181));
            Tabelas.tabelaAcao[303].Add("ATE", new ActionClass(ActionType.Reduce, r181));
            Tabelas.tabelaAcao[303].Add("PARA", new ActionClass(ActionType.Reduce, r181));
            Tabelas.tabelaAcao[303].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r181));
            //Tabelas.tabelaAcao[303].Add("", new ActionClass(ActionType.Reduce, r181));
            //Tabelas.tabelaAcao[303].Add("", new ActionClass(ActionType.Reduce, r181));
            Tabelas.tabelaAcao[303].Add("RETORNE", new ActionClass(ActionType.Reduce, r181));
            Tabelas.tabelaAcao[303].Add("FIM", new ActionClass(ActionType.Reduce, r181));

            #endregion

            #endregion

            #region Argumento Lógico

            // <ARGUMENTO_LOGICO> -> <LST_ARG_LOG_COND> .
            Tabelas.tabelaAcao[350] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[350].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r250));
            Tabelas.tabelaAcao[350].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r250));
            Tabelas.tabelaAcao[350].Add("E", new ActionClass(ActionType.Reduce, r250));
            Tabelas.tabelaAcao[350].Add("OU", new ActionClass(ActionType.Reduce, r250));
            Tabelas.tabelaAcao[350].Add("ENTAO", new ActionClass(ActionType.Reduce, r250));
            Tabelas.tabelaAcao[350].Add("FACA", new ActionClass(ActionType.Reduce, r250));

            // <ARGUMENTO_LOGICO> -> <ARGUMENTO_LOGICO> . <ARG_LOG_OP_LOG> <LST_ARG_LOG_COND>
            Tabelas.tabelaAcao[351] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[351].Add("E", new ActionClass(ActionType.Shift, 410));
            Tabelas.tabelaAcao[351].Add("OU", new ActionClass(ActionType.Shift, 410));

            Tabelas.tabelaGOTO[351] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[351].Add("<ARG_LOG_OP_LOG>", 352);

            // <ARGUMENTO_LOGICO> -> <ARGUMENTO_LOGICO> <ARG_LOG_OP_LOG> . <LST_ARG_LOG_COND>
            Tabelas.tabelaAcao[352] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[352].Add("MENOS", new ActionClass(ActionType.Shift, 373));
            Tabelas.tabelaAcao[352].Add("CONST_LOGICA", new ActionClass(ActionType.Shift, 361));
            Tabelas.tabelaAcao[352].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 354));
            Tabelas.tabelaAcao[352].Add("NAO", new ActionClass(ActionType.Shift, 357));
            Tabelas.tabelaAcao[352].Add("ID", new ActionClass(ActionType.Shift, 400));

            Tabelas.tabelaGOTO[352] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[352].Add("<LST_ARG_LOG_COND>", 353);
            Tabelas.tabelaGOTO[352].Add("<LST_ARG_LOG_VAR>", 367);
            Tabelas.tabelaGOTO[352].Add("<ARG_LOG_VAR>", 362);
            Tabelas.tabelaGOTO[352].Add("<ARG_LOG_VAR_CONST>", 372);

            // <ARGUMENTO_LOGICO> -> <ARGUMENTO_LOGICO> <ARG_LOG_OP_LOG> <LST_ARG_LOG_COND> .
            Tabelas.tabelaAcao[353] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[353].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r251));
            Tabelas.tabelaAcao[353].Add("E", new ActionClass(ActionType.Reduce, r251));
            Tabelas.tabelaAcao[353].Add("OU", new ActionClass(ActionType.Reduce, r251));
            Tabelas.tabelaAcao[353].Add("ENTAO", new ActionClass(ActionType.Reduce, r251));
            Tabelas.tabelaAcao[353].Add("FACA", new ActionClass(ActionType.Reduce, r251));
            Tabelas.tabelaAcao[353].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r251));

            // <LST_ARG_LOG_COND> -> ABRE_PAR . <ARGUMENTO_LOGICO> FECHA_PAR
            Tabelas.tabelaAcao[354] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[354].Add("MENOS", new ActionClass(ActionType.Shift, 373));
            Tabelas.tabelaAcao[354].Add("CONST_INT", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[354].Add("CONST_REAL", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[354].Add("CONST_LOGICA", new ActionClass(ActionType.Shift, 361));
            Tabelas.tabelaAcao[354].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 354));
            Tabelas.tabelaAcao[354].Add("NAO", new ActionClass(ActionType.Shift, 357));
            Tabelas.tabelaAcao[354].Add("ID", new ActionClass(ActionType.Shift, 400));

            Tabelas.tabelaGOTO[354] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[354].Add("<ARGUMENTO_LOGICO>", 355);
            Tabelas.tabelaGOTO[354].Add("<LST_ARG_LOG_COND>", 350);
            Tabelas.tabelaGOTO[354].Add("<LST_ARG_LOG_VAR1>", 367);
            Tabelas.tabelaGOTO[354].Add("<ARG_LOG_VAR>", 362);
            Tabelas.tabelaGOTO[354].Add("<ARG_LOG_VAR_CONST>", 372);

            // <LST_ARG_LOG_COND> -> ABRE_PAR <ARGUMENTO_LOGICO> . FECHA_PAR
            Tabelas.tabelaAcao[355] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[355].Add("FECHA_PAR", new ActionClass(ActionType.Shift, 356));
            Tabelas.tabelaAcao[355].Add("E", new ActionClass(ActionType.Shift, 410));
            Tabelas.tabelaAcao[355].Add("OU", new ActionClass(ActionType.Shift, 410));

            Tabelas.tabelaGOTO[355] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[355].Add("<ARG_LOG_OP_LOG>", 352);

            // <LST_ARG_LOG_COND> -> ABRE_PAR <ARGUMENTO_LOGICO> FECHA_PAR .
            Tabelas.tabelaAcao[356] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[356].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r252));
            Tabelas.tabelaAcao[356].Add("E", new ActionClass(ActionType.Reduce, r252));
            Tabelas.tabelaAcao[356].Add("OU", new ActionClass(ActionType.Reduce, r252));
            Tabelas.tabelaAcao[356].Add("ENTAO", new ActionClass(ActionType.Reduce, r252));
            Tabelas.tabelaAcao[356].Add("FACA", new ActionClass(ActionType.Reduce, r252));
            Tabelas.tabelaAcao[356].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r252));

            // <LST_ARG_LOG_COND> -> NAO . ABRE_PAR <ARGUMENTO_LOGICO> FECHA_PAR
            Tabelas.tabelaAcao[357] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[357].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 358));

            // <LST_ARG_LOG_COND> -> NAO ABRE_PAR . <ARGUMENTO_LOGICO> FECHA_PAR
            Tabelas.tabelaAcao[358] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[358].Add("MENOS", new ActionClass(ActionType.Shift, 373));
            Tabelas.tabelaAcao[358].Add("CONST_INT", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[358].Add("CONST_REAL", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[358].Add("CONST_LOGICA", new ActionClass(ActionType.Shift, 361));
            Tabelas.tabelaAcao[358].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 354));
            Tabelas.tabelaAcao[358].Add("NAO", new ActionClass(ActionType.Shift, 357));
            Tabelas.tabelaAcao[358].Add("ID", new ActionClass(ActionType.Shift, 400));

            Tabelas.tabelaGOTO[358] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[358].Add("<ARGUMENTO_LOGICO>", 359);
            Tabelas.tabelaGOTO[358].Add("<LST_ARG_LOG_COND>", 350);
            Tabelas.tabelaGOTO[358].Add("<LST_ARG_LOG_VAR1>", 367);
            Tabelas.tabelaGOTO[358].Add("<ARG_LOG_VAR>", 362);
            Tabelas.tabelaGOTO[358].Add("<ARG_LOG_VAR_CONST>", 372);

            // <LST_ARG_LOG_COND> -> NAO ABRE_PAR <ARGUMENTO_LOGICO> . FECHA_PAR
            Tabelas.tabelaAcao[359] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[359].Add("FECHA_PAR", new ActionClass(ActionType.Shift, 360));

            // <LST_ARG_LOG_COND> -> NAO ABRE_PAR <ARGUMENTO_LOGICO> FECHA_PAR .
            Tabelas.tabelaAcao[360] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[360].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r253));
            Tabelas.tabelaAcao[360].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r253));
            Tabelas.tabelaAcao[360].Add("E", new ActionClass(ActionType.Reduce, r253));
            Tabelas.tabelaAcao[360].Add("OU", new ActionClass(ActionType.Reduce, r253));
            Tabelas.tabelaAcao[360].Add("ENTAO", new ActionClass(ActionType.Reduce, r253));
            Tabelas.tabelaAcao[360].Add("FACA", new ActionClass(ActionType.Reduce, r253));

            // <LST_ARG_LOG_COND> -> CONST_LOGICA .
            Tabelas.tabelaAcao[361] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[361].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r254));
            Tabelas.tabelaAcao[361].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r254));
            Tabelas.tabelaAcao[361].Add("E", new ActionClass(ActionType.Reduce, r254));
            Tabelas.tabelaAcao[361].Add("OU", new ActionClass(ActionType.Reduce, r254));
            Tabelas.tabelaAcao[361].Add("ENTAO", new ActionClass(ActionType.Reduce, r254));
            Tabelas.tabelaAcao[361].Add("FACA", new ActionClass(ActionType.Reduce, r254));

            // <LST_ARG_LOG_COND> -> <ARG_LOG_VAR> .
            // <LST_ARG_LOG_COND> -> <ARG_LOG_VAR> . <ARG_LOG_OP_REL> <LST_ARG_LOG_VAR2>
            // <LST_ARG_LOG_VAR1> -> <ARG_LOG_VAR> . <ARG_LOG_OP_MAT> <ARG_LOG_VAR_EX>
            Tabelas.tabelaAcao[362] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[362].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r255));
            Tabelas.tabelaAcao[362].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r255));
            Tabelas.tabelaAcao[362].Add("E", new ActionClass(ActionType.Reduce, r255));
            Tabelas.tabelaAcao[362].Add("OU", new ActionClass(ActionType.Reduce, r255));
            Tabelas.tabelaAcao[362].Add("ENTAO", new ActionClass(ActionType.Reduce, r255));
            Tabelas.tabelaAcao[362].Add("MAIOR", new ActionClass(ActionType.Shift, 408));
            Tabelas.tabelaAcao[362].Add("MENOR", new ActionClass(ActionType.Shift, 408));
            Tabelas.tabelaAcao[362].Add("MAIOR_IGUAL", new ActionClass(ActionType.Shift, 408));
            Tabelas.tabelaAcao[362].Add("MENOR_IGUAL", new ActionClass(ActionType.Shift, 408));
            Tabelas.tabelaAcao[362].Add("IGUAL", new ActionClass(ActionType.Shift, 408));
            Tabelas.tabelaAcao[362].Add("DIFERENTE", new ActionClass(ActionType.Shift, 408));
            Tabelas.tabelaAcao[362].Add("MENOS", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[362].Add("MAIS", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[362].Add("ASTERISTICO", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[362].Add("BARRA", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[362].Add("MOD", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[362].Add("DIV", new ActionClass(ActionType.Shift, 409));

            Tabelas.tabelaGOTO[362] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[362].Add("<ARG_LOG_OP_REL>", 363);
            Tabelas.tabelaGOTO[362].Add("<ARG_LOG_OP_MAT>", 365);

            // <LST_ARG_LOG_COND> -> <ARG_LOG_VAR> <ARG_LOG_OP_REL> . <LST_ARG_LOG_VAR2>
            Tabelas.tabelaAcao[363] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[363].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 384));
            Tabelas.tabelaAcao[363].Add("MENOS", new ActionClass(ActionType.Shift, 378));
            Tabelas.tabelaAcao[363].Add("CONST_INT", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[363].Add("CONST_REAL", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[363].Add("ID", new ActionClass(ActionType.Shift, 400));

            Tabelas.tabelaGOTO[363] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[363].Add("<LST_ARG_LOG_VAR2>", 364);
            Tabelas.tabelaGOTO[363].Add("<ARG_LOG_VAR>", 380);
            Tabelas.tabelaGOTO[363].Add("<ARG_LOG_VAR_CONST>", 377);

            // <LST_ARG_LOG_COND> -> <ARG_LOG_VAR> <ARG_LOG_OP_REL> <LST_ARG_LOG_VAR2> .
            // <LST_ARG_LOG_VAR2> -> <LST_ARG_LOG_VAR2> . <ARG_LOG_OP_MAT> <ARG_LOG_VAR_EX>
            Tabelas.tabelaAcao[364] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[364].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r256));
            Tabelas.tabelaAcao[364].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r256));
            Tabelas.tabelaAcao[364].Add("E", new ActionClass(ActionType.Reduce, r256));
            Tabelas.tabelaAcao[364].Add("OU", new ActionClass(ActionType.Reduce, r256));
            Tabelas.tabelaAcao[364].Add("ENTAO", new ActionClass(ActionType.Reduce, r256));
            Tabelas.tabelaAcao[364].Add("FACA", new ActionClass(ActionType.Reduce, r256));
            Tabelas.tabelaAcao[364].Add("MENOS", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[364].Add("MAIS", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[364].Add("ASTERISTICO", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[364].Add("BARRA", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[364].Add("MOD", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[364].Add("DIV", new ActionClass(ActionType.Shift, 409));

            Tabelas.tabelaGOTO[364] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[364].Add("<ARG_LOG_OP_MAT>", 375);

            // <LST_ARG_LOG_VAR1> -> <ARG_LOG_VAR> <ARG_LOG_OP_MAT> . <ARG_LOG_VAR_EX>
            Tabelas.tabelaAcao[365] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[365].Add("MENOS", new ActionClass(ActionType.Shift, 392));
            Tabelas.tabelaAcao[365].Add("CONST_INT", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[365].Add("CONST_REAL", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[365].Add("ID", new ActionClass(ActionType.Shift, 400));

            Tabelas.tabelaGOTO[365] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[365].Add("<ARG_LOG_VAR_EX>", 366);
            Tabelas.tabelaGOTO[365].Add("<ARG_LOG_VAR>", 390);
            Tabelas.tabelaGOTO[365].Add("<ARG_LOG_VAR_CONST>", 391);

            // <LST_ARG_LOG_VAR1> -> <ARG_LOG_VAR> <ARG_LOG_OP_MAT> <ARG_LOG_VAR_EX> .
            Tabelas.tabelaAcao[366] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[366].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r257));
            Tabelas.tabelaAcao[366].Add("E", new ActionClass(ActionType.Reduce, r257));
            Tabelas.tabelaAcao[366].Add("OU", new ActionClass(ActionType.Reduce, r257));
            Tabelas.tabelaAcao[366].Add("ENTAO", new ActionClass(ActionType.Reduce, r257));
            Tabelas.tabelaAcao[366].Add("MAIOR", new ActionClass(ActionType.Reduce, r257));
            Tabelas.tabelaAcao[366].Add("MENOR", new ActionClass(ActionType.Reduce, r257));
            Tabelas.tabelaAcao[366].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r257));
            Tabelas.tabelaAcao[366].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r257));
            Tabelas.tabelaAcao[366].Add("IGUAL", new ActionClass(ActionType.Reduce, r257));
            Tabelas.tabelaAcao[366].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r257));
            Tabelas.tabelaAcao[366].Add("MENOS", new ActionClass(ActionType.Reduce, r257));
            Tabelas.tabelaAcao[366].Add("MAIS", new ActionClass(ActionType.Reduce, r257));
            Tabelas.tabelaAcao[366].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r257));
            Tabelas.tabelaAcao[366].Add("BARRA", new ActionClass(ActionType.Reduce, r257));
            Tabelas.tabelaAcao[366].Add("MOD", new ActionClass(ActionType.Reduce, r257));
            Tabelas.tabelaAcao[366].Add("DIV", new ActionClass(ActionType.Reduce, r257));

            // <LST_ARG_LOG_COND> -> <LST_ARG_LOG_VAR1> . <ARG_LOG_OP_REL> <LST_ARG_LOG_VAR2>
            // <LST_ARG_LOG_VAR1> -> <LST_ARG_LOG_VAR1> . <ARG_LOG_OP_MAT> <ARG_LOG_VAR_EX>
            Tabelas.tabelaAcao[367] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[367].Add("MAIOR", new ActionClass(ActionType.Shift, 408));
            Tabelas.tabelaAcao[367].Add("MENOR", new ActionClass(ActionType.Shift, 408));
            Tabelas.tabelaAcao[367].Add("MAIOR_IGUAL", new ActionClass(ActionType.Shift, 408));
            Tabelas.tabelaAcao[367].Add("MENOR_IGUAL", new ActionClass(ActionType.Shift, 408));
            Tabelas.tabelaAcao[367].Add("IGUAL", new ActionClass(ActionType.Shift, 408));
            Tabelas.tabelaAcao[367].Add("DIFERENTE", new ActionClass(ActionType.Shift, 408));
            Tabelas.tabelaAcao[367].Add("MENOS", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[367].Add("MAIS", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[367].Add("ASTERISTICO", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[367].Add("BARRA", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[367].Add("MOD", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[367].Add("DIV", new ActionClass(ActionType.Shift, 409));

            Tabelas.tabelaGOTO[367] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[367].Add("<ARG_LOG_OP_REL>", 368);
            Tabelas.tabelaGOTO[367].Add("<ARG_LOG_OP_MAT>", 370);

            // <LST_ARG_LOG_COND> -> <LST_ARG_LOG_VAR1> <ARG_LOG_OP_REL> . <LST_ARG_LOG_VAR2>
            Tabelas.tabelaAcao[368] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[368].Add("MENOS", new ActionClass(ActionType.Shift, 378));
            Tabelas.tabelaAcao[368].Add("CONST_INT", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[368].Add("CONST_REAL", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[368].Add("ID", new ActionClass(ActionType.Shift, 400));

            Tabelas.tabelaGOTO[368] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[368].Add("<LST_ARG_LOG_VAR2>", 369);
            Tabelas.tabelaGOTO[368].Add("<ARG_LOG_VAR>", 380);
            Tabelas.tabelaGOTO[368].Add("<ARG_LOG_VAR_CONST>", 377);

            // <LST_ARG_LOG_COND> -> <LST_ARG_LOG_VAR1> <ARG_LOG_OP_REL> <LST_ARG_LOG_VAR2> .
            // <LST_ARG_LOG_VAR2> -> <LST_ARG_LOG_VAR2> . <ARG_LOG_OP_MAT> <ARG_LOG_VAR_EX>
            Tabelas.tabelaAcao[369] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[369].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r258));
            Tabelas.tabelaAcao[369].Add("E", new ActionClass(ActionType.Reduce, r258));
            Tabelas.tabelaAcao[369].Add("OU", new ActionClass(ActionType.Reduce, r258));
            Tabelas.tabelaAcao[369].Add("ENTAO", new ActionClass(ActionType.Reduce, r258));
            Tabelas.tabelaAcao[369].Add("MENOS", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[369].Add("MAIS", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[369].Add("ASTERISTICO", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[369].Add("BARRA", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[369].Add("MOD", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[369].Add("DIV", new ActionClass(ActionType.Shift, 409));

            Tabelas.tabelaGOTO[369] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[369].Add("<ARG_LOG_OP_REL>", 363);
            Tabelas.tabelaGOTO[369].Add("<ARG_LOG_OP_MAT>", 375);

            // <LST_ARG_LOG_VAR1> -> <LST_ARG_LOG_VAR1> <ARG_LOG_OP_MAT> . <ARG_LOG_VAR_EX>
            Tabelas.tabelaAcao[370] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[370].Add("MENOS", new ActionClass(ActionType.Shift, 392));
            Tabelas.tabelaAcao[370].Add("CONST_INT", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[370].Add("CONST_REAL", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[370].Add("ID", new ActionClass(ActionType.Shift, 400));

            Tabelas.tabelaGOTO[370] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[370].Add("<ARG_LOG_VAR_EX>", 371);
            Tabelas.tabelaGOTO[370].Add("<ARG_LOG_VAR>", 390);
            Tabelas.tabelaGOTO[370].Add("<ARG_LOG_VAR_CONST>", 391);

            // <LST_ARG_LOG_VAR1> -> <LST_ARG_LOG_VAR1> <ARG_LOG_OP_MAT> <ARG_LOG_VAR_EX> .
            Tabelas.tabelaAcao[371] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[371].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r259));
            Tabelas.tabelaAcao[371].Add("E", new ActionClass(ActionType.Reduce, r259));
            Tabelas.tabelaAcao[371].Add("OU", new ActionClass(ActionType.Reduce, r259));
            Tabelas.tabelaAcao[371].Add("ENTAO", new ActionClass(ActionType.Reduce, r259));
            Tabelas.tabelaAcao[371].Add("MAIOR", new ActionClass(ActionType.Reduce, r259));
            Tabelas.tabelaAcao[371].Add("MENOR", new ActionClass(ActionType.Reduce, r259));
            Tabelas.tabelaAcao[371].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r259));
            Tabelas.tabelaAcao[371].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r259));
            Tabelas.tabelaAcao[371].Add("IGUAL", new ActionClass(ActionType.Reduce, r259));
            Tabelas.tabelaAcao[371].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r259));
            Tabelas.tabelaAcao[371].Add("MENOS", new ActionClass(ActionType.Reduce, r259));
            Tabelas.tabelaAcao[371].Add("MAIS", new ActionClass(ActionType.Reduce, r259));
            Tabelas.tabelaAcao[371].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r259));
            Tabelas.tabelaAcao[371].Add("BARRA", new ActionClass(ActionType.Reduce, r259));
            Tabelas.tabelaAcao[371].Add("MOD", new ActionClass(ActionType.Reduce, r259));
            Tabelas.tabelaAcao[371].Add("DIV", new ActionClass(ActionType.Reduce, r259));

            // <LST_ARG_LOG_VAR1> -> <ARG_LOG_VAR_CONST> .
            Tabelas.tabelaAcao[372] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[372].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r260));
            Tabelas.tabelaAcao[372].Add("E", new ActionClass(ActionType.Reduce, r260));
            Tabelas.tabelaAcao[372].Add("OU", new ActionClass(ActionType.Reduce, r260));
            Tabelas.tabelaAcao[372].Add("ENTAO", new ActionClass(ActionType.Reduce, r260));
            Tabelas.tabelaAcao[372].Add("MAIOR", new ActionClass(ActionType.Reduce, r260));
            Tabelas.tabelaAcao[372].Add("MENOR", new ActionClass(ActionType.Reduce, r260));
            Tabelas.tabelaAcao[372].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r260));
            Tabelas.tabelaAcao[372].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r260));
            Tabelas.tabelaAcao[372].Add("IGUAL", new ActionClass(ActionType.Reduce, r260));
            Tabelas.tabelaAcao[372].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r260));
            Tabelas.tabelaAcao[372].Add("MENOS", new ActionClass(ActionType.Reduce, r260));
            Tabelas.tabelaAcao[372].Add("MAIS", new ActionClass(ActionType.Reduce, r260));
            Tabelas.tabelaAcao[372].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r260));
            Tabelas.tabelaAcao[372].Add("BARRA", new ActionClass(ActionType.Reduce, r260));
            Tabelas.tabelaAcao[372].Add("MOD", new ActionClass(ActionType.Reduce, r260));
            Tabelas.tabelaAcao[372].Add("DIV", new ActionClass(ActionType.Reduce, r260));

            // <LST_ARG_LOG_VAR1> -> MENOS . <ARG_LOG_VAR_CONST>
            // <ARG_LOG_VAR_CONST> -> . CONST_INT
            // <ARG_LOG_VAR_CONST> -> . CONST_REAL
            Tabelas.tabelaAcao[373] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[373].Add("CONST_INT", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[373].Add("CONST_REAL", new ActionClass(ActionType.Shift, 407));

            Tabelas.tabelaGOTO[373] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[373].Add("<ARG_LOG_VAR_CONST>", 374);

            // <LST_ARG_LOG_VAR1> -> MENOS <ARG_LOG_VAR_CONST> .
            Tabelas.tabelaAcao[374] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[374].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r261));
            Tabelas.tabelaAcao[374].Add("E", new ActionClass(ActionType.Reduce, r261));
            Tabelas.tabelaAcao[374].Add("OU", new ActionClass(ActionType.Reduce, r261));
            Tabelas.tabelaAcao[374].Add("ENTAO", new ActionClass(ActionType.Reduce, r261));

            // <LST_ARG_LOG_VAR2> -> <LST_ARG_LOG_VAR2> <ARG_LOG_OP_MAT> . <ARG_LOG_VAR_EX>
            // <LST_ARG_LOG_VAR2> -> <LST_ARG_LOG_VAR2> <ARG_LOG_OP_MAT> . ABRE_PAR <LST_ARG_LOG_VAR2> FECHA_PAR
            Tabelas.tabelaAcao[375] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[375].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 387));
            Tabelas.tabelaAcao[375].Add("MENOS", new ActionClass(ActionType.Shift, 392));
            Tabelas.tabelaAcao[375].Add("CONST_INT", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[375].Add("CONST_REAL", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[375].Add("ID", new ActionClass(ActionType.Shift, 400));

            Tabelas.tabelaGOTO[375] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[375].Add("<ARG_LOG_VAR_EX>", 376);
            Tabelas.tabelaGOTO[375].Add("<ARG_LOG_VAR>", 390);
            Tabelas.tabelaGOTO[375].Add("<ARG_LOG_VAR_CONST>", 391);

            // <LST_ARG_LOG_VAR2> -> <LST_ARG_LOG_VAR2> <ARG_LOG_OP_MAT> <ARG_LOG_VAR_EX> .
            Tabelas.tabelaAcao[376] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[376].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r262));
            Tabelas.tabelaAcao[376].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r262));
            Tabelas.tabelaAcao[376].Add("E", new ActionClass(ActionType.Reduce, r262));
            Tabelas.tabelaAcao[376].Add("OU", new ActionClass(ActionType.Reduce, r262));
            Tabelas.tabelaAcao[376].Add("ENTAO", new ActionClass(ActionType.Reduce, r262));
            Tabelas.tabelaAcao[376].Add("MENOS", new ActionClass(ActionType.Reduce, r262));
            Tabelas.tabelaAcao[376].Add("MAIS", new ActionClass(ActionType.Reduce, r262));
            Tabelas.tabelaAcao[376].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r262));
            Tabelas.tabelaAcao[376].Add("BARRA", new ActionClass(ActionType.Reduce, r262));
            Tabelas.tabelaAcao[376].Add("MOD", new ActionClass(ActionType.Reduce, r262));
            Tabelas.tabelaAcao[376].Add("DIV", new ActionClass(ActionType.Reduce, r262));

            // <LST_ARG_LOG_VAR2> -> <ARG_LOG_VAR_CONST> .
            Tabelas.tabelaAcao[377] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[377].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r263));
            Tabelas.tabelaAcao[377].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r263));
            Tabelas.tabelaAcao[377].Add("E", new ActionClass(ActionType.Reduce, r263));
            Tabelas.tabelaAcao[377].Add("OU", new ActionClass(ActionType.Reduce, r263));
            Tabelas.tabelaAcao[377].Add("ENTAO", new ActionClass(ActionType.Reduce, r263));
            Tabelas.tabelaAcao[377].Add("FACA", new ActionClass(ActionType.Reduce, r263));
            Tabelas.tabelaAcao[377].Add("MAIOR", new ActionClass(ActionType.Reduce, r263));
            Tabelas.tabelaAcao[377].Add("MENOR", new ActionClass(ActionType.Reduce, r263));
            Tabelas.tabelaAcao[377].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r263));
            Tabelas.tabelaAcao[377].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r263));
            Tabelas.tabelaAcao[377].Add("IGUAL", new ActionClass(ActionType.Reduce, r263));
            Tabelas.tabelaAcao[377].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r263));
            Tabelas.tabelaAcao[377].Add("MENOS", new ActionClass(ActionType.Reduce, r263));
            Tabelas.tabelaAcao[377].Add("MAIS", new ActionClass(ActionType.Reduce, r263));
            Tabelas.tabelaAcao[377].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r263));
            Tabelas.tabelaAcao[377].Add("BARRA", new ActionClass(ActionType.Reduce, r263));
            Tabelas.tabelaAcao[377].Add("MOD", new ActionClass(ActionType.Reduce, r263));
            Tabelas.tabelaAcao[377].Add("DIV", new ActionClass(ActionType.Reduce, r263));

            // <LST_ARG_LOG_VAR2> -> MENOS . <ARG_LOG_VAR_CONST>
            // <ARG_LOG_VAR_CONST> -> . CONST_INT
            // <ARG_LOG_VAR_CONST> -> . CONST_REAL
            Tabelas.tabelaAcao[378] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[378].Add("CONST_INT", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[378].Add("CONST_REAL", new ActionClass(ActionType.Shift, 407));

            Tabelas.tabelaGOTO[378] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[378].Add("<ARG_LOG_VAR_CONST>", 379);

            // <LST_ARG_LOG_VAR2> -> MENOS <ARG_LOG_VAR_CONST> .
            Tabelas.tabelaAcao[379] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[379].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r264));
            Tabelas.tabelaAcao[379].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r264));
            Tabelas.tabelaAcao[379].Add("E", new ActionClass(ActionType.Reduce, r264));
            Tabelas.tabelaAcao[379].Add("OU", new ActionClass(ActionType.Reduce, r264));
            Tabelas.tabelaAcao[379].Add("ENTAO", new ActionClass(ActionType.Reduce, r264));
            Tabelas.tabelaAcao[379].Add("MAIOR", new ActionClass(ActionType.Reduce, r264));
            Tabelas.tabelaAcao[379].Add("MENOR", new ActionClass(ActionType.Reduce, r264));
            Tabelas.tabelaAcao[379].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r264));
            Tabelas.tabelaAcao[379].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r264));
            Tabelas.tabelaAcao[379].Add("IGUAL", new ActionClass(ActionType.Reduce, r264));
            Tabelas.tabelaAcao[379].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r264));
            Tabelas.tabelaAcao[379].Add("MENOS", new ActionClass(ActionType.Reduce, r264));
            Tabelas.tabelaAcao[379].Add("MAIS", new ActionClass(ActionType.Reduce, r264));
            Tabelas.tabelaAcao[379].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r264));
            Tabelas.tabelaAcao[379].Add("BARRA", new ActionClass(ActionType.Reduce, r264));
            Tabelas.tabelaAcao[379].Add("MOD", new ActionClass(ActionType.Reduce, r264));
            Tabelas.tabelaAcao[379].Add("DIV", new ActionClass(ActionType.Reduce, r264));

            // <LST_ARG_LOG_VAR2> -> <ARG_LOG_VAR> .
            Tabelas.tabelaAcao[380] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[380].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r265));
            Tabelas.tabelaAcao[380].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r265));
            Tabelas.tabelaAcao[380].Add("E", new ActionClass(ActionType.Reduce, r265));
            Tabelas.tabelaAcao[380].Add("OU", new ActionClass(ActionType.Reduce, r265));
            Tabelas.tabelaAcao[380].Add("ENTAO", new ActionClass(ActionType.Reduce, r265));
            Tabelas.tabelaAcao[380].Add("MENOS", new ActionClass(ActionType.Reduce, r265));
            Tabelas.tabelaAcao[380].Add("MAIS", new ActionClass(ActionType.Reduce, r265));
            Tabelas.tabelaAcao[380].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r265));
            Tabelas.tabelaAcao[380].Add("BARRA", new ActionClass(ActionType.Reduce, r265));
            Tabelas.tabelaAcao[380].Add("MOD", new ActionClass(ActionType.Reduce, r265));
            Tabelas.tabelaAcao[380].Add("DIV", new ActionClass(ActionType.Reduce, r265));

            //<LST_ARG_LOG_VAR2> -> <LST_ARG_LOG_VAR2> . <ARG_LOG_OP_MAT> <ARG_LOG_VAR_EX>
            Tabelas.tabelaAcao[381] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[381].Add("MENOS", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[381].Add("MAIS", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[381].Add("ASTERISTICO", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[381].Add("BARRA", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[381].Add("MOD", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[381].Add("DIV", new ActionClass(ActionType.Shift, 409));

            Tabelas.tabelaGOTO[381] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[381].Add("<ARG_LOG_OP_MAT>", 382);

            // <LST_ARG_LOG_VAR2> -> <LST_ARG_LOG_VAR2> <ARG_LOG_OP_MAT> . <ARG_LOG_VAR_EX>
            Tabelas.tabelaAcao[382] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[382].Add("MENOS", new ActionClass(ActionType.Shift, 392));
            Tabelas.tabelaAcao[382].Add("CONST_INT", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[382].Add("CONST_REAL", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[382].Add("ID", new ActionClass(ActionType.Shift, 400));

            Tabelas.tabelaGOTO[382] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[382].Add("<ARG_LOG_VAR_EX>", 383);
            Tabelas.tabelaGOTO[382].Add("<ARG_LOG_VAR>", 390);
            Tabelas.tabelaGOTO[382].Add("<ARG_LOG_VAR_CONST>", 391);

            // <LST_ARG_LOG_VAR2> -> <LST_ARG_LOG_VAR2> <ARG_LOG_OP_MAT> <ARG_LOG_VAR_EX> .
            Tabelas.tabelaAcao[383] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[383].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r266));
            Tabelas.tabelaAcao[383].Add("E", new ActionClass(ActionType.Reduce, r266));
            Tabelas.tabelaAcao[383].Add("OU", new ActionClass(ActionType.Reduce, r266));
            Tabelas.tabelaAcao[383].Add("ENTAO", new ActionClass(ActionType.Reduce, r266));

            // <LST_ARG_LOG_VAR2> -> ABRE_PAR . <LST_ARG_LOG_VAR2> FECHA_PAR
            Tabelas.tabelaAcao[384] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[384].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 384));
            Tabelas.tabelaAcao[384].Add("MENOS", new ActionClass(ActionType.Shift, 378));
            Tabelas.tabelaAcao[384].Add("CONST_INT", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[384].Add("CONST_REAL", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[384].Add("ID", new ActionClass(ActionType.Shift, 400));

            Tabelas.tabelaGOTO[384] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[384].Add("<LST_ARG_LOG_VAR2>", 385);
            Tabelas.tabelaGOTO[384].Add("<ARG_LOG_VAR>", 380);
            Tabelas.tabelaGOTO[384].Add("<ARG_LOG_VAR_CONST>", 377);

            // <LST_ARG_LOG_VAR2> -> ABRE_PAR <LST_ARG_LOG_VAR2> . FECHA_PAR
            Tabelas.tabelaAcao[385] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[385].Add("FECHA_PAR", new ActionClass(ActionType.Shift, 386));
            Tabelas.tabelaAcao[385].Add("MENOS", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[385].Add("MAIS", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[385].Add("ASTERISTICO", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[385].Add("BARRA", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[385].Add("MOD", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[385].Add("DIV", new ActionClass(ActionType.Shift, 409));

            Tabelas.tabelaGOTO[385] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[385].Add("<ARG_LOG_OP_MAT>", 375);

            // <LST_ARG_LOG_VAR2> -> ABRE_PAR <LST_ARG_LOG_VAR2> FECHA_PAR .
            Tabelas.tabelaAcao[386] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[386].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r268));
            Tabelas.tabelaAcao[386].Add("E", new ActionClass(ActionType.Reduce, r268));
            Tabelas.tabelaAcao[386].Add("OU", new ActionClass(ActionType.Reduce, r268));
            Tabelas.tabelaAcao[386].Add("MENOS", new ActionClass(ActionType.Reduce, r268));
            Tabelas.tabelaAcao[386].Add("MAIS", new ActionClass(ActionType.Reduce, r268));
            Tabelas.tabelaAcao[386].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r268));
            Tabelas.tabelaAcao[386].Add("BARRA", new ActionClass(ActionType.Reduce, r268));
            Tabelas.tabelaAcao[386].Add("MOD", new ActionClass(ActionType.Reduce, r268));
            Tabelas.tabelaAcao[386].Add("DIV", new ActionClass(ActionType.Reduce, r268));
            Tabelas.tabelaAcao[386].Add("ENTAO", new ActionClass(ActionType.Reduce, r268));
            Tabelas.tabelaAcao[386].Add("FACA", new ActionClass(ActionType.Reduce, r268));
            Tabelas.tabelaAcao[386].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r268));

            // <LST_ARG_LOG_VAR2> -> <LST_ARG_LOG_VAR2> <ARG_LOG_OP_MAT> ABRE_PAR . <LST_ARG_LOG_VAR2> FECHA_PAR
            Tabelas.tabelaAcao[387] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[387].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 384));
            Tabelas.tabelaAcao[387].Add("MENOS", new ActionClass(ActionType.Shift, 378));
            Tabelas.tabelaAcao[387].Add("CONST_INT", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[387].Add("CONST_REAL", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[387].Add("ID", new ActionClass(ActionType.Shift, 400));

            Tabelas.tabelaGOTO[387] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[387].Add("<LST_ARG_LOG_VAR2>", 388);
            Tabelas.tabelaGOTO[387].Add("<ARG_LOG_VAR>", 380);
            Tabelas.tabelaGOTO[387].Add("<ARG_LOG_VAR_CONST>", 377);

            // <LST_ARG_LOG_VAR2> -> <LST_ARG_LOG_VAR2> <ARG_LOG_OP_MAT> ABRE_PAR <LST_ARG_LOG_VAR2> . FECHA_PAR
            // <LST_ARG_LOG_VAR2> -> <LST_ARG_LOG_VAR2> . <ARG_LOG_OP_MAT> <ARG_LOG_VAR_EX>
            Tabelas.tabelaAcao[388] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[388].Add("FECHA_PAR", new ActionClass(ActionType.Shift, 389));
            Tabelas.tabelaAcao[388].Add("MENOS", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[388].Add("MAIS", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[388].Add("ASTERISTICO", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[388].Add("BARRA", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[388].Add("MOD", new ActionClass(ActionType.Shift, 409));
            Tabelas.tabelaAcao[388].Add("DIV", new ActionClass(ActionType.Shift, 409));

            Tabelas.tabelaGOTO[388] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[388].Add("<ARG_LOG_OP_MAT>", 375);

            // <LST_ARG_LOG_VAR2> -> <LST_ARG_LOG_VAR2> <ARG_LOG_OP_MAT> ABRE_PAR <LST_ARG_LOG_VAR2> FECHA_PAR .
            Tabelas.tabelaAcao[389] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[389].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r267));
            Tabelas.tabelaAcao[389].Add("E", new ActionClass(ActionType.Reduce, r267));
            Tabelas.tabelaAcao[389].Add("OU", new ActionClass(ActionType.Reduce, r267));
            Tabelas.tabelaAcao[389].Add("MENOS", new ActionClass(ActionType.Reduce, r267));
            Tabelas.tabelaAcao[389].Add("MAIS", new ActionClass(ActionType.Reduce, r267));
            Tabelas.tabelaAcao[389].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r267));
            Tabelas.tabelaAcao[389].Add("BARRA", new ActionClass(ActionType.Reduce, r267));
            Tabelas.tabelaAcao[389].Add("MOD", new ActionClass(ActionType.Reduce, r267));
            Tabelas.tabelaAcao[389].Add("DIV", new ActionClass(ActionType.Reduce, r267));
            Tabelas.tabelaAcao[389].Add("ENTAO", new ActionClass(ActionType.Reduce, r267));
            Tabelas.tabelaAcao[389].Add("FACA", new ActionClass(ActionType.Reduce, r267));
            Tabelas.tabelaAcao[389].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r267));

            // <ARG_LOG_VAR_EX> -> <ARG_LOG_VAR> .
            Tabelas.tabelaAcao[390] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[390].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r269));
            Tabelas.tabelaAcao[390].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r269));
            Tabelas.tabelaAcao[390].Add("E", new ActionClass(ActionType.Reduce, r269));
            Tabelas.tabelaAcao[390].Add("OU", new ActionClass(ActionType.Reduce, r269));
            Tabelas.tabelaAcao[390].Add("ENTAO", new ActionClass(ActionType.Reduce, r269));
            Tabelas.tabelaAcao[390].Add("MAIOR", new ActionClass(ActionType.Reduce, r269));
            Tabelas.tabelaAcao[390].Add("MENOR", new ActionClass(ActionType.Reduce, r269));
            Tabelas.tabelaAcao[390].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r269));
            Tabelas.tabelaAcao[390].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r269));
            Tabelas.tabelaAcao[390].Add("IGUAL", new ActionClass(ActionType.Reduce, r269));
            Tabelas.tabelaAcao[390].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r269));
            Tabelas.tabelaAcao[390].Add("MENOS", new ActionClass(ActionType.Reduce, r269));
            Tabelas.tabelaAcao[390].Add("MAIS", new ActionClass(ActionType.Reduce, r269));
            Tabelas.tabelaAcao[390].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r269));
            Tabelas.tabelaAcao[390].Add("BARRA", new ActionClass(ActionType.Reduce, r269));
            Tabelas.tabelaAcao[390].Add("MOD", new ActionClass(ActionType.Reduce, r269));
            Tabelas.tabelaAcao[390].Add("DIV", new ActionClass(ActionType.Reduce, r269));

            // <ARG_LOG_VAR_EX> -> <ARG_LOG_VAR_CONST> .
            Tabelas.tabelaAcao[391] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[391].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r270));
            Tabelas.tabelaAcao[391].Add("E", new ActionClass(ActionType.Reduce, r270));
            Tabelas.tabelaAcao[391].Add("OU", new ActionClass(ActionType.Reduce, r270));
            Tabelas.tabelaAcao[391].Add("ENTAO", new ActionClass(ActionType.Reduce, r270));
            Tabelas.tabelaAcao[391].Add("MAIOR", new ActionClass(ActionType.Reduce, r270));
            Tabelas.tabelaAcao[391].Add("MENOR", new ActionClass(ActionType.Reduce, r270));
            Tabelas.tabelaAcao[391].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r270));
            Tabelas.tabelaAcao[391].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r270));
            Tabelas.tabelaAcao[391].Add("IGUAL", new ActionClass(ActionType.Reduce, r270));
            Tabelas.tabelaAcao[391].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r270));
            Tabelas.tabelaAcao[391].Add("MENOS", new ActionClass(ActionType.Reduce, r270));
            Tabelas.tabelaAcao[391].Add("MAIS", new ActionClass(ActionType.Reduce, r270));
            Tabelas.tabelaAcao[391].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r270));
            Tabelas.tabelaAcao[391].Add("BARRA", new ActionClass(ActionType.Reduce, r270));
            Tabelas.tabelaAcao[391].Add("MOD", new ActionClass(ActionType.Reduce, r270));
            Tabelas.tabelaAcao[391].Add("DIV", new ActionClass(ActionType.Reduce, r270));

            // <ARG_LOG_VAR_EX> -> MENOS . <ARG_LOG_VAR_CONST>
            // <ARG_LOG_VAR_CONST> -> . CONST_INT
            // <ARG_LOG_VAR_CONST> -> . CONST_REAL
            Tabelas.tabelaAcao[392] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[392].Add("CONST_INT", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[392].Add("CONST_REAL", new ActionClass(ActionType.Shift, 407));

            Tabelas.tabelaGOTO[392] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[392].Add("<ARG_LOG_VAR_CONST>", 393);

            // <ARG_LOG_VAR_EX> -> MENOS <ARG_LOG_VAR_CONST> .
            Tabelas.tabelaAcao[393] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[393].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r271));
            Tabelas.tabelaAcao[393].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r271));
            Tabelas.tabelaAcao[393].Add("E", new ActionClass(ActionType.Reduce, r271));
            Tabelas.tabelaAcao[393].Add("OU", new ActionClass(ActionType.Reduce, r271));
            Tabelas.tabelaAcao[393].Add("ENTAO", new ActionClass(ActionType.Reduce, r271));
            
            // <ARG_LOG_VAR> -> ID .
            // <ARG_LOG_VAR> -> ID . ABRE_COL <LST_ATRIB_VET_INDEX> FECHA_COL
            // <ARG_LOG_VAR> -> ID . ABRE_PAR <LST_ATRIB_FUNC_PARAM> FECHA_PAR
            Tabelas.tabelaAcao[400] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[400].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r280));
            Tabelas.tabelaAcao[400].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r280));
            Tabelas.tabelaAcao[400].Add("E", new ActionClass(ActionType.Reduce, r280));
            Tabelas.tabelaAcao[400].Add("OU", new ActionClass(ActionType.Reduce, r280));
            Tabelas.tabelaAcao[400].Add("ENTAO", new ActionClass(ActionType.Reduce, r280));
            Tabelas.tabelaAcao[400].Add("MAIOR", new ActionClass(ActionType.Reduce, r280));
            Tabelas.tabelaAcao[400].Add("MENOR", new ActionClass(ActionType.Reduce, r280));
            Tabelas.tabelaAcao[400].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r280));
            Tabelas.tabelaAcao[400].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r280));
            Tabelas.tabelaAcao[400].Add("IGUAL", new ActionClass(ActionType.Reduce, r280));
            Tabelas.tabelaAcao[400].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r280));
            Tabelas.tabelaAcao[400].Add("MENOS", new ActionClass(ActionType.Reduce, r280));
            Tabelas.tabelaAcao[400].Add("MAIS", new ActionClass(ActionType.Reduce, r280));
            Tabelas.tabelaAcao[400].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r280));
            Tabelas.tabelaAcao[400].Add("BARRA", new ActionClass(ActionType.Reduce, r280));
            Tabelas.tabelaAcao[400].Add("MOD", new ActionClass(ActionType.Reduce, r280));
            Tabelas.tabelaAcao[400].Add("DIV", new ActionClass(ActionType.Reduce, r280));
            Tabelas.tabelaAcao[400].Add("ABRE_COL", new ActionClass(ActionType.Shift, 401));
            Tabelas.tabelaAcao[400].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 404));

            // <ARG_LOG_VAR> -> ID ABRE_COL . <LST_ATRIB_VET_INDEX> FECHA_COL
            Tabelas.tabelaAcao[401] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[401].Add("ID", new ActionClass(ActionType.Shift, 239));
            Tabelas.tabelaAcao[401].Add("CONST_INT", new ActionClass(ActionType.Shift, 239));

            Tabelas.tabelaGOTO[401] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[401].Add("<LST_ATRIB_VET_INDEX>", 402);
            Tabelas.tabelaGOTO[401].Add("<ATRIB_VET_INDEX_VAL>", 234);

            // <ARG_LOG_VAR> -> ID ABRE_COL <LST_ATRIB_VET_INDEX> . FECHA_COL
            Tabelas.tabelaAcao[402] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[402].Add("VIRGULA", new ActionClass(ActionType.Shift, 235));
            Tabelas.tabelaAcao[402].Add("FECHA_COL", new ActionClass(ActionType.Shift, 403));

            // <ARG_LOG_VAR> -> ID ABRE_COL <LST_ATRIB_VET_INDEX> FECHA_COL .
            Tabelas.tabelaAcao[403] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[403].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r281));
            Tabelas.tabelaAcao[403].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r281));
            Tabelas.tabelaAcao[403].Add("E", new ActionClass(ActionType.Reduce, r281));
            Tabelas.tabelaAcao[403].Add("OU", new ActionClass(ActionType.Reduce, r281));
            Tabelas.tabelaAcao[403].Add("ENTAO", new ActionClass(ActionType.Reduce, r281));
            Tabelas.tabelaAcao[403].Add("MAIOR", new ActionClass(ActionType.Reduce, r281));
            Tabelas.tabelaAcao[403].Add("MENOR", new ActionClass(ActionType.Reduce, r281));
            Tabelas.tabelaAcao[403].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r281));
            Tabelas.tabelaAcao[403].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r281));
            Tabelas.tabelaAcao[403].Add("IGUAL", new ActionClass(ActionType.Reduce, r281));
            Tabelas.tabelaAcao[403].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r281));
            Tabelas.tabelaAcao[403].Add("MENOS", new ActionClass(ActionType.Reduce, r281));
            Tabelas.tabelaAcao[403].Add("MAIS", new ActionClass(ActionType.Reduce, r281));
            Tabelas.tabelaAcao[403].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r281));
            Tabelas.tabelaAcao[403].Add("BARRA", new ActionClass(ActionType.Reduce, r281));
            Tabelas.tabelaAcao[403].Add("MOD", new ActionClass(ActionType.Reduce, r281));
            Tabelas.tabelaAcao[403].Add("DIV", new ActionClass(ActionType.Reduce, r281));

            // <ARG_LOG_VAR> -> ID ABRE_PAR . <LST_ATRIB_FUNC_PARAM> FECHA_PAR
            Tabelas.tabelaAcao[404] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[404].Add("ID", new ActionClass(ActionType.Shift, 227));
            Tabelas.tabelaAcao[404].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r147));

            Tabelas.tabelaGOTO[404] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[404].Add("<LST_ATRIB_FUNC_PARAM>", 405);

            // <ARG_LOG_VAR> -> ID ABRE_PAR <LST_ATRIB_FUNC_PARAM> . FECHA_PAR
            Tabelas.tabelaAcao[405] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[405].Add("FECHA_PAR", new ActionClass(ActionType.Shift, 406));
            Tabelas.tabelaAcao[405].Add("VIRGULA", new ActionClass(ActionType.Shift, 228));

            Tabelas.tabelaGOTO[405] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[405].Add("<LST_ATRIB_FUNC_PARAM>", 224);
            
            // <ARG_LOG_VAR> -> ID ABRE_PAR <LST_ATRIB_FUNC_PARAM> FECHA_PAR .
            Tabelas.tabelaAcao[406] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[406].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r282));
            Tabelas.tabelaAcao[406].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r282));
            Tabelas.tabelaAcao[406].Add("E", new ActionClass(ActionType.Reduce, r282));
            Tabelas.tabelaAcao[406].Add("OU", new ActionClass(ActionType.Reduce, r282));
            Tabelas.tabelaAcao[406].Add("ENTAO", new ActionClass(ActionType.Reduce, r282));
            Tabelas.tabelaAcao[406].Add("MAIOR", new ActionClass(ActionType.Reduce, r282));
            Tabelas.tabelaAcao[406].Add("MENOR", new ActionClass(ActionType.Reduce, r282));
            Tabelas.tabelaAcao[406].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r282));
            Tabelas.tabelaAcao[406].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r282));
            Tabelas.tabelaAcao[406].Add("IGUAL", new ActionClass(ActionType.Reduce, r282));
            Tabelas.tabelaAcao[406].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r282));
            Tabelas.tabelaAcao[406].Add("MENOS", new ActionClass(ActionType.Reduce, r282));
            Tabelas.tabelaAcao[406].Add("MAIS", new ActionClass(ActionType.Reduce, r282));
            Tabelas.tabelaAcao[406].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r282));
            Tabelas.tabelaAcao[406].Add("BARRA", new ActionClass(ActionType.Reduce, r282));
            Tabelas.tabelaAcao[406].Add("MOD", new ActionClass(ActionType.Reduce, r282));
            Tabelas.tabelaAcao[406].Add("DIV", new ActionClass(ActionType.Reduce, r282));

            // <ARG_LOG_VAR_CONST> -> CONST_INT . | CONST_REAL .
            Tabelas.tabelaAcao[407] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[407].Add("PONTO_VIRGULA", new ActionClass(ActionType.Reduce, r283));
            Tabelas.tabelaAcao[407].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r283));
            Tabelas.tabelaAcao[407].Add("E", new ActionClass(ActionType.Reduce, r283));
            Tabelas.tabelaAcao[407].Add("OU", new ActionClass(ActionType.Reduce, r283));
            Tabelas.tabelaAcao[407].Add("ENTAO", new ActionClass(ActionType.Reduce, r283));
            Tabelas.tabelaAcao[407].Add("FACA", new ActionClass(ActionType.Reduce, r283));
            Tabelas.tabelaAcao[407].Add("MAIOR", new ActionClass(ActionType.Reduce, r283));
            Tabelas.tabelaAcao[407].Add("MENOR", new ActionClass(ActionType.Reduce, r283));
            Tabelas.tabelaAcao[407].Add("MAIOR_IGUAL", new ActionClass(ActionType.Reduce, r283));
            Tabelas.tabelaAcao[407].Add("MENOR_IGUAL", new ActionClass(ActionType.Reduce, r283));
            Tabelas.tabelaAcao[407].Add("IGUAL", new ActionClass(ActionType.Reduce, r283));
            Tabelas.tabelaAcao[407].Add("DIFERENTE", new ActionClass(ActionType.Reduce, r283));
            Tabelas.tabelaAcao[407].Add("MENOS", new ActionClass(ActionType.Reduce, r283));
            Tabelas.tabelaAcao[407].Add("MAIS", new ActionClass(ActionType.Reduce, r283));
            Tabelas.tabelaAcao[407].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r283));
            Tabelas.tabelaAcao[407].Add("BARRA", new ActionClass(ActionType.Reduce, r283));
            Tabelas.tabelaAcao[407].Add("MOD", new ActionClass(ActionType.Reduce, r283));
            Tabelas.tabelaAcao[407].Add("DIV", new ActionClass(ActionType.Reduce, r283));

            // <ARG_LOG_OP_REL> -> MAIOR . | MENOR . | MAIOR_IGUAL . | MENOR_IGUAL . | IGUAL . | DIFERENTE .
            Tabelas.tabelaAcao[408] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[408].Add("MENOS", new ActionClass(ActionType.Reduce, r284));
            Tabelas.tabelaAcao[408].Add("CONST_INT", new ActionClass(ActionType.Reduce, r284));
            Tabelas.tabelaAcao[408].Add("CONST_REAL", new ActionClass(ActionType.Reduce, r284));
            Tabelas.tabelaAcao[408].Add("ABRE_PAR", new ActionClass(ActionType.Reduce, r284));
            Tabelas.tabelaAcao[408].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r284));
            Tabelas.tabelaAcao[408].Add("E", new ActionClass(ActionType.Reduce, r284));
            Tabelas.tabelaAcao[408].Add("OU", new ActionClass(ActionType.Reduce, r284));
            Tabelas.tabelaAcao[408].Add("ENTAO", new ActionClass(ActionType.Reduce, r284));
            Tabelas.tabelaAcao[408].Add("ID", new ActionClass(ActionType.Reduce, r284));

            // <ARG_LOG_OP_MAT> -> MENOS . | MAIS . | ASTERISTICO . | BARRA . | MOD . | DIV .
            Tabelas.tabelaAcao[409] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[409].Add("ABRE_PAR", new ActionClass(ActionType.Reduce, r285));
            Tabelas.tabelaAcao[409].Add("MENOS", new ActionClass(ActionType.Reduce, r285));
            Tabelas.tabelaAcao[409].Add("CONST_INT", new ActionClass(ActionType.Reduce, r285));
            Tabelas.tabelaAcao[409].Add("CONST_REAL", new ActionClass(ActionType.Reduce, r285));
            Tabelas.tabelaAcao[409].Add("E", new ActionClass(ActionType.Reduce, r285));
            Tabelas.tabelaAcao[409].Add("OU", new ActionClass(ActionType.Reduce, r285));
            Tabelas.tabelaAcao[409].Add("ENTAO", new ActionClass(ActionType.Reduce, r285));
            Tabelas.tabelaAcao[409].Add("ID", new ActionClass(ActionType.Reduce, r285));

            // <ARG_LOG_OP_LOG> -> E . | OU .
            Tabelas.tabelaAcao[410] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[410].Add("ABRE_PAR", new ActionClass(ActionType.Reduce, r286));
            //Tabelas.tabelaAcao[410].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r286));
            Tabelas.tabelaAcao[410].Add("CONST_INT", new ActionClass(ActionType.Reduce, r286));
            Tabelas.tabelaAcao[410].Add("CONST_REAL", new ActionClass(ActionType.Reduce, r286));
            Tabelas.tabelaAcao[410].Add("CONST_LOGICA", new ActionClass(ActionType.Reduce, r286));
            Tabelas.tabelaAcao[410].Add("NAO", new ActionClass(ActionType.Reduce, r286));
            Tabelas.tabelaAcao[410].Add("ID", new ActionClass(ActionType.Reduce, r286));
            Tabelas.tabelaAcao[410].Add("ENTAO", new ActionClass(ActionType.Reduce, r286));
            
            #endregion

            #region Comando de Seleção 'Se-Senão'

            // <LISTA_COMANDOS> -> <CMD_SEL_IF> .
            Tabelas.tabelaAcao[500] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[500].Add("LEIA", new ActionClass(ActionType.Reduce, r350));
            Tabelas.tabelaAcao[500].Add("ESCREVA", new ActionClass(ActionType.Reduce, r350));
            Tabelas.tabelaAcao[500].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r350));
            Tabelas.tabelaAcao[500].Add("ID", new ActionClass(ActionType.Reduce, r350));
            Tabelas.tabelaAcao[500].Add("SE", new ActionClass(ActionType.Reduce, r350));
            Tabelas.tabelaAcao[500].Add("SENAO", new ActionClass(ActionType.Reduce, r350));
            Tabelas.tabelaAcao[500].Add("FIM_SE", new ActionClass(ActionType.Reduce, r350));
            Tabelas.tabelaAcao[500].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r350));
            Tabelas.tabelaAcao[500].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r350));
            Tabelas.tabelaAcao[500].Add("REPITA", new ActionClass(ActionType.Reduce, r350));
            Tabelas.tabelaAcao[500].Add("ATE", new ActionClass(ActionType.Reduce, r350));
            Tabelas.tabelaAcao[500].Add("PARA", new ActionClass(ActionType.Reduce, r350));
            Tabelas.tabelaAcao[500].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r350));
            //Tabelas.tabelaAcao[500].Add("", new ActionClass(ActionType.Reduce, r350));
            //Tabelas.tabelaAcao[500].Add("", new ActionClass(ActionType.Reduce, r350));
            Tabelas.tabelaAcao[500].Add("RETORNE", new ActionClass(ActionType.Reduce, r350));
            Tabelas.tabelaAcao[500].Add("FIM", new ActionClass(ActionType.Reduce, r350));

            // <LISTA_COMANDOS> -> <LISTA_COMANDOS> <CMD_SEL_IF> .
            Tabelas.tabelaAcao[501] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[501].Add("LEIA", new ActionClass(ActionType.Reduce, r351));
            Tabelas.tabelaAcao[501].Add("ESCREVA", new ActionClass(ActionType.Reduce, r351));
            Tabelas.tabelaAcao[501].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r351));
            Tabelas.tabelaAcao[501].Add("ID", new ActionClass(ActionType.Reduce, r351));
            Tabelas.tabelaAcao[501].Add("SE", new ActionClass(ActionType.Reduce, r351));
            Tabelas.tabelaAcao[501].Add("SENAO", new ActionClass(ActionType.Reduce, r351));
            Tabelas.tabelaAcao[501].Add("FIM_SE", new ActionClass(ActionType.Reduce, r351));
            Tabelas.tabelaAcao[501].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r351));
            Tabelas.tabelaAcao[501].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r351));
            Tabelas.tabelaAcao[501].Add("REPITA", new ActionClass(ActionType.Reduce, r351));
            Tabelas.tabelaAcao[501].Add("ATE", new ActionClass(ActionType.Reduce, r351));
            Tabelas.tabelaAcao[501].Add("PARA", new ActionClass(ActionType.Reduce, r351));
            Tabelas.tabelaAcao[501].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r351));
            //Tabelas.tabelaAcao[501].Add("", new ActionClass(ActionType.Reduce, r351));
            //Tabelas.tabelaAcao[501].Add("", new ActionClass(ActionType.Reduce, r351));
            Tabelas.tabelaAcao[501].Add("FIM", new ActionClass(ActionType.Reduce, r351));

            // <CMD_SEL_IF> -> SE . <ARGUMENTO_LOGICO> ENTAO <LISTA_COMANDOS> <CMD_SEL_IF_BLOCO_ELSE> FIM_SE PONTO_VIRGULA
            Tabelas.tabelaAcao[502] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[502].Add("CONST_INT", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[502].Add("CONST_REAL", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[502].Add("CONST_LOGICA", new ActionClass(ActionType.Shift, 361));
            Tabelas.tabelaAcao[502].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 354));
            Tabelas.tabelaAcao[502].Add("NAO", new ActionClass(ActionType.Shift, 357));
            Tabelas.tabelaAcao[502].Add("ID", new ActionClass(ActionType.Shift, 400));

            Tabelas.tabelaGOTO[502] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[502].Add("<ARGUMENTO_LOGICO>", 503);
            Tabelas.tabelaGOTO[502].Add("<LST_ARG_LOG_COND>", 350);
            Tabelas.tabelaGOTO[502].Add("<LST_ARG_LOG_VAR1>", 367);
            Tabelas.tabelaGOTO[502].Add("<ARG_LOG_VAR>", 362);
            Tabelas.tabelaGOTO[502].Add("<ARG_LOG_VAR_CONST>", 372);

            // <CMD_SEL_IF> -> SE <ARGUMENTO_LOGICO> . ENTAO <LISTA_COMANDOS> <CMD_SEL_IF_BLOCO_ELSE> FIM_SE PONTO_VIRGULA
            // <ARGUMENTO_LOGICO> -> <ARGUMENTO_LOGICO> . <ARG_LOG_OP_LOG> <LST_ARG_LOG_COND>
            Tabelas.tabelaAcao[503] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[503].Add("E", new ActionClass(ActionType.Shift, 410));
            Tabelas.tabelaAcao[503].Add("OU", new ActionClass(ActionType.Shift, 410));
            Tabelas.tabelaAcao[503].Add("ENTAO", new ActionClass(ActionType.Shift, 504));

            Tabelas.tabelaGOTO[503] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[503].Add("<ARG_LOG_OP_LOG>", 352);

            // <CMD_SEL_IF> -> SE <ARGUMENTO_LOGICO> ENTAO . <LISTA_COMANDOS> <CMD_SEL_IF_BLOCO_ELSE> FIM_SE PONTO_VIRGULA
            Tabelas.tabelaAcao[504] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[504].Add("LEIA", new ActionClass(ActionType.Shift, 652));
            Tabelas.tabelaAcao[504].Add("ESCREVA", new ActionClass(ActionType.Shift, 702));
            Tabelas.tabelaAcao[504].Add("ESCREVALN", new ActionClass(ActionType.Shift, 709));
            Tabelas.tabelaAcao[504].Add("ID", new ActionClass(ActionType.Shift, 202));
            Tabelas.tabelaAcao[504].Add("SE", new ActionClass(ActionType.Shift, 502));
            Tabelas.tabelaAcao[504].Add("ENQUANTO", new ActionClass(ActionType.Shift, 532));
            Tabelas.tabelaAcao[504].Add("REPITA", new ActionClass(ActionType.Shift, 552));
            Tabelas.tabelaAcao[504].Add("PARA", new ActionClass(ActionType.Shift, 602));
            //Tabelas.tabelaAcao[504].Add("", new ActionClass(ActionType.Shift, ));
            //Tabelas.tabelaAcao[504].Add("", new ActionClass(ActionType.Shift, ));

            Tabelas.tabelaGOTO[504] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[504].Add("<LISTA_COMANDOS>", 505);
            Tabelas.tabelaGOTO[504].Add("<CMD_LEIA>", 650);
            Tabelas.tabelaGOTO[504].Add("<CMD_ESCREVA>", 700);
            Tabelas.tabelaGOTO[504].Add("<CMD_ESCREVALN>", 707);
            Tabelas.tabelaGOTO[504].Add("<CMD_ATRIBUICAO>", 200);
            Tabelas.tabelaGOTO[504].Add("<CMD_ATRIB_VAR_REC>", 203);
            Tabelas.tabelaGOTO[504].Add("<CMD_CHAMA_PROC>", 150);
            Tabelas.tabelaGOTO[504].Add("<CMD_SEL_IF>", 500);
            Tabelas.tabelaGOTO[504].Add("<CMD_REP_ENQUANTO>", 530);
            Tabelas.tabelaGOTO[504].Add("<CMD_REP_REPITA>", 550);
            Tabelas.tabelaGOTO[504].Add("<CMD_REP_PARA>", 600);
            //Tabelas.tabelaGOTO[504].Add("<>", );
            //Tabelas.tabelaGOTO[504].Add("<>", );

            // <CMD_SEL_IF> -> SE <ARGUMENTO_LOGICO> ENTAO <LISTA_COMANDOS> . <CMD_SEL_IF_BLOCO_ELSE> FIM_SE PONTO_VIRGULA
            // <CMD_SEL_IF_BLOCO_ELSE> -> .
            Tabelas.tabelaAcao[505] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[505].Add("LEIA", new ActionClass(ActionType.Shift, 652));
            Tabelas.tabelaAcao[505].Add("ESCREVA", new ActionClass(ActionType.Shift, 702));
            Tabelas.tabelaAcao[505].Add("ESCREVALN", new ActionClass(ActionType.Shift, 709));
            Tabelas.tabelaAcao[505].Add("ID", new ActionClass(ActionType.Shift, 202));
            Tabelas.tabelaAcao[505].Add("SE", new ActionClass(ActionType.Shift, 502));
            Tabelas.tabelaAcao[505].Add("SENAO", new ActionClass(ActionType.Shift, 509));
            Tabelas.tabelaAcao[505].Add("FIM_SE", new ActionClass(ActionType.Reduce, r353));
            Tabelas.tabelaAcao[505].Add("ENQUANTO", new ActionClass(ActionType.Shift, 532));
            Tabelas.tabelaAcao[505].Add("REPITA", new ActionClass(ActionType.Shift, 552));
            Tabelas.tabelaAcao[505].Add("PARA", new ActionClass(ActionType.Shift, 602));
            //Tabelas.tabelaAcao[505].Add("", new ActionClass(ActionType.Shift, ));
            //Tabelas.tabelaAcao[505].Add("", new ActionClass(ActionType.Shift, ));

            Tabelas.tabelaGOTO[505] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[505].Add("<CMD_LEIA>", 651);
            Tabelas.tabelaGOTO[505].Add("<CMD_ESCREVA>", 701);
            Tabelas.tabelaGOTO[505].Add("<CMD_ESCREVALN>", 708);
            Tabelas.tabelaGOTO[505].Add("<CMD_ATRIBUICAO>", 201);
            Tabelas.tabelaGOTO[505].Add("<CMD_ATRIB_VAR_REC>", 203);
            Tabelas.tabelaGOTO[505].Add("<CMD_CHAMA_PROC>", 151);
            Tabelas.tabelaGOTO[505].Add("<CMD_SEL_IF>", 501);
            Tabelas.tabelaGOTO[505].Add("<CMD_REP_ENQUANTO>", 531);
            Tabelas.tabelaGOTO[505].Add("<CMD_REP_REPITA>", 551);
            Tabelas.tabelaGOTO[505].Add("<CMD_REP_PARA>", 601);
            //Tabelas.tabelaGOTO[505].Add("<>", );
            //Tabelas.tabelaGOTO[505].Add("<>", );
            Tabelas.tabelaGOTO[505].Add("<CMD_SEL_IF_BLOCO_ELSE>", 506);

            // <CMD_SEL_IF> -> SE <ARGUMENTO_LOGICO> ENTAO <LISTA_COMANDOS> <CMD_SEL_IF_BLOCO_ELSE> . FIM_SE PONTO_VIRGULA
            Tabelas.tabelaAcao[506] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[506].Add("FIM_SE", new ActionClass(ActionType.Shift, 507));

            // <CMD_SEL_IF> -> SE <ARGUMENTO_LOGICO> ENTAO <LISTA_COMANDOS> <CMD_SEL_IF_BLOCO_ELSE> FIM_SE . PONTO_VIRGULA
            Tabelas.tabelaAcao[507] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[507].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 508));

            // <CMD_SEL_IF> -> SE <ARGUMENTO_LOGICO> ENTAO <LISTA_COMANDOS> <CMD_SEL_IF_BLOCO_ELSE> FIM_SE PONTO_VIRGULA .
            Tabelas.tabelaAcao[508] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[508].Add("LEIA", new ActionClass(ActionType.Reduce, r352));
            Tabelas.tabelaAcao[508].Add("ESCREVA", new ActionClass(ActionType.Reduce, r352));
            Tabelas.tabelaAcao[508].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r352));
            Tabelas.tabelaAcao[508].Add("ID", new ActionClass(ActionType.Reduce, r352));
            Tabelas.tabelaAcao[508].Add("SE", new ActionClass(ActionType.Reduce, r352));
            Tabelas.tabelaAcao[508].Add("SENAO", new ActionClass(ActionType.Reduce, r352));
            Tabelas.tabelaAcao[508].Add("FIM_SE", new ActionClass(ActionType.Reduce, r352));
            Tabelas.tabelaAcao[508].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r352));
            Tabelas.tabelaAcao[508].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r352));
            Tabelas.tabelaAcao[508].Add("REPITA", new ActionClass(ActionType.Reduce, r352));
            Tabelas.tabelaAcao[508].Add("ATE", new ActionClass(ActionType.Reduce, r352));
            Tabelas.tabelaAcao[508].Add("PARA", new ActionClass(ActionType.Reduce, r352));
            Tabelas.tabelaAcao[508].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r352));
            //Tabelas.tabelaAcao[508].Add("", new ActionClass(ActionType.Reduce, r352));
            //Tabelas.tabelaAcao[508].Add("", new ActionClass(ActionType.Reduce, r352));
            Tabelas.tabelaAcao[508].Add("RETORNE", new ActionClass(ActionType.Reduce, r352));
            Tabelas.tabelaAcao[508].Add("FIM", new ActionClass(ActionType.Reduce, r352));
            
            // <CMD_SEL_IF_BLOCO_ELSE> -> SENAO . <LISTA_COMANDOS>
            Tabelas.tabelaAcao[509] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[509].Add("LEIA", new ActionClass(ActionType.Shift, 652));
            Tabelas.tabelaAcao[509].Add("ESCREVA", new ActionClass(ActionType.Shift, 702));
            Tabelas.tabelaAcao[509].Add("ESCREVALN", new ActionClass(ActionType.Shift, 709));
            Tabelas.tabelaAcao[509].Add("ID", new ActionClass(ActionType.Shift, 202));
            Tabelas.tabelaAcao[509].Add("SE", new ActionClass(ActionType.Shift, 502));
            Tabelas.tabelaAcao[509].Add("ENQUANTO", new ActionClass(ActionType.Shift, 532));
            Tabelas.tabelaAcao[509].Add("REPITA", new ActionClass(ActionType.Shift, 552));
            Tabelas.tabelaAcao[509].Add("PARA", new ActionClass(ActionType.Shift, 602));
            //Tabelas.tabelaAcao[509].Add("", new ActionClass(ActionType.Shift, ));
            //Tabelas.tabelaAcao[509].Add("", new ActionClass(ActionType.Shift, ));

            Tabelas.tabelaGOTO[509] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[509].Add("<LISTA_COMANDOS>", 510);
            Tabelas.tabelaGOTO[509].Add("<CMD_LEIA>", 650);
            Tabelas.tabelaGOTO[509].Add("<CMD_ESCREVA>", 700);
            Tabelas.tabelaGOTO[509].Add("<CMD_ESCREVALN>", 707);
            Tabelas.tabelaGOTO[509].Add("<CMD_ATRIBUICAO>", 200);
            Tabelas.tabelaGOTO[509].Add("<CMD_ATRIB_VAR_REC>", 203);
            Tabelas.tabelaGOTO[509].Add("<CMD_CHAMA_PROC>", 150);
            Tabelas.tabelaGOTO[509].Add("<CMD_SEL_IF>", 500);
            Tabelas.tabelaGOTO[509].Add("<CMD_REP_ENQUANTO>", 530);
            Tabelas.tabelaGOTO[509].Add("<CMD_REP_REPITA>", 550);
            Tabelas.tabelaGOTO[509].Add("<CMD_REP_PARA>", 600);
            //Tabelas.tabelaGOTO[509].Add("<>", );
            //Tabelas.tabelaGOTO[509].Add("<>", );

            // <CMD_SEL_IF_BLOCO_ELSE> -> SENAO <LISTA_COMANDOS> .
            Tabelas.tabelaAcao[510] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[510].Add("LEIA", new ActionClass(ActionType.Shift, 652));
            Tabelas.tabelaAcao[510].Add("ESCREVA", new ActionClass(ActionType.Shift, 702));
            Tabelas.tabelaAcao[510].Add("ESCREVALN", new ActionClass(ActionType.Shift, 709));
            Tabelas.tabelaAcao[510].Add("ID", new ActionClass(ActionType.Shift, 202));
            Tabelas.tabelaAcao[510].Add("SE", new ActionClass(ActionType.Shift, 502));
            Tabelas.tabelaAcao[510].Add("FIM_SE", new ActionClass(ActionType.Reduce, r354));
            Tabelas.tabelaAcao[510].Add("ENQUANTO", new ActionClass(ActionType.Shift, 532));
            Tabelas.tabelaAcao[510].Add("REPITA", new ActionClass(ActionType.Shift, 552));
            Tabelas.tabelaAcao[510].Add("PARA", new ActionClass(ActionType.Shift, 602));
            //Tabelas.tabelaAcao[510].Add("", new ActionClass(ActionType.Shift, ));
            //Tabelas.tabelaAcao[510].Add("", new ActionClass(ActionType.Shift, ));

            Tabelas.tabelaGOTO[510] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[510].Add("<CMD_LEIA>", 651);
            Tabelas.tabelaGOTO[510].Add("<CMD_ESCREVA>", 701);
            Tabelas.tabelaGOTO[510].Add("<CMD_ESCREVALN>", 708);
            Tabelas.tabelaGOTO[510].Add("<CMD_ATRIBUICAO>", 201);
            Tabelas.tabelaGOTO[510].Add("<CMD_ATRIB_VAR_REC>", 203);
            Tabelas.tabelaGOTO[510].Add("<CMD_CHAMA_PROC>", 151);
            Tabelas.tabelaGOTO[510].Add("<CMD_SEL_IF>", 501);
            Tabelas.tabelaGOTO[510].Add("<CMD_REP_ENQUANTO>", 531);
            Tabelas.tabelaGOTO[510].Add("<CMD_REP_REPITA>", 551);
            Tabelas.tabelaGOTO[510].Add("<CMD_REP_PARA>", 601);
            //Tabelas.tabelaGOTO[510].Add("<>", );
            //Tabelas.tabelaGOTO[510].Add("<>", );

            #endregion

            #region Comando Laço de Repetição 'Enquanto-Faça'

            // <LISTA_COMANDOS> -> <CMD_REP_ENQUANTO> .
            Tabelas.tabelaAcao[530] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[530].Add("LEIA", new ActionClass(ActionType.Reduce, r375));
            Tabelas.tabelaAcao[530].Add("ESCREVA", new ActionClass(ActionType.Reduce, r375));
            Tabelas.tabelaAcao[530].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r375));
            Tabelas.tabelaAcao[530].Add("ID", new ActionClass(ActionType.Reduce, r375));
            Tabelas.tabelaAcao[530].Add("SE", new ActionClass(ActionType.Reduce, r375));
            Tabelas.tabelaAcao[530].Add("SENAO", new ActionClass(ActionType.Reduce, r375));
            Tabelas.tabelaAcao[530].Add("FIM_SE", new ActionClass(ActionType.Reduce, r375));
            Tabelas.tabelaAcao[530].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r375));
            Tabelas.tabelaAcao[530].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r375));
            Tabelas.tabelaAcao[530].Add("REPITA", new ActionClass(ActionType.Reduce, r375));
            Tabelas.tabelaAcao[530].Add("ATE", new ActionClass(ActionType.Reduce, r375));
            Tabelas.tabelaAcao[530].Add("PARA", new ActionClass(ActionType.Reduce, r375));
            Tabelas.tabelaAcao[530].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r375));
            //Tabelas.tabelaAcao[530].Add("", new ActionClass(ActionType.Reduce, r375));
            //Tabelas.tabelaAcao[530].Add("", new ActionClass(ActionType.Reduce, r375));
            Tabelas.tabelaAcao[530].Add("RETORNE", new ActionClass(ActionType.Reduce, r375));
            Tabelas.tabelaAcao[530].Add("FIM", new ActionClass(ActionType.Reduce, r375));

            // <LISTA_COMANDOS> -> <LISTA_COMANDOS> <CMD_REP_ENQUANTO> .
            Tabelas.tabelaAcao[531] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[531].Add("LEIA", new ActionClass(ActionType.Reduce, r376));
            Tabelas.tabelaAcao[531].Add("ESCREVA", new ActionClass(ActionType.Reduce, r376));
            Tabelas.tabelaAcao[531].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r376));
            Tabelas.tabelaAcao[531].Add("ID", new ActionClass(ActionType.Reduce, r376));
            Tabelas.tabelaAcao[531].Add("SE", new ActionClass(ActionType.Reduce, r376));
            Tabelas.tabelaAcao[531].Add("SENAO", new ActionClass(ActionType.Reduce, r376));
            Tabelas.tabelaAcao[531].Add("FIM_SE", new ActionClass(ActionType.Reduce, r376));
            Tabelas.tabelaAcao[531].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r376));
            Tabelas.tabelaAcao[531].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r376));
            Tabelas.tabelaAcao[531].Add("REPITA", new ActionClass(ActionType.Reduce, r376));
            Tabelas.tabelaAcao[531].Add("ATE", new ActionClass(ActionType.Reduce, r376));
            Tabelas.tabelaAcao[531].Add("PARA", new ActionClass(ActionType.Reduce, r376));
            Tabelas.tabelaAcao[531].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r376));
            //Tabelas.tabelaAcao[531].Add("", new ActionClass(ActionType.Reduce, r376));
            //Tabelas.tabelaAcao[531].Add("", new ActionClass(ActionType.Reduce, r376));
            Tabelas.tabelaAcao[531].Add("RETORNE", new ActionClass(ActionType.Reduce, r376));
            Tabelas.tabelaAcao[531].Add("FIM", new ActionClass(ActionType.Reduce, r376));

            // <CMD_REP_ENQUANTO> -> ENQUANTO . <ARGUMENTO_LOGICO> FACA <LISTA_COMANDOS> FIM_ENQUANTO PONTO_VIRGULA
            Tabelas.tabelaAcao[532] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[532].Add("CONST_INT", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[532].Add("CONST_REAL", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[532].Add("CONST_LOGICA", new ActionClass(ActionType.Shift, 361));
            Tabelas.tabelaAcao[532].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 354));
            Tabelas.tabelaAcao[532].Add("NAO", new ActionClass(ActionType.Shift, 357));
            Tabelas.tabelaAcao[532].Add("ID", new ActionClass(ActionType.Shift, 400));

            Tabelas.tabelaGOTO[532] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[532].Add("<ARGUMENTO_LOGICO>", 533);
            Tabelas.tabelaGOTO[532].Add("<LST_ARG_LOG_COND>", 350);
            Tabelas.tabelaGOTO[532].Add("<LST_ARG_LOG_VAR1>", 367);
            Tabelas.tabelaGOTO[532].Add("<ARG_LOG_VAR>", 362);
            Tabelas.tabelaGOTO[532].Add("<ARG_LOG_VAR_CONST>", 372);

            // <CMD_REP_ENQUANTO> -> ENQUANTO <ARGUMENTO_LOGICO> . FACA <LISTA_COMANDOS> FIM_ENQUANTO PONTO_VIRGULA
            // <ARGUMENTO_LOGICO> -> <ARGUMENTO_LOGICO> . <ARG_LOG_OP_LOG> <LST_ARG_LOG_COND>
            Tabelas.tabelaAcao[533] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[533].Add("E", new ActionClass(ActionType.Shift, 410));
            Tabelas.tabelaAcao[533].Add("OU", new ActionClass(ActionType.Shift, 410));
            Tabelas.tabelaAcao[533].Add("FACA", new ActionClass(ActionType.Shift, 534));

            Tabelas.tabelaGOTO[533] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[533].Add("<ARG_LOG_OP_LOG>", 352);

            // <CMD_REP_ENQUANTO> -> ENQUANTO <ARGUMENTO_LOGICO> FACA . <LISTA_COMANDOS> FIM_ENQUANTO PONTO_VIRGULA
            Tabelas.tabelaAcao[534] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[534].Add("LEIA", new ActionClass(ActionType.Shift, 652));
            Tabelas.tabelaAcao[534].Add("ESCREVA", new ActionClass(ActionType.Shift, 702));
            Tabelas.tabelaAcao[534].Add("ESCREVALN", new ActionClass(ActionType.Shift, 709));
            Tabelas.tabelaAcao[534].Add("ID", new ActionClass(ActionType.Shift, 202));
            Tabelas.tabelaAcao[534].Add("SE", new ActionClass(ActionType.Shift, 502));
            Tabelas.tabelaAcao[534].Add("ENQUANTO", new ActionClass(ActionType.Shift, 532));
            Tabelas.tabelaAcao[534].Add("REPITA", new ActionClass(ActionType.Shift, 552));
            Tabelas.tabelaAcao[534].Add("PARA", new ActionClass(ActionType.Shift, 602));
            //Tabelas.tabelaAcao[534].Add("", new ActionClass(ActionType.Shift, ));
            //Tabelas.tabelaAcao[534].Add("", new ActionClass(ActionType.Shift, ));

            Tabelas.tabelaGOTO[534] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[534].Add("<LISTA_COMANDOS>", 535);
            Tabelas.tabelaGOTO[534].Add("<CMD_LEIA>", 650);
            Tabelas.tabelaGOTO[534].Add("<CMD_ESCREVA>", 700);
            Tabelas.tabelaGOTO[534].Add("<CMD_ESCREVALN>", 707);
            Tabelas.tabelaGOTO[534].Add("<CMD_ATRIBUICAO>", 200);
            Tabelas.tabelaGOTO[534].Add("<CMD_ATRIB_VAR_REC>", 203);
            Tabelas.tabelaGOTO[534].Add("<CMD_CHAMA_PROC>", 150);
            Tabelas.tabelaGOTO[534].Add("<CMD_SEL_IF>", 500);
            Tabelas.tabelaGOTO[534].Add("<CMD_REP_ENQUANTO>", 530);
            Tabelas.tabelaGOTO[534].Add("<CMD_REP_REPITA>", 550);
            Tabelas.tabelaGOTO[534].Add("<CMD_REP_PARA>", 600);
            //Tabelas.tabelaGOTO[534].Add("<>", );
            //Tabelas.tabelaGOTO[534].Add("<>", );

            // <CMD_REP_ENQUANTO> -> ENQUANTO <ARGUMENTO_LOGICO> FACA <LISTA_COMANDOS> . FIM_ENQUANTO PONTO_VIRGULA
            Tabelas.tabelaAcao[535] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[535].Add("LEIA", new ActionClass(ActionType.Shift, 652));
            Tabelas.tabelaAcao[535].Add("ESCREVA", new ActionClass(ActionType.Shift, 702));
            Tabelas.tabelaAcao[535].Add("ESCREVALN", new ActionClass(ActionType.Shift, 709));
            Tabelas.tabelaAcao[535].Add("ID", new ActionClass(ActionType.Shift, 202));
            Tabelas.tabelaAcao[535].Add("SE", new ActionClass(ActionType.Shift, 502));
            Tabelas.tabelaAcao[535].Add("SENAO", new ActionClass(ActionType.Shift, 509));
            Tabelas.tabelaAcao[535].Add("ENQUANTO", new ActionClass(ActionType.Shift, 532));
            Tabelas.tabelaAcao[535].Add("FIM_ENQUANTO", new ActionClass(ActionType.Shift, 536));
            Tabelas.tabelaAcao[535].Add("REPITA", new ActionClass(ActionType.Shift, 552));
            Tabelas.tabelaAcao[535].Add("PARA", new ActionClass(ActionType.Shift, 602));
            //Tabelas.tabelaAcao[535].Add("", new ActionClass(ActionType.Shift, ));
            //Tabelas.tabelaAcao[535].Add("", new ActionClass(ActionType.Shift, ));

            Tabelas.tabelaGOTO[535] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[535].Add("<CMD_LEIA>", 651);
            Tabelas.tabelaGOTO[535].Add("<CMD_ESCREVA>", 701);
            Tabelas.tabelaGOTO[535].Add("<CMD_ESCREVALN>", 708);
            Tabelas.tabelaGOTO[535].Add("<CMD_ATRIBUICAO>", 201);
            Tabelas.tabelaGOTO[535].Add("<CMD_ATRIB_VAR_REC>", 203);
            Tabelas.tabelaGOTO[535].Add("<CMD_CHAMA_PROC>", 151);
            Tabelas.tabelaGOTO[535].Add("<CMD_SEL_IF>", 501);
            Tabelas.tabelaGOTO[535].Add("<CMD_SEL_IF_BLOCO_ELSE>", 506);
            Tabelas.tabelaGOTO[535].Add("<CMD_REP_ENQUANTO>", 531);
            Tabelas.tabelaGOTO[535].Add("<CMD_REP_REPITA>", 551);
            Tabelas.tabelaGOTO[535].Add("<CMD_REP_PARA>", 601);
            //Tabelas.tabelaGOTO[535].Add("<>", );
            //Tabelas.tabelaGOTO[535].Add("<>", );

            // <CMD_REP_ENQUANTO> -> ENQUANTO <ARGUMENTO_LOGICO> FACA <LISTA_COMANDOS> FIM_ENQUANTO . PONTO_VIRGULA
            Tabelas.tabelaAcao[536] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[536].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 537));

            // <CMD_REP_ENQUANTO> -> ENQUANTO <ARGUMENTO_LOGICO> FACA <LISTA_COMANDOS> FIM_ENQUANTO PONTO_VIRGULA .
            Tabelas.tabelaAcao[537] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[537].Add("LEIA", new ActionClass(ActionType.Reduce, r377));
            Tabelas.tabelaAcao[537].Add("ESCREVA", new ActionClass(ActionType.Reduce, r377));
            Tabelas.tabelaAcao[537].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r377));
            Tabelas.tabelaAcao[537].Add("ID", new ActionClass(ActionType.Reduce, r377));
            Tabelas.tabelaAcao[537].Add("SE", new ActionClass(ActionType.Reduce, r377));
            Tabelas.tabelaAcao[537].Add("SENAO", new ActionClass(ActionType.Reduce, r377));
            Tabelas.tabelaAcao[537].Add("FIM_SE", new ActionClass(ActionType.Reduce, r377));
            Tabelas.tabelaAcao[537].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r377));
            Tabelas.tabelaAcao[537].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r377));
            Tabelas.tabelaAcao[537].Add("REPITA", new ActionClass(ActionType.Reduce, r377));
            Tabelas.tabelaAcao[537].Add("PARA", new ActionClass(ActionType.Reduce, r377));
            Tabelas.tabelaAcao[537].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r377));
            //Tabelas.tabelaAcao[537].Add("", new ActionClass(ActionType.Reduce, r377));
            //Tabelas.tabelaAcao[537].Add("", new ActionClass(ActionType.Reduce, r377));
            Tabelas.tabelaAcao[537].Add("RETORNE", new ActionClass(ActionType.Reduce, r377));
            Tabelas.tabelaAcao[537].Add("FIM", new ActionClass(ActionType.Reduce, r377));

            #endregion

            #region Comando Laço de Repetição 'Repita-Até-Que'

            // <LISTA_COMANDOS> -> <CMD_REP_REPITA> .
            Tabelas.tabelaAcao[550] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[550].Add("LEIA", new ActionClass(ActionType.Reduce, r400));
            Tabelas.tabelaAcao[550].Add("ESCREVA", new ActionClass(ActionType.Reduce, r400));
            Tabelas.tabelaAcao[550].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r400));
            Tabelas.tabelaAcao[550].Add("ID", new ActionClass(ActionType.Reduce, r400));
            Tabelas.tabelaAcao[550].Add("SE", new ActionClass(ActionType.Reduce, r400));
            Tabelas.tabelaAcao[550].Add("SENAO", new ActionClass(ActionType.Reduce, r400));
            Tabelas.tabelaAcao[550].Add("FIM_SE", new ActionClass(ActionType.Reduce, r400));
            Tabelas.tabelaAcao[550].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r400));
            Tabelas.tabelaAcao[550].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r400));
            Tabelas.tabelaAcao[550].Add("REPITA", new ActionClass(ActionType.Reduce, r400));
            Tabelas.tabelaAcao[550].Add("ATE", new ActionClass(ActionType.Reduce, r400));
            Tabelas.tabelaAcao[550].Add("PARA", new ActionClass(ActionType.Reduce, r400));
            Tabelas.tabelaAcao[550].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r400));
            //Tabelas.tabelaAcao[550].Add("", new ActionClass(ActionType.Reduce, r400));
            //Tabelas.tabelaAcao[550].Add("", new ActionClass(ActionType.Reduce, r400));
            Tabelas.tabelaAcao[550].Add("FIM", new ActionClass(ActionType.Reduce, r400));

            // <LISTA_COMANDOS> -> <LISTA_COMANDOS> <CMD_REP_REPITA> .
            Tabelas.tabelaAcao[551] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[551].Add("LEIA", new ActionClass(ActionType.Reduce, r401));
            Tabelas.tabelaAcao[551].Add("ESCREVA", new ActionClass(ActionType.Reduce, r401));
            Tabelas.tabelaAcao[551].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r401));
            Tabelas.tabelaAcao[551].Add("ID", new ActionClass(ActionType.Reduce, r401));
            Tabelas.tabelaAcao[551].Add("SE", new ActionClass(ActionType.Reduce, r401));
            Tabelas.tabelaAcao[551].Add("SENAO", new ActionClass(ActionType.Reduce, r401));
            Tabelas.tabelaAcao[551].Add("FIM_SE", new ActionClass(ActionType.Reduce, r401));
            Tabelas.tabelaAcao[551].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r401));
            Tabelas.tabelaAcao[551].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r401));
            Tabelas.tabelaAcao[551].Add("REPITA", new ActionClass(ActionType.Reduce, r401));
            Tabelas.tabelaAcao[551].Add("ATE", new ActionClass(ActionType.Reduce, r401));
            Tabelas.tabelaAcao[551].Add("PARA", new ActionClass(ActionType.Reduce, r401));
            Tabelas.tabelaAcao[551].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r401));
            //Tabelas.tabelaAcao[551].Add("", new ActionClass(ActionType.Reduce, r401));
            //Tabelas.tabelaAcao[551].Add("", new ActionClass(ActionType.Reduce, r401));
            Tabelas.tabelaAcao[551].Add("FIM", new ActionClass(ActionType.Reduce, r401));

            // <CMD_REP_REPITA> -> REPITA . <LISTA_COMANDOS> ATE QUE <ARGUMENTO_LOGICO> PONTO_VIRGULA
            Tabelas.tabelaAcao[552] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[552].Add("LEIA", new ActionClass(ActionType.Shift, 652));
            Tabelas.tabelaAcao[552].Add("ESCREVA", new ActionClass(ActionType.Shift, 702));
            Tabelas.tabelaAcao[552].Add("ESCREVALN", new ActionClass(ActionType.Shift, 709));
            Tabelas.tabelaAcao[552].Add("ID", new ActionClass(ActionType.Shift, 202));
            Tabelas.tabelaAcao[552].Add("SE", new ActionClass(ActionType.Shift, 502));
            Tabelas.tabelaAcao[552].Add("ENQUANTO", new ActionClass(ActionType.Shift, 532));
            Tabelas.tabelaAcao[552].Add("REPITA", new ActionClass(ActionType.Shift, 552));
            Tabelas.tabelaAcao[552].Add("PARA", new ActionClass(ActionType.Shift, 602));
            //Tabelas.tabelaAcao[552].Add("", new ActionClass(ActionType.Shift, ));
            //Tabelas.tabelaAcao[552].Add("", new ActionClass(ActionType.Shift, ));

            Tabelas.tabelaGOTO[552] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[552].Add("<LISTA_COMANDOS>", 553);
            Tabelas.tabelaGOTO[552].Add("<CMD_LEIA>", 650);
            Tabelas.tabelaGOTO[552].Add("<CMD_ESCREVA>", 700);
            Tabelas.tabelaGOTO[552].Add("<CMD_ESCREVALN>", 701);
            Tabelas.tabelaGOTO[552].Add("<CMD_ATRIBUICAO>", 200);
            Tabelas.tabelaGOTO[552].Add("<CMD_ATRIB_VAR_REC>", 203);
            Tabelas.tabelaGOTO[552].Add("<CMD_CHAMA_PROC>", 150);
            Tabelas.tabelaGOTO[552].Add("<CMD_SEL_IF>", 500);
            Tabelas.tabelaGOTO[552].Add("<CMD_REP_ENQUANTO>", 530);
            Tabelas.tabelaGOTO[552].Add("<CMD_REP_REPITA>", 550);
            Tabelas.tabelaGOTO[552].Add("<CMD_REP_PARA>", 600);
            //Tabelas.tabelaGOTO[552].Add("<>", );
            //Tabelas.tabelaGOTO[552].Add("<>", );

            // <CMD_REP_REPITA> -> REPITA <LISTA_COMANDOS> . ATE QUE <ARGUMENTO_LOGICO> PONTO_VIRGULA
            Tabelas.tabelaAcao[553] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[553].Add("LEIA", new ActionClass(ActionType.Shift, 652));
            Tabelas.tabelaAcao[553].Add("ESCREVA", new ActionClass(ActionType.Shift, 702));
            Tabelas.tabelaAcao[553].Add("ESCREVALN", new ActionClass(ActionType.Shift, 709));
            Tabelas.tabelaAcao[553].Add("ID", new ActionClass(ActionType.Shift, 202));
            Tabelas.tabelaAcao[553].Add("SE", new ActionClass(ActionType.Shift, 502));
            Tabelas.tabelaAcao[553].Add("SENAO", new ActionClass(ActionType.Shift, 509));
            Tabelas.tabelaAcao[553].Add("ENQUANTO", new ActionClass(ActionType.Shift, 532));
            Tabelas.tabelaAcao[553].Add("FIM_ENQUANTO", new ActionClass(ActionType.Shift, 536));
            Tabelas.tabelaAcao[553].Add("REPITA", new ActionClass(ActionType.Shift, 552));
            Tabelas.tabelaAcao[553].Add("ATE", new ActionClass(ActionType.Shift, 554));
            Tabelas.tabelaAcao[553].Add("PARA", new ActionClass(ActionType.Shift, 602));
            //Tabelas.tabelaAcao[553].Add("", new ActionClass(ActionType.Shift, ));
            //Tabelas.tabelaAcao[553].Add("", new ActionClass(ActionType.Shift, ));

            Tabelas.tabelaGOTO[553] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[553].Add("<CMD_LEIA>", 651);
            Tabelas.tabelaGOTO[553].Add("<CMD_ESCREVA>", 701);
            Tabelas.tabelaGOTO[553].Add("<CMD_ESCREVALN>", 708);
            Tabelas.tabelaGOTO[553].Add("<CMD_ATRIBUICAO>", 201);
            Tabelas.tabelaGOTO[553].Add("<CMD_ATRIB_VAR_REC>", 203);
            Tabelas.tabelaGOTO[553].Add("<CMD_CHAMA_PROC>", 151);
            Tabelas.tabelaGOTO[553].Add("<CMD_SEL_IF>", 501);
            Tabelas.tabelaGOTO[553].Add("<CMD_SEL_IF_BLOCO_ELSE>", 506);
            Tabelas.tabelaGOTO[553].Add("<CMD_REP_ENQUANTO>", 531);
            Tabelas.tabelaGOTO[553].Add("<CMD_REP_REPITA>", 551);
            Tabelas.tabelaGOTO[553].Add("<CMD_REP_PARA>", 601);
            //Tabelas.tabelaGOTO[553].Add("<>", );
            //Tabelas.tabelaGOTO[553].Add("<>", );

            // <CMD_REP_REPITA> -> REPITA <LISTA_COMANDOS> ATE . QUE <ARGUMENTO_LOGICO> PONTO_VIRGULA
            Tabelas.tabelaAcao[554] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[554].Add("QUE", new ActionClass(ActionType.Shift, 555));

            // <CMD_REP_REPITA> -> REPITA <LISTA_COMANDOS> ATE QUE . <ARGUMENTO_LOGICO> PONTO_VIRGULA
            Tabelas.tabelaAcao[555] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[555].Add("CONST_INT", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[555].Add("CONST_REAL", new ActionClass(ActionType.Shift, 407));
            Tabelas.tabelaAcao[555].Add("CONST_LOGICA", new ActionClass(ActionType.Shift, 361));
            Tabelas.tabelaAcao[555].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 354));
            Tabelas.tabelaAcao[555].Add("NAO", new ActionClass(ActionType.Shift, 357));
            Tabelas.tabelaAcao[555].Add("ID", new ActionClass(ActionType.Shift, 400));

            Tabelas.tabelaGOTO[555] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[555].Add("<ARGUMENTO_LOGICO>", 556);
            Tabelas.tabelaGOTO[555].Add("<LST_ARG_LOG_COND>", 350);
            Tabelas.tabelaGOTO[555].Add("<LST_ARG_LOG_VAR1>", 367);
            Tabelas.tabelaGOTO[555].Add("<ARG_LOG_VAR>", 362);
            Tabelas.tabelaGOTO[555].Add("<ARG_LOG_VAR_CONST>", 372);

            // <CMD_REP_REPITA> -> REPITA <LISTA_COMANDOS> ATE QUE <ARGUMENTO_LOGICO> . PONTO_VIRGULA
            // <ARGUMENTO_LOGICO> -> <ARGUMENTO_LOGICO> . <ARG_LOG_OP_LOG> <LST_ARG_LOG_COND>
            Tabelas.tabelaAcao[556] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[556].Add("E", new ActionClass(ActionType.Shift, 410));
            Tabelas.tabelaAcao[556].Add("OU", new ActionClass(ActionType.Shift, 410));
            Tabelas.tabelaAcao[556].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 557));

            Tabelas.tabelaGOTO[556] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[556].Add("<ARG_LOG_OP_LOG>", 352);

            // <CMD_REP_REPITA> -> REPITA <LISTA_COMANDOS> ATE QUE <ARGUMENTO_LOGICO> PONTO_VIRGULA .
            Tabelas.tabelaAcao[557] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[557].Add("LEIA", new ActionClass(ActionType.Reduce, r402));
            Tabelas.tabelaAcao[557].Add("ESCREVA", new ActionClass(ActionType.Reduce, r402));
            Tabelas.tabelaAcao[557].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r402));
            Tabelas.tabelaAcao[557].Add("ID", new ActionClass(ActionType.Reduce, r402));
            Tabelas.tabelaAcao[557].Add("SE", new ActionClass(ActionType.Reduce, r402));
            Tabelas.tabelaAcao[557].Add("SENAO", new ActionClass(ActionType.Reduce, r402));
            Tabelas.tabelaAcao[557].Add("FIM_SE", new ActionClass(ActionType.Reduce, r402));
            Tabelas.tabelaAcao[557].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r402));
            Tabelas.tabelaAcao[557].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r402));
            Tabelas.tabelaAcao[557].Add("REPITA", new ActionClass(ActionType.Reduce, r402));
            Tabelas.tabelaAcao[557].Add("ATE", new ActionClass(ActionType.Reduce, r402));
            Tabelas.tabelaAcao[557].Add("PARA", new ActionClass(ActionType.Reduce, r402));
            Tabelas.tabelaAcao[557].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r402));
            //Tabelas.tabelaAcao[557].Add("", new ActionClass(ActionType.Reduce, r402));
            //Tabelas.tabelaAcao[557].Add("", new ActionClass(ActionType.Reduce, r402));
            Tabelas.tabelaAcao[557].Add("FIM", new ActionClass(ActionType.Reduce, r402));

            #endregion

            #region Comando Laço de Repetição 'Para-Faça'

            // <LISTA_COMANDOS> -> <CMD_REP_PARA> .
            Tabelas.tabelaAcao[600] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[600].Add("LEIA", new ActionClass(ActionType.Reduce, r425));
            Tabelas.tabelaAcao[600].Add("ESCREVA", new ActionClass(ActionType.Reduce, r425));
            Tabelas.tabelaAcao[600].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r425));
            Tabelas.tabelaAcao[600].Add("ID", new ActionClass(ActionType.Reduce, r425));
            Tabelas.tabelaAcao[600].Add("SE", new ActionClass(ActionType.Reduce, r425));
            Tabelas.tabelaAcao[600].Add("SENAO", new ActionClass(ActionType.Reduce, r425));
            Tabelas.tabelaAcao[600].Add("FIM_SE", new ActionClass(ActionType.Reduce, r425));
            Tabelas.tabelaAcao[600].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r425));
            Tabelas.tabelaAcao[600].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r425));
            Tabelas.tabelaAcao[600].Add("REPITA", new ActionClass(ActionType.Reduce, r425));
            Tabelas.tabelaAcao[600].Add("ATE", new ActionClass(ActionType.Reduce, r425));
            Tabelas.tabelaAcao[600].Add("PARA", new ActionClass(ActionType.Reduce, r425));
            Tabelas.tabelaAcao[600].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r425));
            //Tabelas.tabelaAcao[600].Add("", new ActionClass(ActionType.Reduce, r425));
            //Tabelas.tabelaAcao[600].Add("", new ActionClass(ActionType.Reduce, r425));
            Tabelas.tabelaAcao[600].Add("RETORNE", new ActionClass(ActionType.Reduce, r425));
            Tabelas.tabelaAcao[600].Add("FIM", new ActionClass(ActionType.Reduce, r425));

            // <LISTA_COMANDOS> -> <LISTA_COMANDOS> <CMD_REP_PARA> .
            Tabelas.tabelaAcao[601] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[601].Add("LEIA", new ActionClass(ActionType.Reduce, r426));
            Tabelas.tabelaAcao[601].Add("ESCREVA", new ActionClass(ActionType.Reduce, r426));
            Tabelas.tabelaAcao[601].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r426));
            Tabelas.tabelaAcao[601].Add("ID", new ActionClass(ActionType.Reduce, r426));
            Tabelas.tabelaAcao[601].Add("SE", new ActionClass(ActionType.Reduce, r426));
            Tabelas.tabelaAcao[601].Add("SENAO", new ActionClass(ActionType.Reduce, r426));
            Tabelas.tabelaAcao[601].Add("FIM_SE", new ActionClass(ActionType.Reduce, r426));
            Tabelas.tabelaAcao[601].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r426));
            Tabelas.tabelaAcao[601].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r426));
            Tabelas.tabelaAcao[601].Add("REPITA", new ActionClass(ActionType.Reduce, r426));
            Tabelas.tabelaAcao[601].Add("ATE", new ActionClass(ActionType.Reduce, r426));
            Tabelas.tabelaAcao[601].Add("PARA", new ActionClass(ActionType.Reduce, r426));
            Tabelas.tabelaAcao[601].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r426));
            //Tabelas.tabelaAcao[601].Add("", new ActionClass(ActionType.Reduce, r426));
            //Tabelas.tabelaAcao[601].Add("", new ActionClass(ActionType.Reduce, r426));
            Tabelas.tabelaAcao[601].Add("RETORNE", new ActionClass(ActionType.Reduce, r426));
            Tabelas.tabelaAcao[601].Add("FIM", new ActionClass(ActionType.Reduce, r426));

            // <CMD_REP_PARA> -> PARA . ID ATRIBUICAO <LISTA_CMD_ATRIB_VAL> <CMD_REP_PARA_MODO> <LISTA_CMD_ATRIB_VAL> FACA <LISTA_COMANDOS> FIM_PARA PONTO_VIRGULA
            Tabelas.tabelaAcao[602] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[602].Add("ID", new ActionClass(ActionType.Shift, 603));

            // <CMD_REP_PARA> -> PARA ID . ATRIBUICAO <LISTA_CMD_ATRIB_VAL> <CMD_REP_PARA_MODO> <LISTA_CMD_ATRIB_VAL> FACA <LISTA_COMANDOS> FIM_PARA PONTO_VIRGULA
            Tabelas.tabelaAcao[603] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[603].Add("ATRIBUICAO", new ActionClass(ActionType.Shift, 604));

            // <CMD_REP_PARA> -> PARA ID ATRIBUICAO . <LISTA_CMD_ATRIB_VAL> <CMD_REP_PARA_MODO> <LISTA_CMD_ATRIB_VAL> FACA <LISTA_COMANDOS> FIM_PARA PONTO_VIRGULA
            Tabelas.tabelaAcao[604] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[604].Add("ID", new ActionClass(ActionType.Shift, 213));
            Tabelas.tabelaAcao[604].Add("CONST_INT", new ActionClass(ActionType.Shift, 222));
            Tabelas.tabelaAcao[604].Add("CONST_REAL", new ActionClass(ActionType.Shift, 222));
            Tabelas.tabelaAcao[604].Add("MENOS", new ActionClass(ActionType.Shift, 215));
            Tabelas.tabelaAcao[604].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 604));

            Tabelas.tabelaGOTO[604] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[604].Add("<LISTA_CMD_ATRIB_VAL>", 605);
            Tabelas.tabelaGOTO[604].Add("<CMD_ATRIB_VAL>", 210);
            Tabelas.tabelaGOTO[604].Add("<CMD_ATRIB_VET>", 230);
            Tabelas.tabelaGOTO[604].Add("<CMD_ATRIB_FUNC>", 220);
            Tabelas.tabelaGOTO[604].Add("<CMD_ATRIB_CONST>", 214);

            // <CMD_REP_PARA> -> PARA ID ATRIBUICAO <LISTA_CMD_ATRIB_VAL> . <CMD_REP_PARA_MODO> <LISTA_CMD_ATRIB_VAL> FACA <LISTA_COMANDOS> FIM_PARA PONTO_VIRGULA
            // <LISTA_CMD_ATRIB_VAL> -> <LISTA_CMD_ATRIB_VAL> . <CMD_ATRIB_OP> <CMD_ATRIB_VAL>
            Tabelas.tabelaAcao[605] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[605].Add("MENOS", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[605].Add("MAIS", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[605].Add("ASTERISTICO", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[605].Add("BARRA", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[605].Add("MOD", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[605].Add("DIV", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[605].Add("FECHA_PAR", new ActionClass(ActionType.Shift, 219));
            Tabelas.tabelaAcao[605].Add("ATE", new ActionClass(ActionType.Shift, 612));
            Tabelas.tabelaAcao[605].Add("DECRESCENTE", new ActionClass(ActionType.Shift, 613));

            Tabelas.tabelaGOTO[605] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[605].Add("<CMD_ATRIB_OP>", 211);
            Tabelas.tabelaGOTO[605].Add("<CMD_REP_PARA_MODO>", 606);

            // <CMD_REP_PARA> -> PARA ID ATRIBUICAO <LISTA_CMD_ATRIB_VAL> <CMD_REP_PARA_MODO> . <LISTA_CMD_ATRIB_VAL> FACA <LISTA_COMANDOS> FIM_PARA PONTO_VIRGULA
            Tabelas.tabelaAcao[606] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[606].Add("ID", new ActionClass(ActionType.Shift, 213));
            Tabelas.tabelaAcao[606].Add("CONST_INT", new ActionClass(ActionType.Shift, 222));
            Tabelas.tabelaAcao[606].Add("CONST_REAL", new ActionClass(ActionType.Shift, 222));
            Tabelas.tabelaAcao[606].Add("MENOS", new ActionClass(ActionType.Shift, 215));
            Tabelas.tabelaAcao[606].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 606));

            Tabelas.tabelaGOTO[606] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[606].Add("<LISTA_CMD_ATRIB_VAL>", 607);
            Tabelas.tabelaGOTO[606].Add("<CMD_ATRIB_VAL>", 210);
            Tabelas.tabelaGOTO[606].Add("<CMD_ATRIB_VET>", 230);
            Tabelas.tabelaGOTO[606].Add("<CMD_ATRIB_FUNC>", 220);
            Tabelas.tabelaGOTO[606].Add("<CMD_ATRIB_CONST>", 214);

            // <CMD_REP_PARA> -> PARA ID ATRIBUICAO <LISTA_CMD_ATRIB_VAL> <CMD_REP_PARA_MODO> <LISTA_CMD_ATRIB_VAL> . FACA <LISTA_COMANDOS> FIM_PARA PONTO_VIRGULA
            // <LISTA_CMD_ATRIB_VAL> -> <LISTA_CMD_ATRIB_VAL> . <CMD_ATRIB_OP> <CMD_ATRIB_VAL>
            Tabelas.tabelaAcao[607] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[607].Add("MENOS", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[607].Add("MAIS", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[607].Add("ASTERISTICO", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[607].Add("BARRA", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[607].Add("MOD", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[607].Add("DIV", new ActionClass(ActionType.Shift, 223));
            Tabelas.tabelaAcao[607].Add("FECHA_PAR", new ActionClass(ActionType.Shift, 219));
            Tabelas.tabelaAcao[607].Add("FACA", new ActionClass(ActionType.Shift, 608));

            Tabelas.tabelaGOTO[607] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[607].Add("<CMD_ATRIB_OP>", 211);

            // <CMD_REP_PARA> -> PARA ID ATRIBUICAO <LISTA_CMD_ATRIB_VAL> <CMD_REP_PARA_MODO> <LISTA_CMD_ATRIB_VAL> FACA . <LISTA_COMANDOS> FIM_PARA PONTO_VIRGULA
            Tabelas.tabelaAcao[608] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[608].Add("LEIA", new ActionClass(ActionType.Shift, 652));
            Tabelas.tabelaAcao[608].Add("ESCREVA", new ActionClass(ActionType.Shift, 702));
            Tabelas.tabelaAcao[608].Add("ESCREVALN", new ActionClass(ActionType.Shift, 709));
            Tabelas.tabelaAcao[608].Add("ID", new ActionClass(ActionType.Shift, 202));
            Tabelas.tabelaAcao[608].Add("SE", new ActionClass(ActionType.Shift, 502));
            Tabelas.tabelaAcao[608].Add("ENQUANTO", new ActionClass(ActionType.Shift, 532));
            Tabelas.tabelaAcao[608].Add("REPITA", new ActionClass(ActionType.Shift, 552));
            Tabelas.tabelaAcao[608].Add("PARA", new ActionClass(ActionType.Shift, 602));
            //Tabelas.tabelaAcao[608].Add("", new ActionClass(ActionType.Shift, ));
            //Tabelas.tabelaAcao[608].Add("", new ActionClass(ActionType.Shift, ));

            Tabelas.tabelaGOTO[608] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[608].Add("<LISTA_COMANDOS>", 609);
            Tabelas.tabelaGOTO[608].Add("<CMD_LEIA>", 650);
            Tabelas.tabelaGOTO[608].Add("<CMD_ESCREVA>", 700);
            Tabelas.tabelaGOTO[608].Add("<CMD_ESCREVALN>", 707);
            Tabelas.tabelaGOTO[608].Add("<CMD_ATRIBUICAO>", 200);
            Tabelas.tabelaGOTO[608].Add("<CMD_ATRIB_VAR_REC>", 203);
            Tabelas.tabelaGOTO[608].Add("<CMD_CHAMA_PROC>", 150);
            Tabelas.tabelaGOTO[608].Add("<CMD_SEL_IF>", 500);
            Tabelas.tabelaGOTO[608].Add("<CMD_REP_ENQUANTO>", 530);
            Tabelas.tabelaGOTO[608].Add("<CMD_REP_REPITA>", 550);
            Tabelas.tabelaGOTO[608].Add("<CMD_REP_PARA>", 600);
            //Tabelas.tabelaGOTO[608].Add("<>", );
            //Tabelas.tabelaGOTO[608].Add("<>", );

            // <CMD_REP_PARA> -> PARA ID ATRIBUICAO <LISTA_CMD_ATRIB_VAL> <CMD_REP_PARA_MODO> <LISTA_CMD_ATRIB_VAL> FACA <LISTA_COMANDOS> . FIM_PARA PONTO_VIRGULA
            Tabelas.tabelaAcao[609] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[609].Add("LEIA", new ActionClass(ActionType.Shift, 652));
            Tabelas.tabelaAcao[609].Add("ESCREVA", new ActionClass(ActionType.Shift, 702));
            Tabelas.tabelaAcao[609].Add("ESCREVALN", new ActionClass(ActionType.Shift, 709));
            Tabelas.tabelaAcao[609].Add("ID", new ActionClass(ActionType.Shift, 202));
            Tabelas.tabelaAcao[609].Add("SE", new ActionClass(ActionType.Shift, 502));
            Tabelas.tabelaAcao[609].Add("SENAO", new ActionClass(ActionType.Shift, 509));
            Tabelas.tabelaAcao[609].Add("ENQUANTO", new ActionClass(ActionType.Shift, 532));
            Tabelas.tabelaAcao[609].Add("FIM_ENQUANTO", new ActionClass(ActionType.Shift, 536));
            Tabelas.tabelaAcao[609].Add("REPITA", new ActionClass(ActionType.Shift, 552));
            Tabelas.tabelaAcao[609].Add("PARA", new ActionClass(ActionType.Shift, 602));
            Tabelas.tabelaAcao[609].Add("FIM_PARA", new ActionClass(ActionType.Shift, 610));
            //Tabelas.tabelaAcao[609].Add("", new ActionClass(ActionType.Shift, ));
            //Tabelas.tabelaAcao[609].Add("", new ActionClass(ActionType.Shift, ));

            Tabelas.tabelaGOTO[609] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[609].Add("<CMD_LEIA>", 651);
            Tabelas.tabelaGOTO[609].Add("<CMD_ESCREVA>", 701);
            Tabelas.tabelaGOTO[609].Add("<CMD_ESCREVALN>", 708);
            Tabelas.tabelaGOTO[609].Add("<CMD_ATRIBUICAO>", 201);
            Tabelas.tabelaGOTO[609].Add("<CMD_ATRIB_VAR_REC>", 203);
            Tabelas.tabelaGOTO[609].Add("<CMD_CHAMA_PROC>", 151);
            Tabelas.tabelaGOTO[609].Add("<CMD_SEL_IF>", 501);
            Tabelas.tabelaGOTO[609].Add("<CMD_SEL_IF_BLOCO_ELSE>", 506);
            Tabelas.tabelaGOTO[609].Add("<CMD_REP_ENQUANTO>", 531);
            Tabelas.tabelaGOTO[609].Add("<CMD_REP_REPITA>", 551);
            Tabelas.tabelaGOTO[609].Add("<CMD_REP_PARA>", 601);
            //Tabelas.tabelaGOTO[609].Add("<>", );
            //Tabelas.tabelaGOTO[609].Add("<>", );

            // <CMD_REP_PARA> -> PARA ID ATRIBUICAO <LISTA_CMD_ATRIB_VAL> <CMD_REP_PARA_MODO> <LISTA_CMD_ATRIB_VAL> FACA <LISTA_COMANDOS> FIM_PARA . PONTO_VIRGULA
            Tabelas.tabelaAcao[610] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[610].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 611));

            // <CMD_REP_PARA> -> PARA ID ATRIBUICAO <LISTA_CMD_ATRIB_VAL> <CMD_REP_PARA_MODO> <LISTA_CMD_ATRIB_VAL> FACA <LISTA_COMANDOS> FIM_PARA PONTO_VIRGULA .
            Tabelas.tabelaAcao[611] = new Dictionary<string, ActionClass>();
            //Tabelas.tabelaAcao[611].Add("", new ActionClass(ActionType.Shift, ));
            Tabelas.tabelaAcao[611] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[611].Add("LEIA", new ActionClass(ActionType.Reduce, r427));
            Tabelas.tabelaAcao[611].Add("ESCREVA", new ActionClass(ActionType.Reduce, r427));
            Tabelas.tabelaAcao[611].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r427));
            Tabelas.tabelaAcao[611].Add("ID", new ActionClass(ActionType.Reduce, r427));
            Tabelas.tabelaAcao[611].Add("SE", new ActionClass(ActionType.Reduce, r427));
            Tabelas.tabelaAcao[611].Add("SENAO", new ActionClass(ActionType.Reduce, r427));
            Tabelas.tabelaAcao[611].Add("FIM_SE", new ActionClass(ActionType.Reduce, r427));
            Tabelas.tabelaAcao[611].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r427));
            Tabelas.tabelaAcao[611].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r427));
            Tabelas.tabelaAcao[611].Add("REPITA", new ActionClass(ActionType.Reduce, r427));
            Tabelas.tabelaAcao[611].Add("ATE", new ActionClass(ActionType.Reduce, r427));
            Tabelas.tabelaAcao[611].Add("PARA", new ActionClass(ActionType.Reduce, r427));
            Tabelas.tabelaAcao[611].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r427));
            //Tabelas.tabelaAcao[611].Add("", new ActionClass(ActionType.Reduce, r427));
            //Tabelas.tabelaAcao[611].Add("", new ActionClass(ActionType.Reduce, r427));
            Tabelas.tabelaAcao[611].Add("RETORNE", new ActionClass(ActionType.Reduce, r427));
            Tabelas.tabelaAcao[611].Add("FIM", new ActionClass(ActionType.Reduce, r427));

            // <CMD_REP_PARA_MODO> -> ATE .
            Tabelas.tabelaAcao[612] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[612].Add("ID", new ActionClass(ActionType.Reduce, r428));
            Tabelas.tabelaAcao[612].Add("CONST_INT", new ActionClass(ActionType.Reduce, r428));
            Tabelas.tabelaAcao[612].Add("CONST_REAL", new ActionClass(ActionType.Reduce, r428));
            Tabelas.tabelaAcao[612].Add("MENOS", new ActionClass(ActionType.Reduce, r428));
            Tabelas.tabelaAcao[612].Add("ABRE_PAR", new ActionClass(ActionType.Reduce, r428));

            // <CMD_REP_PARA_MODO> -> DECRESCENTE . ATE
            Tabelas.tabelaAcao[613] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[613].Add("ATE", new ActionClass(ActionType.Shift, 614));

            //<CMD_REP_PARA_MODO> -> DECRESCENTE ATE .
            Tabelas.tabelaAcao[614] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[614].Add("ID", new ActionClass(ActionType.Reduce, r429));
            Tabelas.tabelaAcao[614].Add("CONST_INT", new ActionClass(ActionType.Reduce, r429));
            Tabelas.tabelaAcao[614].Add("CONST_REAL", new ActionClass(ActionType.Reduce, r429));
            Tabelas.tabelaAcao[614].Add("MENOS", new ActionClass(ActionType.Reduce, r429));
            Tabelas.tabelaAcao[614].Add("ABRE_PAR", new ActionClass(ActionType.Reduce, r429));

            #endregion

            #region Comando 'Leia'

            // <LISTA_COMANDOS> -> <CMD_LEIA> .
            Tabelas.tabelaAcao[650] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[650].Add("LEIA", new ActionClass(ActionType.Reduce, r500));
            Tabelas.tabelaAcao[650].Add("ESCREVA", new ActionClass(ActionType.Reduce, r500));
            Tabelas.tabelaAcao[650].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r500));
            Tabelas.tabelaAcao[650].Add("ID", new ActionClass(ActionType.Reduce, r500));
            Tabelas.tabelaAcao[650].Add("SE", new ActionClass(ActionType.Reduce, r500));
            Tabelas.tabelaAcao[650].Add("SENAO", new ActionClass(ActionType.Reduce, r500));
            Tabelas.tabelaAcao[650].Add("FIM_SE", new ActionClass(ActionType.Reduce, r500));
            Tabelas.tabelaAcao[650].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r500));
            Tabelas.tabelaAcao[650].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r500));
            Tabelas.tabelaAcao[650].Add("REPITA", new ActionClass(ActionType.Reduce, r500));
            Tabelas.tabelaAcao[650].Add("ATE", new ActionClass(ActionType.Reduce, r500));
            Tabelas.tabelaAcao[650].Add("PARA", new ActionClass(ActionType.Reduce, r500));
            Tabelas.tabelaAcao[650].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r500));
            //Tabelas.tabelaAcao[650].Add("", new ActionClass(ActionType.Reduce, r500));
            //Tabelas.tabelaAcao[650].Add("", new ActionClass(ActionType.Reduce, r500));
            Tabelas.tabelaAcao[650].Add("RETORNE", new ActionClass(ActionType.Reduce, r500));
            Tabelas.tabelaAcao[650].Add("FIM", new ActionClass(ActionType.Reduce, r500));

            // <LISTA_COMANDOS> -> <LISTA_COMANDOS> <CMD_LEIA> .
            Tabelas.tabelaAcao[651] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[651].Add("LEIA", new ActionClass(ActionType.Reduce, r501));
            Tabelas.tabelaAcao[651].Add("ESCREVA", new ActionClass(ActionType.Reduce, r501));
            Tabelas.tabelaAcao[651].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r501));
            Tabelas.tabelaAcao[651].Add("ID", new ActionClass(ActionType.Reduce, r501));
            Tabelas.tabelaAcao[651].Add("SE", new ActionClass(ActionType.Reduce, r501));
            Tabelas.tabelaAcao[651].Add("SENAO", new ActionClass(ActionType.Reduce, r501));
            Tabelas.tabelaAcao[651].Add("FIM_SE", new ActionClass(ActionType.Reduce, r501));
            Tabelas.tabelaAcao[651].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r501));
            Tabelas.tabelaAcao[651].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r501));
            Tabelas.tabelaAcao[651].Add("REPITA", new ActionClass(ActionType.Reduce, r501));
            Tabelas.tabelaAcao[651].Add("ATE", new ActionClass(ActionType.Reduce, r501));
            Tabelas.tabelaAcao[651].Add("PARA", new ActionClass(ActionType.Reduce, r501));
            Tabelas.tabelaAcao[651].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r501));
            //Tabelas.tabelaAcao[651].Add("", new ActionClass(ActionType.Reduce, r101));
            //Tabelas.tabelaAcao[651].Add("", new ActionClass(ActionType.Reduce, r101));
            Tabelas.tabelaAcao[651].Add("RETORNE", new ActionClass(ActionType.Reduce, r501));
            Tabelas.tabelaAcao[651].Add("FIM", new ActionClass(ActionType.Reduce, r501));

            // <CMD_LEIA> -> LEIA . ABRE_PAR <LISTA_CMD_LEIA_VAR> FECHA_PAR PONTO_VIRGULA
            Tabelas.tabelaAcao[652] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[652].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 653));

            // <CMD_LEIA> -> LEIA ABRE_PAR . <LISTA_CMD_LEIA_VAR> FECHA_PAR PONTO_VIRGULA
            Tabelas.tabelaAcao[653] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[653].Add("ID", new ActionClass(ActionType.Shift, 660));

            Tabelas.tabelaGOTO[653] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[653].Add("<LISTA_CMD_LEIA_VAR>", 654);
            Tabelas.tabelaGOTO[653].Add("<CMD_LEIA_VAR>", 657);

            // <CMD_LEIA> -> LEIA ABRE_PAR <LISTA_CMD_LEIA_VAR> . FECHA_PAR PONTO_VIRGULA
            Tabelas.tabelaAcao[654] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[654].Add("FECHA_PAR", new ActionClass(ActionType.Shift, 655));
            Tabelas.tabelaAcao[654].Add("VIRGULA", new ActionClass(ActionType.Shift, 658));

            Tabelas.tabelaGOTO[654] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[654].Add("<LISTA_CMD_LEIA_VAR>", 653);

            // <CMD_LEIA> -> LEIA ABRE_PAR <LISTA_CMD_LEIA_VAR> FECHA_PAR . PONTO_VIRGULA
            Tabelas.tabelaAcao[655] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[655].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 656));

            // <CMD_LEIA> -> LEIA ABRE_PAR <LISTA_CMD_LEIA_VAR> FECHA_PAR PONTO_VIRGULA .
            Tabelas.tabelaAcao[656] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[656].Add("LEIA", new ActionClass(ActionType.Reduce, r502));
            Tabelas.tabelaAcao[656].Add("ESCREVA", new ActionClass(ActionType.Reduce, r502));
            Tabelas.tabelaAcao[656].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r502));
            Tabelas.tabelaAcao[656].Add("ID", new ActionClass(ActionType.Reduce, r502));
            Tabelas.tabelaAcao[656].Add("SE", new ActionClass(ActionType.Reduce, r502));
            Tabelas.tabelaAcao[656].Add("SENAO", new ActionClass(ActionType.Reduce, r502));
            Tabelas.tabelaAcao[656].Add("FIM_SE", new ActionClass(ActionType.Reduce, r502));
            Tabelas.tabelaAcao[656].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r502));
            Tabelas.tabelaAcao[656].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r502));
            Tabelas.tabelaAcao[656].Add("REPITA", new ActionClass(ActionType.Reduce, r502));
            Tabelas.tabelaAcao[656].Add("ATE", new ActionClass(ActionType.Reduce, r502));
            Tabelas.tabelaAcao[656].Add("PARA", new ActionClass(ActionType.Reduce, r502));
            Tabelas.tabelaAcao[656].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r502));
            //Tabelas.tabelaAcao[656].Add("", new ActionClass(ActionType.Reduce, r102));
            //Tabelas.tabelaAcao[656].Add("", new ActionClass(ActionType.Reduce, r102));
            Tabelas.tabelaAcao[656].Add("RETORNE", new ActionClass(ActionType.Reduce, r502));
            Tabelas.tabelaAcao[656].Add("FIM", new ActionClass(ActionType.Reduce, r502));

            // <LISTA_CMD_LEIA_VAR> -> ID .
            Tabelas.tabelaAcao[657] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[657].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r503));
            Tabelas.tabelaAcao[657].Add("VIRGULA", new ActionClass(ActionType.Reduce, r503));

            // <LISTA_CMD_LEIA_VAR> -> <LISTA_CMD_LEIA_VAR> VIRGULA . <CMD_LEIA_VAR>
            Tabelas.tabelaAcao[658] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[658].Add("ID", new ActionClass(ActionType.Shift, 660));

            Tabelas.tabelaGOTO[658] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[658].Add("<CMD_LEIA_VAR>", 659);

            // <LISTA_CMD_LEIA_VAR> -> <LISTA_CMD_LEIA_VAR> VIRGULA <CMD_LEIA_VAR> .
            Tabelas.tabelaAcao[659] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[659].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r504));
            Tabelas.tabelaAcao[659].Add("VIRGULA", new ActionClass(ActionType.Reduce, r504));

            // <CMD_LEIA_VAR> -> ID .
            // <CMD_LEIA_VAR> -> ID . ABRE_COL <LST_ATRIB_VET_INDEX> FECHA_COL
            Tabelas.tabelaAcao[660] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[660].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r505));
            Tabelas.tabelaAcao[660].Add("VIRGULA", new ActionClass(ActionType.Reduce, r505));
            Tabelas.tabelaAcao[660].Add("ABRE_COL", new ActionClass(ActionType.Shift, 661));

            // <CMD_LEIA_VAR> -> ID ABRE_COL . <LST_ATRIB_VET_INDEX> FECHA_COL
            // <LST_ATRIB_VET_INDEX> -> . <LST_ATRIB_VET_INDEX> VIRGULA <ATRIB_VET_INDEX_VAL>
            // <LST_ATRIB_VET_INDEX> -> . <LST_ATRIB_VET_INDEX> <ATRIB_VET_INDEX_OP> <ATRIB_VET_INDEX_VAL>
            Tabelas.tabelaAcao[661] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[661].Add("ID", new ActionClass(ActionType.Shift, 239));
            Tabelas.tabelaAcao[661].Add("CONST_INT", new ActionClass(ActionType.Shift, 239));

            Tabelas.tabelaGOTO[661] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[661].Add("<LST_ATRIB_VET_INDEX>", 662);
            Tabelas.tabelaGOTO[661].Add("<ATRIB_VET_INDEX_VAL>", 234);
            
            // <CMD_LEIA_VAR> -> ID ABRE_COL <LST_ATRIB_VET_INDEX> . FECHA_COL
            Tabelas.tabelaAcao[662] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[662].Add("FECHA_COL", new ActionClass(ActionType.Shift, 663));
            Tabelas.tabelaAcao[662].Add("VIRGULA", new ActionClass(ActionType.Shift, 235));
            Tabelas.tabelaAcao[662].Add("MENOS", new ActionClass(ActionType.Shift, 240));
            Tabelas.tabelaAcao[662].Add("MAIS", new ActionClass(ActionType.Shift, 240));
            Tabelas.tabelaAcao[662].Add("ASTERISTICO", new ActionClass(ActionType.Shift, 240));
            Tabelas.tabelaAcao[662].Add("BARRA", new ActionClass(ActionType.Shift, 240));

            Tabelas.tabelaGOTO[662] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[662].Add("<ATRIB_VET_INDEX_OP>", 237);

            // <CMD_LEIA_VAR> -> ID ABRE_COL <LST_ATRIB_VET_INDEX> FECHA_COL .
            Tabelas.tabelaAcao[663] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[663].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r506));
            Tabelas.tabelaAcao[663].Add("VIRGULA", new ActionClass(ActionType.Reduce, r506));

            #endregion

            #region Comandos 'Escreva' e "Escrevaln'

            // <LISTA_COMANDOS> -> <CMD_ESCREVA> .
            Tabelas.tabelaAcao[700] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[700].Add("LEIA", new ActionClass(ActionType.Reduce, r525));
            Tabelas.tabelaAcao[700].Add("ESCREVA", new ActionClass(ActionType.Reduce, r525));
            Tabelas.tabelaAcao[700].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r525));
            Tabelas.tabelaAcao[700].Add("ID", new ActionClass(ActionType.Reduce, r525));
            Tabelas.tabelaAcao[700].Add("SE", new ActionClass(ActionType.Reduce, r525));
            Tabelas.tabelaAcao[700].Add("SENAO", new ActionClass(ActionType.Reduce, r525));
            Tabelas.tabelaAcao[700].Add("FIM_SE", new ActionClass(ActionType.Reduce, r525));
            Tabelas.tabelaAcao[700].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r525));
            Tabelas.tabelaAcao[700].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r525));
            Tabelas.tabelaAcao[700].Add("REPITA", new ActionClass(ActionType.Reduce, r525));
            Tabelas.tabelaAcao[700].Add("ATE", new ActionClass(ActionType.Reduce, r525));
            Tabelas.tabelaAcao[700].Add("PARA", new ActionClass(ActionType.Reduce, r525));
            Tabelas.tabelaAcao[700].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r525));
            //Tabelas.tabelaAcao[700].Add("", new ActionClass(ActionType.Reduce, r105));
            //Tabelas.tabelaAcao[700].Add("", new ActionClass(ActionType.Reduce, r105));
            Tabelas.tabelaAcao[700].Add("RETORNE", new ActionClass(ActionType.Reduce, r525));
            Tabelas.tabelaAcao[700].Add("FIM", new ActionClass(ActionType.Reduce, r525));

            // <LISTA_COMANDOS> -> <LISTA_COMANDOS> <CMD_ESCREVA> .
            Tabelas.tabelaAcao[701] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[701].Add("LEIA", new ActionClass(ActionType.Reduce, r526));
            Tabelas.tabelaAcao[701].Add("ESCREVA", new ActionClass(ActionType.Reduce, r526));
            Tabelas.tabelaAcao[701].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r526));
            Tabelas.tabelaAcao[701].Add("ID", new ActionClass(ActionType.Reduce, r526));
            Tabelas.tabelaAcao[701].Add("SE", new ActionClass(ActionType.Reduce, r526));
            Tabelas.tabelaAcao[701].Add("SENAO", new ActionClass(ActionType.Reduce, r526));
            Tabelas.tabelaAcao[701].Add("FIM_SE", new ActionClass(ActionType.Reduce, r526));
            Tabelas.tabelaAcao[701].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r526));
            Tabelas.tabelaAcao[701].Add("REPITA", new ActionClass(ActionType.Reduce, r526));
            Tabelas.tabelaAcao[701].Add("ATE", new ActionClass(ActionType.Reduce, r526));
            Tabelas.tabelaAcao[701].Add("PARA", new ActionClass(ActionType.Reduce, r526));
            Tabelas.tabelaAcao[701].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r526));
            //Tabelas.tabelaAcao[701].Add("", new ActionClass(ActionType.Reduce, r106));
            //Tabelas.tabelaAcao[701].Add("", new ActionClass(ActionType.Reduce, r106));
            Tabelas.tabelaAcao[701].Add("RETORNE", new ActionClass(ActionType.Reduce, r526));
            Tabelas.tabelaAcao[701].Add("FIM", new ActionClass(ActionType.Reduce, r526));

            // <CMD_ESCREVA> -> ESCREVA . ABRE_PAR <LISTA_IMP_CMD_ESCREVA> FECHA_PAR PONTO_VIRGULA
            Tabelas.tabelaAcao[702] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[702].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 703));

            // <CMD_ESCREVA> -> ESCREVA ABRE_PAR . <LISTA_IMP_CMD_ESCREVA> FECHA_PAR PONTO_VIRGULA
            Tabelas.tabelaAcao[703] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[703].Add("ID", new ActionClass(ActionType.Shift, 722));
            Tabelas.tabelaAcao[703].Add("CONST_INT", new ActionClass(ActionType.Shift, 723));
            Tabelas.tabelaAcao[703].Add("CONST_REAL", new ActionClass(ActionType.Shift, 723));
            Tabelas.tabelaAcao[703].Add("CONST_TEXTO", new ActionClass(ActionType.Shift, 714));

            Tabelas.tabelaGOTO[703] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[703].Add("<LISTA_IMP_CMD_ESCREVA>", 704);
            Tabelas.tabelaGOTO[703].Add("<LISTA_CMD_ESCREVA_VAR>", 719);
            Tabelas.tabelaGOTO[703].Add("<CMD_ESCREVA_VAR>", 718);

            // <CMD_ESCREVA> -> ESCREVA ABRE_PAR <LISTA_IMP_CMD_ESCREVA> . FECHA_PAR PONTO_VIRGULA
            // <LISTA_IMP_CMD_ESCREVA> -> <LISTA_IMP_CMD_ESCREVA> . VIRGULA <LISTA_CMD_ESCREVA_VAR>
            // <LISTA_IMP_CMD_ESCREVA> -> <LISTA_IMP_CMD_ESCREVA> . VIRGULA CONST_TEXTO
            Tabelas.tabelaAcao[704] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[704].Add("FECHA_PAR", new ActionClass(ActionType.Shift, 705));
            Tabelas.tabelaAcao[704].Add("VIRGULA", new ActionClass(ActionType.Shift, 715));

            // <CMD_ESCREVA> -> ESCREVA ABRE_PAR <LISTA_IMP_CMD_ESCREVA> FECHA_PAR . PONTO_VIRGULA
            Tabelas.tabelaAcao[705] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[705].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 706));

            // <CMD_ESCREVA> -> ESCREVA ABRE_PAR <LISTA_IMP_CMD_ESCREVA> FECHA_PAR PONTO_VIRGULA .
            Tabelas.tabelaAcao[706] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[706].Add("LEIA", new ActionClass(ActionType.Reduce, r527));
            Tabelas.tabelaAcao[706].Add("ESCREVA", new ActionClass(ActionType.Reduce, r527));
            Tabelas.tabelaAcao[706].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r527));
            Tabelas.tabelaAcao[706].Add("ID", new ActionClass(ActionType.Reduce, r527));
            Tabelas.tabelaAcao[706].Add("SE", new ActionClass(ActionType.Reduce, r527));
            Tabelas.tabelaAcao[706].Add("SENAO", new ActionClass(ActionType.Reduce, r527));
            Tabelas.tabelaAcao[706].Add("FIM_SE", new ActionClass(ActionType.Reduce, r527));
            Tabelas.tabelaAcao[706].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r527));
            Tabelas.tabelaAcao[706].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r527));
            Tabelas.tabelaAcao[706].Add("REPITA", new ActionClass(ActionType.Reduce, r527));
            Tabelas.tabelaAcao[706].Add("ATE", new ActionClass(ActionType.Reduce, r527));
            Tabelas.tabelaAcao[706].Add("PARA", new ActionClass(ActionType.Reduce, r527));
            Tabelas.tabelaAcao[706].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r527));
            //Tabelas.tabelaAcao[706].Add("", new ActionClass(ActionType.Reduce, r107));
            //Tabelas.tabelaAcao[706].Add("", new ActionClass(ActionType.Reduce, r107));
            Tabelas.tabelaAcao[706].Add("RETORNE", new ActionClass(ActionType.Reduce, r527));
            Tabelas.tabelaAcao[706].Add("FIM", new ActionClass(ActionType.Reduce, r527));

            // <LISTA_COMANDOS> -> <CMD_ESCREVALN> .
            Tabelas.tabelaAcao[707] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[707].Add("LEIA", new ActionClass(ActionType.Reduce, r528));
            Tabelas.tabelaAcao[707].Add("ESCREVA", new ActionClass(ActionType.Reduce, r528));
            Tabelas.tabelaAcao[707].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r528));
            Tabelas.tabelaAcao[707].Add("ID", new ActionClass(ActionType.Reduce, r528));
            Tabelas.tabelaAcao[707].Add("SE", new ActionClass(ActionType.Reduce, r528));
            Tabelas.tabelaAcao[707].Add("SENAO", new ActionClass(ActionType.Reduce, r528));
            Tabelas.tabelaAcao[707].Add("FIM_SE", new ActionClass(ActionType.Reduce, r528));
            Tabelas.tabelaAcao[707].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r528));
            Tabelas.tabelaAcao[707].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r528));
            Tabelas.tabelaAcao[707].Add("REPITA", new ActionClass(ActionType.Reduce, r528));
            Tabelas.tabelaAcao[707].Add("ATE", new ActionClass(ActionType.Reduce, r528));
            Tabelas.tabelaAcao[707].Add("PARA", new ActionClass(ActionType.Reduce, r528));
            Tabelas.tabelaAcao[707].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r528));
            //Tabelas.tabelaAcao[707].Add("", new ActionClass(ActionType.Reduce, r528));
            //Tabelas.tabelaAcao[707].Add("", new ActionClass(ActionType.Reduce, r528));
            Tabelas.tabelaAcao[707].Add("RETORNE", new ActionClass(ActionType.Reduce, r528));
            Tabelas.tabelaAcao[707].Add("FIM", new ActionClass(ActionType.Reduce, r528));

            // <LISTA_COMANDOS> -> <LISTA_COMANDOS> <CMD_ESCREVALN> .
            Tabelas.tabelaAcao[708] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[708].Add("LEIA", new ActionClass(ActionType.Reduce, r529));
            Tabelas.tabelaAcao[708].Add("ESCREVA", new ActionClass(ActionType.Reduce, r529));
            Tabelas.tabelaAcao[708].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r529));
            Tabelas.tabelaAcao[708].Add("ID", new ActionClass(ActionType.Reduce, r529));
            Tabelas.tabelaAcao[708].Add("SE", new ActionClass(ActionType.Reduce, r529));
            Tabelas.tabelaAcao[708].Add("SENAO", new ActionClass(ActionType.Reduce, r529));
            Tabelas.tabelaAcao[708].Add("FIM_SE", new ActionClass(ActionType.Reduce, r529));
            Tabelas.tabelaAcao[708].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r529));
            Tabelas.tabelaAcao[708].Add("REPITA", new ActionClass(ActionType.Reduce, r529));
            Tabelas.tabelaAcao[708].Add("ATE", new ActionClass(ActionType.Reduce, r529));
            Tabelas.tabelaAcao[708].Add("PARA", new ActionClass(ActionType.Reduce, r529));
            Tabelas.tabelaAcao[708].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r529));
            //Tabelas.tabelaAcao[708].Add("", new ActionClass(ActionType.Reduce, r529));
            //Tabelas.tabelaAcao[708].Add("", new ActionClass(ActionType.Reduce, r529));
            Tabelas.tabelaAcao[708].Add("RETORNE", new ActionClass(ActionType.Reduce, r529));
            Tabelas.tabelaAcao[708].Add("FIM", new ActionClass(ActionType.Reduce, r529));

            // <CMD_ESCREVALN> -> ESCREVALN . ABRE_PAR <LISTA_IMP_CMD_ESCREVA> FECHA_PAR PONTO_VIRGULA
            Tabelas.tabelaAcao[709] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[709].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 710));

            // <CMD_ESCREVALN> -> ESCREVALN ABRE_PAR . <LISTA_IMP_CMD_ESCREVA> FECHA_PAR PONTO_VIRGULA
            Tabelas.tabelaAcao[710] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[710].Add("ID", new ActionClass(ActionType.Shift, 722));
            Tabelas.tabelaAcao[710].Add("CONST_INT", new ActionClass(ActionType.Shift, 723));
            Tabelas.tabelaAcao[710].Add("CONST_REAL", new ActionClass(ActionType.Shift, 723));
            Tabelas.tabelaAcao[710].Add("CONST_TEXTO", new ActionClass(ActionType.Shift, 714));

            Tabelas.tabelaGOTO[710] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[710].Add("<LISTA_IMP_CMD_ESCREVA>", 711);
            Tabelas.tabelaGOTO[710].Add("<LISTA_CMD_ESCREVA_VAR>", 719);
            Tabelas.tabelaGOTO[710].Add("<CMD_ESCREVA_VAR>", 718);

            // <CMD_ESCREVALN> -> ESCREVALN ABRE_PAR <LISTA_IMP_CMD_ESCREVA> . FECHA_PAR PONTO_VIRGULA
            // <LISTA_IMP_CMD_ESCREVA> -> <LISTA_IMP_CMD_ESCREVA> . VIRGULA <LISTA_CMD_ESCREVA_VAR>
            // <LISTA_IMP_CMD_ESCREVA> -> <LISTA_IMP_CMD_ESCREVA> . VIRGULA CONST_TEXTO
            Tabelas.tabelaAcao[711] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[711].Add("FECHA_PAR", new ActionClass(ActionType.Shift, 712));
            Tabelas.tabelaAcao[711].Add("VIRGULA", new ActionClass(ActionType.Shift, 715));

            // <CMD_ESCREVALN> -> ESCREVALN ABRE_PAR <LISTA_IMP_CMD_ESCREVA> FECHA_PAR . PONTO_VIRGULA
            Tabelas.tabelaAcao[712] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[712].Add("PONTO_VIRGULA", new ActionClass(ActionType.Shift, 713));

            // <CMD_ESCREVALN> -> ESCREVALN ABRE_PAR <LISTA_IMP_CMD_ESCREVA> FECHA_PAR PONTO_VIRGULA .
            Tabelas.tabelaAcao[713] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[713].Add("LEIA", new ActionClass(ActionType.Reduce, r530));
            Tabelas.tabelaAcao[713].Add("ESCREVA", new ActionClass(ActionType.Reduce, r530));
            Tabelas.tabelaAcao[713].Add("ESCREVALN", new ActionClass(ActionType.Reduce, r530));
            Tabelas.tabelaAcao[713].Add("ID", new ActionClass(ActionType.Reduce, r530));
            Tabelas.tabelaAcao[713].Add("SE", new ActionClass(ActionType.Reduce, r530));
            Tabelas.tabelaAcao[713].Add("SENAO", new ActionClass(ActionType.Reduce, r530));
            Tabelas.tabelaAcao[713].Add("FIM_SE", new ActionClass(ActionType.Reduce, r530));
            Tabelas.tabelaAcao[713].Add("ENQUANTO", new ActionClass(ActionType.Reduce, r530));
            Tabelas.tabelaAcao[713].Add("FIM_ENQUANTO", new ActionClass(ActionType.Reduce, r530));
            Tabelas.tabelaAcao[713].Add("REPITA", new ActionClass(ActionType.Reduce, r530));
            Tabelas.tabelaAcao[713].Add("ATE", new ActionClass(ActionType.Reduce, r530));
            Tabelas.tabelaAcao[713].Add("PARA", new ActionClass(ActionType.Reduce, r530));
            Tabelas.tabelaAcao[713].Add("FIM_PARA", new ActionClass(ActionType.Reduce, r530));
            //Tabelas.tabelaAcao[713].Add("", new ActionClass(ActionType.Reduce, r107));
            //Tabelas.tabelaAcao[713].Add("", new ActionClass(ActionType.Reduce, r107));
            Tabelas.tabelaAcao[713].Add("RETORNE", new ActionClass(ActionType.Reduce, r530));
            Tabelas.tabelaAcao[713].Add("FIM", new ActionClass(ActionType.Reduce, r530));

            // <LISTA_IMP_CMD_ESCREVA> -> CONST_TEXTO .
            Tabelas.tabelaAcao[714] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[714].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r531));
            Tabelas.tabelaAcao[714].Add("VIRGULA", new ActionClass(ActionType.Reduce, r531));

            // <LISTA_IMP_CMD_ESCREVA> -> <LISTA_IMP_CMD_ESCREVA> VIRGULA . <LISTA_CMD_ESCREVA_VAR>
            // <LISTA_IMP_CMD_ESCREVA> -> <LISTA_IMP_CMD_ESCREVA> VIRGULA . CONST_TEXTO
            Tabelas.tabelaAcao[715] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[715].Add("ID", new ActionClass(ActionType.Shift, 722));
            Tabelas.tabelaAcao[715].Add("CONST_INT", new ActionClass(ActionType.Shift, 723));
            Tabelas.tabelaAcao[715].Add("CONST_REAL", new ActionClass(ActionType.Shift, 723));
            Tabelas.tabelaAcao[715].Add("CONST_TEXTO", new ActionClass(ActionType.Shift, 717));

            Tabelas.tabelaGOTO[715] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[715].Add("<LISTA_CMD_ESCREVA_VAR>", 716);
            Tabelas.tabelaGOTO[715].Add("<CMD_ESCREVA_VAR>", 718);

            // <LISTA_IMP_CMD_ESCREVA> -> <LISTA_IMP_CMD_ESCREVA> VIRGULA <LISTA_CMD_ESCREVA_VAR> .
            // <LISTA_CMD_ESCREVA_VAR> -> <LISTA_CMD_ESCREVA_VAR> . <CMD_ESCREVA_OP> <CMD_ESCREVA_VAR>
            Tabelas.tabelaAcao[716] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[716].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r534));
            Tabelas.tabelaAcao[716].Add("VIRGULA", new ActionClass(ActionType.Reduce, r534));
            Tabelas.tabelaAcao[716].Add("MENOS", new ActionClass(ActionType.Shift, 730));
            Tabelas.tabelaAcao[716].Add("MAIS", new ActionClass(ActionType.Shift, 730));
            Tabelas.tabelaAcao[716].Add("ASTERISTICO", new ActionClass(ActionType.Shift, 730));
            Tabelas.tabelaAcao[716].Add("BARRA", new ActionClass(ActionType.Shift, 730));

            Tabelas.tabelaGOTO[716] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[716].Add("<CMD_ESCREVA_OP>", 720);

            // <LISTA_IMP_CMD_ESCREVA> -> <LISTA_IMP_CMD_ESCREVA> VIRGULA CONST_TEXTO .
            Tabelas.tabelaAcao[717] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[717].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r533));
            Tabelas.tabelaAcao[717].Add("VIRGULA", new ActionClass(ActionType.Reduce, r533));

            // <LISTA_CMD_ESCREVA_VAR> -> <CMD_ESCREVA_VAR> .
            Tabelas.tabelaAcao[718] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[718].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r535));
            Tabelas.tabelaAcao[718].Add("VIRGULA", new ActionClass(ActionType.Reduce, r535));
            Tabelas.tabelaAcao[718].Add("MENOS", new ActionClass(ActionType.Reduce, r535));
            Tabelas.tabelaAcao[718].Add("MAIS", new ActionClass(ActionType.Reduce, r535));
            Tabelas.tabelaAcao[718].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r535));
            Tabelas.tabelaAcao[718].Add("BARRA", new ActionClass(ActionType.Reduce, r535));

            // <LISTA_IMP_CMD_ESCREVA> -> <LISTA_CMD_ESCREVA_VAR> .
            // <LISTA_CMD_ESCREVA_VAR> -> <LISTA_CMD_ESCREVA_VAR> . <CMD_ESCREVA_OP> <CMD_ESCREVA_VAR>
            Tabelas.tabelaAcao[719] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[719].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r532));
            Tabelas.tabelaAcao[719].Add("VIRGULA", new ActionClass(ActionType.Reduce, r532));
            Tabelas.tabelaAcao[719].Add("MENOS", new ActionClass(ActionType.Shift, 730));
            Tabelas.tabelaAcao[719].Add("MAIS", new ActionClass(ActionType.Shift, 730));
            Tabelas.tabelaAcao[719].Add("ASTERISTICO", new ActionClass(ActionType.Shift, 730));
            Tabelas.tabelaAcao[719].Add("BARRA", new ActionClass(ActionType.Shift, 730));

            Tabelas.tabelaGOTO[719] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[719].Add("<CMD_ESCREVA_OP>", 720);

            // <LISTA_CMD_ESCREVA_VAR> -> <LISTA_CMD_ESCREVA_VAR> <CMD_ESCREVA_OP> . <CMD_ESCREVA_VAR>
            Tabelas.tabelaAcao[720] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[720].Add("ID", new ActionClass(ActionType.Shift, 722));
            Tabelas.tabelaAcao[720].Add("CONST_INT", new ActionClass(ActionType.Shift, 723));
            Tabelas.tabelaAcao[720].Add("CONST_REAL", new ActionClass(ActionType.Shift, 723));

            Tabelas.tabelaGOTO[720] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[720].Add("<CMD_ESCREVA_VAR>", 721);

            // <LISTA_CMD_ESCREVA_VAR> -> <LISTA_CMD_ESCREVA_VAR> <CMD_ESCREVA_OP> <CMD_ESCREVA_VAR> .
            Tabelas.tabelaAcao[721] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[721].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r536));
            Tabelas.tabelaAcao[721].Add("VIRGULA", new ActionClass(ActionType.Reduce, r536));
            Tabelas.tabelaAcao[721].Add("MENOS", new ActionClass(ActionType.Reduce, r536));
            Tabelas.tabelaAcao[721].Add("MAIS", new ActionClass(ActionType.Reduce, r536));
            Tabelas.tabelaAcao[721].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r536));
            Tabelas.tabelaAcao[721].Add("BARRA", new ActionClass(ActionType.Reduce, r536));

            // <CMD_ESCREVA_VAR> -> ID .
            // <CMD_ESCREVA_VAR> -> ID . ABRE_COL <LST_ATRIB_VET_INDEX> FECHA_COL
            // <CMD_ESCREVA_VAR> -> ID . ABRE_PAR <LST_ATRIB_FUNC_PARAM> FECHA_PAR
            Tabelas.tabelaAcao[722] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[722].Add("ABRE_COL", new ActionClass(ActionType.Shift, 724));
            Tabelas.tabelaAcao[722].Add("ABRE_PAR", new ActionClass(ActionType.Shift, 727));
            Tabelas.tabelaAcao[722].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r537));
            Tabelas.tabelaAcao[722].Add("VIRGULA", new ActionClass(ActionType.Reduce, r537));
            Tabelas.tabelaAcao[722].Add("MENOS", new ActionClass(ActionType.Reduce, r537));
            Tabelas.tabelaAcao[722].Add("MAIS", new ActionClass(ActionType.Reduce, r537));
            Tabelas.tabelaAcao[722].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r537));
            Tabelas.tabelaAcao[722].Add("BARRA", new ActionClass(ActionType.Reduce, r537));
            
            // <CMD_ESCREVA_VAR> -> CONST_INT .
            // <CMD_ESCREVA_VAR> -> CONST_REAL .
            Tabelas.tabelaAcao[723] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[723].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r537));
            Tabelas.tabelaAcao[723].Add("VIRGULA", new ActionClass(ActionType.Reduce, r537));
            Tabelas.tabelaAcao[723].Add("MENOS", new ActionClass(ActionType.Reduce, r537));
            Tabelas.tabelaAcao[723].Add("MAIS", new ActionClass(ActionType.Reduce, r537));
            Tabelas.tabelaAcao[723].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r537));
            Tabelas.tabelaAcao[723].Add("BARRA", new ActionClass(ActionType.Reduce, r537));

            // <CMD_ESCREVA_VAR> -> ID ABRE_COL . <LST_ATRIB_VET_INDEX> FECHA_COL
            // <LST_ATRIB_VET_INDEX> -> . <LST_ATRIB_VET_INDEX> VIRGULA <ATRIB_VET_INDEX_VAL>
            // <LST_ATRIB_VET_INDEX> -> . <LST_ATRIB_VET_INDEX> <ATRIB_VET_INDEX_OP> <ATRIB_VET_INDEX_VAL>
            Tabelas.tabelaAcao[724] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[724].Add("ID", new ActionClass(ActionType.Shift, 239));
            Tabelas.tabelaAcao[724].Add("CONST_INT", new ActionClass(ActionType.Shift, 239));

            Tabelas.tabelaGOTO[724] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[724].Add("<LST_ATRIB_VET_INDEX>", 725);
            Tabelas.tabelaGOTO[724].Add("<ATRIB_VET_INDEX_VAL>", 234);

            // <CMD_ESCREVA_VAR> -> ID ABRE_COL <LST_ATRIB_VET_INDEX> . FECHA_COL
            // <LST_ATRIB_VET_INDEX> -> <LST_ATRIB_VET_INDEX> . VIRGULA <ATRIB_VET_INDEX_VAL>
            // <LST_ATRIB_VET_INDEX> -> <LST_ATRIB_VET_INDEX> . <ATRIB_VET_INDEX_OP> <ATRIB_VET_INDEX_VAL>
            Tabelas.tabelaAcao[725] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[725].Add("FECHA_COL", new ActionClass(ActionType.Shift, 726));
            Tabelas.tabelaAcao[725].Add("VIRGULA", new ActionClass(ActionType.Shift, 235));
            Tabelas.tabelaAcao[725].Add("MENOS", new ActionClass(ActionType.Shift, 240));
            Tabelas.tabelaAcao[725].Add("MAIS", new ActionClass(ActionType.Shift, 240));
            Tabelas.tabelaAcao[725].Add("ASTERISTICO", new ActionClass(ActionType.Shift, 240));
            Tabelas.tabelaAcao[725].Add("BARRA", new ActionClass(ActionType.Shift, 240));

            Tabelas.tabelaGOTO[725] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[725].Add("<ATRIB_VET_INDEX_OP>", 237);

            // <CMD_ESCREVA_VAR> -> ID ABRE_COL <LST_ATRIB_VET_INDEX> FECHA_COL .
            Tabelas.tabelaAcao[726] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[726].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r538));
            Tabelas.tabelaAcao[726].Add("VIRGULA", new ActionClass(ActionType.Reduce, r538));
            Tabelas.tabelaAcao[726].Add("MENOS", new ActionClass(ActionType.Reduce, r538));
            Tabelas.tabelaAcao[726].Add("MAIS", new ActionClass(ActionType.Reduce, r538));
            Tabelas.tabelaAcao[726].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r538));
            Tabelas.tabelaAcao[726].Add("BARRA", new ActionClass(ActionType.Reduce, r538));

            // <CMD_ESCREVA_VAR> -> ID ABRE_PAR . <LST_ATRIB_FUNC_PARAM> FECHA_PAR
            Tabelas.tabelaAcao[727] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[727].Add("ID", new ActionClass(ActionType.Shift, 227));
            Tabelas.tabelaAcao[727].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r147));

            Tabelas.tabelaGOTO[727] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[727].Add("<LST_ATRIB_FUNC_PARAM>", 728);

            // <CMD_ESCREVA_VAR> -> ID ABRE_PAR <LST_ATRIB_FUNC_PARAM> . FECHA_PAR
            Tabelas.tabelaAcao[728] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[728].Add("FECHA_PAR", new ActionClass(ActionType.Shift, 729));
            Tabelas.tabelaAcao[728].Add("VIRGULA", new ActionClass(ActionType.Shift, 228));

            Tabelas.tabelaGOTO[728] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[728].Add("<LST_ATRIB_FUNC_PARAM>", 224);

            // <CMD_ESCREVA_VAR> -> ID ABRE_PAR <LST_ATRIB_FUNC_PARAM> FECHA_PAR .
            Tabelas.tabelaAcao[729] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[729].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r539));
            Tabelas.tabelaAcao[729].Add("VIRGULA", new ActionClass(ActionType.Reduce, r539));
            Tabelas.tabelaAcao[729].Add("MENOS", new ActionClass(ActionType.Reduce, r539));
            Tabelas.tabelaAcao[729].Add("MAIS", new ActionClass(ActionType.Reduce, r539));
            Tabelas.tabelaAcao[729].Add("ASTERISTICO", new ActionClass(ActionType.Reduce, r539));
            Tabelas.tabelaAcao[729].Add("BARRA", new ActionClass(ActionType.Reduce, r539));

            // <CMD_ESCREVA_OP> -> MENOS .
            // <CMD_ESCREVA_OP> -> MAIS .
            // <CMD_ESCREVA_OP> -> ASTERISTICO .
            // <CMD_ESCREVA_OP> -> BARRA .
            Tabelas.tabelaAcao[730] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[730].Add("FECHA_PAR", new ActionClass(ActionType.Reduce, r540));
            Tabelas.tabelaAcao[730].Add("ID", new ActionClass(ActionType.Reduce, r540));
            Tabelas.tabelaAcao[730].Add("CONST_INT", new ActionClass(ActionType.Reduce, r540));
            Tabelas.tabelaAcao[730].Add("CONST_REAL", new ActionClass(ActionType.Reduce, r540));

            #endregion

            /*
            
            Tabelas.tabelaAcao[7] = new Dictionary<string, ActionClass>();
            Tabelas.tabelaAcao[7].Add("", new ActionClass(ActionType.Shift, ));
            
            Tabelas.tabelaGOTO[7] = new Dictionary<string, int>();
            Tabelas.tabelaGOTO[7].Add("<>", );

            */
        }
    }
}
