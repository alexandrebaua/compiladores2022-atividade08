algoritmo "Consumo"
variáveis
   consumo, distancia, volume: real;
inicio
   escreva("Calcula o consumo médio de combustível\n");
   escreva ("------- - ------- ----- -- -----------\n");
   //escreva (" ");
   escreva ("Qual a distancia (Km) percorrida pelo veiculo? ");
   leia(distancia);
   escreva ("Quantos litros de combustível foram gastos no trajeto? ");
   leia(volume);
   consumo <- volume/distancia;
   escreva("O consumo medio de combustível foi de ", consumo, " litros/km\n");
   escreva("O carro fez ", distancia/volume, "km com 1 litro");
fim.
