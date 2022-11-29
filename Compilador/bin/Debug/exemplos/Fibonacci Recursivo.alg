algoritmo "Sequência de Fibonacci - Recursivo"

função fibo(n: inteiro): inteiro
variáveis
   n1, n2: inteiro;
inicio
   se n >= 2 então
      n1 <- n - 1;
      n2 <- n - 2;
      n <- fibo(n1) + fibo(n2);
   fim_se;

   retorne n;
fim;

variáveis
   i: inteiro;
inicio
   // Imprime os 30 primeiros termos:
   para i <- 0 até 30 faça
      escrevaln("(", i, "):", fibo(i));
   fim_para;
fim.
