/*****************************************************
 *  Um algoritmo para ler 10 números inteiros e      *
 * armazená-los num vetor. Após essas etapa, obter   *
 * a soma de todos os pares contidos no vetor        *
 *****************************************************/
algoritmo "soma_vetor_pares"
variáveis
   i, n, soma: inteiro;
   V: vetor[1..10] de inteiro;
inicio
   para i <- 1 até 10 faça
      escreva(i, "º número: ");
      leia(V[i]);
   fim_para;
   
   soma <- 0;
   para i <- 1 até 10 faça
      se V[i] mod 2 = 0 então
         soma <- soma + V[i];
      fim_se;
   fim_para;
   escreva ("Soma dos pares = ", soma);
fim.
