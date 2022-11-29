algoritmo "Teste Atribuição Valores Negativos"
variáveis
   x, y, z: inteiro;
   u, v, w: real;
   a, b, c: lógico;
   A: vetor[1..3] de inteiro;
   B: vetor[1..3] de real;

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

// Rotina Principal
inicio
   x <- 7;
   u <- 2.0;
   A[1] <- 3;
   B[1] <- 4.0;
   a <- verdadeiro;

   y <- -123;
   v <- -321.4;
   escreva("y = ", y, ", v = ", v, "\n");

   A[2] <- -369;
   B[2] <- -456.78;
   escreva("A[2] = ", A[2], ", B[2] = ", B[2], "\n");

   escreva("x = ", x, "\n");
   //y <- x * -1;
   y <- (x * 2) - -2;  // 16
   //y <- x * (-3 - 2);  // -35
/*   y <- x mod -4;
   y <- x div -5;
   y <- -8 - produto(x,x);
   y <- produto(x,x) - -9;
   escreva("A[1] = ", A[1], "\n");
   y <- A[1] * -6;
   y <- -7 + A[1];*/
   escreva("y = ", y, "\n");
/*
   escreva("u = ", u, "\n");
   v <- u * -1.0;
   //v <- (u * 2.0 ) - -2.0;  // Parenteses erro
   //v <- u * (-3.0 - 2.0);
   v <- -6.0 / produtoReal(u,u);
   v <- produtoReal(u,u) - -7.0;*/
   escreva("B[1] = ", B[1], "\n");
   v <- B[1] * -4.0;
   v <- -5.0 + B[1];
   escreva("v = ", v, "\n");
/*
   v <- u * -10;
   //v <- (u * 2.0 ) - -20;  // Parenteses erro
   //v <- u * (-30 - 2.0);
   v <- -60 / produtoReal(u,u);
   v <- produtoReal(u,u) - -70;*/
   escreva("B[1] = ", B[1], "\n");
   v <- B[1] * -40;
   v <- -50 + B[1];
   escreva("v = ", v, "\n");

   escreva("Lógicos\n");
   b <- 10 > -40;
   b <- 10 < -40;
   b <- 10 < -40 ou a;
   escreva("b = ", b, "\n");
   b <- -50 < x;
   b <- -50 > x;
   b <- -50 > x ou a;
   escreva("b = ", b, "\n");
fim.
