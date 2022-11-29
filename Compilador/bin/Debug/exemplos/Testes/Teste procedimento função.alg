algoritmo "teste_procedimento_função"
variáveis
   x, y, z: inteiro;
   u, v, w: real;
   r, s, t: caracter;
   A: vetor[1..3] de inteiro;
   B: vetor[1..3, 1..4] de real;

procedimento procVazio()
inicio
   escreva("ola");
fim;

procedimento procMsg(msg: caracter)
inicio
   escreva(msg);
fim;

procedimento procMulti(p1, p2: inteiro; px1, px2: real)
inicio
   escreva("ola");
fim;

procedimento trocar(var a, b: inteiro)
variáveis
   aux: inteiro;
inicio
   aux <- a;
   a <- b;
   b <- aux;
fim;

procedimento trocarReal(var a, b: real)
variáveis
   aux: real;
inicio
   aux <- a;
   a <- b;
   b <- aux;
fim;

procedimento trocarReal2(a, b: real)
variáveis
   aux: real;
inicio
   aux <- a;
   a <- b;
   b <- aux;
fim;

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

função produtoReal2(var X, Y: real): real
variáveis
   P: real;
inicio
   P <- X * Y;
   retorne P;
fim;

// Rotina Principal
inicio
   x <- 123;
   u <- 321.0;
   v <- 789.1;
   r <- "Olá mundo!";

   x <- trocar(x, y);   // Erro procedimento
   x <- produto(x, y);  // Erro y não atribuido
   x <- produto(x, u);  // Erro u tipo
   produto(x, y);       // Erro utilização inválida para função
   x <- produto(x, x);

   x <- produtoReal(u, x);  // Erro tipo retorno função
   u <- produtoReal(u, x); 

   u <- produtoReal2(u, x); // Erro x tipo (ponteiro)
   u <- produtoReal2(u, v);

   //procVazio();  // Erro

   //procMsg(1);     // Erro constante
   //procMsg("Olá"); // Erro constante
   //procMsg(A[1]);  // Erro vetor
   procMsg(x);     // Erro tipo
   procMsg(s);     // Erro não atribuido
   procMsg(r, r);  // Erro quantia parâmetros
   procMsg(r);

   trocar(x, u);   // Erro u tipo
   trocar(x, x);

   trocarReal(u, x);  // Erro x tipo (ponteiro)
   trocarReal(u, v);

   trocarReal2(u, x);
fim.
