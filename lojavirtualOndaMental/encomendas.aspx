<%@ Page Title="" Language="C#" MasterPageFile="~/template.Master" AutoEventWireup="true" CodeBehind="encomendas.aspx.cs" Inherits="lojavirtualOndaMental.encomendas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    
    <div class="head">
        <p id="p">Lista De Utilizadores</p>
        <div class="s"></div>
    </div>
    <asp:Repeater ID="rpt_lista_encomendas" runat="server" DataSourceID="SqlDataSource1" OnItemDataBound="rpt_lista_encomendas_ItemDataBound">


        <HeaderTemplate>
            <!-- Content Table -->
            <div class="container">
                <table>
                    <thead>
                        <th>ID Utilizador</th>
                        <th>Nome Completo</th>
                        <th>Produtos</th>
                        <th>Data Encomenda</th>
                        <th>Data Entrega</th>
                        <th>Preço Total</th>

                    </thead>
                    <tbody>
        </HeaderTemplate>


        <ItemTemplate>

            <tr>
                <td>
                    <asp:Label ID="lbl_utilizador_id" runat="server"></asp:Label></td>

                <td>
                    <asp:Label ID="lbl_nome_utilizador" runat="server"></asp:Label></td>

                <td>
                    <asp:Label ID="lbl_produtos_id" runat="server"></asp:Label></td>

                <td>
                    <asp:Label ID="lbl_data_encomenda" runat="server"></asp:Label></td>

                <td>
                    <asp:Label ID="lbl_data_entrega" runat="server"></asp:Label></td>

                <td>
                    <asp:Label ID="lbl_preco" runat="server"></asp:Label></td>
                
                <td>
                    <asp:Image ID="img_entrega"  runat="server"  class="fa fa-check"/>
                </td>
               
                <td><asp:LinkButton ID="lb_detalhe" runat="server" OnClick="lb_detalhe_Click"><i class="fa fa-angle-right arrow-right"></i></asp:LinkButton></td>
            </tr>

        </ItemTemplate>

        <FooterTemplate>
            </tbody>
            </table>
            </div>

        </FooterTemplate>



    </asp:Repeater>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:lojavirtualOndaMentalConnectionString %>" SelectCommand="SELECT e.utilizador_id, CAST(e.data_encomenda AS DATE) as data_encomenda, CAST(e.data_entrega AS DATE) as data_entrega, u.nome_completo, SUM(e.preco) AS total_preco, STUFF((SELECT ', ' + p.nome FROM tb_produtos p WHERE p.id_produto IN (SELECT produto_id FROM tb_encomendas WHERE utilizador_id = e.utilizador_id AND CAST(data_encomenda AS DATE) = CAST(e.data_encomenda AS DATE)) FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS lista_produtos FROM tb_encomendas e INNER JOIN tb_utilizadores u ON e.utilizador_id = u.id_utilizador  GROUP BY e.utilizador_id, CAST(e.data_encomenda AS DATE), CAST(e.data_entrega AS DATE), u.nome_completo ORDER BY CAST(e.data_encomenda AS DATE) DESC"></asp:SqlDataSource>



    <style>
        body {
            background-color: #eff3f6;
        }

        .EntregaFeita{
            color: #0097ff;
            border-bottom-style: dotted;
            border-radius: 30%;
        }
        .EntregaNaoFeita{
            color: #ff0000;
            border-bottom-style: dotted;
            border-radius: 30%;
        }

       

     
        table {
            width: 100%;
            border-collapse: separate;
            border-spacing: 0px 8px;
        }

        th {
            text-align: left;
            padding: 5px;
            text-transform: uppercase;
            font-weight: 100;
            font-size: 11px;
            color: #aab3bb;
        }

        tr {
            box-shadow: 1px 1px 1px rgba(228, 228, 228, 0.25);
        }

            tr thead {
                background: transparent !important;
                border-color: transparent !important;
            }

            tr td {
                background-color: #ffffff;
                border-bottom: 1px solid #e7e7e7;
                font-size: 12px;
                font-weight: bold;
            }

        td {
            padding: 13px;
        }

        img {
            height: 30px;
        }

        /* Icons */
        .fa {
            padding: 0px 9px;
            color: #c1c4c9;
            font-size: 1.1em;
        }

        .arrow-right {
            font-size: 1.9em;
            float: right;
        }


        /* Circle */
        .large {
            background: #20c73a;
            width: 18px;
            height: 18px;
        }

        .medium {
            margin-top: 2px;
            background: #ffffff;
            width: 14px;
            height: 14px;
        }

        .small {
            margin-top: 2px;
            background: #20c73a;
            width: 10px;
            height: 10px;
        }

        .circle {
            display: inline-block;
            text-align: center;
            border-radius: 50%;
        }
    </style>
    
</asp:Content>
