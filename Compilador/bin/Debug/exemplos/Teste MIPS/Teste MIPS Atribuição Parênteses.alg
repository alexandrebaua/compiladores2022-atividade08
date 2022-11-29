algoritmo "Teste MIPS Atribuição Parênteses"
variáveis
   x, y, z: inteiro;
   u, v, w: real;
   r, s, t: caracter;
   A: vetor[1..3] de inteiro;
   B: vetor[1..3, 1..4] de real;
/*
função produto(X, Y: inteiro): inteiro
variáveis
   P: inteiro;
inicio
   P <- X * Y;
   retorne P;
fim;

função produtoReal(X, Y: real): real
variáveis
   P: real;
inicio
   P <- X * Y;
   retorne P;
fim;
*/
// Rotina Principal
inicio
   x <- (123);
   u <- (147.69);
   //r <- "Olá mundo!";
   escreva("x=", x, " u=", u);

   y <- (x + 789) * 10;    // 9120
   v <- (369.87 - u) * 10; // 2221.8
   escreva("\ny<-(x+789)*10 = ", y, ", v<-(369.87-u)*10 = ", v);

   y <- x + (789 * 10);    // 8013
   v <- 369.87 - (u * 10); // -1107,03
   escreva("\ny<-x+(789*10) = ", y, ", v<-369.87-(u*10) = ", v);

   y <- -1 * (x + (-1 * (789 * 10) + 62 - (875 - (-1 * x) * 2)));
   escreva("\ny=", y);  // 8826
/*
   u <- 456;
   escreva("\nu=", u);

   u <- x;
   escreva("\nu<-x - u= ", u);

   u <- v;
   escreva("\nu<-v - u= ", u);

   A[1] <- x + 789 * 10;
   escreva("\nA[1]<-x+789 - A[1]=", A[1]);

   escreva("\nA[3]=", A[3]);
   A[3] <- A[1];
   escreva("\nA[3]<-A[1] - A[3]=", A[3]);
*/
   //y <- x mod 3;
   //y <- x div 3;
   //y <- A[x];
   //y <- A[1];
   //y <- A[1+x];
   //y <- A[x] div 3;
   //y <- A[x] mod 3;
   //y <- A[x] + produto(x,x) div produto(x,x);
   //y <- A[x] + produto(x,x) mod produto(x,x);

   //v <- B[1, 1];
   //v <- B[x+2, x*x];
   //v <- B[x, x] + produtoReal(u,u);
   //v <- B[x, x] + produtoReal(u,u) div produto(x,x);
   //v <- B[x, x] + produtoReal(u,u) mod produto(x,x);
fim.
