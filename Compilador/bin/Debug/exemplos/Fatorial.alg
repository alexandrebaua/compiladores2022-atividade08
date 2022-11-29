// Calcular o fatorial de um número inteiro positivo
algoritmo "Fatorial"
variáveis
   n, Cont, Fat: inteiro;
inicio
   n <- 0;
   repita
      escreva("Informe um número inteiro: ");
      leia(n);
   até que (n >= 0);  //Erro
   //até que n >= 0;

   Cont <- 1;
   Fat <- 1;
   repita
      Fat <- Fat * Cont;
      Cont <- Cont + 1;
   até que (Cont > n);  //Erro
   //até que Cont > n;

   escreva("O fatorial de ", n, "! é igual a ", Fat);
fim.
