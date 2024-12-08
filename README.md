# **API e Serviço de notificação para Sistema de Controle de Estoque**

Este repositório contém uma API e um serviço de monitoramento com envio de notificação para um **Sistema de Controle de Estoque**, um projeto desenvolvido como parte da disciplina de **Back-End** no curso de **Análise e Desenvolvimento de Sistemas (ADS)** da **UNIVILLE**. <br> O objetivo deste projeto na disciplina foi desenvolver uma aplicação Back-End com foco na implementação de soluções que envolvem persistência de dados, gerenciamento eficiente de recursos e criação de APIs. O projeto foi desenvolvido utilizando .NET 8, foi feita a implementação da API de estoque usando a Web API do ASP.NET Core, para a persistência de dados e operações de CRUD, foi utilizado o Entity Framework com a implementação de migrations para garantir a evolução do banco de dados. Além disso, foi desenvolvido um serviço de monitoramento e envio de notificações sobre o status do estoque, utilizando Windows Forms também com .NET 8.

Este projeto foi projetado para consolidar conhecimentos teóricos e práticos adquiridos na disciplina, permitindo a criação de uma solução funcional, foi possível revisar e praticar os principais conceitos de **Programação Orientada a Objetos (POO)** aplicados durante o desenvolvimento da aplicação, como:
  - Encapsulamento, Herança, Abstração e Polimorfismo.
  - Princípio de Responsabilidade Única e Código limpo.
  - Manipulação de bancos de dados.
  - Organização e boas práticas de programação.

 ![Captura de Tela do Projeto](https://github.com/Jonas-Emir/trabalho-academico_API/blob/main/Demonstracao/Objetivos%20do%20Projeto.PNG)
    
---

 ![Captura de Tela do Projeto](https://github.com/Jonas-Emir/trabalho-academico_API/blob/main/Demonstracao/Planejamento%20do%20Projeto.PNG)

## 1. Endpoints da API 
Os endpoints para suportar as principais funcionalidades, como consultas, cadastros e operações para o controle de estoque.

![Captura de Tela do Projeto](https://github.com/Jonas-Emir/trabalho-academico_API/blob/main/Demonstracao/Cruds.PNG)

## 2. Serviço de Monitoramento 
O serviço de monitoramento do estoque é para verificar níveis de estoque em tempo real e enviar alertas por e-mail conforme configurações predefinidas.

![Captura de Tela do Projeto](https://github.com/Jonas-Emir/trabalho-academico_API/blob/main/Demonstracao/MonitoramentoEstoque.PNG)
![Captura de Tela do Projeto](https://github.com/Jonas-Emir/trabalho-academico_API/blob/main/Demonstracao/ConfiguracoesEstoque.PNG)

## 3. Exemplo de E-mail Recebido
O serviço de monitoramento notifica os responsáveis pelo estoque automaticamente em situações críticas, como baixo ou alto nível de estoque.

![Captura de Tela do Projeto](https://github.com/Jonas-Emir/trabalho-academico_API/blob/main/Demonstracao/EmailMonitoramento.PNG)

## **Tecnologias Utilizadas**

- **Linguagem**: C#  
- **Framework**: .NET  
- **Banco de Dados**: Microsoft SQL Server  
