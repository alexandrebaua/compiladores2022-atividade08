algoritmo "teste"
variáveis
   x, y: inteiro;
   z: real;
   nome: caracter;
   V1: vetor[1..10] de inteiro;
   V2: vetor[1..10, 2..20] de real;
   V3: vetor[1..10, 2..20, 3..30] de caracter;

procedimento trocar(var a, b: inteiro)
variáveis
   aux: inteiro;
inicio
   aux <- a;
   a <- b;
   b <- aux;
fim;

função produto(X, Y: inteiro): inteiro
variáveis
   P: inteiro;
inicio
   P <- X * Y;
   retorne P;
fim;

// principal
inicio

   repita
      se(produto(x) > 2.5) então
         repita
            escreva("Ok");
            repita
               escreva("Ok");
            até que (soma < 100) e (impar <5);
         até que (soma < 100) e (impar <5);
      senão
         repita
            escreva("Baixo");
            repita
               leia(x);
            até que (soma < 100) e (impar <5);
         até que (soma < 100) e (impar <5);
      fim_se;
      repita
         a <- 0;
      até que (soma < 100) e (impar <5);
   até que (soma < 100) e (impar <5);

fim.
