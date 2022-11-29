algoritmo "Teste MIPS Atribuição Simples"
variáveis
   x, y, z: inteiro;
   u, v, w: real;
   r, s, t: caracter;
   A: vetor[1..3] de inteiro;
   B: vetor[1..3, 1..4] de real;

// Rotina Principal
inicio
   x <- 123;
   u <- 147.69;
   escrevaln("x=", x, " u=", u);

   y <- 789;
   v <- 369.87;
   escrevaln("y=", y, " v=", v);

   u <- 456;
   escrevaln("u=", u);

   u <- x;
   escrevaln("u<-x - u= ", u);

   u <- v;
   escrevaln("u<-v - u= ", u);

   A[1] <- 789;
   escrevaln("A[1]<-789 - A[1]=", A[1]);

   escrevaln("A[3]=", A[3]);
   A[3] <- A[1];
   escrevaln("A[3]<-A[1] - A[3]=", A[3]);

   escreva("Informe x: ");
   leia(x);

   y <- x mod 3;
   escrevaln("x mod 3 = ", y);

   y <- x div 3;
   escrevaln("x div 3 = ", y);

fim.
