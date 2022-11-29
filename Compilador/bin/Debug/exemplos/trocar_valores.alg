/********************************************************************
 *  Elaborar um subprograma que receba os valores de a e b e afetue *
 * a troca deste valores.                                           *
 ********************************************************************/
algoritmo "trocar_valores"
variáveis
   x, y: inteiro;
procedimento trocar(var a, b: inteiro)
variáveis
   aux: inteiro;
inicio
   aux <- a;
   a <- b;
   b <- aux;
fim;
// principal
inicio
   escreva("Informe X e Y:\n");
   leia(x, y);
   trocar(x, y);
   escreva("Agora X = ", x);
   escreva(" e Y = ", y);
fim.
