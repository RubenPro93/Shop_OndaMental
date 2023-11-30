<%@ Page Title="" Language="C#" MasterPageFile="~/template.Master" AutoEventWireup="true" CodeBehind="alterar_pw.aspx.cs" Inherits="lojavirtualOndaMental.alterar_pw" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <main>
        <section class="sign-in-form section-padding">
            <div class="container">
                <div class="row">

                    <div class="col-lg-8 mx-auto col-12">

                        <h1 class="hero-title text-center mb-5">Alterar Palavra-Passe</h1>



                        <div class="row">
                            <div class="col-lg-8 col-11 mx-auto">
                                <div role="form" method="post">


                                    <div class="form-floating">
                                        <!--Palavra-passe antiga-->
                                        <asp:TextBox class="form-control" placeholder="Palavra-Passe antiga" ID="tb_pw_antiga" runat="server" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_pw_antiga" ErrorMessage="&quot;Palavra-Passe antiga é obrigatorio&quot;" ForeColor="Red">*</asp:RequiredFieldValidator>

                                        <label for="palavrapasseantiga">Palavra-passe antiga</label>
                                    </div>

                                    <div class="form-floating">
                                        <!--Palavra-passe nova-->
                                        <asp:TextBox class="form-control" placeholder="Palavra-Passe nova" ID="tb_pw_nova" runat="server" type="password"></asp:TextBox>
                                        <label for="palavrapassenova">Palavra-passe nova</label>
                                    </div>

                                    <div class="form-floating">
                                        <!--Confirme a Palavra-passe-->
                                        <asp:TextBox class="form-control" placeholder="Confirme a Palavra-Passe" ID="tb_pw_nova_confirm" runat="server" type="password"></asp:TextBox>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="tb_pw_nova_confirm" ControlToValidate="tb_pw_nova" ErrorMessage="Palavra-passe não são iguais!!" ForeColor="Red">Palavra-passe não são iguais!!</asp:CompareValidator>
                                        <label for="palavrapasseconfirme">Confirme a Palavra-Passe</label>
                                    </div>



                                    <!--avisos gerais-->
                                    <asp:Label class="text-center" ID="lbl_mensagem" runat="server"></asp:Label>
                                    <asp:Label class="text-center" ID="lbl_mensagemFRACA" runat="server"></asp:Label>
                                    <asp:ValidationSummary class="text-center" ID="ValidationSummary1" runat="server" />

                                    <!--Botao confirmar-->

                                    <asp:Button class="btn custom-btn form-control mt-4 mb-3 btn btn-dark" ID="tb_alterar_pw" runat="server" Text="Confirmar Alteração" OnClick="tb_alterar_pw_Click" />

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
