/********************************************************************
 *  O algoritmo deverá ler o nome do aluno, calcular a frequência   *
 * com base no número de faltas informado sobre a carga horária     *
 * de 72 h/a. Verificar se o aluno estará aprovado ou reprovado     *
 * por média ou frequência.                                         *
 *                                                                  *
 * Resumo dos critérios:                                            *
 * média >= 7 _ sim -> freq >= 75% _ sim -> "aprovado"              *
 *           |                    |_ não -> "reprovado"             *
 *           |_ não -> entre 2,5 e 6,9 _ sim -> "em exame"          *
 *                                    |_ não -> "reprovado"         *
 *                                                                  *
 ********************************************************************/
algoritmo "media_freq"
variáveis
   aluno: caracter;
   n1, n2, n3, n4, media: real;
   faltas: inteiro;
   freq: real;
inicio
   escreva("Nome do aluno: ");
   leia(aluno);
   escreva("Notas dos 4 periodos: ");
   leia(n1, n2, n3, n4);
   media <- (n1 + n2 + n3 + n4) / 4;
   escreva("Faltas: ");
   leia(faltas);
   freq <- 100 - (faltas / 72 * 100);
   se freq < 75 então
      escreva("Reprovado por frequência insuficiente.");
   senão
      se media >= 7 então
         escreva("Aprovado com média: ", media);
      senão
         se media > 2.5 então
            escreva("Em exame");
         senão
            escreva("Média insuficiente, reprovado");
         fim_se;
      fim_se;
   fim_se;
fim.
