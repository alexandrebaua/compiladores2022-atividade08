/************************************************************
 *  Ler vários números e parar quando for digitado um       *
 * número negativo. Então mostrar quantos foram digitados.  *
 ************************************************************/
algoritmo "ex_flag_2"
variáveis
   n, contador: inteiro;
   digitouNegativo: lógico;

// Rotina Principal
inicio
   contador <- 0;
   digitouNegativo <- falso;
   //escreva("Digite um número: ");
   //leia(n);
   enquanto não(digitouNegativo) faça
      escreva("Digita um número: ");
      leia(n);
      se(n < 0) então
         digitouNegativo <- verdadeiro;
      senão
         contador <- contador + 1;
      fim_se;
   fim_enquanto;
   escreva("Foram digitados ", contador, " números.");
fim.
