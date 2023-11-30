<%@ Page Title="" Language="C#" MasterPageFile="~/template.Master" AutoEventWireup="true" CodeBehind="detalhe_encomenda.aspx.cs" Inherits="lojavirtualOndaMental.detalhe_encomenda" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="container mt-3">
       
       <h1><asp:Label ID="lbl_utilizador" runat="server"></asp:Label></h1>
        <div class="row">
            <div class="col-md-12">
                <asp:Repeater ID="rpt_detalhe_encomenda" runat="server">
                    <HeaderTemplate>
                        <table class="table table-striped table-bordered">
                        <thead class="thead-dark">
                        <tr>
                            <th>ID</th>
                            <th>Nome Produto</th>
                            <th>Quantidade</th>
                            <th>Data Encomenda</th>
                            <th>Data Entrega</th>
                            <th>Preço</th>
                            
                        </tr>
                        </thead>
                        <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("id_encomenda") %></td>
                            <td><%# Eval("nome") %></td>
                            <td><%# Eval("quantidade") %></td>
                            <td><%# Eval("data_encomenda") %></td>
                            <td><%# Eval("data_entrega") %></td>
                            <td><%# Eval("preco") %></td>
                       
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                    </tbody>
                    </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
   <style>@charset "UTF-8";
@import url(https://fonts.googleapis.com/css?family=Open+Sans:300,400,700);


.container td:first-child { color: #FB667A; }

.container tr:hover {
  background-color: #494949;
    -moz-box-shadow: 0 6px 6px -6px #0E1119!important;
    box-shadow: 2px 8px 6px -6px #0E1119;
    -webkit-box-shadow: -7px 3px 6px -6px #0E1119;
	
}

.container td:hover {
   background-color: #000000;
    color: #ffffff;
    font-weight: bold;
    box-shadow: #121212 -1px 1px, #121212 -2px 2px, #121212 -3px 3px, #121212 -4px 4px, #121212 -5px 5px, #121212 -6px 6px;
    transform: translate3d(6px, -6px, 0);
    transition-delay: 0s;
    transition-duration: 0.4s;
    transition-property: all;
  transition-timing-function: line;
}

@media (max-width: 800px) {
.container td:nth-child(4),
.container th:nth-child(4) { display: none; }
}</style>

</asp:Content>
