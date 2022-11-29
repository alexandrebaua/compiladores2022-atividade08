### Implementação da geração do código para a linguagem assembly compatível com a arquitetura MIPS.

Desenvolvido durante o semestre 02/2022 na matéria de Compiladores, do curso de Bacharel em Ciência da Computação, no Instituto Federal Catarinense - Campus Rio do Sul.

Aceita uma gramática para os elementos definidos abaixo (baseado em algoritmos) e implementa o analisador sintático ascendente SLR, e a análise semântica juntamente com a tabela de símbolos e a árvore de análise sintática para esta gramática, além da geração de código Assembly MIPS, compativel com o simulador MARS MIPS:
```
/************************************************************
 *  Exemplo de um algoritmo que lê dois números inteiros e  *
 * usa uma função para efetuar o produto entre eles         *
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
```

### Simulador e Documentação MARS MIPS:
- [MARS (MIPS Assembler and Runtime Simulator)](http://courses.missouristate.edu/KenVollmar/MARS/index.htm)
- [Funções SYSCALL disponíveis no MARS](https://courses.missouristate.edu/kenvollmar/mars/help/syscallhelp.html)

### Arquivos relacionados:
- [Documentação (Gramática, estados, reduções e tabela)](docs)
- [Formulário principal](Compilador/FormMain.cs)
- [Diretório analizador léxico](Compilador/Lexico)
- [Diretório analisador sintático](Compilador/Sintatico/Ascendente/SLR)
- [Diretório analisador semântico](Compilador/Semantico)
- [Diretório gerador código Assembly MIPS](Compilador/GeradorCódigo/MIPS)
- [Arquivo executável](Compilador/bin/Debug/Compilador.exe)
