algoritmo "Teste MIPS Para-Faça"
variáveis
   k, i, a, b, c: inteiro;
   x, y, z: real;
   A: vetor[0..9] de inteiro;
   w: vetor[1..3] de real;

inicio
   escreva("Digite 10 números:\n");

   para i <- 1 até 10 faça
      escreva("Escreva o ", i, "º número: ");
      leia(A[i-1]);
   fim_para;

   escreva("\n");
   escreva(A, " ");
fim.
