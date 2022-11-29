algoritmo "teste_procedimento_função"
variáveis
   x, y, z: inteiro;
   u, v, w, m, n: caracter;
   r, s, t: caracter;
   A: vetor[1..3] de inteiro;
   B: vetor[1..3, 1..4] de real;

procedimento procMsgNum(num: inteiro)
inicio
   escreva("\nprocMsgNum(", num, ");");
fim;

procedimento procMsgNum2(prr1, prr2: lógico)
inicio
   escreva("\nprocMsgNum2(", prr1, ", ", prr2, ");");
fim;

procedimento trocarRealMsg(a, b: caracter)
variáveis
   aux: caracter;
inicio
   escreva("\na=", a, ", b=", b, "\nTrocando...");
   aux <- a;
   a <- b;
   b <- aux;
   escreva("\na=", a, ", b=", b);
fim;

procedimento trocarReal(var a, b: caracter)
variáveis
   aux: caracter;
inicio
   aux <- a;
   a <- b;
   b <- aux;
fim;

função produto(X, Y: caracter): caracter
variáveis
   P: caracter;
inicio
   P <- X + Y;
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
   u <- "Olá ";
   v <- "Mundo!!!";

   escreva("\nu=", u, ", v=", v);
   n <- produto(u, v);
   escreva("\nu + v = ", n, "\n");
/*
   escreva("Informe o primeiro valor lógico: ");
   leia(w);
   escreva("Informe o segundo valor lógico: ");
   leia(m);
   n <- produto(w, m);
   escreva("w and m = ", n, "\n");

   //u <- produtoReal(u, x); 

   procMsgNum2(u, v);
   //procMsg(r);

   //trocar(x, x);
*/
   trocarRealMsg(u, v);

   escreva("\n\nTroca por ponteiro:\nu=", u, ", v=", v, "\nTrocando...");
   trocarReal(u, v);
   escreva("\nu=", u, ", v=", v);
fim.
