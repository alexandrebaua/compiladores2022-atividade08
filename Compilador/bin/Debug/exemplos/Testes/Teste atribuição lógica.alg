algoritmo "teste_validação_matriz"
variáveis
   l, m, n, o: lógico;
   x, y, z: inteiro;
   u, v, w: real;
   r, s, t: caracter;
   A: vetor[1..3] de lógico;
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

função status(X, Y: real): lógico
variáveis
   P: lógico;
inicio
   P <- X >= Y;
   retorne P;
fim;

// Rotina Principal
inicio
   x <- 123;
   u <- 321.0;
   r <- "Olá mundo!";
   l <- falso;
   m <- verdadeiro;

   //x <- l;      // Erro l tipo
   //x <- x ou u; // Erro operador lógico
   //x <- x > u;  // Erro operador relacional
   //x <- x > 5;  // Erro operador relacional
   //x <- 5 > x;  // Erro operador relacional

   //l <- x;     // Erro x tipo
   //l <- o;     // Erro não atribuido
   l <- m;
   l <- l ou m;
   l <- A[0];
   l <- B[0,0];       // Erro B tipo
   l <- B[0,0] ou m;  // Erro operação inválida
   l <- l ou B[0,0];  // Erro não gera resultado lógico
   l <- A[0] ou m;
   l <- l ou A[0];
   l <- produtoReal(u,u);       // Erro retorno tipo
   l <- produtoReal(u,u) ou m;  // Erro operador inválido
   l <- l ou produtoReal(u,u);  // Erro não gera resultado lógico
   l <- status(u,u) ou m;
   l <- l ou status(u,u);
   l <- x > u;
   l <- x > u ou falso;

   //y <- u;         // Erro u tipo
   //y <- z;         // Erro não atribuido
   //y <- y mod 3;   // Erro y não atribuido
   //y <- x mod 3;
   //y <- x div 3;
   //y <- A[x];
   //y <- A[1];
   //y <- A[1+x];
   //y <- A[x] div 3;
   //y <- A[x] mod 3;
   //y <- A[x] + produtoReal(x,x);  // Erro tipo retorno
   //y <- A[x] + produto(x,x) div produto(x,x);
   //y <- A[x] + produto(x,x) mod produto(x,x);

   //u <- x mod 3;     // Erro operador MOD
   //u <- x div 3;     // Erro operador DIV
   //u <- B[1, 1];
   //u <- B[x+2, x*x];
   //u <- B[x, x] + produtoReal(u,u);
   //u <- B[x, x] + produtoReal(u,u) div produto(x,x);
   //u <- B[x, x] + produtoReal(u,u) mod produto(x,x);
fim.
