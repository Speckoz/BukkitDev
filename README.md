<h1 align="center">
    BukkitDev API
</h1>

<div align="center">
    <img src="bucketLava.gif"/>
</div>

### API Do BukkitDevSystem

A lógica da API até o momento é verificar se uma licença de um plugin existe e é valida.

Se alguém tentar usar uma licença inválida ou de um IP não permitido, a API salvará o IP da pessoa no arquivo de log.

Essa plataforma foi feita para ser usada em conjunto com o [BukkitDev Desktop](https://github.com/Logikoz/BukkitDev-System), por la você pode configurar e adicionar plugins/licenças.

## Antes de rodar o projeto:
- Certifique-se de ter o Node.js instalado.

- Instale todas as dependencias do projeto (`package.json`). Isso vai instalar tudo que você precisa.

```
npm install
```

- Insira seus dados de conexão no arquivo `.env.example`, renomeando para `.env`


## Rodando o projeto:
- Com tudo pronto, para rodar o projeto digite:

```
npm start
```

## Como consultar a API
Para usar a API é simples, basta apenas, dentro de seus plugins, fazer uma requisição para a url da API passando a Key da licença e verificar o retorno

Formato da URI: http://localhost:3000/licenca/234324324234

Se a licença for válida, a API irá retornar um JSON com os dados da licença

Se for inválida ou estiver suspensa, irá retornar o status 404 (not found)

Basta apenas você verificar o status/conteudo da *reposta da api*

## desenvolvedores
- [@Logikoz](https://github.com/Logikoz) - [Software](https://github.com/Speckoz/)
- [@marcopandolfo](https://github.com/marcopandolfo) - [Web](https://github.com/Speckoz)
- [@marcopandolfo](https://github.com/marcopandolfo) - [API](https://github.com/Speckoz)

Somos membros da comunidade He4rt Developers, que tem como objetivo disponibilizar a troca de conhecimentos entre desenvolvedores, majoritariamente brasileiros, para que possam crescer juntos como profissionais na área.
<br>
<div align="center">
    <a href="https://discord.gg/J3saJqq" target="_blank">
    <img src="https://discordapp.com/api/guilds/452926217558163456/embed.png" alt="Discord server"/></a>
</div>

<div align="center">
    Que tal avaliar nosso projeto?<br>
    <a href="https://forms.gle/x9jJCCy1HAzfJCXw5" target="_blank">
      Clique Aqui
</div>

## Licença [MIT](https://github.com/marcopandolfo/BukkitDev-Web/blob/master/LICENSE)
O programa é gratuito e pode ser editado, compartilhado, e pode ser usado de forma comercial apenas para o uso do próprio desenvolvedor.
Não pode ser vendido de forma alguma, caso haja alguma pessoa fazendo isso, deve nos contatar.
<br>
