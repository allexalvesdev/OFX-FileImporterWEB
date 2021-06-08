<h1>Projeto: Sistema WEB de importação de Extrato Bancário via arquivo .OFX</h1>

<h3>Problema</h3>
Bruna precisa concilicar o extrato bancário da empresa com as entradas e saídas que ela registrou no Excel do último mês. Para isso é utilizado um arquivo do tipo OFX, que contém o registro do banco de todas as entradas e saídas de um período e é fácil de exportar pelo Bankline.
O problema é que Xayah baixou dois arquivos OFX: um que contém transações do dia 01 ao dia 20 e outro que contém transações do dia 15 ao 31. E agora ela não sabe o que fazer para garantir que todas as transações sejam conciliadas corretamente, uma vez que os arquivos OFX possuem transações de um período em comum.

<h5>O que você deverá fazer: </h5>
Criar um sistema onde ela possa importar dois ou mais arquivos OFX e, no final, poderá ver todas as transações bancárias que ocorreram no período. Bruna deve poder ver a lista de transações por uma interface web responsiva e poder pesquisar transação dentro de um gap de datas. Além de poder abrir uma modal de detalhes de de cada transação para adicionar uma observação em texto.
Lembre-se que os arquivos OFX poderão conter transações de um mesmo período. Essas transações devem ser exibidas sem duplicidade. Também lembre-se de que é possível que existam transações do mesmo valor em um mesmo dia.

<h2>Tecnologias e práticas utilizadas</h2>

<ul>
<li>ASP.NET Core 2.1</li>
<li>Entity Framework Core</li>
<li>MVC</li>
<li>Injeção de Dependência</li>
<li>Banco Local SQL</li>
</ul>

<h3>Funcionalidades</h3>
Importação de Arquivos .OFX, Listagem, Detalhes, Atualização e Busca </br>
