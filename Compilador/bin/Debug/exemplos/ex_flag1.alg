/************************************************************
 *  Ler vários números e parar quando for digitado um       *
 * número negativo. Então mostrar quantos foram digitados.  *
 ************************************************************/
algoritmo "ex_flag_1"
variáveis
   n, contador: inteiro;

// Rotina Principal
inicio
   escreva("Digita um número: ");
   leia(n);
   contador <- 0;
   enquanto n >= 0 faça
      contador <- contador + 1;
      escreva("Digita um número: ");
      leia(n);
   fim_enquanto;
   escreva("Foram digitados ", contador, " números.");
fim.
