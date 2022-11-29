algoritmo "teste_validação_matriz"
variáveis
   x, y, z: inteiro;
   u, v, w: real;
   r, s, t: caracter;
   A: vetor[1..3] de inteiro;
   B: vetor[1..3, 1..4] de real;

// Rotina Principal
inicio
   x <- 123;
   u <- 321.0;
   r <- "Olá mundo!";

   //y <- A[A[1]];  // Erro vetor no indice
   //y <- A[1.0];   // Erro tipo
   y <- A[x, x];  // Erro quantidade argumentos
   y <- A[1, 1];  // Erro quantidade argumentos
   y <- A[z];     // Erro z não atribuido
   y <- A[u];     // Erro u tipo
   y <- B[x, x];  // Erro B tipo
   y <- A[x];
   y <- A[1];
   y <- A[1+x];

   //u <- B[1.0];   // Erro tipo
   u <- B[z];     // Erro z não atribuido
   u <- B[x];     // Erro quantidade argumentos
   u <- B[1];     // Erro quantidade argumentos
   u <- B[u];     // Erro u tipo
   u <- B[x, x];
   u <- B[1, 1];
   u <- B[x+2, x*x];

   A[1] <- x;
   A[1] <- z;     // Erro z não atribuido
   A[y] <- x;
   A[1 ,1] <- x;  // Erro quantidade argumentos
   A[y ,y] <- x;  // Erro quantidade argumentos
fim.
