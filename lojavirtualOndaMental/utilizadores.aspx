<%@ Page Title="" Language="C#" MasterPageFile="~/template.Master" AutoEventWireup="true" CodeBehind="utilizadores.aspx.cs" Inherits="lojavirtualOndaMental.utilizadores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="head">
        <p id="p">Lista De Utilizadores</p>
        <div class="s"></div>
    </div>

    <div class="table-responsive container-fluid">
        <table class="table table-striped table-bordered">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Perfil</th>
                    <th>Nome</th>
                    <th>Email</th>
                    <th>Password</th>
                    <th>Nif</th>
                    <th>Sexo</th>
                    <th>Data Nasc.</th>
                    <th>Telemóvel</th>
                    <th>Morada</th>
                    <th>Ativo</th>
                    <th>Funções</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rpt_lista_de_utilizadores" runat="server" OnItemCommand="rpt_lista_de_utilizadores_ItemCommand" OnItemDataBound="rpt_lista_de_utilizadores_ItemDataBound" DataSourceID="SqlDataSource1">
                    <ItemTemplate>
                        <tr>
                            <td data-label="ID">
                                <asp:Label ID="lbl_id_utilizador" runat="server"></asp:Label></td>
                            <td data-label="Perfil">
                                <asp:DropDownList ID="ddl_perfil" runat="server" DataSourceID="SqlDataSource2" DataTextField="perfil" DataValueField="id_perfil" SelectedValue='<%# Bind("perfil_id") %>' CssClass="form-control  textbox-perfil textbox-grande" /></td>
                            <td data-label="Nome">
                                <asp:TextBox ID="tb_nome" runat="server" CssClass="form-control textbox-grande"></asp:TextBox></td>
                            <td data-label="Email">
                                <asp:TextBox ID="tb_email" runat="server" CssClass="form-control textbox-grande"></asp:TextBox>
                                <asp:Label ID="lblErrorMessage" runat="server" CssClass="error-message" Visible="false" ForeColor="Red"></asp:Label>
                            </td>
                            <td data-label="Password">
                                <asp:TextBox ID="tb_password" runat="server" CssClass="form-control textbox-grande"></asp:TextBox></td>
                            <td data-label="Nif">
                                <asp:TextBox ID="tb_nif" runat="server" CssClass="form-control textbox-media"></asp:TextBox></td>
                            <td data-label="Sexo">
                                <asp:TextBox ID="tb_sexo" runat="server" CssClass="form-control textbox-media"></asp:TextBox></td>
                            <td data-label="Data Nasc">
                                <asp:TextBox ID="tb_data_nasc" runat="server" TextMode="Date" CssClass="form-control textbox-media"></asp:TextBox>
                            </td>
                            <td data-label="Telemóvel">
                                <asp:TextBox ID="tb_telemovel" runat="server" CssClass="form-control textbox-media"></asp:TextBox></td>
                            <td data-label="Morada">
                                <asp:TextBox ID="tb_morada" runat="server" CssClass="form-control textbox-grande"></asp:TextBox></td>
                            <td data-label="Ativo">
                                <asp:CheckBox ID="cb_ativo" runat="server" />
                            </td>


                            <td data-label="Funções">
                                <asp:ImageButton ID="btn_grava" runat="server" ImageUrl="img/save.jpg" CommandName="btn_grava" />
                                <asp:ImageButton ID="btn_delete" runat="server" ImageUrl="img/delete.jpg" OnClientClick="return confirm('Tem certeza que deseja excluir este produto?');" CommandName="btn_apaga" /></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>



    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:lojavirtualOndaMentalConnectionString %>" SelectCommand="SELECT * FROM [tb_utilizadores]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:lojavirtualOndaMentalConnectionString %>" SelectCommand="SELECT * FROM [tb_perfis]"></asp:SqlDataSource>
    <style>
        .checkbox-true {
            background-color: blue !important;
        }

        .checkbox-false {
            background-color: red !important;
        }

        .table {
            font-size: 12px;
            width: 100%;
        }

        th, td {
            padding: 8px;
            text-align: left;
        }

        /* Estilos para os inputs e dropdowns */
        .form-control {
            font-size: 12px;
            padding: 6px;
        }


        .textbox-grande {
            max-width: 200px;
        }

        .textbox-perfil {
            max-width: 80px;
        }

        .textbox-media {
            max-width: 130px;
        }

        .textbox-pequena {
            max-width: 95px;
        }

        /* Estilos para os botões de imagem */
        .img-funcoes {
            width: 20px;
            height: 20px;
        }



        @media screen and (min-width: 1200px) {
            .botao-todos {
                margin-left: 10px;
            }

            .img-adm {
                width: calc(50% - 10px);
                height: 70px;
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


</asp:Content>
