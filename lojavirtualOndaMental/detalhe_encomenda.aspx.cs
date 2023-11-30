using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace lojavirtualOndaMental
{
    public partial class detalhe_encomenda : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["logado"] == null || Session["logado"] != "Sim")
                {
                    Response.Redirect("mostra_produtos.aspx");
                }
                using (SqlConnection con = new SqlConnection(ConfigurationManager
                               .ConnectionStrings["lojavirtualOndaMentalConnectionString"].ConnectionString))
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
                            if (perfil != 1)
                            {
                                Response.Redirect("mostra_produtos.aspx");
                            }


                        }

                        con.Close();
                    }
                }


                string utilizadorId = Request.QueryString["utilizador_id"];
                string dataEncomenda = Request.QueryString["data_encomenda"];

                if (!string.IsNullOrEmpty(utilizadorId) && !string.IsNullOrEmpty(dataEncomenda))
                {
                    CarregarDetalhesEncomenda(utilizadorId, dataEncomenda);

                    string connectionString = ConfigurationManager.ConnectionStrings["lojavirtualOndaMentalConnectionString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        string query = "SELECT nome_completo FROM tb_utilizadores WHERE id_utilizador = @UtilizadorId";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@UtilizadorId", utilizadorId);

                            con.Open();
                            string nomeCompleto = (string)cmd.ExecuteScalar();

                            lbl_utilizador.Text = "Encomenda de " + nomeCompleto;
                        }
                    }
                }
            }

        }
        private void CarregarDetalhesEncomenda(string utilizadorId, string dataEncomenda)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["lojavirtualOndaMentalConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT e.*, p.nome
                FROM tb_encomendas AS e
                INNER JOIN tb_produtos AS p ON e.produto_id = p.id_produto
                WHERE e.utilizador_id = @UtilizadorId AND 
                      CONVERT(DATE, e.data_encomenda) = CONVERT(DATE, @DataEncomenda)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UtilizadorId", utilizadorId);
                    cmd.Parameters.AddWithValue("@DataEncomenda", dataEncomenda);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rpt_detalhe_encomenda.DataSource = dt;
                    rpt_detalhe_encomenda.DataBind();
                }
            }
        }

    }
}