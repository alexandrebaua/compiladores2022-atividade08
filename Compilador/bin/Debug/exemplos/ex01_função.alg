/************************************************************
 *  Elaborar um algoritmo que leia dois números inteiros e  *
 * solicite uma função para efetuar o produto entre elas    *
 ************************************************************/
algoritmo "ex_1_função"
variáveis
   a, b, r: inteiro;

função produto(X, Y: inteiro): inteiro
variáveis
   P: inteiro;
inicio
   P <- X * Y;
   retorne P;
fim;

// Rotina Principal
inicio
   escreva("Valor de a: ");
   leia(a);
   escreva("Valor de b: ");
   leia(b);
   r <- produto(a, b);
   escreva("O produto é ", r);
fim.
