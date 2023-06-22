# ShopZilla.Pedidos.Api

API do projeto ShopZilla responsável por gerenciar os pedidos realizados.

## Geral

Este projeto faz parte de um conjunto de outros projetos ShopZilla, destinados aos estudos da arquitetura orientada a eventos, Kafka, Entity Framework e execução de processos em segundo plano via BackgroundServices.

O ShopZilla é um projeto que busca simular a efetivação de compras, atualização de estoque e envio de notificações para os clientes através de aplicações independentes, onde aproveitando da arquitetura orientada a eventos, caso algum sistema esteja completamente fora, não vai afetar o conjunto como um todo.

![alt text](https://github.com/felipetoscano/shopzilla_pedidos-api-dotnet/blob/main/resources/shopzilla-geral.jpg)

## API Pedidos

Esta API possui alguns serviços expostos para o gerenciamento de pedidos feitos por um operador (devidamente autenticado).

Ao realizar um pedido, é feito um disparo de um evento no tópico Kafka NOVO_PEDIDO para ser processado por APIs consumidoras, como no caso a API de Estoque.

Além disso, consome o tópico CONFIRMACAO_PEDIDO, e atualiza o status de um pedido. 

![alt text](https://github.com/felipetoscano/shopzilla_pedidos-api-dotnet/blob/main/resources/shopzilla-pedidos.jpg)

## Execução

### Para a geração da imagem Docker

A API, utilizando os recursos mais recentes do .NET 7, possui suporte integrado a geração de imagens Docker.

Basta inserir o comando abaixo na raiz do projeto: 

`
dotnet publish --os linux --arch x64 -p:PublishProfile=DefaultContainer -c Release 
`

## Para a execução do ecossistema

Para a simulação no ambiente local, executar o arquivo docker-compose.yml presente no projeto abaixo:

https://github.com/felipetoscano/shopzilla-scripts-docker

Além disso, garanta que as imagens de todos as aplicações .NET foram geradas.
