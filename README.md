# Binance Order Book Collector

## Descrição
Este projeto é um coletor de livro de ordens (Order Book) para o par de criptomoedas BNB/BTC na exchange Binance. Ele se conecta ao WebSocket da Binance para receber atualizações em tempo real do livro de ordens e também obtém um snapshot inicial do estado do livro de ordens via uma API REST.

## Funcionalidades
- Conexão com o WebSocket da Binance para o par BNB/BTC.
- Obtenção de um snapshot inicial do livro de ordens.
- Processamento de atualizações do livro de ordens em tempo real.
- Gerenciamento de mensagens fora de sequência.

## Tecnologias Utilizadas
- C#
- .NET Core
- Newtonsoft.Json (para processamento de JSON)

## Configuração e Instalação
Para executar este projeto, você precisará do .NET Core instalado em seu ambiente. Além disso, você precisa instalar a biblioteca Newtonsoft.Json, que pode ser feita via NuGet Package Manager com o comando:

```bash
Install-Package Newtonsoft.Json
```

##Uso
Para executar o coletor, basta executar o comando a seguir no terminal (assegure-se de estar no diretório do projeto):

```bash
dotnet run
```
O programa se conectará ao WebSocket da Binance, obterá o snapshot inicial do livro de ordens e começará a processar as mensagens recebidas.
