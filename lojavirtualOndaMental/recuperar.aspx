<%@ Page Title="" Language="C#" MasterPageFile="~/template.Master" AutoEventWireup="true" CodeBehind="recuperar.aspx.cs" Inherits="lojavirtualOndaMental.recuperar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <main>
        <section class="sign-in-form section-padding">
            <div class="container">
                <div class="row">

                    <div class="col-lg-8 mx-auto col-12">

                        <h1 class="hero-title text-center mb-5">Recuperação da Palavra-Passe</h1>

                        <div class="row">
                            <div class="col-lg-8 col-11 mx-auto">
                                <div role="form" method="post">


                                        <div class="form-floating">
                                            <!--Email-->
                                            <asp:TextBox class="form-control" placeholder="Email" ID="tb_email" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="tb_email" ErrorMessage="&quot;E-mail é obrigatorio&quot;" ForeColor="Red">*</asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="tb_email" ErrorMessage="E-mail não é valido" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>

                                            <label for="email">Email</label>
                                        </div>


                                        <!--Button recuperar conta-->

                                        <asp:Button class="btn custom-btn form-control mt-4 mb-3 btn btn-dark" ID="btn_trocar_pw" runat="server" Text="Trocar Senha" OnClick="btn_trocar_pw_Click" />


                                        <p class="text-center">Realizar novamente o login?<a href="login.aspx"> Clique aqui</a></p>

                                        <!--Label de avisos gerais -->
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
