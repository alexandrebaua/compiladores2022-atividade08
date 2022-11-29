algoritmo "teste MIPS leia escreva"
variáveis
   a, b, c, d, abc: inteiro;
   x, y, z, xyz: real;
   t: caracter;
   V1: vetor[1..10] de inteiro;
   V2: vetor[1..10, 1..2] de inteiro;

inicio
   escreva("Informe o valor de 'a': ");
   leia(a);
   escrevaln("a=", a);

   escrevaln("Informe o valor de 'b' e 'c':");
   leia(b, c);
   escrevaln("b=", b, ", c=", c);

   escreva("Informe o valor de 'x': ");
   leia(x);
   escrevaln("x=", x);

   escrevaln("Informe o valor de 'y' e 'z':");
   leia(y, z);
   escrevaln("y=", y, ", z=", z);

   escrevaln("Constante inteiro: ", 123);
   escrevaln("Constante inteiro: ", 456, ", ", 789);
   escrevaln("Constante real: ", 321.147);
   escrevaln("Constante real: ", 654.159, ", ", 987.369);

   escrevaln("Informe os valores para o vetor:");
   leia(V1);
   escrevaln("Vetor: ", V1);

   escreva("Informe o valor para um elemento no vetor V1[1]: ");
   leia(V1[1]);
   escrevaln("Informado: ", V1[1]);
   escrevaln("Vetor: ", V1);

   escreva("Informe a posição do vetor: ");
   leia(a);
   escreva("Informe o valor para a posição V1[", a, "]: ");
   leia(V1[a]);
   escrevaln("Informado: ", V1[a]);
   escrevaln("Vetor: ", V1);

   escreva("Informe o valor para um elemento no vetor V2[1, 1]: ");
   leia(V2[1, 1]);
   escrevaln("Informado: ", V2[1, 1]);
   escrevaln("Vetor: ", V2);

   escrevaln("Informe a posição do vetor V2[i1, i2]:");
   leia(a, b);
   escrevaln("Informe o valor para a posição V2[", a, ", ", b, "]:");
   leia(V2[a, b]);
   escrevaln("Informado: ", V2[a, b]);
   escreva("Vetor: ", V2);

fim.
