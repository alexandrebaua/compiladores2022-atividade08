using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador.Lexico
{
    /// <summary>
    /// Tabelas de transições e estados finais do Autômato Finito Determinístico (AFD).
    /// </summary>
    public static class TabelaEstados
    {
        private static Dictionary<char, int>[] afd = null;
        private static Dictionary<int, string> finais = null;

        /// <summary>
        /// Tabela de transições do AFD.
        /// </summary>
        public static Dictionary<char, int>[] AFD
        {
            get
            {
                if (TabelaEstados.afd == null)
                    TabelaEstados.InitTablesAFD();

                return TabelaEstados.afd;
            }
        }

        /// <summary>
        /// Tabela de estados finais do AFD.
        /// </summary>
        public static Dictionary<int, string> Finais
        {
            get
            {
                if (TabelaEstados.finais == null)
                    TabelaEstados.InitTableFinais();

                return TabelaEstados.finais;
            }
        }


        /// <summary>
        /// Inicializa a tabela de transição.
        /// </summary>
        private static void InitTablesAFD()
        {
            TabelaEstados.afd = new Dictionary<char, int>[231];

            /*TabelaEstados.afd[0] = TabelaEstados.CriarTransições("[a-z][^r]", 1);
            TabelaEstados.afd[0] = TabelaEstados.CriarTransições("[a-z]^r", 2);
            TabelaEstados.afd[0] = TabelaEstados.CriarTransições("[a-z][^bcd]", 3);
            TabelaEstados.afd[0] = TabelaEstados.CriarTransições("[a-zA-Z][^bcd]", 4);
            TabelaEstados.afd[0] = TabelaEstados.CriarTransições("[À-ÿ][^Ç]", 5);
            TabelaEstados.afd[0] = TabelaEstados.CriarTransições("[0-9]", 5);
            TabelaEstados.afd[0] = TabelaEstados.CriarTransições("[a-zA-Z0-9]", 5);
            TabelaEstados.afd[0] = TabelaEstados.CriarTransições("[.]", 6);
            TabelaEstados.afd[0] = TabelaEstados.CriarTransições("[.]^ABC", 7);*/

            TabelaEstados.afd[0] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ_-_]^acdefilmnopqrsv", 1);
            TabelaEstados.afd[0].Add(' ', 2);
            TabelaEstados.afd[0].Add('"', 3);
            TabelaEstados.IncluirTransições(TabelaEstados.afd[0], "[0-9]", 5);

            TabelaEstados.afd[0].Add('+', 20);
            TabelaEstados.afd[0].Add('-', 21);
            TabelaEstados.afd[0].Add('*', 22);
            TabelaEstados.afd[0].Add('/', 23);
            TabelaEstados.afd[0].Add('>', 24);
            TabelaEstados.afd[0].Add('<', 26);
            TabelaEstados.afd[0].Add('=', 30);
            TabelaEstados.afd[0].Add('(', 31);
            TabelaEstados.afd[0].Add(')', 32);
            TabelaEstados.afd[0].Add('[', 33);
            TabelaEstados.afd[0].Add(']', 34);
            TabelaEstados.afd[0].Add(':', 35);
            TabelaEstados.afd[0].Add(';', 36);
            TabelaEstados.afd[0].Add(',', 37);
            TabelaEstados.afd[0].Add('.', 38);

            TabelaEstados.afd[0].Add('a', 61);
            TabelaEstados.afd[0].Add('c', 72);
            TabelaEstados.afd[0].Add('d', 82);
            TabelaEstados.afd[0].Add('e', 94);
            TabelaEstados.afd[0].Add('f', 113);
            TabelaEstados.afd[0].Add('i', 146);
            TabelaEstados.afd[0].Add('l', 157);
            TabelaEstados.afd[0].Add('m', 166);
            TabelaEstados.afd[0].Add('n', 169);
            TabelaEstados.afd[0].Add('o', 172);
            TabelaEstados.afd[0].Add('p', 174);
            TabelaEstados.afd[0].Add('q', 189);
            TabelaEstados.afd[0].Add('r', 192);
            TabelaEstados.afd[0].Add('s', 205);
            TabelaEstados.afd[0].Add('v', 210);

            TabelaEstados.afd[1] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[2] = null;

            TabelaEstados.afd[3] = TabelaEstados.CriarTransições("[.]^\"", 3);
            TabelaEstados.afd[3].Add('"', 4);

            TabelaEstados.afd[4] = null;

            TabelaEstados.afd[5] = TabelaEstados.CriarTransições("[0-9]", 5);
            TabelaEstados.afd[5].Add('.', 6);

            TabelaEstados.afd[6] = TabelaEstados.CriarTransições("[0-9]", 7);
            TabelaEstados.afd[6].Add('.', 8);

            TabelaEstados.afd[7] = TabelaEstados.CriarTransições("[0-9]", 7);

            TabelaEstados.afd[8] = TabelaEstados.CriarTransições("[0-9]", 9);

            TabelaEstados.afd[9] = TabelaEstados.CriarTransições("[0-9]", 9);
            
            TabelaEstados.afd[20] = null;
            TabelaEstados.afd[21] = null;
            TabelaEstados.afd[22] = null;

            TabelaEstados.afd[23] = new Dictionary<char, int>();
            TabelaEstados.afd[23].Add('/', 39);
            TabelaEstados.afd[23].Add('*', 40);

            TabelaEstados.afd[24] = new Dictionary<char, int>();
            TabelaEstados.afd[24].Add('=', 25);

            TabelaEstados.afd[25] = null;

            TabelaEstados.afd[26] = new Dictionary<char, int>();
            TabelaEstados.afd[26].Add('=', 27);
            TabelaEstados.afd[26].Add('>', 28);
            TabelaEstados.afd[26].Add('-', 29);

            TabelaEstados.afd[27] = null;
            TabelaEstados.afd[28] = null;
            TabelaEstados.afd[29] = null;
            TabelaEstados.afd[30] = null;
            TabelaEstados.afd[31] = null;
            TabelaEstados.afd[32] = null;
            TabelaEstados.afd[33] = null;
            TabelaEstados.afd[34] = null;
            TabelaEstados.afd[35] = null;
            TabelaEstados.afd[36] = null;
            TabelaEstados.afd[37] = null;
            TabelaEstados.afd[38] = null;

            TabelaEstados.afd[39] = TabelaEstados.CriarTransições("[.]", 39);

            TabelaEstados.afd[40] = TabelaEstados.CriarTransições("[.]^*", 40);
            TabelaEstados.afd[40].Add('*', 41);

            TabelaEstados.afd[41] = TabelaEstados.CriarTransições("[.]^*/", 40);
            TabelaEstados.afd[41].Add('*', 41);
            TabelaEstados.afd[41].Add('/', 42);

            TabelaEstados.afd[42] = null;


            TabelaEstados.afd[61] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^lt", 1);
            TabelaEstados.afd[61].Add('l', 62);
            TabelaEstados.afd[61].Add('t', 70);

            TabelaEstados.afd[62] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^g", 1);
            TabelaEstados.afd[62].Add('g', 63);

            TabelaEstados.afd[63] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^o", 1);
            TabelaEstados.afd[63].Add('o', 64);

            TabelaEstados.afd[64] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^r", 1);
            TabelaEstados.afd[64].Add('r', 65);

            TabelaEstados.afd[65] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^i", 1);
            TabelaEstados.afd[65].Add('i', 66);

            TabelaEstados.afd[66] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^t", 1);
            TabelaEstados.afd[66].Add('t', 67);

            TabelaEstados.afd[67] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^m", 1);
            TabelaEstados.afd[67].Add('m', 68);

            TabelaEstados.afd[68] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^o", 1);
            TabelaEstados.afd[68].Add('o', 69);

            TabelaEstados.afd[69] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[70] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^é", 1);
            TabelaEstados.afd[70].Add('é', 71);

            TabelaEstados.afd[71] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[72] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^a", 1);
            TabelaEstados.afd[72].Add('a', 73);

            TabelaEstados.afd[73] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^rs", 1);
            TabelaEstados.afd[73].Add('r', 74);
            TabelaEstados.afd[73].Add('s', 80);

            TabelaEstados.afd[74] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^a", 1);
            TabelaEstados.afd[74].Add('a', 75);

            TabelaEstados.afd[75] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^c", 1);
            TabelaEstados.afd[75].Add('c', 76);

            TabelaEstados.afd[76] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^t", 1);
            TabelaEstados.afd[76].Add('t', 77);

            TabelaEstados.afd[77] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^e", 1);
            TabelaEstados.afd[77].Add('e', 78);

            TabelaEstados.afd[78] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^r", 1);
            TabelaEstados.afd[78].Add('r', 79);

            TabelaEstados.afd[79] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[80] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^o", 1);
            TabelaEstados.afd[80].Add('o', 81);

            TabelaEstados.afd[81] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);


            TabelaEstados.afd[82] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^ei", 1);
            TabelaEstados.afd[82].Add('e', 83);
            TabelaEstados.afd[82].Add('i', 92);

            TabelaEstados.afd[83] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^c", 1);
            TabelaEstados.afd[83].Add('c', 84);

            TabelaEstados.afd[84] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^r", 1);
            TabelaEstados.afd[84].Add('r', 85);

            TabelaEstados.afd[85] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^e", 1);
            TabelaEstados.afd[85].Add('e', 86);

            TabelaEstados.afd[86] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^s", 1);
            TabelaEstados.afd[86].Add('s', 87);

            TabelaEstados.afd[87] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^e", 1);
            TabelaEstados.afd[87].Add('e', 88);

            TabelaEstados.afd[88] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^n", 1);
            TabelaEstados.afd[88].Add('n', 89);

            TabelaEstados.afd[89] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^t", 1);
            TabelaEstados.afd[89].Add('t', 90);

            TabelaEstados.afd[90] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^e", 1);
            TabelaEstados.afd[90].Add('e', 91);

            TabelaEstados.afd[91] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[92] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^v", 1);
            TabelaEstados.afd[92].Add('v', 93);

            TabelaEstados.afd[93] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[94] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^ns", 1);
            TabelaEstados.afd[94].Add('n', 95);
            TabelaEstados.afd[94].Add('s', 105);

            TabelaEstados.afd[95] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^qt", 1);
            TabelaEstados.afd[95].Add('q', 96);
            TabelaEstados.afd[95].Add('t', 102);

            TabelaEstados.afd[96] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^u", 1);
            TabelaEstados.afd[96].Add('u', 97);

            TabelaEstados.afd[97] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^a", 1);
            TabelaEstados.afd[97].Add('a', 98);

            TabelaEstados.afd[98] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^n", 1);
            TabelaEstados.afd[98].Add('n', 99);

            TabelaEstados.afd[99] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^t", 1);
            TabelaEstados.afd[99].Add('t', 100);

            TabelaEstados.afd[100] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^o", 1);
            TabelaEstados.afd[100].Add('o', 101);

            TabelaEstados.afd[101] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[102] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^ã", 1);
            TabelaEstados.afd[102].Add('ã', 103);

            TabelaEstados.afd[103] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^o", 1);
            TabelaEstados.afd[103].Add('o', 104);

            TabelaEstados.afd[104] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[105] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^c", 1);
            TabelaEstados.afd[105].Add('c', 106);

            TabelaEstados.afd[106] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^r", 1);
            TabelaEstados.afd[106].Add('r', 107);

            TabelaEstados.afd[107] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^e", 1);
            TabelaEstados.afd[107].Add('e', 108);

            TabelaEstados.afd[108] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^v", 1);
            TabelaEstados.afd[108].Add('v', 109);

            TabelaEstados.afd[109] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^a", 1);
            TabelaEstados.afd[109].Add('a', 110);

            TabelaEstados.afd[110] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^l", 1);
            TabelaEstados.afd[110].Add('l', 111);

            TabelaEstados.afd[111] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^n", 1);
            TabelaEstados.afd[111].Add('n', 112);

            TabelaEstados.afd[112] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);



            TabelaEstados.afd[113] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^aiu", 1);
            TabelaEstados.afd[113].Add('a', 114);
            TabelaEstados.afd[113].Add('i', 120);
            TabelaEstados.afd[113].Add('u', 141);

            TabelaEstados.afd[114] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^çl", 1);
            TabelaEstados.afd[114].Add('ç', 115);
            TabelaEstados.afd[114].Add('l', 117);

            TabelaEstados.afd[115] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^a", 1);
            TabelaEstados.afd[115].Add('a', 116);

            TabelaEstados.afd[116] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[117] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^s", 1);
            TabelaEstados.afd[117].Add('s', 118);

            TabelaEstados.afd[118] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^o", 1);
            TabelaEstados.afd[118].Add('o', 119);

            TabelaEstados.afd[119] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[120] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^m", 1);
            TabelaEstados.afd[120].Add('m', 121);

            TabelaEstados.afd[121] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^_", 1);
            TabelaEstados.afd[121].Add('_', 122);

            TabelaEstados.afd[122] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^efps", 1);
            TabelaEstados.afd[122].Add('e', 123);
            TabelaEstados.afd[122].Add('f', 131);
            TabelaEstados.afd[122].Add('p', 135);
            TabelaEstados.afd[122].Add('s', 139);

            TabelaEstados.afd[123] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^n", 1);
            TabelaEstados.afd[123].Add('n', 124);

            TabelaEstados.afd[124] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^q", 1);
            TabelaEstados.afd[124].Add('q', 125);

            TabelaEstados.afd[125] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^u", 1);
            TabelaEstados.afd[125].Add('u', 126);

            TabelaEstados.afd[126] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^a", 1);
            TabelaEstados.afd[126].Add('a', 127);

            TabelaEstados.afd[127] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^n", 1);
            TabelaEstados.afd[127].Add('n', 128);

            TabelaEstados.afd[128] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^t", 1);
            TabelaEstados.afd[128].Add('t', 129);

            TabelaEstados.afd[129] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^o", 1);
            TabelaEstados.afd[129].Add('o', 130);

            TabelaEstados.afd[130] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[131] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^a", 1);
            TabelaEstados.afd[131].Add('a', 132);

            TabelaEstados.afd[132] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^ç", 1);
            TabelaEstados.afd[132].Add('ç', 133);

            TabelaEstados.afd[133] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^a", 1);
            TabelaEstados.afd[133].Add('a', 134);

            TabelaEstados.afd[134] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[135] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^a", 1);
            TabelaEstados.afd[135].Add('a', 136);

            TabelaEstados.afd[136] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^r", 1);
            TabelaEstados.afd[136].Add('r', 137);

            TabelaEstados.afd[137] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^a", 1);
            TabelaEstados.afd[137].Add('a', 138);

            TabelaEstados.afd[138] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[139] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^e", 1);
            TabelaEstados.afd[139].Add('e', 140);

            TabelaEstados.afd[140] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[141] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^n", 1);
            TabelaEstados.afd[141].Add('n', 142);

            TabelaEstados.afd[142] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^ç", 1);
            TabelaEstados.afd[142].Add('ç', 143);

            TabelaEstados.afd[143] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^ã", 1);
            TabelaEstados.afd[143].Add('ã', 144);

            TabelaEstados.afd[144] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^o", 1);
            TabelaEstados.afd[144].Add('o', 145);

            TabelaEstados.afd[145] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);



            TabelaEstados.afd[146] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^n", 1);
            TabelaEstados.afd[146].Add('n', 147);

            TabelaEstados.afd[147] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^it", 1);
            TabelaEstados.afd[147].Add('i', 148);
            TabelaEstados.afd[147].Add('t', 152);

            TabelaEstados.afd[148] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^c", 1);
            TabelaEstados.afd[148].Add('c', 149);

            TabelaEstados.afd[149] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^i", 1);
            TabelaEstados.afd[149].Add('i', 150);

            TabelaEstados.afd[150] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^o", 1);
            TabelaEstados.afd[150].Add('o', 151);

            TabelaEstados.afd[151] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[152] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^e", 1);
            TabelaEstados.afd[152].Add('e', 153);

            TabelaEstados.afd[153] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^i", 1);
            TabelaEstados.afd[153].Add('i', 154);

            TabelaEstados.afd[154] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^r", 1);
            TabelaEstados.afd[154].Add('r', 155);

            TabelaEstados.afd[155] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^o", 1);
            TabelaEstados.afd[155].Add('o', 156);

            TabelaEstados.afd[156] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[157] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^eó", 1);
            TabelaEstados.afd[157].Add('e', 158);
            TabelaEstados.afd[157].Add('ó', 161);

            TabelaEstados.afd[158] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^i", 1);
            TabelaEstados.afd[158].Add('i', 159);

            TabelaEstados.afd[159] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^a", 1);
            TabelaEstados.afd[159].Add('a', 160);

            TabelaEstados.afd[160] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[161] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^g", 1);
            TabelaEstados.afd[161].Add('g', 162);

            TabelaEstados.afd[162] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^i", 1);
            TabelaEstados.afd[162].Add('i', 163);

            TabelaEstados.afd[163] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^c", 1);
            TabelaEstados.afd[163].Add('c', 164);

            TabelaEstados.afd[164] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^o", 1);
            TabelaEstados.afd[164].Add('o', 165);

            TabelaEstados.afd[165] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[166] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^o", 1);
            TabelaEstados.afd[166].Add('o', 167);

            TabelaEstados.afd[167] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^d", 1);
            TabelaEstados.afd[167].Add('d', 168);

            TabelaEstados.afd[168] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[169] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^ã", 1);
            TabelaEstados.afd[169].Add('ã', 170);

            TabelaEstados.afd[170] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^o", 1);
            TabelaEstados.afd[170].Add('o', 171);

            TabelaEstados.afd[171] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[172] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^u", 1);
            TabelaEstados.afd[172].Add('u', 173);

            TabelaEstados.afd[173] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[174] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^ar", 1);
            TabelaEstados.afd[174].Add('a', 175);
            TabelaEstados.afd[174].Add('r', 178);

            TabelaEstados.afd[175] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^r", 1);
            TabelaEstados.afd[175].Add('r', 176);

            TabelaEstados.afd[176] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^a", 1);
            TabelaEstados.afd[176].Add('a', 177);

            TabelaEstados.afd[177] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[178] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^o", 1);
            TabelaEstados.afd[178].Add('o', 179);

            TabelaEstados.afd[179] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^c", 1);
            TabelaEstados.afd[179].Add('c', 180);

            TabelaEstados.afd[180] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^e", 1);
            TabelaEstados.afd[180].Add('e', 181);

            TabelaEstados.afd[181] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^d", 1);
            TabelaEstados.afd[181].Add('d', 182);

            TabelaEstados.afd[182] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^i", 1);
            TabelaEstados.afd[182].Add('i', 183);

            TabelaEstados.afd[183] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^m", 1);
            TabelaEstados.afd[183].Add('m', 184);

            TabelaEstados.afd[184] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^e", 1);
            TabelaEstados.afd[184].Add('e', 185);

            TabelaEstados.afd[185] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^n", 1);
            TabelaEstados.afd[185].Add('n', 186);

            TabelaEstados.afd[186] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^t", 1);
            TabelaEstados.afd[186].Add('t', 187);

            TabelaEstados.afd[187] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^o", 1);
            TabelaEstados.afd[187].Add('o', 188);

            TabelaEstados.afd[188] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);



            TabelaEstados.afd[189] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^u", 1);
            TabelaEstados.afd[189].Add('u', 190);

            TabelaEstados.afd[190] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^e", 1);
            TabelaEstados.afd[190].Add('e', 191);

            TabelaEstados.afd[191] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[192] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^e", 1);
            TabelaEstados.afd[192].Add('e', 193);

            TabelaEstados.afd[193] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^apt", 1);
            TabelaEstados.afd[193].Add('a', 194);
            TabelaEstados.afd[193].Add('p', 196);
            TabelaEstados.afd[193].Add('t', 200);

            TabelaEstados.afd[194] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^l", 1);
            TabelaEstados.afd[194].Add('l', 195);

            TabelaEstados.afd[195] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[196] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^i", 1);
            TabelaEstados.afd[196].Add('i', 197);

            TabelaEstados.afd[197] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^t", 1);
            TabelaEstados.afd[197].Add('t', 198);

            TabelaEstados.afd[198] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^a", 1);
            TabelaEstados.afd[198].Add('a', 199);

            TabelaEstados.afd[199] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[200] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^o", 1);
            TabelaEstados.afd[200].Add('o', 201);

            TabelaEstados.afd[201] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^r", 1);
            TabelaEstados.afd[201].Add('r', 202);

            TabelaEstados.afd[202] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^n", 1);
            TabelaEstados.afd[202].Add('n', 203);

            TabelaEstados.afd[203] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^e", 1);
            TabelaEstados.afd[203].Add('e', 204);

            TabelaEstados.afd[204] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[205] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^e", 1);
            TabelaEstados.afd[205].Add('e', 206);

            TabelaEstados.afd[206] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^n", 1);
            TabelaEstados.afd[206].Add('n', 207);

            TabelaEstados.afd[207] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^ã", 1);
            TabelaEstados.afd[207].Add('ã', 208);

            TabelaEstados.afd[208] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^o", 1);
            TabelaEstados.afd[208].Add('o', 209);

            TabelaEstados.afd[209] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[210] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^ae", 1);
            TabelaEstados.afd[210].Add('a', 211);
            TabelaEstados.afd[210].Add('e', 219);

            TabelaEstados.afd[211] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^r", 1);
            TabelaEstados.afd[211].Add('r', 212);

            TabelaEstados.afd[212] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^i", 1);
            TabelaEstados.afd[212].Add('i', 213);

            TabelaEstados.afd[213] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^á", 1);
            TabelaEstados.afd[213].Add('á', 214);

            TabelaEstados.afd[214] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^v", 1);
            TabelaEstados.afd[214].Add('v', 215);

            TabelaEstados.afd[215] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^e", 1);
            TabelaEstados.afd[215].Add('e', 216);

            TabelaEstados.afd[216] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^i", 1);
            TabelaEstados.afd[216].Add('i', 217);

            TabelaEstados.afd[217] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^s", 1);
            TabelaEstados.afd[217].Add('s', 218);

            TabelaEstados.afd[218] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[219] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^rt", 1);
            TabelaEstados.afd[219].Add('r', 220);
            TabelaEstados.afd[219].Add('t', 228);

            TabelaEstados.afd[220] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^d", 1);
            TabelaEstados.afd[220].Add('d', 221);

            TabelaEstados.afd[221] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^a", 1);
            TabelaEstados.afd[221].Add('a', 222);

            TabelaEstados.afd[222] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^d", 1);
            TabelaEstados.afd[222].Add('d', 223);

            TabelaEstados.afd[223] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^e", 1);
            TabelaEstados.afd[223].Add('e', 224);

            TabelaEstados.afd[224] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^i", 1);
            TabelaEstados.afd[224].Add('i', 225);

            TabelaEstados.afd[225] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^r", 1);
            TabelaEstados.afd[225].Add('r', 226);

            TabelaEstados.afd[226] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^o", 1);
            TabelaEstados.afd[226].Add('o', 227);

            TabelaEstados.afd[227] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);

            TabelaEstados.afd[228] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^o", 1);
            TabelaEstados.afd[228].Add('o', 229);

            TabelaEstados.afd[229] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]^r", 1);
            TabelaEstados.afd[229].Add('r', 230);

            TabelaEstados.afd[230] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9_-_]", 1);


            /*
            TabelaEstados.afd[1] = TabelaEstados.CriarTransições("[a-zA-ZÀ-ÿ0-9]^", 1);
            TabelaEstados.afd[1].Add('', 1);*/
        }

        /// <summary>
        /// Inicializa a tabela de estados finais.
        /// </summary>
        private static void InitTableFinais()
        {
            TabelaEstados.finais = new Dictionary<int, string>();
            TabelaEstados.finais.Add(1, "ID");
            //TabelaEstados.finais.Add(2, "ESPACO");
            TabelaEstados.finais.Add(2, null);
            TabelaEstados.finais.Add(4, "CONST_TEXTO");
            TabelaEstados.finais.Add(5, "CONST_INT");
            TabelaEstados.finais.Add(7, "CONST_REAL");
            TabelaEstados.finais.Add(9, "CONST_FAIXA_VETOR");

            TabelaEstados.finais.Add(20, "MAIS");
            TabelaEstados.finais.Add(21, "MENOS");
            TabelaEstados.finais.Add(22, "ASTERISTICO");
            TabelaEstados.finais.Add(23, "BARRA");
            TabelaEstados.finais.Add(24, "MAIOR");
            TabelaEstados.finais.Add(25, "MAIOR_IGUAL");
            TabelaEstados.finais.Add(26, "MENOR");
            TabelaEstados.finais.Add(27, "MENOR_IGUAL");
            TabelaEstados.finais.Add(28, "DIFERENTE");
            TabelaEstados.finais.Add(29, "ATRIBUICAO");
            TabelaEstados.finais.Add(30, "IGUAL");
            TabelaEstados.finais.Add(31, "ABRE_PAR");
            TabelaEstados.finais.Add(32, "FECHA_PAR");
            TabelaEstados.finais.Add(33, "ABRE_COL");
            TabelaEstados.finais.Add(34, "FECHA_COL");
            TabelaEstados.finais.Add(35, "DOIS_PONTO");
            TabelaEstados.finais.Add(36, "PONTO_VIRGULA");
            TabelaEstados.finais.Add(37, "VIRGULA");
            TabelaEstados.finais.Add(38, "PONTO");

            TabelaEstados.finais.Add(39, null);  // Comentário de Linha
            TabelaEstados.finais.Add(42, null);  // Comentário de Múltiplas Linhas
            /*TabelaEstados.finais.Add(39, "Comentário Linha");
            TabelaEstados.finais.Add(42, "Comentário Múltiplas Linhas");*/

            TabelaEstados.IncluirFinais(61, 68, "ID");
            TabelaEstados.finais.Add(69, "ALGORITMO");
            TabelaEstados.finais.Add(70, "ID");
            TabelaEstados.finais.Add(71, "ATE");
            TabelaEstados.IncluirFinais(72, 78, "ID");
            TabelaEstados.finais.Add(79, "CARACTER");
            TabelaEstados.finais.Add(80, "ID");
            TabelaEstados.finais.Add(81, "CASO");

            TabelaEstados.finais.Add(82, "ID");
            TabelaEstados.finais.Add(83, "DE");
            TabelaEstados.IncluirFinais(84, 90, "ID");
            TabelaEstados.finais.Add(91, "DECRESCENTE");
            TabelaEstados.finais.Add(92, "ID");
            TabelaEstados.finais.Add(93, "DIV");
            TabelaEstados.finais.Add(94, "E");
            TabelaEstados.IncluirFinais(95, 100, "ID");
            TabelaEstados.finais.Add(101, "ENQUANTO");
            TabelaEstados.finais.Add(102, "ID");
            TabelaEstados.finais.Add(103, "ID");
            TabelaEstados.finais.Add(104, "ENTAO");
            TabelaEstados.IncluirFinais(105, 109, "ID");
            TabelaEstados.finais.Add(110, "ESCREVA");
            TabelaEstados.finais.Add(111, "ID");
            TabelaEstados.finais.Add(112, "ESCREVALN");

            TabelaEstados.IncluirFinais(113, 115, "ID");
            TabelaEstados.finais.Add(116, "FACA");
            TabelaEstados.finais.Add(117, "ID");
            TabelaEstados.finais.Add(118, "ID");
            TabelaEstados.finais.Add(119, "CONST_LOGICA");
            TabelaEstados.finais.Add(120, "ID");
            TabelaEstados.finais.Add(121, "FIM");
            TabelaEstados.IncluirFinais(122, 129, "ID");
            TabelaEstados.finais.Add(130, "FIM_ENQUANTO");
            TabelaEstados.IncluirFinais(131, 133, "ID");
            TabelaEstados.finais.Add(134, "FIM_FACA");
            TabelaEstados.IncluirFinais(135, 137, "ID");
            TabelaEstados.finais.Add(138, "FIM_PARA");
            TabelaEstados.finais.Add(139, "ID");
            TabelaEstados.finais.Add(140, "FIM_SE");
            TabelaEstados.IncluirFinais(141, 144, "ID");
            TabelaEstados.finais.Add(145, "FUNCAO");
            TabelaEstados.IncluirFinais(146, 150, "ID");
            TabelaEstados.finais.Add(151, "INICIO");
            TabelaEstados.IncluirFinais(152, 155, "ID");
            TabelaEstados.finais.Add(156, "INTEIRO");
            TabelaEstados.IncluirFinais(157, 159, "ID");
            TabelaEstados.finais.Add(160, "LEIA");
            TabelaEstados.IncluirFinais(161, 164, "ID");
            TabelaEstados.finais.Add(165, "LOGICO");
            TabelaEstados.finais.Add(166, "ID");
            TabelaEstados.finais.Add(167, "ID");
            TabelaEstados.finais.Add(168, "MOD");
            TabelaEstados.finais.Add(169, "ID");
            TabelaEstados.finais.Add(170, "ID");
            TabelaEstados.finais.Add(171, "NAO");
            TabelaEstados.finais.Add(172, "ID");
            TabelaEstados.finais.Add(173, "OU");
            TabelaEstados.IncluirFinais(174, 176, "ID");
            TabelaEstados.finais.Add(177, "PARA");
            TabelaEstados.IncluirFinais(178, 187, "ID");
            TabelaEstados.finais.Add(188, "PROCEDIMENTO");
            
            TabelaEstados.finais.Add(189, "ID");
            TabelaEstados.finais.Add(190, "ID");
            TabelaEstados.finais.Add(191, "QUE");
            TabelaEstados.IncluirFinais(192, 194, "ID");
            TabelaEstados.finais.Add(195, "REAL");
            TabelaEstados.IncluirFinais(196, 198, "ID");
            TabelaEstados.finais.Add(199, "REPITA");
            TabelaEstados.IncluirFinais(200, 203, "ID");
            TabelaEstados.finais.Add(204, "RETORNE");
            TabelaEstados.finais.Add(205, "ID");
            TabelaEstados.finais.Add(206, "SE");
            TabelaEstados.finais.Add(207, "ID");
            TabelaEstados.finais.Add(208, "ID");
            TabelaEstados.finais.Add(209, "SENAO");
            TabelaEstados.finais.Add(210, "ID");
            TabelaEstados.finais.Add(211, "ID");
            TabelaEstados.finais.Add(212, "VAR");
            TabelaEstados.IncluirFinais(213, 217, "ID");
            TabelaEstados.finais.Add(218, "VARIAVEIS");
            TabelaEstados.IncluirFinais(219, 226, "ID");
            TabelaEstados.finais.Add(227, "CONST_LOGICA");
            TabelaEstados.finais.Add(228, "ID");
            TabelaEstados.finais.Add(229, "ID");
            TabelaEstados.finais.Add(230, "VETOR");
        }

        /// <summary>
        /// Cria uma lista de transições para uma transição definida, respeitando as regras informadas.
        /// Formatos:
        /// [inicio-fim{+n}]^simbolo, simbolos contidos na(s) faixa(s), exceto o simbolo. Exemplo: [a-z]^x ou [a-zA-Z]^x
        /// [inicio-fim{+n}][^simbolos], simbolos contidos na(s) faixa(s), exceto os simbolos agrupados. Exemplo: [a-z][^bcd] ou [a-zA-Z][^bcd]
        /// [inicio-fim{+n}], exemplo: [a-z] ou [a-zA-Z], todos os simbolos contidos na(s) faixa(s). Exemplo: [a-z] ou [a-zA-Z]
        /// </summary>
        /// <param name="conjunto">Os conjuntos de simbolos associados à transição.</param>
        /// <param name="transicao">A transição associada aos simbolos.</param>
        /// <returns>Uma lista de transições resultante dos parâmetros passados.</returns>
        private static Dictionary<char, int> CriarTransições(string conjunto, int transicao)
        {
            Dictionary<char, int> transicoes = new Dictionary<char, int>();
            TabelaEstados.IncluirTransições(transicoes, conjunto, transicao);
            return transicoes;
        }

        /// <summary>
        /// Inclui novas transições em uma lista de transição já existente, para uma transição definida, respeitando as regras informadas.
        /// </summary>
        /// <param name="listaTransicoes">A lista de transição à incluir as novas transições geradas.</param>
        /// <param name="conjunto">Os conjuntos de simbolos associados à transição a ser incluida.</param>
        /// <param name="transicao">A transição associada aos simbolos a serem incluidos.</param>
        private static void IncluirTransições(Dictionary<char, int> listaTransicoes, string conjunto, int transicao)
        {
            // Verifica se foi informado um conjunto, senão, informa erro:
            if (!conjunto.StartsWith("[") || !conjunto.Contains("]"))
                throw new Exception($"Conjunto informado não foi reconhecido: {conjunto}");

            // Cria um vetor com os conjuntos de faixas:
            string[] conjuntos = conjunto.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

            // Cria variável para armazenar os simbolos que não serão adicionados à lista de transições.
            char[] exclusos = null;

            // Testa se foi informado um conjunto de faixas de simbolos à adicionar, e um simbolo ou comjunto de simbolos que serão ignorados:
            if (conjuntos.Length == 2)
            {
                if (!conjuntos[1].StartsWith("^"))
                    throw new Exception($"Conjunto informado não foi reconhecido: {conjunto}");

                // Remove a negação no início do conjunto.
                conjuntos[1] = conjuntos[1].Remove(0, 1);

                if (String.IsNullOrEmpty(conjuntos[1]))
                    throw new Exception("Simbolos negados não pode ser vazio!");

                // Cria o vetor de simbolos que não serão adicionados à lista de transições.
                exclusos = conjuntos[1].ToCharArray();
            }
            else if (conjuntos.Length != 1)
            {
                throw new Exception($"Conjunto informado não foi reconhecido: {conjunto}");
            }

            // Se foi informado que deve aceitar qualquer caracter, então:
            if (conjuntos[0].Equals("."))
            {
                // Cria uma transição con todos caracteres da tabela ASCII, exceto caracteres que não deverão ser adicionados à lista de transições.
                for (char i = Convert.ToChar(0); i < Convert.ToChar(255); i++)
                {
                    if (exclusos == null || !exclusos.Contains(i))
                        listaTransicoes.Add(i, transicao);
                }

                return;
            }

            // Testa se foi informado corretamente um conjunto de faixas de simbolos à adicionar (agrupamentos de 3 caracteres):
            if (conjuntos[0].Length < 3 || (conjuntos[0].Length % 3 != 0))
                throw new Exception($"Conjunto informado de faixas não foi reconhecido: {conjunto}");

            // Cria um vetor com os caracteres dos conjuntos de faixas de simbolos à adicionar:
            char[] ranges = conjuntos[0].ToCharArray();

            int r = 0;  // Cotador para o caracter inicial da faixa
            while (r < ranges.Length)
            {
                // Verifica se foi informado o separador de faixa, senão, informa erro:
                if (ranges[r + 1] != '-')
                    throw new Exception($"Conjunto informado de faixas não foi reconhecido: {conjunto}");

                // Percorre a faixa de caracteres atual, adicionando à lista de transição:
                for (char i = ranges[r]; i <= ranges[r + 2]; i++)
                {
                    if (exclusos == null || !exclusos.Contains(i))
                        listaTransicoes.Add(i, transicao);
                }

                // Avança para a próxima faixa a ser adicionada.
                r += 3;
            }
        }

        /// <summary>
        /// Inclui novos estados finais na lista de finais existente, conforme valores de inicio e fim da faixa de inclusão, e o valor do estado final incluido.
        /// </summary>
        /// <param name="inicio">O valor inicial da faixa de inclusão.</param>
        /// <param name="fim">O valor final da faixa de inclusão.</param>
        /// <param name="final">O valor para os estados incluidos na faixa espacificada.</param>
        private static void IncluirFinais(int inicio, int fim, string final)
        {
            for (int i = inicio; i <= fim; i++)
                TabelaEstados.finais.Add(i, final);
        }
    }
}
