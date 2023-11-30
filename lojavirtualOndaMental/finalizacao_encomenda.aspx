<%@ Page Title="" Language="C#" MasterPageFile="~/template.Master" AutoEventWireup="true" CodeBehind="finalizacao_encomenda.aspx.cs" Inherits="lojavirtualOndaMental.finalizacao_encomenda" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h4>PRODUTOS CARRINHO</h4>


   <asp:Repeater ID="rptCarrinho" runat="server">
        <ItemTemplate>
            <div>
                <span><%# Eval("Quantidade", "(Qtd: {0}) -") %></span>
                <span><%# Eval("Nome") %></span>
                <span><%# Eval("Preco", " - {0:C}") %></span>
                 <asp:Label ID="lblSubtotal" runat="server" 
                    Text='<%#  String.Format(" - Subtotal = {0:C}", Convert.ToDecimal(Eval("Preco")) * Convert.ToInt32(Eval("Quantidade"))) %>'>
                </asp:Label>
            </div> 
        </ItemTemplate>
    </asp:Repeater>      
    <br />
   
    <asp:Label ID="lbl_valorTotal" runat="server" Text=""></asp:Label>
    <br />
    <asp:Label ID="lbl_produtoTotal" runat="server" Text=""></asp:Label>

    <br />
    <br />

    <h4>INFORMAÇÕES PARA ENTREGA DA ENCOMENDA</h4>

    <!--DADOS DO UTILIZADOR PARA A FATURA-->
    Nome:
    <asp:Label ID="lbl_nome" runat="server" ></asp:Label>
    <br />
    E-mail:
    <asp:Label ID="lbl_email" runat="server" ></asp:Label>
    <br />
    NIF:
    <asp:Label ID="lbl_nif" runat="server" Text="Label"></asp:Label>
    <asp:TextBox ID="txtNif" runat="server" Visible="false"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvNif" runat="server" ControlToValidate="txtNif" ErrorMessage="NIF é obrigatório" Visible="false"></asp:RequiredFieldValidator>
    <br />
    Telemovel:
    <asp:Label ID="lbl_telemovel" runat="server"></asp:Label>
    <asp:TextBox ID="txtTelemovel" runat="server" Visible="false"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvTelemovel" runat="server" ControlToValidate="txtTelemovel" ErrorMessage="Telemóvel é obrigatório" Visible="false"></asp:RequiredFieldValidator>
    <br />
    Morada:
    <asp:Label ID="lbl_morada" runat="server"></asp:Label>
    <asp:TextBox ID="txtMorada" runat="server" Visible="false"></asp:TextBox>
    <asp:RequiredFieldValidator ID="rfvMorada" runat="server" ControlToValidate="txtMorada" ErrorMessage="Morada é obrigatória" Visible="false"></asp:RequiredFieldValidator>

    <br />
    <br />
    <br />

    <asp:Label ID="lbl_prod" runat="server" Text="" Visible="false"></asp:Label>

    
    <asp:Button ID="btn_concluir_encomenda" runat="server" Text="CONCLUIR ENCOMENDA" onclick="btn_concluir_encomenda_Click"/>    
    <br />
    <a style="color:red;">Ao clicar em "Concluir, vamos enviar um e-mail com sua fatura</a>
    <br />
    <br />
    <br />
    <!--Retornar a pagina de produtos-->
   Continuar comprando? <a href='mostra_produtos.aspx'>Clique aqui</a>

</asp:Content>
