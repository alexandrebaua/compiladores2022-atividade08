/************************************************************
 *  Escrever um algoritmo para definir os elementos da      *
 * matriz 'A'3x4 sabando que:       |- 2 i-j, i<j           *
 *                            a ij -|  i², i=j              *
 *                                  |- j-i, i>j             *
 ************************************************************/
algoritmo "ex_define_matriz"
variáveis
   A: vetor[1..3, 1..4] de inteiro;
   i, j: inteiro;

// Rotina Principal
inicio
   para i <- 1 até 3 faça
      para j <- 1 até 4 faça
         se i<j então
            A[i,j] <- 2*i-j;
         senão
            se i=j então
               A[i,j] <- i*i;
            senão
               A[i,j] <- j-i;
            fim_se;
         fim_se;
      fim_para;
   fim_para;

   para i<-1 até 3 faça
      para j<-1 até 4 faça
         escreva("a", i, j, " = ", A[i,j], "\n");
      fim_para;
   fim_para;
fim.
