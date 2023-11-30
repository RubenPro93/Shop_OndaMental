using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static lojavirtualOndaMental.mostra_produtos;
using ASPSnippets.GoogleAPI;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Data.Common;

namespace lojavirtualOndaMental
{
    public partial class login : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["logado"] = "Nâo";

            if (Session["erro_login"] == "Sim")
            {
                lbl_erro_login.Text = "Esse email já tem registo em nosso site!!! Realize o login normal";
                lbl_erro_login.Visible = true;
                Session.Remove("erro_login");
            }


            //CONFIGURAÇÃO LOGIN GOOGLE
            GoogleConnect.ClientId = "804760204150-r9v4hbm9kh2dj7guf6839bkskenmru8v.apps.googleusercontent.com";
            GoogleConnect.ClientSecret = "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";
            GoogleConnect.RedirectUri = ConfigurationManager.AppSettings["URLSite"]+"Default.aspx";

        }



        //onclick login google
        protected void Login(object sender, EventArgs e)
        {
            GoogleConnect.Authorize("profile", "email");
        }



        private void TransferirCarrinhoBD(int utilizadorID)
        {
            if (Session["Carrinho"] != null)
            {
                var carrinho = (List<ProdutoCarrinho>)Session["Carrinho"];
                using (SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["lojavirtualOndaMentalConnectionString"].ConnectionString))
                {
                    myConn.Open();

                    // Aqui você pode optar por mesclar ou substituir os itens no BD
                    string deleteQuery = "delete from tb_carrinho WHERE utilizador_id = @Utilizador_id";
                    SqlCommand deleteCmd = new SqlCommand(deleteQuery, myConn);
                    deleteCmd.Parameters.AddWithValue("@Utilizador_id", utilizadorID);
                    deleteCmd.ExecuteNonQuery();

                    // Inserir itens do carrinho na base de dados
                    string insertQuery = "insert into tb_carrinho (utilizador_id, produto_id, quantidade, preco, data_adicionado) values (@Utilizador_id, @Produto_id, @Quantidade, @Preco, @Data_adicionado)";
                    foreach (var item in carrinho)
                    {
                        SqlCommand insertCmd = new SqlCommand(insertQuery, myConn);
                        insertCmd.Parameters.AddWithValue("@Utilizador_id", utilizadorID);
                        insertCmd.Parameters.AddWithValue("@Produto_id", item.Id);
                        insertCmd.Parameters.AddWithValue("@Quantidade", item.Quantidade);
                        insertCmd.Parameters.AddWithValue("@Preco", item.Preco);
                        insertCmd.Parameters.AddWithValue("@Data_adicionado", DateTime.Now);
                        insertCmd.ExecuteNonQuery();
                    }

                    myConn.Close();
                }

                
            } 
        }


        protected void btn_entrar_Click(object sender, EventArgs e)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings
              ["lojavirtualOndaMentalConnectionString"].ConnectionString);



            SqlCommand myCommand = new SqlCommand();



            myCommand.Parameters.AddWithValue("@email", tb_email.Text);
            myCommand.Parameters.AddWithValue("@pw", EncryptString(tb_pw.Text));


            SqlParameter valor = new SqlParameter();
            valor.ParameterName = "@retorno";
            valor.Direction = ParameterDirection.Output;
            valor.SqlDbType = SqlDbType.Int;  //0 caso seja inserido e 1 caso n
            myCommand.Parameters.Add(valor);



            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "login";



            myCommand.Connection = myConn;
            myConn.Open();
            myCommand.ExecuteNonQuery();

            int resposta = Convert.ToInt32(myCommand.Parameters["@retorno"].Value);



            myConn.Close();



            if (resposta == 1) //verifica se o retorno é correto com a BD, se for 1 é sim e caso 0 não. Seguindo as regras da SP da base de dados..
            {
                myConn.Open();
                using (SqlDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int utilizador_ID = reader.GetInt32(0); // buscar a primeira coluna id_utilizador...
                        Session["utilizador_id"] = utilizador_ID;
                    }

                }
                myConn.Close();

                Session["email"] = tb_email.Text; //levar variavel email para trocar senha do utilizador, caso queira!

                if (Session["Carrinho"] != null) {
                    int utilizadorID = (int)Session["utilizador_id"];
                    TransferirCarrinhoBD(utilizadorID);
                    Session["logado"] = "Sim";
                    Response.Redirect("mostra_produtos.aspx");
                }
                else {
                    Session["logado"] = "Sim";
                    Response.Redirect("mostra_produtos.aspx");

                }

            }
            else if (resposta == 2)
            {
                lbl_mensagem.Text = "Utilizador inativo!";

            }
            else
            {

                lbl_mensagem.Text = "Utilizador e/ou palavra-passe errados!";
            } 

        }



        //encriptaçao
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