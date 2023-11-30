using ASPSnippets.GoogleAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace lojavirtualOndaMental
{
    public partial class template : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
         
            if (!IsPostBack)
            {
                

                if (Convert.ToString(Session["logado"]) == "Sim")
                {
                    btn_sair_conta.Visible = true; //aparece botao de sair
                    if (Session["loginGoogle"] != null)
                    {
                        lbl_alterarPW.Visible = false;
                    }
                    else
                    {
                        lbl_alterarPW.Visible = true;
                    }

                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["lojavirtualOndaMentalConnectionString"].ConnectionString))
                    {
                        string query = "SELECT perfil_id FROM tb_utilizadores WHERE id_utilizador = @UtilizadorId";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@UtilizadorId", Session["utilizador_id"].ToString());

                            con.Open();
                            object result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                int perfil = Convert.ToInt32(result);
                                if (perfil == 1)
                                {

                                    lb_nav2.Text = "Utilizadores";
                                    lb_nav2.PostBackUrl = "utilizadores.aspx";
                                    lb_nav2.Visible = true;

                                    lb_nav3.Text = "Produtos";
                                    lb_nav3.PostBackUrl = "produtos.aspx";
                                    lb_nav3.Visible = true;

                                    lb_nav4.Text = "Encomendas";
                                    lb_nav4.PostBackUrl = "encomendas.aspx";
                                    lb_nav4.Visible = true;
                                }
                                
                              
                            }

                            con.Close();
                        }
                    }

                }

              

            }
        }

        protected void btn_sair_conta_Click(object sender, EventArgs e)
        {
                Session["logado"] = "Não"; // fica sem estar logado
                btn_sair_conta.Visible = false; // esconde botão de sair
                lbl_alterarPW.Visible = false; // esconde label de trocar pw
           

            if (Session["loginGoogle"] != null)
            {
                String code = Request.QueryString["code"];
                GoogleConnect.Clear(Request.QueryString[code]);
            }
            Response.Redirect("login.aspx"); // redireciona para a página de produtos
        }
    }
}