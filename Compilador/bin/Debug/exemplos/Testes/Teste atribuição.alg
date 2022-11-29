algoritmo "teste_validação_matriz"
variáveis
   x, y, z: inteiro;
   u, v, w: real;
   r, s, t: caracter;
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

// Rotina Principal
inicio
   x <- 123;
   u <- 321.0;
   r <- "Olá mundo!";

   y <- u;         // Erro u tipo
   y <- z;         // Erro não atribuido
   y <- z mod 3;   // Erro z não atribuido
   y <- x mod 3;
   y <- x div 3;
   //y <- A[x];
   //y <- A[1];
   //y <- A[1+x];
   y <- A[x] div 3;
   y <- A[x] mod 3;
   y <- A[x] + produtoReal(x,x);  // Erro tipo retorno
   y <- A[x] + produto(x,x) div produto(x,x);
   y <- A[x] + produto(x,x) mod produto(x,x);

   v <- x mod 3;     // Erro operador MOD
   v <- x div 3;     // Erro operador DIV
   v <- u mod 3;     // Erro operador MOD
   v <- u div 3;     // Erro operador DIV
   //v <- B[1, 1];
   //v <- B[x+2, x*x];
   v <- B[x, x] + produtoReal(u,u);
   //v <- B[x, x] + produtoReal(u,u) div produto(x,x);
   //v <- B[x, x] + produtoReal(u,u) mod produto(x,x);
fim.
