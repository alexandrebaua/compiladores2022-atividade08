algoritmo "Teste MIPS Para-Faça"
variáveis
   k, i, a, b, c: inteiro;
   x, y, z: real;
   v: vetor[1..3] de inteiro;
   w: vetor[1..3] de real;

inicio
   para k <- 0 até 10 faça
      escreva(k, " ");
   fim_para;

   escreva("\n");

   para k <- 10 decresente até 0 faça
      escreva(k, " ");
   fim_para;

   para k <- 2 até 15 faça
      para i <- 1 até 10 faça
         escreva("\n", k, " * ", i, " = ", k*i);
      fim_para;
   fim_para;

   escreva("\nInforme um valor inicial: ");
   leia(a);
   para k <- a - 5 até a + 10 faça
      escreva(k, " ");
   fim_para;
fim.
