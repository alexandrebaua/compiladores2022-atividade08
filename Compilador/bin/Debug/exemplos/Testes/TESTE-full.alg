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
   escreva("Informe X e Y: ");
   leia(x);
   leia(x, y, z);
   escreva("Trocando...");
   trocar(x, y);
   escreva("Agora X = ", x);
   escreva("e Y = ", y);
   escreva("X + Y = ", x + y);
   escreva(x + y, "X + Y = ");
   aux <- a;
   a <- b;
   b <- aux;
   aux1 <- a - f1(x);
   aux2 <- f2(x);
   aux3 <- f3(x, y, z);
   aux4 <- f(x) - ((f(y) * f(z)) / f(r));
   p1(x);
   p2(x, y);
   a <- b + v[1];
   b <- v[2];
   c <- v[3 + id * 2] + ((v[67] / v[id] - 6 * v[id * 3]) * -1);
   v1[1] <- a;
   v2[2 + i] <- b;
   v2[i + 3] <- c;
   v3[1, z, y] <- d;

   z <- id > 2 * x;
   z <- id - j > 2 * x;
   z <- id - j * -1 > 2 * x;
   z <- (id - j * -1) > (2 * x);
   z <- (id - j * -1) > (1+(2 * x)+1);
   z <- id - j * -1 > 2 * x / -55;

   t <- "texto 1";
   t <- id + "texto";
   x <- falso;
   y <- 0 = 0;

   enquanto (soma < 100) e (impar < 5) faça
      se(produto(x) > 2.5) então
         enquanto (soma < 100) e (impar <5) faça
            escreva("Ok");
            enquanto (soma < 100) e (impar <5) faça
               escreva("Ok");
            fim_enquanto;
         fim_enquanto;
      senão
         enquanto (soma < 100) e (impar <5) faça
            escreva("Baixo");
            enquanto (soma < 100) e (impar <5) faça
               leia(x);
            fim_enquanto;
         fim_enquanto;
      fim_se;

      enquanto (soma < 100) e (impar <5) faça
         a <- 0;
      fim_enquanto;
   fim_enquanto;

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
