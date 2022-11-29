algoritmo "Teste Atribuição Valores Negativos"
variáveis
   x, y, z: inteiro;
   u, v, w: real;/*
   A: vetor[1..3] de inteiro;
   B: vetor[1..3, 1..4] de real;

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
   x <- -123;
   y <- 2;
   u <- -321.0;
   v <- 2.0;

   z <- x * -1;
   z <- (x * 2 ) - -2;
   z <- x * (-3 - 2);
   z <- x mod -4;
   z <- x div -5;
   z <- A[x] * -6;
   z <- -7 + A[1];
   z <- -8 / produto(x,x);
   z <- produto(x,x) - -9;

   w <- u * -1.0;
   w <- (u * 2.0 ) - -2.0;
   w <- u * (-3.0 - 2.0);
   w <- B[x] * -4.0;
   w <- -5.0 + B[1];
   w <- -6.0 / produtoReal(x,x);
   w <- produtoReal(x,x) - -7.0;

   w <- u * -10;
   w <- (u * 2.0 ) - -20;
   w <- u * (-30 - 2.0);
   w <- B[x] * -40;
   w <- -50 + B[1];
   w <- -60 / produtoReal(x,x);
   w <- produtoReal(x,x) - -70;
fim.
