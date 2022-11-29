// Calcular o fatorial de um número inteiro positivo
algoritmo "Fatorial Interativo"

função fatorial(num: inteiro): inteiro
variáveis
   resultado: inteiro;
inicio
   se num = 0 então
      num <- 1;
   fim_se;

   resultado <- num;
   enquanto num > 1 faça
      num <- num - 1;
      resultado <- resultado * num;
   fim_enquanto;

   retorne resultado;
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
