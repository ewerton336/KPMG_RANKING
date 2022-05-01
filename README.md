# KPMG_RANKING
 Esquema de lançamento de placar dos jogadores e classificação



## 🚀 Sobre mim
Eu sou uma pessoa desenvolvedora full-stack, com experiência sólida no back-end, e estudando frameworks de front-end, principalmente react.
 


## Autor

- [Ewerton Guimarães - Github](https://www.github.com/ewerton336)
- [LinkedIn](https://www.linkedin.com/in/ewerton-guimar%C3%A3es-b97579113/)

## Stack utilizada

**Front-end:** Bootstrap, CSS e Javascript

**Back-end:** .NET Framework, Dapper.

**Database:** mariaDB.





## Funcionalidades

**Página WEB:**
- Ranking com top 100 jogadores


**API:**

- Obter lista com o ranking dos 100 melhores jogadores e suas pontuações.
- Salvar nova pontuação em cachê.
- Obter pontuações salvas em cachê.
- Limpar dados em cachê.
- Obter intervalo de atualização dos dados do cachê.
- Definir intervalo de atualização dos dados do cachê.
- Obter data da última vez em que as pontuações em cachê foram salvas no banco de dados.
- Atualizar data da última atualização no banco de dados.


## Instalação

Basta abrir a aplicação no visual studio e compilar o projeto do website, ou acessar o link com a aplicação rodando em nuvem:


 - [www.ewertondev.com.br](https://www.ewertondev.com.br)

    
## Documentação da API

#### Retorna o ranking com os 100 melhores jogadores

```http
  GET /api/GameResult
```

| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `nenhum` | ` null ` | Não é necessário indicar nenhum parâmetro |


#### Retorna o ranking com os 100 melhores jogadores

```http
  POST /api/GameResult
```

| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `playerId` | ` int ` | Id do jogador |
| `gameScore` | ` int ` | Pontuação do jogador (positiva ou negativa) |
| `gameDate` | ` DateTime ` | Data e hora da partida |

Obs: GameId é gerado automaticamente pelo banco de dados.



#### Retorna os dados que estão em cachê

```http
  GET /api/GameResult/cached
```

| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `nenhum` | ` null ` | Não é necessário indicar nenhum parâmetro |

#### Limpar dados salvos em cachê

```http
  POST /api/GameResult/cached
```

| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `nenhum` | ` null ` | Não é necessário indicar nenhum parâmetro |

#### Retorna o intervalo de atualização em que os dados em cachê são persistidos no banco de dados

```http
  GET /api/GameResult/intervalRefresh
```

| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `nenhum` | ` null ` | Não é necessário indicar nenhum parâmetro |



#### Altera o intervalo de atualização em que os dados em cachê são persistidos no banco de dados

```http
  POST /api/GameResult/intervalRefresh
```

| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `minutes` | ` int ` | Número em minutos |

#### Obter a data e hora da última atualização dos dados no banco de dados

```http
  GET /api/GameResult/intervalRefresh
```

| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `nenhum` | ` null ` | Não é necessário indicar nenhum parâmetro |

#### Obter a data e hora da última atualização dos dados no banco de dados

```http
  POST /api/GameResult/intervalRefresh
```

| Parâmetro   | Tipo       | Descrição                           |
| :---------- | :--------- | :---------------------------------- |
| `lastUpdate` | ` DateTime ` | Data e hora em que foi realizada a última atualização |


