<%@ Page Title="" Language="C#" MasterPageFile="~/template.Master" AutoEventWireup="true" CodeBehind="registro_utilizador.aspx.cs" Inherits="lojavirtualOndaMental.registro_utilizador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <main>
        <section class="sign-in-form section-padding">
            <div class="container">
                <div class="row">

                    <div class="col-lg-8 mx-auto col-12">

                        <h1 class="hero-title text-center mb-5">Registar Conta</h1>




                        <div class="row">
                            <div class="col-lg-8 col-11 mx-auto">
                                <div role="form" method="post">

                                    <div class="form-floating">
                                        <!--Nome Completo-->
                                        <asp:TextBox class="form-control" placeholder="Nome Completo" ID="tb_nome" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_nome" ErrorMessage="&quot;Nome é obrigatorio&quot;" ForeColor="Red">*</asp:RequiredFieldValidator>

                                        <label for="nome">Nome Completo</label>
                                    </div>

                                    <div class="form-floating">
                                        <!--Nif-->
                                        <asp:TextBox class="form-control" placeholder="Nif" ID="tb_nif" runat="server"></asp:TextBox>

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="tb_nif" ErrorMessage="&quot;Nif é obrigatorio&quot;" ForeColor="Red">*</asp:RequiredFieldValidator>

                                        <label for="nif">Nif</label>
                                    </div>


                                    <div class="form-floating">
                                        <!--Sexo-->
                                        <asp:DropDownList class="form-control" ID="ddl_sexo" runat="server" placeholder="Género">
                                            <asp:ListItem>Masculino</asp:ListItem>
                                            <asp:ListItem>Feminino</asp:ListItem>
                                        </asp:DropDownList>

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="ddl_sexo" ErrorMessage="&quot;Género é obrigatorio&quot;" ForeColor="Red">*</asp:RequiredFieldValidator>
                                        <label for="genero">Género</label>
                                    </div>


                                    <div class="form-floating">
                                        <!--Nascimento-->
                                        <asp:TextBox ID="tb_nascimento" runat="server" TextMode="Date" class="form-control" placeholder="Data de Nascimento"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="tb_nascimento" ErrorMessage="&quot;Data de Nascimento é obrigatorio&quot;" ForeColor="Red">*</asp:RequiredFieldValidator>
                                        <label for="nascimento">Data de Nascimento</label>
                                    </div>

                                    <div class="form-floating">
                                        <!--Telemovel-->
                                        <asp:TextBox ID="tb_telefone" runat="server" class="form-control" placeholder="Telemóvel"></asp:TextBox>

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="tb_telefone" ErrorMessage="&quot;Telemóvel é obrigatorio&quot;" ForeColor="Red">*</asp:RequiredFieldValidator>

                                        <label for="telemovel">Telemóvel</label>
                                    </div>


                                    <div class="form-floating">
                                        <!--Morada-->
                                        <asp:TextBox ID="tb_morada" runat="server" class="form-control" placeholder="Morada"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="tb_morada" ErrorMessage="&quot;Morada é obrigatorio&quot;" ForeColor="Red">*</asp:RequiredFieldValidator>

                                        <label for="morada">Morada</label>
                                    </div>


                                    <div class="form-floating">
                                        <!--Email-->
                                        <asp:TextBox class="form-control" placeholder="Email" ID="tb_email" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="tb_email" ErrorMessage="&quot;E-mail é obrigatorio&quot;" ForeColor="Red">*</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="tb_email" ErrorMessage="E-mail não é valido" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>

                                        <label for="email">Email</label>
                                    </div>

                                    <div class="form-floating my-4">
                                        <!--Password-->
                                        <asp:TextBox class="form-control" placeholder="Palavra Passe" ID="tb_pw" runat="server" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="tb_pw" ErrorMessage="&quot;Palavra-Passe é obrigatorio&quot;" ForeColor="Red">*</asp:RequiredFieldValidator>

                                        <label for="password">Password</label>
                                    </div>

                                    <!--Button criar conta-->

                                    <asp:Button class="btn custom-btn form-control mt-4 mb-3 btn btn-dark" ID="tb_registrar" runat="server" Text="Criar Conta" OnClick="tb_registrar_Click1" />

                                    <p class="text-center">já tem uma conta?<a href="login.aspx"> Faça login</a></p>

                                    <!--Label de avisos gerais -->
                                    <asp:Label class="text-center" ID="lbl_mensagem" runat="server"></asp:Label>
                                    <asp:ValidationSummary class="text-center" ID="ValidationSummary1" runat="server" />


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
