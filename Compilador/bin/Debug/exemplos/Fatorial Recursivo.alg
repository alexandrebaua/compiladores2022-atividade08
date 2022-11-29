// Calcular o fatorial de um número inteiro positivo
algoritmo "Fatorial Recursivo"

função fatorial(num: inteiro): inteiro
variáveis
   num1: inteiro;
inicio
   se num < 0 então
      num <- -1;
   senão
      se num = 0 então
         num <- 1;
      senão
         num1 <- num - 1;
         num <- num * fatorial(num1);
         //num <- fatorial(num1) * num;
      fim_se;
   fim_se;

   retorne num;
fim;

variáveis
   n, Fat: inteiro;
inicio
   n <- 0;
   repita
      escreva("Informe um número inteiro: ");
      leia(n);
   //até que (n >= 0);  //Erro
   até que n >= 0;

   Fat <- fatorial(n);
   escreva("O fatorial de ", n, "! é igual a ", Fat);
fim.
