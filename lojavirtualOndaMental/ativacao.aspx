<%@ Page Title="" Language="C#" MasterPageFile="~/template.Master" AutoEventWireup="true" CodeBehind="ativacao.aspx.cs" Inherits="lojavirtualOndaMental.ativacao" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<main>
    <section class="sign-in-form section-padding">
        <div class="container">
            <div class="row">

                <div class="col-lg-8 mx-auto col-12">

                    <h1 class="hero-title text-center mb-5">Ativação da conta</h1>

                    <div class="row">
                        <div class="col-lg-8 col-11 mx-auto">
                            <div role="form" method="post">

                             <!--Label de avisos ativacao -->
                         <asp:Label class="text-center" ID="lbl_mensagem" runat="server"></asp:Label>

                          </div>
                       
                        </div>

                    </div>
                    </div>
                </div>
            </div>
    </section>
</main>


        <!-- JAVASCRIPT FILES -->
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.bundle.min.js"></script>
    <script src="js/Headroom.js"></script>
    <script src="js/jQuery.headroom.js"></script>
    <script src="js/slick.min.js"></script>
    <script src="js/custom.js"></script>

</asp:Content>
