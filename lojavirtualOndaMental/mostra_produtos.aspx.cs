using GoogleApi.Entities.Maps.DistanceMatrix.Response;
using iTextSharp.text;
using iTextSharp.text.pdf.spatial.units;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static iTextSharp.text.pdf.AcroFields;
using static System.Net.Mime.MediaTypeNames;

namespace lojavirtualOndaMental
{
    public partial class mostra_produtos : System.Web.UI.Page
    {
        //Instancia o objeto de paginação
        protected void Page_Load(object sender, EventArgs e)
        {

       
            if (!IsPostBack)
            {
                ViewState["CurrentQuery"] = "SELECT id_produto, nome, descricao, preco FROM tb_produtos";
                CurrentPage = 1;
                pageQuerys(ViewState["CurrentQuery"].ToString(), CurrentPage, 6);
            }

            //usuário realiza login com sucesso e puxa o carrinho caso tenha
            if (Session["logado"] == "Sim")
            {
                int utilizadorID = Convert.ToInt32(Session["utilizador_id"]);
                CarregarCarrinhoBD(utilizadorID); //carregar o carrinho da BD
                AtualizarCarrinhoNaLabel(); //atualiza o carrinho com as labels na tela

            } else if(Session["logado"] != "Sim")
            {
                lbl_mensagem.Text = "Não Logado";
                
            }

            
        }


        public int CurrentPage
        {
            get { return ViewState["CurrentPage"] != null ? Convert.ToInt32(ViewState["CurrentPage"]) : 1; }
            set { ViewState["CurrentPage"] = value; }
        }



        //ddl para filtrar as pesquisar de acordo com o utilizador escolher
        protected void ddl_filtro_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = "SELECT id_produto, nome, descricao, preco FROM tb_produtos ";

            switch (ddl_filtro.SelectedItem.Text)
            {
                case "Aleatório":
                    break;
                case "Produto (A - Z)":
                    query += "ORDER BY nome ASC ";
                    break;
                case "Produto (Z - A)":
                    query += "ORDER BY nome DESC ";
                    break;
                case "Preço (Baixo / Alto)":
                    query += "ORDER BY preco ASC ";
                    break;
                case "Preço (Alto / Baixo)":
                    query += "ORDER BY preco DESC ";
                    break;
                default:
                    break;
            }

            ViewState["CurrentQuery"] = query;
            CurrentPage = 1;
            pageQuerys(query, 1, 6);

        }
        
        protected void btn_filtrar_Click(object sender, EventArgs e)
        {
            string query = "select id_produto, nome, descricao, preco from tb_produtos WHERE nome LIKE '%" + tb_pesquisa.Text + "%'";
            CurrentPage = 1;
            pageQuerys(query, 1, 6);
        }

        private void pageQuerys(string query, int pageAtual, int tamPage)
        {
            int offset = (pageAtual - 1) * tamPage;
            if (offset < 0) offset = 0;

            // Adiciona 'ORDER BY' apenas se não estiver presente na consulta
            if (!query.Trim().ToUpper().Contains("ORDER BY"))
            {
                query += " ORDER BY nome";
            }

            string queryPaginada = query + $" OFFSET {offset} ROWS FETCH NEXT {tamPage} ROWS ONLY";
            ProdutosAoRepeater(queryPaginada);
        }


        protected void lnkPrevious_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                pageQuerys(ViewState["CurrentQuery"].ToString(), CurrentPage, 6);
            }
        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            pageQuerys(ViewState["CurrentQuery"].ToString(), CurrentPage, 6);

        }




        private void ProdutosAoRepeater(string query)
        {
             

            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["lojavirtualOndaMentalConnectionString"].ConnectionString);


            DataTable dtProdutos = new DataTable();

            using (SqlCommand myCommand = new SqlCommand(query, myConn))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(myCommand))
                {
                    sda.Fill(dtProdutos);
                }
            }

            // add nova coluna para armazenar as imagens em base64
            dtProdutos.Columns.Add("Imagens", typeof(List<string>));

            myConn.Open(); 

            foreach (DataRow row in dtProdutos.Rows)
            {
                int produtoId = (int)row["id_produto"];
                SqlCommand cmdImagens = new SqlCommand("SELECT content_type, dados_imagem FROM tb_imagens WHERE produto_id = @produtoId", myConn);
                cmdImagens.Parameters.AddWithValue("@produtoId", produtoId);

                List<string> imagensBase64 = new List<string>();

                using (SqlDataReader reader = cmdImagens.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        byte[] dadosImagem = (byte[])reader["dados_imagem"];
                        string contentType = reader["content_type"].ToString();
                        string imgBase64 = $"data:{contentType};base64,{Convert.ToBase64String(dadosImagem)}";
                        imagensBase64.Add(imgBase64);
                    }
                }

                // Armazena a lista de imagens na nova coluna
                row["Imagens"] = imagensBase64;
            }

            myConn.Close(); // Fecha a conexão

            // Define o DataTable como fonte de dados para o Repeater
            rptProdutos.DataSource = dtProdutos;
            rptProdutos.DataBind();

        }


        /* 
         * Função para criar cada item do carrossel com as imagens do produto
         *
         *
         */
        protected string Pegar_Itens_Carrossel_imagens(object imagens)
        {
            var Lista_imagens = (List<string>)imagens;
            var items = new StringBuilder();
            for (int i = 0; i < Lista_imagens.Count; i++)
            {
                items.AppendFormat("<div class='carousel-item{1}'><img src='{0}' class='d-block w-100 img-produto lazy card-img-top' alt='...'  ></div>", Lista_imagens[i], i == 0 ? " active" : "");
            }
            return items.ToString();
        }




        protected void rptProdutos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dr = (DataRowView)e.Item.DataItem;

                ((Button)e.Item.FindControl("btn_addCarrinho")).CommandArgument = dr["id_produto"].ToString();
                ((Button)e.Item.FindControl("btn_removerCarrinho")).CommandArgument = dr["id_produto"].ToString();

            }
        }

       
        public class ProdutoCarrinho{
            public int Id { get; set; }
            public string Nome { get; set; }
            public decimal Preco { get; set; }

            public int Quantidade { get; set; }
        
        }

        protected void rptProdutos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("btn_addCarrinho"))
            {
                int produtoId = Convert.ToInt32(e.CommandArgument);
                AdicionarProdutoAoCarrinho(produtoId);

            }

            else if (e.CommandName == "btn_removerCarrinho")
            {
                int produtoId = Convert.ToInt32(e.CommandArgument);
                RemoverProdutoDoCarrinho(produtoId);
                
            }

            AtualizarCarrinhoNaLabel();

            if (Session["logado"] == "Sim") {
                SalvarCarrinhoBD(); //Se estiver logado, salva automaticamente os dados no carrinho da BD....
            }
              
        }



        //remover item do carrinho
        private void RemoverProdutoDoCarrinho(int produtoId)
        {
            var carrinho = (List<ProdutoCarrinho>)Session["Carrinho"];
            var produtoParaRemover = carrinho.Find(p => p.Id == produtoId);

            //se tiver produto, irá decrementar.. quando chegar a 0, remove total.
            if (produtoParaRemover != null)
            {
                produtoParaRemover.Quantidade--;
                if (produtoParaRemover.Quantidade <= 0)
                {
                    carrinho.Remove(produtoParaRemover);
                }
                Session["Carrinho"] = carrinho;
            }
        }



        //adicionar item no carrinho
        private void AdicionarProdutoAoCarrinho(int produtoId)
        {
            List<ProdutoCarrinho> carrinho;

            if (Session["Carrinho"] == null)
            {
                carrinho = new List<ProdutoCarrinho>();
            }
            else
            {
                carrinho = (List<ProdutoCarrinho>)Session["Carrinho"];
            }

                //conexao BD
                using (SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["lojavirtualOndaMentalConnectionString"].ConnectionString))
                {

                var produto_existente = carrinho.FirstOrDefault(p => p.Id == produtoId);
                if (produto_existente != null)
                {
                    // aumenta a qtd se o produto já existe no carrinho
                    produto_existente.Quantidade++;
                }
                else
                {

                    //detalhes do produto
                    string query = "select nome, preco from tb_produtos where id_produto = @ProdutoId";
                    SqlCommand myCommand = new SqlCommand(query, myConn);
                    myCommand.Parameters.AddWithValue("@ProdutoId", produtoId);

                    myConn.Open();
                    using (SqlDataReader reader = myCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // cria um novo objeto ProdutoCarrinho e preenche com os dados do produto
                            ProdutoCarrinho novoProduto = new ProdutoCarrinho
                            {
                                Id = produtoId,
                                Nome = reader["nome"].ToString(),
                                Preco = (decimal)reader["preco"],
                                Quantidade = 1

                            };

                            // Adiciona o novo produto ao carrinho
                            carrinho.Add(novoProduto);
                        }
                    }
                }
                myConn.Close();
                }
            

            // mostra atualizado o carrinho 
            Session["Carrinho"] = carrinho;
            
        }

        //atualiza os itens do carrinho
        private void AtualizarCarrinhoNaLabel()
        {
            if (Session["Carrinho"] != null)
            {

                var carrinho = (List<ProdutoCarrinho>)Session["Carrinho"];
                var sb = new StringBuilder();
                decimal total = 0;
                decimal produtoTotal = 0;

                foreach (var item in carrinho)
                {
                    decimal qtdProduto = item.Quantidade;
                    decimal totalProduto = item.Preco * item.Quantidade;
                    total += totalProduto;
                    produtoTotal += qtdProduto;

                    sb.AppendLine($" (Qtd: {item.Quantidade}) - {item.Nome} - {item.Preco} - Subtotal = {totalProduto.ToString("C")} <br /> ");
                }
                string totaisPrecoCarrinho = $"Valor Total: {total:C}";
                string totaisProduto = $"Produto Total: {produtoTotal}";

                
                lbl_totalCarrinho.Text = totaisPrecoCarrinho;
                lbl_mensagem.Text = sb.ToString();
                lbl_totalProdutos.Text = totaisProduto;
            }
            else
            {
                lbl_mensagem.Text = "O carrinho está vazio.";
                lbl_totalCarrinho.Text = "Valor Total: R$0,00";
                lbl_totalProdutos.Text = "Produtos Total: 0";
            }
        }


      


        // Método para salvar o carrinho no banco de dados
        private void SalvarCarrinhoBD()
        {
            var carrinho = (List<ProdutoCarrinho>)Session["Carrinho"];
            int utilizadorID = Convert.ToInt32(Session["utilizador_id"]);

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["lojavirtualOndaMentalConnectionString"].ConnectionString))
            {
                myConn.Open();

                // Primeiro, limpa o carrinho atual no banco de dados
                string deleteQuery = "delete from tb_carrinho WHERE utilizador_id = @Utilizador_id";
                SqlCommand deleteCmd = new SqlCommand(deleteQuery, myConn);
                deleteCmd.Parameters.AddWithValue("@Utilizador_id", utilizadorID);
                deleteCmd.ExecuteNonQuery();

                // Agora, insere os itens do carrinho atual no banco de dados
                string insertQuery = "insert into tb_carrinho (utilizador_id, produto_id, quantidade, preco, data_adicionado) values (@Utilizador_id, @Produto_id, @Quantidade, @Preco, @Data_adicionado)";
                foreach (var item in carrinho)
                {
                    SqlCommand insertCmd = new SqlCommand(insertQuery, myConn);
                    insertCmd.Parameters.AddWithValue("@Utilizador_id", utilizadorID);
                    insertCmd.Parameters.AddWithValue("@Produto_id", item.Id);
                    insertCmd.Parameters.AddWithValue("@Quantidade", item.Quantidade); // Supondo que você está rastreando a quantidade
                    insertCmd.Parameters.AddWithValue("@Preco", item.Preco);
                    insertCmd.Parameters.AddWithValue("@Data_adicionado", DateTime.Now); // Adiciona a data e hora atual
                    insertCmd.ExecuteNonQuery();
                }

                myConn.Close();
            }
        }


        // Método para carregar o carrinho do banco de dados
        private void CarregarCarrinhoBD(int utilizadorID)
        {
            List<ProdutoCarrinho> carrinho = new List<ProdutoCarrinho>();

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["lojavirtualOndaMentalConnectionString"].ConnectionString))
            {
                myConn.Open();

                string query = "select c.produto_id, c.quantidade, c.preco, p.nome FROM tb_carrinho c INNER JOIN tb_produtos p ON c.produto_id = p.id_produto WHERE c.utilizador_id = @Utilizador_id";
    
                SqlCommand selectCmd = new SqlCommand(query, myConn);
                selectCmd.Parameters.AddWithValue("@Utilizador_id", utilizadorID);

                using (SqlDataReader reader = selectCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProdutoCarrinho item = new ProdutoCarrinho
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("produto_id")),
                            Quantidade = reader.GetInt32(reader.GetOrdinal("quantidade")),
                            Preco = reader.GetDecimal(reader.GetOrdinal("preco")),
                            Nome = reader.GetString(reader.GetOrdinal("nome"))
                    };
                        carrinho.Add(item);
                    }
                }

                myConn.Close();
            }

            Session["Carrinho"] = carrinho; // Atualiza a sessão com o carrinho carregado do banco de dados
        }



        protected void btn_finalizarCompra_Click(object sender, EventArgs e)
        {
            if (Session["logado"] != "Sim")
            {
               /* Response.Redirect("login.aspx?redirect=finalizacao_encomenda.aspx"); */
                Response.Redirect("login.aspx");
                
            }
            else
            {
                Response.Redirect("finalizacao_encomenda.aspx");
            }
        }

    }
}


