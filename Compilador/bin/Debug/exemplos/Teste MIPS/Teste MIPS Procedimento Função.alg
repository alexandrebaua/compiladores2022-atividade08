algoritmo "teste_procedimento_função"
variáveis
   x, y, z: inteiro;
   u, v, w: real;
   r, s, t: caracter;
   A: vetor[1..3] de inteiro;
   B: vetor[1..3, 1..4] de real;

procedimento procMsgNum(num: inteiro)
inicio
   escreva("\nprocMsgNum(", num, ");");
fim;

procedimento procMsgNum2(num: inteiro; prr1, prr2: real)
inicio
   escreva("\nprocMsgNum2(", num, ", ", prr1, ", ", prr2, ");");
fim;

procedimento trocarRealMsg(a, b: real)
variáveis
   aux: real;
   Ax: vetor[1..3] de inteiro;
inicio
   //Ax[1] <- Ax[2];
   escreva("\na=", a, ", b=", b, "\nTrocando...");
   aux <- a;
   a <- b;
   b <- aux;
   escreva("\na=", a, ", b=", b);
fim;

procedimento trocarReal(var a, b: real)
variáveis
   aux: real;
   Ax: vetor[1..3] de inteiro;
inicio
   //Ax[1] <- Ax[2];
   aux <- a;
   a <- b;
   b <- aux;
fim;
/*
procedimento procVazio()
inicio
   escreva("ola");
fim;

procedimento procMsg(msg: caracter)
inicio
   escreva(msg);
fim;

procedimento trocar(var a, b: inteiro)
variáveis
   aux: inteiro;
inicio
   aux <- a;
   a <- b;
   b <- aux;
fim;
*/
função produto(X, Y: inteiro): inteiro
variáveis
   P: inteiro;
inicio
   P <- X * Y;
   retorne P;
fim;

função produtoReal(X, Y: real): real
variáveis
   P: real;
inicio
   P <- X * Y;
   retorne P;
fim;

// Rotina Principal
inicio
   x <- 123;
   u <- 321.24;
   v <- 456.6969;
   //r <- "Olá mundo!";

   escreva("Informe um número: ");
   leia(y);
   x <- 1 + 2 * produto(y, y);
   escreva("Produto de y=", x, "\n");

   //u <- produtoReal(u, x); 

   procMsgNum2(x, u, v);
   //procMsg(r);

   //trocar(x, x);

   trocarRealMsg(u, v);

   escreva("\n\nTroca por ponteiro:\nu=", u, ", v=", v, "\nTrocando...");
   trocarReal(u, v);
   escreva("\nu=", u, ", v=", v);
fim.
