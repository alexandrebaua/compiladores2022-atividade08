/********************************************************************
 *  Elabore um algoritmo que leia vários números inteiros, podendo  *
 * ser negativos, e ao final mostre qual o maior número digitado.   *
 * O usuário determina quantos números digitar.                     *
 ********************************************************************/
algoritmo "maior"
variáveis
   k, kn: inteiro;
   n, m: real;

inicio
   m <- -9999;
   escreva("Quantos números serão digitados? (Nº > 2): ");
   leia(kn);
   se kn < 2 então
      escreva("Valor menor que o minímo!");
   senão
      para k <- 1 até kn faça
         escreva("Digite o ", k, " número: ");
         leia(n);
         se n > m então
            m <- n;
         fim_se;
      fim_para;
      escreva("Omaior número digitado foi: ", m);
   fim_se;
fim.
