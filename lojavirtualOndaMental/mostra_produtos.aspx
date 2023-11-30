<%@ Page Title="" Language="C#" MasterPageFile="~/template.Master" AutoEventWireup="true" CodeBehind="mostra_produtos.aspx.cs" Inherits="lojavirtualOndaMental.mostra_produtos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

  
    <!-- Carrinho -->
    <div class="modal fade" id="modalCarrinho" tabindex="-1" role="dialog" aria-labelledby="modalCarrinhoLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalCarrinhoLabel">Carrinho de Compras</h5>
                
                    <button type="button" class="close" data-dismiss="modal" aria-label="Fechar" onclick="fecharModalCarrinho()">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lbl_mensagem" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lbl_totalCarrinho" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lbl_totalProdutos" runat="server" Text=""></asp:Label>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal" onclick="fecharModalCarrinho()">Continuar Comprando</button>
                    <asp:Button ID="btn_finalizarCompra" runat="server" Text="Finalizar Compra" class="btn btn-primary" onclick="btn_finalizarCompra_Click"/>
                </div>
            </div>
        </div>
    </div>



<div class="container">

    ORDENAR: <asp:DropDownList ID="ddl_filtro" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_filtro_SelectedIndexChanged">
        <asp:ListItem>Aleatório</asp:ListItem>
        <asp:ListItem>Produto (A - Z)</asp:ListItem>
        <asp:ListItem>Produto (Z - A)</asp:ListItem>
        <asp:ListItem>Preço (Baixo / Alto)</asp:ListItem>
        <asp:ListItem>Preço (Alto / Baixo)</asp:ListItem>
    </asp:DropDownList>
    &nbsp;

    <asp:TextBox ID="tb_pesquisa" runat="server"></asp:TextBox> <asp:Button ID="btn_filtrar" runat="server" Text="FILTRAR" onclick="btn_filtrar_Click"/>


    <div class="d-flex align-items-center my-3">
      <span class="px-2 bg-white text-uppercase font-weight-bold">Produtos</span>
      <hr class="flex-grow-1"></hr>
    </div>

    <div class ="row">
    <asp:Repeater ID="rptProdutos" runat="server" OnItemCommand="rptProdutos_ItemCommand" OnItemDataBound="rptProdutos_ItemDataBound">
<ItemTemplate>
    <div class="col-md-4">
        <div class="card mb-4 box-shadow">
            <!-- Carousel -->
            <div id='carousel<%# Eval("id_produto") %>' class='carousel slide' data-ride='carousel'>
                <!-- Wrapper for slides -->
                <div class='carousel-inner'>
                    <%# Pegar_Itens_Carrossel_imagens(Eval("Imagens")) %>
                </div>
                <!-- setas < > -->
                <a class='carousel-control-prev' href='#carousel<%# Eval("id_produto") %>' role='button' data-slide='prev'>
                    <span class='carousel-control-prev-icon' aria-hidden='true'></span>
                    <span class='sr-only'>Anterior</span>
                </a>
                <a class='carousel-control-next' href='#carousel<%# Eval("id_produto") %>' role='button' data-slide='next'>
                    <span class='carousel-control-next-icon' aria-hidden='true'></span>
                    <span class='sr-only'>Próximo</span>
                </a>
            </div>
            <!-- card body -->
            <div class="card-body">
                <h5 class="card-title"><%# Eval("nome") %></h5>
                <p class="card-text"><%# Eval("descricao") %></p>
                <div class="d-flex justify-content-between align-items-center">
                    <small class="text-muted"><%# Eval("preco") %></small>
                </div>
            </div>
            
            <!--botao adicionar item-->
             <asp:Button ID="btn_addCarrinho" runat="server" CommandName="btn_addCarrinho" Text="Adicionar ao Carrinho" CssClass="btn btn-primary" />
            <!--botao remover item-->
             <asp:Button ID="btn_removerCarrinho" runat="server" CommandName="btn_removerCarrinho" Text="Remover" CssClass="btn btn-danger"/>
        </div>
    </div>
</ItemTemplate>
    </asp:Repeater>
    </div>
   
     <div style="text-align:center;">
    <asp:LinkButton ID="lnkPrevious" runat="server" OnClick="lnkPrevious_Click">Anterior</asp:LinkButton>
    &nbsp;
    <asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click">Próximo</asp:LinkButton>
</div>

  </div>

       
 <!--JAVASCRIPT DO CHAT TALK.TO-->
<script type="text/javascript">
    var Tawk_API = Tawk_API || {}, Tawk_LoadStart = new Date();
    (function () {
        var s1 = document.createElement("script"), s0 = document.getElementsByTagName("script")[0];
        s1.async = true;
        s1.src = 'https://embed.tawk.to/654e8933958be55aeaae82df/1hetcfsh3';
        s1.charset = 'UTF-8';
        s1.setAttribute('crossorigin', '*');
        s0.parentNode.insertBefore(s1, s0);
    })();
</script>
 <!--FIM JAVASCRIPT DO CHAT TALK.TO-->
       

    
<style>
       
    .btnPageClass:hover {
        background-color: #e69500;
    }
    .img-produto {
object-fit: cover;
background-color: white;
width: 280px;
height: 320px;
background-size: cover;
background-position: center;
background-repeat: no-repeat;
transform: scale(1);
}
</style>
</asp:Content>
