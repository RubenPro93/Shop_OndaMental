using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.UI.WebControls;
using Label = System.Reflection.Emit.Label;


namespace lojavirtualOndaMental
{
    public partial class produtos : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
          

            if (!IsPostBack)
            {
                if (Session["logado"] == null || Session["logado"] != "Sim")
                {
                    Response.Redirect("mostra_produtos.aspx");
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
                            if (perfil != 1)
                            {
                                Response.Redirect("mostra_produtos.aspx");
                            }


                        }

                        con.Close();
                    }
                }

                ConstrucaoProdutos();
            }
        }

        private void ConstrucaoProdutos()
        {

            List<prod> listaProdutos = new List<prod>();

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["lojavirtualOndaMentalConnectionString"].ConnectionString))
            {

                string query = "SELECT id_produto, nome, descricao, preco, categoria_id, estoque FROM tb_produtos";

                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myConn.Open();
                    using (SqlDataReader reader = myCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            prod produto = new prod
                            {
                                id_produto = reader.GetInt32(0),
                                nome = reader.GetString(1),
                                descricao = reader.GetString(2),
                                preco = reader.GetDecimal(3),
                                categoria = reader.GetInt32(4),
                                estoque = reader.GetInt32(5),
                                imagens = new List<img>()
                            };
                            listaProdutos.Add(produto);
                        }
                    }

                    // Buscar as imagens para cada produto
                    foreach (prod produto in listaProdutos)
                    {
                        string queryImagens =
                            "SELECT content_type, dados_imagem FROM tb_imagens WHERE produto_id = @produto_id";
                        using (SqlCommand myCommand2 = new SqlCommand(queryImagens, myConn))
                        {
                            myCommand2.Parameters.AddWithValue("@produto_id", produto.id_produto);

                            using (SqlDataReader reader2 = myCommand2.ExecuteReader())
                            {
                                while (reader2.Read())
                                {
                                    img imagem = new img
                                    {
                                        content_type = reader2.GetString(0),
                                        dados_imagem = (byte[])reader2["dados_imagem"]
                                    };
                                    produto.imagens.Add(imagem);
                                }
                            }
                        }
                    }
                }
            }


            Repeater1.DataSource = listaProdutos;
            Repeater1.DataBind();

            // Após o DataBind, verificar para cada produto se há imagens e definir a visibilidade dos controles
            foreach (RepeaterItem item in Repeater1.Items)
            {
                var rptImages = (Repeater)item.FindControl("rptImages");
                var imgDelete = (Button)item.FindControl("img_delete");
                var uploadImagem = (FileUpload)item.FindControl("upload_imagem");

                // Se o rptImages tem itens, significa que existem imagens
                if (rptImages.Items.Count > 0)
                {
                    // Se existe imagem mostre apenas o botão de exclusão
                    imgDelete.Visible = true;

                }
                else
                {
                    // Se não existem imagens, então oculte o botão de exclusão e mostre o FileUpload
                    imgDelete.Visible = false;
                    uploadImagem.Visible = true;
                }
            }
        }





        //classe da BD
        public class prod
        {
            public int id_produto { get; set; }
            public string nome { get; set; }
            public string descricao { get; set; }
            public decimal preco { get; set; }
            public int categoria { get; set; }
            public int estoque { get; set; }
            public List<img> imagens { get; set; }
        }

        public class img
        {
            public string content_type { get; set; }
            public byte[] dados_imagem { get; set; }
        }


        protected void btn_adicionar_Click(object sender, EventArgs e)
        {
            int produtoId = 0;

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["lojavirtualOndaMentalConnectionString"].ConnectionString))
            {

                if (Upload_Imagem.PostedFiles.Count > 4)
                {
                    lbl_erro_file.Text =
                        $"Limite de Imagens: 4, Você está tentando enviar {Upload_Imagem.PostedFiles.Count} imagens";
                    lbl_erro_file.Visible = true;
                    return;
                }
                lbl_erro_file.Visible = false;
                myConn.Open();

                using (SqlCommand myCommand = new SqlCommand("inserir_produto", myConn))
                {
                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("@nome", tb_nome.Text);
                    myCommand.Parameters.AddWithValue("@descricao", tb_descricao.Text);
                    myCommand.Parameters.AddWithValue("@preco", decimal.TryParse(tb_preco.Text, out decimal preco) ? decimal.Round(preco, 2) : 0.0m);

                    myCommand.Parameters.AddWithValue("@categoria_id", Int32.Parse(ddl_categoria.SelectedValue));
                    myCommand.Parameters.AddWithValue("@estoque", Int32.Parse(tb_estoque.Text));

                    SqlParameter outProdutoId = new SqlParameter("@produto_id", SqlDbType.Int);
                    outProdutoId.Direction = ParameterDirection.Output;
                    myCommand.Parameters.Add(outProdutoId);

                    myCommand.ExecuteNonQuery();

                    produtoId = (int)outProdutoId.Value;
                }

                if (Upload_Imagem.HasFiles)
                {
                    foreach (HttpPostedFile postedFile in Upload_Imagem.PostedFiles)
                    {
                        byte[] imgBinaryData = new byte[postedFile.ContentLength];
                        postedFile.InputStream.Read(imgBinaryData, 0, postedFile.ContentLength);

                        using (SqlCommand imgCommand = new SqlCommand("inserir_imagem", myConn))
                        {
                            imgCommand.CommandType = CommandType.StoredProcedure;
                            imgCommand.Parameters.AddWithValue("@produto_id", produtoId);
                            imgCommand.Parameters.AddWithValue("@content_type", postedFile.ContentType);
                            imgCommand.Parameters.AddWithValue("@dados_imagem", imgBinaryData);

                            imgCommand.ExecuteNonQuery();
                        }
                    }
                }
            }

            Response.Redirect(Request.RawUrl);
        }




        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // variavel para armazenar erro
            string errorMessage = "";


            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["lojavirtualOndaMentalConnectionString"].ConnectionString))
            {
                try
                {
                    myConn.Open();
                    SqlCommand mycommand = new SqlCommand();
                    mycommand.Connection = myConn;

                    if (e.CommandName.Equals("btn_grava"))
                    {
                        mycommand.CommandText = "UPDATE tb_produtos SET nome = @nome, descricao = @descricao, preco = PARSE(@preco AS decimal(6,2) USING 'PT-pt'), categoria_id = @categoria_id, estoque = @estoque WHERE id_produto = @id_produto";
                        mycommand.Parameters.AddWithValue("@nome", ((TextBox)e.Item.FindControl("tb_nome")).Text);
                        mycommand.Parameters.AddWithValue("@descricao", ((TextBox)e.Item.FindControl("tb_descricao")).Text);
                        mycommand.Parameters.AddWithValue("@preco", ((TextBox)e.Item.FindControl("tb_preco")).Text);
                        mycommand.Parameters.AddWithValue("@categoria_id", ((DropDownList)e.Item.FindControl("ddl_categoria2")).Text);
                        mycommand.Parameters.AddWithValue("@estoque", ((TextBox)e.Item.FindControl("tb_estoque")).Text);
                        mycommand.Parameters.AddWithValue("@id_produto", Convert.ToInt32(e.CommandArgument));

                        mycommand.ExecuteNonQuery();


                      FileUpload uploadImagem = (FileUpload)e.Item.FindControl("upload_imagem");
                        if (uploadImagem.HasFiles)
                        {
                            foreach (HttpPostedFile postedFile in uploadImagem.PostedFiles)
                            {
                                byte[] imgBinaryData = new byte[postedFile.ContentLength];
                                postedFile.InputStream.Read(imgBinaryData, 0, postedFile.ContentLength);

                                using (SqlCommand imgCommand = new SqlCommand("inserir_imagem", myConn))
                                {
                                    imgCommand.CommandType = CommandType.StoredProcedure;
                                    imgCommand.Parameters.AddWithValue("@produto_id", Convert.ToInt32(e.CommandArgument));
                                    imgCommand.Parameters.AddWithValue("@content_type", postedFile.ContentType);
                                    imgCommand.Parameters.AddWithValue("@dados_imagem", imgBinaryData);

                                    imgCommand.ExecuteNonQuery();
                                }
                            }
                        }

                    }
                    else if (e.CommandName.Equals("btn_apaga"))
                    {

                        // primeiro executa o de excluir as imagens daquele produto
                        SqlCommand mycommand2 = new SqlCommand();
                        mycommand2.Connection = myConn;

                        mycommand2.CommandText = "DELETE FROM tb_imagens WHERE produto_id = @id_produto";
                        mycommand2.Parameters.AddWithValue("@id_produto", Convert.ToInt32(e.CommandArgument));
                        mycommand2.ExecuteNonQuery();

                        // apaga o produto
                        mycommand.CommandText = "DELETE FROM tb_produtos WHERE id_produto = @id_produto";
                        mycommand.Parameters.AddWithValue("@id_produto", Convert.ToInt32(e.CommandArgument));
                        mycommand.ExecuteNonQuery();
                    }
                    else if (e.CommandName == "img_delete")
                    {
                        // Deleta todas as imagens
                        mycommand.CommandText = "DELETE FROM tb_imagens WHERE produto_id = @id_produto";
                        mycommand.Parameters.AddWithValue("@id_produto", Convert.ToInt32(e.CommandArgument));
                        mycommand.ExecuteNonQuery();
                    }

                }
                catch (Exception)
                {
                    errorMessage = "Ocorreu um erro inesperado. Por favor, tente novamente.";
                }

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", $"alert('{errorMessage}');", true);
                }
            }

            // Recarrega a página para refletir as mudanças no banco de dados
            Response.Redirect(Request.RawUrl);
        }
    }
}