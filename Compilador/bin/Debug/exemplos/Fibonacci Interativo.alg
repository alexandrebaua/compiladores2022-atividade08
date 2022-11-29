algoritmo "Sequência de Fibonacci - Interativo"

função fibo(n: inteiro): inteiro
variáveis
   F, ant, i: inteiro;
inicio
   F <- 0;
   ant <- 0;

   para i <- 1 até n faça
      se i = 1 então
         F <- 1;
         ant <- 0;
      senão
         F <- F + ant;
         ant <- F - ant;
      fim_se;
   fim_para;

   retorne F;
fim;

variáveis
   i: inteiro;
inicio
   // Imprime os 30 primeiros termos:
   para i <- 0 até 30 faça
      escrevaln("(", i, "):", fibo(i));
   fim_para;
fim.
