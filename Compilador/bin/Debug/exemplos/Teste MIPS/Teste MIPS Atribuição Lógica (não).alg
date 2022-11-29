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
   escrevaln("a = ", a);
   a <- não(a);
   escrevaln("não(a) = ", a);

   b <- x + x * y < y;
   escrevaln("x+x*y < y = ", b);
   b <- não(x + x * y < y);
   escrevaln("não(x+x*y < y) = ", b);/*
   b <- verdadeiro ou a;
   b <- x < y;
   b <- x + x * y < y;
   b <- x < y ou z < y;*/
   b <- x < y ou z < y ou y < z;
   escrevaln(b);
/*
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
*/
   para k <- 1 até 10 faça
      B[k] <- verdadeiro;
   fim_para;
   escrevaln(B);

   para k <- 1 até 10 faça
      B[k] <- falso;
   fim_para;
   escrevaln(B);

   para k <- 1 até 10 faça
      se k mod 2 = 0 então
         B[k] <- verdadeiro;
      fim_se;
   fim_para;
   escrevaln(B);

   escreva(A[1+1]);
   escreva(" ");
   escreva(B[x+y]);
   escreva(" ");

   escreva(A[1]);
   escreva(" ");
   escrevaln(B[x]);

   escreva("Informe A[1]: ");
   leia(A[1]);
   escrevaln(">", A[1]);

   escreva("Informe B[x]: ");
   leia(B[x]);
   escrevaln(">", B[x]);

   para i <- 1 até 3 faça
      escreva("Escreva o ", i, "º número: ");
      leia(A[i]);
   fim_para;
   escrevaln(A);

   escreva("Informe A[]:\n");
   leia(A);
   escrevaln(A);

   escreva("Informe a: ");
   leia(a);
   escreva(a);
fim.
