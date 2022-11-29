algoritmo "Teste MIPS Atribuição Lógica"
variáveis
   n, contador: inteiro;
   digitouNegativo: lógico;
   a, b, c: lógico;
   x, y, z, k, i: inteiro;
   u, v, w: real;
   A: vetor[1..3] de lógico;
   B: vetor[1..10] de lógico;
   C: vetor[1..2, 1..4] de lógico;
   D: vetor[1..2, 1..4, 1..5] de lógico;

inicio
   x <- 1;
   y <- 2;
   z <- 3;
   u <- 0.5;

   a <- falso;
   b <- verdadeiro ou a;
   c <- x < y;
   c <- x + x * y < y;
   c <- x < y ou z < y;
   c <- x < y ou z < y ou y < z;

   c <- x < u;

   A[1] <- verdadeiro;
   B[9] <- verdadeiro;
   C[1, 2] <- verdadeiro;
   D[1, 2, 3] <- verdadeiro;

   a <- A[1];

   A[1] <- falso;
   B[9] <- falso;
   C[1, 2] <- falso;
   D[1, 2, 3] <- falso;

   para k <- 1 até 10 faça
      B[k] <- verdadeiro;
   fim_para;
   escreva(B, "\n");

   para k <- 1 até 10 faça
      B[k] <- falso;
   fim_para;
   escreva(B, "\n");

   escreva(A[1+1]);
   escreva(B[x+y]);

   escreva(A[1]);
   escreva(B[x]);
   escreva("\n");

   leia(A[1]);
   escreva(">", A[1], "\n");

   leia(B[x]);
   escreva(">", B[x], "\n");

   para i <- 1 até 3 faça
      escreva("Escreva o ", i, "º número: ");
      leia(A[i]);
   fim_para;
   escreva(A, "\n");

   leia(A);
   escreva(A, "\n");

   leia(a);
   escreva(a);
fim.
