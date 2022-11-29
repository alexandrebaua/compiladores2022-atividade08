algoritmo "teste_leia_escreva"
variáveis
   a, b, c, d: inteiro;
   x, y, z: real;
   t: caracter;
   v: vetor[1..10] de caracter;

inicio
   leia(kn);
   leia(kn, d);

   escreva("Texto 01");
   escreva("Texto 02", kn);
   escreva("Texto 03a", kn, "Texto 03b");
   escreva("Texto 04a", a, "Texto 04b", t);

   leia(a);
   escreva("Texto 05a", a, "Texto 05b", t, "Texto 05c", v);
   escreva("Texto 06a", a, "Texto 06b", t);

   leia(v);
   leia(a, x, t);
   escreva("Texto 07a", a, "Texto 07b", t,"Texto 07c", v);

   leia(v[a], v[c]);
   leia(v[a + 1], v[c + 2]);
   leia(v[1, 2, 3], v[a, b, c]);
   leia(v[1], v[2]);

   escreva("Texto 08a", v[a], "Texto 08b", v[c]);
   escreva("Texto 09a", v[a + 1], "Texto 09b", v[c + 2]);
   escreva("Texto 10a", v[1, 2, 3], "Texto 10b", v[a, b, c]);
   escreva("Texto 11a", v[1], "Texto 11b", v[2]);
   escreva("Texto 12a", v[a] + 62, "Texto 12b", v[1] * 2, i + 1);
fim.
