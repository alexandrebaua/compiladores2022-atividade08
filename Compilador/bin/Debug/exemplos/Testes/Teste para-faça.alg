algoritmo "teste_para_faça"
variáveis
   k, i, a, b, c: inteiro;
   x, y, z: real;
   v: vetor[1..3] de inteiro;
   w: vetor[1..3] de real;

inicio
   para k <- 2 até 15 faça
      para i <- 1 até 10 faça
         escreva(k, " * ", i, " = ", k*i);
      fim_para;
   fim_para;

   para x <- 2 até 15 faça
      para y <- 1 até 10 faça
         escreva(k);
      fim_para;
   fim_para;

   para k <- 2.0 até 15.0 faça
      para i <- 1.0 até 10.0 faça
         escreva(i);
      fim_para;
   fim_para;

   para k <- a + b até c faça
      para i <- 1 + c até 10 * c faça
         escreva(c);
      fim_para;
   fim_para;

   para k <- a + b até c faça
      para i <- 1 + c até 10 * c faça
         escreva(c);
      fim_para;
   fim_para;

   para k <- v[1] + a - 1 até 1 - b + v[2] faça
      para i <- 1 + w[1] até w[2] + 10 faça
         escreva(x);
      fim_para;
   fim_para;
fim.
