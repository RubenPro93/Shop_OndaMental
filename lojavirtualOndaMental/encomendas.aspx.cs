using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static lojavirtualOndaMental.encomendas;
using static lojavirtualOndaMental.produtos;

namespace lojavirtualOndaMental
{
    public partial class encomendas : System.Web.UI.Page
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
            }

        }

        protected void rpt_lista_encomendas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dr = (DataRowView)e.Item.DataItem;

                ((Label)e.Item.FindControl("lbl_utilizador_id")).Text = dr["utilizador_id"].ToString();
                ((Label)e.Item.FindControl("lbl_nome_utilizador")).Text = dr["nome_completo"].ToString();
                ((Label)e.Item.FindControl("lbl_produtos_id")).Text = dr["lista_produtos"].ToString();
                ((Label)e.Item.FindControl("lbl_data_encomenda")).Text = ((DateTime)dr["data_encomenda"]).ToString("dd-MM-yyyy");
                DateTime dataEntrega = Convert.ToDateTime(dr["data_entrega"].ToString());
                ((Label)e.Item.FindControl("lbl_data_entrega")).Text = dataEntrega.ToString("dd-MM-yyyy");

                ((Label)e.Item.FindControl("lbl_preco")).Text = dr["total_preco"].ToString();
                
                

                Image imgEntrega = (Image)e.Item.FindControl("img_entrega");
                //design para entrega, caso nao entrega fica vermelho, se entregou fica azul
                if (dataEntrega < DateTime.Now)
                {
                    imgEntrega.ImageUrl = "img/entrega/entregou.png";
                    imgEntrega.CssClass = "EntregaFeita";
                }
                else
                {
                    imgEntrega.ImageUrl = "img/entrega/naoentregou.png";
                    imgEntrega.CssClass = "EntregaNaoFeita";
                }

                ((LinkButton)e.Item.FindControl("lb_detalhe")).CommandArgument = dr["utilizador_id"] + ";" + ((DateTime)dr["data_encomenda"]).ToString("yyyy-MM-dd");

                
            }
        }

        protected void lb_detalhe_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            string idEncomenda = lb.CommandArgument;
            string[] args = idEncomenda.Split(';');
            string utilizadorId = args[0];
            string dataEncomenda = args[1];
            Response.Redirect($"detalhe_encomenda.aspx?utilizador_id={utilizadorId}&data_encomenda={dataEncomenda}");

           
        }

          




    }
}