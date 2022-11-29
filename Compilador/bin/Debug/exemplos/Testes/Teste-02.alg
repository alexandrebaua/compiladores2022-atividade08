algoritmo "teste"
variáveis
   x, y: inteiro;


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

   enquanto (soma < 100) e (impar <5) faça
      se(produto(x) > 2.5) então
         escreva("Ok");
      senão
         se(produto(x) > 2.5) então
            escreva("Ok");
         senão
            leia(x);
            enquanto (soma < 100) e (impar <5) faça
               leia(x);
            fim_enquanto;
         fim_se;
         leia(x);
      fim_se;
   fim_enquanto;

fim.
