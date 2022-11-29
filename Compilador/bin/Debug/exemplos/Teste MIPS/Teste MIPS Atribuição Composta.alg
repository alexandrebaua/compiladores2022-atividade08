algoritmo "Teste MIPS Atribuição Composta"
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

   y <- x + 789 * 10;    // 8013
   v <- 369.87 - u * 10; // -1107.03
   escrevaln("y<-x+789*10 = ", y, ", v<-369.87-u*10 = ", v);

   u <- 456;
   escrevaln("u<-456 - u=", u);

   u <- x;
   escrevaln("u<-x - u= ", u);

   u <- v;
   escrevaln("u<-v - u= ", u);

   A[1] <- x + 789 * 10; // 8013
   escrevaln("A[1]<-x+789*10 - A[1]=", A[1]);

   escrevaln("A[3]=", A[3]);
   A[3] <- A[1];
   escreva("A[3]<-A[1] - A[3]=", A[3]);

fim.
