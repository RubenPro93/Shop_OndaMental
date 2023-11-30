using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text; //pdf
using iTextSharp.text.pdf; //pdf
using iTextSharp.text.html.simpleparser; //pdf


using static lojavirtualOndaMental.mostra_produtos;
using System.Text;

namespace lojavirtualOndaMental
{
    public partial class finalizacao_encomenda : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["logado"] == "Sim")
                {
                    int utilizadorId = Convert.ToInt32(Session["utilizador_id"]);
                    CarregarDadosUtilizador(utilizadorId); //carrega os dados do utilizador logado

                    // verifica se já existe um carrinho na BD para o utilizador
                    if (CarrinhoExistenteBD(utilizadorId))
                    {
                        // carrega o carrinho
                        CarregarCarrinhoBD(utilizadorId);
                        dadosTotal(); //label como valor total
                    }


                }
                else
                {
                    // Redirecionar para a página de login ou registro
                    Response.Redirect("login.aspx");
                }


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
                            Nome = reader.GetString(reader.GetOrdinal("nome")),

                        };
                        carrinho.Add(item);
                    }

                }

                myConn.Close();
            }
            rptCarrinho.DataSource = carrinho;
            rptCarrinho.DataBind();

            Session["Carrinho"] = carrinho; // Atualiza a sessão com o carrinho carregado do banco de dados
        }


        //verificar se existe carrinho para o utilizador_id (no caso é quando ja vem logado do mostra_produtos)
        private bool CarrinhoExistenteBD(int utilizadorId)
        {
            bool carrinhoExiste = false;
            string connectionString = ConfigurationManager.ConnectionStrings["lojavirtualOndaMentalConnectionString"].ConnectionString;

            using (SqlConnection myConn = new SqlConnection(connectionString))
            {
                myConn.Open();

                string query = "SELECT COUNT(1) FROM tb_carrinho WHERE utilizador_id = @UtilizadorId";
                SqlCommand myCommand = new SqlCommand(query, myConn);
                myCommand.Parameters.AddWithValue("@UtilizadorId", utilizadorId);

                // ExecuteScalar retorna a primeira coluna da primeira linha do resultado
                // Neste caso, estamos contando os registros, então vamos receber um número.
                int count = Convert.ToInt32(myCommand.ExecuteScalar());

                if (count > 0)
                {
                    carrinhoExiste = true; // Carrinho existe se a contagem for maior que 0
                }

                myConn.Close();
            }

            return carrinhoExiste;
        }


        //carregar os dados do utilizador para apresentar na encomenda
        private void CarregarDadosUtilizador(int utilizadorId)
        {

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["lojavirtualOndaMentalConnectionString"].ConnectionString))
            {
                myConn.Open();

                string query = "SELECT nome_completo, email, nif, telemovel, morada FROM tb_utilizadores WHERE id_utilizador = @Utilizador_id";
                SqlCommand selectCmd = new SqlCommand(query, myConn);
                selectCmd.Parameters.AddWithValue("@Utilizador_id", utilizadorId);

                using (SqlDataReader reader = selectCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lbl_nome.Text = reader["nome_completo"].ToString();
                        lbl_email.Text = reader["email"].ToString();
                        lbl_nif.Text = reader["nif"].ToString();
                        lbl_telemovel.Text = reader["telemovel"].ToString();
                        lbl_morada.Text = reader["morada"].ToString();

                        // Exibir TextBox e ativar validador se o campo correspondente estiver vazio
                        txtNif.Visible = rfvNif.Visible = string.IsNullOrEmpty(lbl_nif.Text);
                        txtTelemovel.Visible = rfvTelemovel.Visible = string.IsNullOrEmpty(lbl_telemovel.Text);
                        txtMorada.Visible = rfvMorada.Visible = string.IsNullOrEmpty(lbl_morada.Text);

                    }
                }

                myConn.Close();
            }
        }



        /*
         * Funcao apenas para facilitar os calculos do total de produto & quantidade
         * 
         */
        private void dadosTotal()
        {
            string itensCarrinho = "";
            decimal valorTotal = 0;
            decimal produtoTotal = 0;

            foreach (var item in (List<ProdutoCarrinho>)Session["Carrinho"])
            {
                decimal qtdProduto = item.Quantidade;
                decimal totalProduto = item.Preco * item.Quantidade;
                valorTotal += totalProduto;
                produtoTotal += qtdProduto;

                itensCarrinho += $"(Qtd: {item.Quantidade}) {item.Nome} - {item.Preco:C} = SubTotal: {totalProduto.ToString("C")}\r\n";
            }

            string totaisPrecoCarrinho = $"Valor Total: {valorTotal:C}";
            string totaisProduto = $"Produto Total: {produtoTotal}";

            lbl_valorTotal.Text = totaisPrecoCarrinho;
            lbl_produtoTotal.Text = totaisProduto;
        }


        /* Botao concluir encomenda
         * 
         * Vai mandar email ao utilizador id (lbl_email)
         * Vai transferir os produtos do carrinho para a tb_encomendas
         * Vai limpar a session["Carrinho"] do utilizador_id e depois limpa a tabela tb_carrinho.
         */
        protected void btn_concluir_encomenda_Click(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            if (txtNif.Visible)
            {
                int utilizadorId2 = Convert.ToInt32(Session["utilizador_id"]);
                string nif = txtNif.Text;
                string telemovel = txtTelemovel.Text;
                string morada = txtMorada.Text;

                using (SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["lojavirtualOndaMentalConnectionString"].ConnectionString))
                {
                    myConn.Open();

                    string query = "UPDATE tb_utilizadores SET nif = @nif, telemovel = @telemovel, morada = @morada WHERE id_utilizador = @Utilizador_id";
                    SqlCommand updateCmd = new SqlCommand(query, myConn);
                    updateCmd.Parameters.AddWithValue("@nif", nif);
                    updateCmd.Parameters.AddWithValue("@telemovel", telemovel);
                    updateCmd.Parameters.AddWithValue("@morada", morada);
                    updateCmd.Parameters.AddWithValue("@Utilizador_id", utilizadorId2);


                    lbl_nif.Text = nif;
                    lbl_telemovel.Text = telemovel;
                    lbl_morada.Text = morada;

                    updateCmd.ExecuteNonQuery();
                    myConn.Close();
                }
            }




            string itensCarrinho = "";
            decimal valorTotal = 0;

            //dados para puxar no pdf com itens do carrinho
            foreach (var item in (List<ProdutoCarrinho>)Session["Carrinho"])
            {

                decimal totalProduto = item.Preco * item.Quantidade;
                valorTotal += totalProduto;

                itensCarrinho += $"(Qtd: {item.Quantidade}) {item.Nome} - {item.Preco:C} = SubTotal: {totalProduto.ToString("C")}\r\n";
            }

            string totaisPrecoCarrinho = $"Valor Total: {valorTotal:C}";
            lbl_valorTotal.Text = totaisPrecoCarrinho;

            //transferir produtos do utilizador_id para a tb_encomendas
            int utilizadorId = Convert.ToInt32(Session["utilizador_id"]);

            //insere os produtos do carrinho na tb_ecomendas e remove o tb_carrinho do utilizador_id
            transf_carrinho_Encomendas(utilizadorId);

            //configuracao email
            MailMessage mail = new MailMessage();
            SmtpClient servidor = new SmtpClient();

            mail.From = new MailAddress("esther.candido.t0123765@edu.atec.pt");

            mail.To.Add(new MailAddress(lbl_email.Text));
            mail.Subject = "ENCOMENDA LOJA ONDAMENTAL";

            mail.IsBodyHtml = true;
            mail.Body = "Segue em Anexo o seu pedido";


            String caminho = ConfigurationSettings.AppSettings.Get("PathFicheiros");
            string pdfTemplate = caminho + "template\\form_fatura.pdf";
            string nomePDF = EncryptString(lbl_nome.Text.Replace(" ", "") + DateTime.Now.ToString().Replace(":", "").Replace("/", "").Replace(" ", "")) + ".pdf";
            string newFile = caminho + "pdfs\\" + nomePDF;

            PdfReader reader = new PdfReader(pdfTemplate);
            PdfStamper stamper = new PdfStamper(reader, new FileStream(newFile, FileMode.Create));
            AcroFields campos = stamper.AcroFields;


            campos.SetField("nome", lbl_nome.Text);
            campos.SetField("nif", lbl_nif.Text);
            campos.SetField("telemovel", lbl_telemovel.Text);
            campos.SetField("morada", lbl_morada.Text);
            campos.SetField("itensCarrinho", itensCarrinho);
            campos.SetField("totalCompra", totaisPrecoCarrinho);

            stamper.Close();


            Attachment anexo = new Attachment(newFile);
            mail.Attachments.Add(anexo);

            servidor.Host = ConfigurationManager.AppSettings["SMTP_HOST"];
            servidor.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_PORT"]);

            string utilizador = ConfigurationManager.AppSettings["SMTP_USER"];
            string pw = ConfigurationManager.AppSettings["SMTP_PASS"];
            servidor.Credentials = new NetworkCredential(utilizador, pw);

            servidor.EnableSsl = true;

            servidor.Send(mail);

            Session.Remove("Carrinho"); //limpa a sessao com os dados do carrinho
            Response.Redirect("mostra_produtos.aspx");
        }



        /*
         * Funcao na qual vai inserir os produtos atual do carrinho para a bd tb_encomendas
         * Ira remover os itens existente do utilizador_id da tabela tb_carrinho
         * 
         */
        private void transf_carrinho_Encomendas(int utilizadorId)
        {
            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["lojavirtualOndaMentalConnectionString"].ConnectionString))
            {
                myConn.Open();

                // ira inserir os itens do carrinho na tabela tb_encomendas BD
                foreach (var item in (List<ProdutoCarrinho>)Session["Carrinho"])
                {
                    string query_inserir = "insert into tb_encomendas (utilizador_id, produto_id, quantidade, preco) VALUES (@UtilizadorId, @ProdutoId, @Quantidade, @Preco)";
                    SqlCommand insertCmd = new SqlCommand(query_inserir, myConn);
                    insertCmd.Parameters.AddWithValue("@UtilizadorId", utilizadorId);
                    insertCmd.Parameters.AddWithValue("@ProdutoId", item.Id);
                    insertCmd.Parameters.AddWithValue("@Quantidade", item.Quantidade);
                    insertCmd.Parameters.AddWithValue("@Preco", item.Preco);

                    insertCmd.ExecuteNonQuery();
                }

                // ira remover os itens da tabela tb_carrinho.. 
                string query_deletar = "DELETE FROM tb_carrinho WHERE utilizador_id = @UtilizadorId";
                SqlCommand deleteCmd = new SqlCommand(query_deletar, myConn);
                deleteCmd.Parameters.AddWithValue("@UtilizadorId", utilizadorId);
                deleteCmd.ExecuteNonQuery();

                myConn.Close();
            }
        }




        //Encryptacao
        public static string EncryptString(string Message)
        {
            string Passphrase = "atec";
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();



            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below



            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));



            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();



            // Step 3. Setup the encoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;



            // Step 4. Convert the input string to a byte[]
            byte[] DataToEncrypt = UTF8.GetBytes(Message);



            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }



            // Step 6. Return the encrypted string as a base64 encoded string
            string enc = Convert.ToBase64String(Results);
            enc = enc.Replace("+", "KKK");
            enc = enc.Replace("/", "JJJ");
            enc = enc.Replace("\\", "III");
            return enc;
        }


    }
}