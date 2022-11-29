algoritmo "teste MIPS enquanto-faça"
variáveis
   i, x, y: inteiro;
   z: real;
   nome: caracter;
   V1: vetor[1..10] de inteiro;
   V2: vetor[1..10, 2..20] de real;
   V3: vetor[1..10, 2..20, 3..30] de caracter;

// principal
inicio
   i <- 0;
   enquanto i < 10 faça
      //escreva(".");
      escreva(i, " ");
      i <- i + 1;
   fim_enquanto;
   escreva("Ok");

   escreva("\nLaço com número real:\n");
   z <- 0.0;
   enquanto z < 1.0 faça
      //escreva(".");
      escreva(z, " ");
      z <- z + 0.1;
   fim_enquanto;
   escreva("Ok");
fim.
