<%@ Page Title="" Language="C#" MasterPageFile="~/template.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="lojavirtualOndaMental.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<main>
    <title>Login</title>

    <section class="login-full-page">
        <div class="container login-container">

            <div class="col-sm-offset-4">
                <div class="form-main-inner-box">
                    <div class="logo-box text-center">
                        <img src="img/ondamental_logo.png" style="padding-block: 30px; width: 125px;">
                        <h5 class="h4">Faça login para acessar sua conta</h5>
                    </div>
                    <div class="social-box" style="margin-top: 20px;">
                        <asp:LinkButton ID="ImageButton1" runat="server" CssClass="social-login google" OnClick="Login">
                        <img src="https://cdn.freebiesupply.com/logos/large/2x/google-icon-logo-png-transparent.png" alt="Google Logo" />
                        <span>Google</span>
                        </asp:LinkButton>

                    </div>
                    <div class="social-box" style="margin-top: 15px;">

                        <h4>OU</h4>

                    </div>
                    <div class="box">
                        <div class="form-group">
                            <label>Email * </label>
                            <asp:TextBox ID="tb_email" runat="server" type="text" class="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Password * </label>
                            <asp:TextBox ID="tb_pw" runat="server" type="text" class="form-control" TextMode="Password"></asp:TextBox>
                        </div>
                        <br />
                        <asp:Label ID="lbl_mensagem" runat="server" ForeColor="Red"></asp:Label>
                        <asp:Label ID="lbl_erro_login" runat="server" Visible="False" ForeColor="Red"></asp:Label>
                        <div class="form-group text-center">
                            <asp:Button ID="btn_entrar" runat="server" class="login-btn" style="background-color: black;" Text="Login" OnClick="btn_entrar_Click" />

                            <a href='recuperar.aspx' class="pull-right" style="padding-top: 5px; color: #d21a00; text-decoration: none; font-size: 80%!important; font-weight: 400!important; background-color: transparent;">Esqueceu sua senha?</a>
                        </div>
                    </div>
                </div>
                <div class="texto-dica small text-cad">
                    Ainda não tem uma conta? <a href='registro_utilizador.aspx' style="color: #00a50b; text-decoration: none; background-color: transparent;"> Cadastre-se agora!</a>
                </div>
            </div>

        </div>
    </section>
    </main>

    <style>
        body, html {
            min-height: 100%;
        }

        .login-container {
            display: contents !important;
        }

        .login-full-page {
            display: flex;
            padding: 20px;
            background: #f5f5f5;
            height: 100%;
            justify-content: center;
        }

        .social-box {
            display: flex;
            align-items: center;
            justify-content: center;
        }

            .social-box .social-login {
                background: rgba(0, 0, 0, 0);
                border: 1px solid #ababab;
                padding: 15px 55px;
                border-radius: 4px;
                transition: all .60s;
            }

                .social-box .social-login:hover {
                    background: #ff685c;
                    color: #fff;
                }

                .social-box .social-login img {
                    width: 20px;
                    position: relative;
                    left: -20px;
                }

        .social-login.google {
            text-decoration: none;
            color: black;
        }

            .social-login.google span {
                text-decoration: none;
                color: black;
                vertical-align: middle;
            }

        .form-main-inner-box {
            background: #fff;
            padding: 20px;
            box-shadow: 0px 0px 15px 2px #CCC;
            margin-top: 8%;
        }

        .login-btn {
            width: 100%;
            background: #193076;
            border: none;
            padding: 10px 20px;
            color: #fff;
            font-size: 16px;
            font-weight: bold;
        }

        .form-group a {
            display: block;
            color: #333;
        }

        .text-cad {
            padding-top: 5px;
            color: rgb(0, 0, 0);
            display: flex;
            font-size: 80% !important;
            font-weight: 400 !important;
            justify-content: center;
        }

        @media (max-width: 767px) {
            .social-box .social-login {
                width: 100%;
                margin-bottom: 10px;
            }
        }
    </style>


</asp:Content>
