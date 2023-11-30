<%@ Page Title="" Language="C#" MasterPageFile="~/template.Master" AutoEventWireup="true" CodeBehind="produtos.aspx.cs" Inherits="lojavirtualOndaMental.produtos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="head">
        <p id="p">Inserir Produtos</p>
    </div>

    <div class="container">
        <div class="row">
            <div class="col-12 mb-3">
                <label associatedcontrolid="tb_nome">Nome:</label>
                <asp:TextBox ID="tb_nome" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorNome" runat="server" ControlToValidate="tb_nome" ErrorMessage="Nome é obrigatório" ForeColor="Red" CssClass="text-danger" Display="Dynamic" ValidationGroup="inserir_produto" />
            </div>

            <div class="col-12 mb-3">
                <label associatedcontrolid="tb_descricao">Descrição:</label>
                <asp:TextBox ID="tb_descricao" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorDescricao" runat="server" ControlToValidate="tb_descricao" ErrorMessage="Descrição é obrigatória" ForeColor="Red" CssClass="text-danger" Display="Dynamic" ValidationGroup="inserir_produto" />
            </div>

            <div class="col-12 col-md-3 mb-3">
                <label associatedcontrolid="tb_preco">Preço:</label>
                <asp:TextBox ID="tb_preco" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tb_preco" ErrorMessage="Número inválido" ForeColor="Red" ValidationExpression="[+-]?([0-9]{1,4}(\,[0-9]{0,2})?)?" CssClass="text-danger" Display="Dynamic" ValidationGroup="inserir_produto" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorPreco" runat="server" ControlToValidate="tb_preco" ErrorMessage="Preço é obrigatório" ForeColor="Red" CssClass="text-danger" Display="Dynamic" ValidationGroup="inserir_produto" />
            </div>

            <div class="col-12 col-md-3 mb-3">
                <label associatedcontrolid="ddl_categoria">Categoria:</label>
                <asp:DropDownList ID="ddl_categoria" runat="server" CssClass="form-control" DataSourceID="SqlDataSource1" DataTextField="categoria" DataValueField="id_categoria"></asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:lojavirtualOndaMentalConnectionString %>" SelectCommand="SELECT * FROM [tb_categorias]"></asp:SqlDataSource>
            </div>

            <div class="col-12 col-md-3 mb-3">
                <label associatedcontrolid="tb_estoque">Estoque:</label>
                <asp:TextBox ID="tb_estoque" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorEstoque" runat="server" ControlToValidate="tb_estoque" ErrorMessage="Estoque é obrigatório" ForeColor="Red" CssClass="text-danger" Display="Dynamic" ValidationGroup="inserir_produto" />
            </div>

            <div class="col-12 col-md-3 mb-3">
                <label associatedcontrolid="Upload_Imagem">Imagem:</label>
                <asp:FileUpload ID="Upload_Imagem" runat="server" CssClass="form-control" AllowMultiple="true" />
                <asp:Label ID="lbl_erro_file" runat="server" CssClass="text-danger" Visible="False"></asp:Label>
            </div>

            <div class="col-12 col-md-12 mb-3">
                <asp:Button ID="btn_adicionar" runat="server" CssClass="btn btn-dark" Text="ADICIONAR" OnClick="btn_adicionar_Click" ValidationGroup="inserir_produto" />
            </div>
        </div>
    </div>
    <div class="head">
        <div class="s"></div>
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="col">
                <div style="overflow-x: auto;">
                    <table class="table table-hover">
                        <thead class="thead-dark">
                            <tr>
                                <th>Id</th>
                                <th>Imagem</th>
                                <th>Nome</th>
                                <th>Descrição</th>
                                <th>Preço</th>
                                <th>Categoria</th>
                                <th>Estoque</th>
                                <th>Funções</th>
                            </tr>
                        </thead>
                        <tbody>
                            <!--repeater lista de produtos da BD -->
                            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <th data-label="Id">
                                            <asp:Label ID="lbl_id_produto" runat="server" Text='<%# Eval("id_produto") %>'></asp:Label>
                                        </th>

                                        <td class="divImg-adm" data-label="Imagem">
                                            <div class="imageItem">
                                                <asp:Repeater ID="rptImages" runat="server" DataSource='<%# ((prod)Container.DataItem).imagens %>' Visible="true">
                                                    <ItemTemplate>
                                                        <img class="lazyload img-adm" data-src='data:<%# Eval("content_type") %>;base64,<%# Convert.ToBase64String((byte[])Eval("dados_imagem")) %>' />
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <asp:Button ID="img_delete" runat="server" CommandArgument='<%# Eval("id_produto") %>' Text="X" CommandName="img_delete" CssClass="img_delete" OnClientClick="return confirm('Tem certeza que deseja excluir estas imagem?');" />

                                            </div>
                                            <asp:FileUpload ID="upload_imagem" runat="server" CommandName="upload_imagem" AllowMultiple="true" Visible="False" CssClass="form-control upload_imagem txt-letra" />
                                        </td>

                                        <td data-label="Nome">
                                            <asp:TextBox ID="tb_nome" runat="server" Text='<%# Bind("nome") %>' CssClass="form-control txt-letra"></asp:TextBox></td>
                                        <td data-label="Descrição">
                                            <asp:TextBox ID="tb_descricao" runat="server" Text='<%# Bind("descricao") %>' CssClass="form-control txt-letra"></asp:TextBox></td>

                                        <td data-label="Preço">
                                            <asp:TextBox ID="tb_preco" runat="server" Text='<%# Bind("preco", "{0:F2}") %>' CssClass="form-control txt-letra"></asp:TextBox></td>

                                        <td data-label="Categoria">
                                            <asp:DropDownList ID="ddl_categoria2" runat="server" DataSourceID="SqlDataSource1" DataTextField="categoria" DataValueField="id_categoria" SelectedValue='<%# Bind("categoria") %>' CssClass="form-control txt-letra"></asp:DropDownList></td>
                                        <td data-label="Estoque">
                                            <asp:TextBox ID="tb_estoque" runat="server" Text='<%# Bind("estoque") %>' CssClass="form-control txt-letra"></asp:TextBox></td>

                                        <td data-label="Funções">
                                            <asp:ImageButton ID="btn_grava" runat="server" ImageUrl="img/save.jpg" CommandArgument='<%# Eval("id_produto") %>' CommandName="btn_grava" OnClientClick="return validarQuantidadeDeImagens(this);" />
                                            <asp:ImageButton ID="btn_delete" runat="server" ImageUrl="img/delete.jpg" CommandArgument='<%# Eval("id_produto") %>' OnClientClick="return confirm('Tem certeza que deseja excluir este produto?');" CommandName="btn_apaga" /></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>

            </div>
        </div>

    </div>



    <style>
        body {
            background-color: #f7f7f7;
        }

        /* ----- Table ----*/
        table {
            border-collapse: separate;
            border-spacing: 0px 8px;
        }

        th {
            text-align: left;
            padding: 5px;
            text-transform: uppercase;
            font-weight: 500;
            font-size: 11px;
            color: blue !important;
        }

        tr {
            box-shadow: 1px 1px 1px rgba(228, 228, 228, 0.25);
        }

            tr thead {
                background: transparent !important;
                border-color: transparent !important;
            }

        .txt-letra {
            background-color: #ffffff !important;
            border-bottom: 1px solid #e7e7e7 !important;
            font-size: 12px !important;
            font-weight: 400 !important;
        }

        td {
            padding: 13px;
        }


        .container {
            width: 70%;
        }

        .upload_imagem {
            max-width: 205px;
        }

        .img_delete {
            transform: translateY(-20px);
            width: 16px;
            height: 17px;
            padding: 0;
            margin: 0px 0 0 -19px;
            border: 0 none;
            border-radius: 100%;
            font-size: xx-small;
            font-family: 'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif;
            color: rgb(255, 255, 255);
            background-color: red;
        }

        .divImg-adm {
            object-fit: cover;
            object-fit: cover;
        }

        .img-adm {
            object-fit: cover !important;
            background-size: cover !important;
            background-position: center !important;
            background-repeat: no-repeat !important;
            background-color: rgb(0, 0, 0) !important;
            width: 40px;
            height: 40px;
            border: 1px black solid !important;
        }


        @media screen and (min-width: 1200px) {
            .botao-todos {
                margin-left: 10px;
            }

            .img-adm {
                width: calc(20%);
                height: 50px;
            }
        }

        @media screen and (max-width: 767px) {

            table,
            thead,
            tbody,
            th,
            td,
            tr {
                display: block;
            }

                thead tr {
                    position: absolute;
                    top: -9999px;
                    left: -9999px;
                }

            tr {
                border: 1px solid #ccc;
                margin-bottom: 10px;
            }

            td {
                border: none;
                border-bottom: 1px solid #eee;
                position: relative;
                padding-left: 50%;
            }

                td::before {
                    content: attr(data-label);
                    float: left;
                    font-weight: bold;
                    margin-right: 100%;
                }

            .img-adm {
                width: 48px;
                height: 50px;
            }
        }
    </style>



    <script>

        // Função otimização no carregamento das imagens, carrega apenas imagens que estão na sua VIEWPORT
        document.addEventListener("DOMContentLoaded", function () {
            // Verifica se o navegador suporta 'IntersectionObserver'
            if ('IntersectionObserver' in window) {
                // Seleciona todas as imagens com a classe 'lazyload'
                var imagensParaCarregar = document.querySelectorAll("img.lazyload");
                var observadorDeImagens = new IntersectionObserver(function (entradas, observador) {
                    // Itera sobre as entradas observadas
                    entradas.forEach(function (entrada) {
                        // Verifica se a imagem está visível (entrando na viewport)
                        if (entrada.isIntersecting) {
                            var imagem = entrada.target;
                            // Atualiza o 'src' com o 'data-src' para iniciar o carregamento da imagem
                            imagem.src = imagem.dataset.src;
                            // Define um evento para quando a imagem for carregada
                            imagem.onload = function () {
                                imagem.classList.add("fade-in");
                            };
                            // Remove a classe 'lazyload' após o carregamento
                            imagem.classList.remove("lazyload");
                            // Para de observar a imagem após o carregamento
                            observadorDeImagens.unobserve(imagem);
                        }
                    });
                }, {
                    root: null, // Utiliza o viewport como área de referência para a visibilidade
                    rootMargin: "0px", // Margem em torno do root
                    threshold: 0.01 // Porcentagem da imagem que deve estar visível para carregar
                });

                // Observa cada imagem com lazy loading
                imagensParaCarregar.forEach(function (imagem) {
                    observadorDeImagens.observe(imagem);
                });
            } else {
                // Caso o navegador não suporte 'IntersectionObserver', carrega todas as imagens diretamente
                var imagensParaCarregar = document.querySelectorAll("img.lazyload");
                // Função para carregar as imagens imediatamente
                function carregarImagensImediatamente(imagens) {
                    imagens.forEach(function (imagem) {
                        imagem.src = imagem.dataset.src;
                    });
                }
                // Carrega todas as imagens
                carregarImagensImediatamente(imagensParaCarregar);
            }

        });


        /**
        Função Verificacao do total de imagens na alteração do produto.
        Caso coloque mais imagens que o permitido, surge um aviso, impedindo de ir para a BD.
        */
        function validarQuantidadeDeImagens(btn) {
            // Encontra o controle FileUpload mais próximo do botão
            var upload = btn.closest('tr').querySelector('.upload_imagem');
            if (upload.files.length > 4) {
                alert('Limite de 4 imagens. Você selecionou ' + upload.files.length + ' imagens.');
                return false; // Impede o envio da Alteração
            }
            return true; // Permite o envio da Alteração
        }




    </script>


</asp:Content>
