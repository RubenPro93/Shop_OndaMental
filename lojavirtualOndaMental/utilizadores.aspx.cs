using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static lojavirtualOndaMental.produtos;
using System.Security.Cryptography;

namespace lojavirtualOndaMental
{
    public partial class utilizadores : System.Web.UI.Page
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



        protected void rpt_lista_de_utilizadores_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dr = (DataRowView)e.Item.DataItem;

                ((Label)e.Item.FindControl("lbl_id_utilizador")).Text = dr["id_utilizador"].ToString();

                ((DropDownList)e.Item.FindControl("ddl_perfil")).SelectedValue = dr["perfil_id"].ToString();
                ((TextBox)e.Item.FindControl("tb_nome")).Text = dr["nome_completo"].ToString();
                ((TextBox)e.Item.FindControl("tb_email")).Text = dr["email"].ToString();


                // Password
                TextBox tbPassword = (TextBox)e.Item.FindControl("tb_password");
                if (dr["password"] != DBNull.Value)
                {
                    tbPassword.Text = DecryptString(dr["password"].ToString());
                }
                else
                {
                    tbPassword.Enabled = false;
                }

                // NIF
                TextBox tbNif = (TextBox)e.Item.FindControl("tb_nif");
                if (dr["nif"] != DBNull.Value)
                {
                    tbNif.Text = dr["nif"].ToString();
                }
                else
                {
                    tbNif.Enabled = false;
                }
                // Sexo
                TextBox tbSexo = (TextBox)e.Item.FindControl("tb_sexo");
                if (dr["sexo"] != DBNull.Value)
                {
                    tbSexo.Text = dr["sexo"].ToString();
                }
                else
                {
                    tbSexo.Enabled = false;
                }
                // Data Nascimento
                TextBox tbDataNasc = (TextBox)e.Item.FindControl("tb_data_nasc");
                if (dr["data_nasc"] != DBNull.Value)
                {
                    tbDataNasc.Text = ((DateTime)dr["data_nasc"]).ToString("yyyy-MM-dd");
                }
                else
                {
                    tbDataNasc.Enabled = false;
                }
                // Telemóvel
                TextBox tbTelemovel = (TextBox)e.Item.FindControl("tb_telemovel");
                if (dr["telemovel"] != DBNull.Value)
                {
                    tbTelemovel.Text = dr["telemovel"].ToString();
                }
                else
                {
                    tbTelemovel.Enabled = false;
                }

                // Morada
                TextBox tbMorada = (TextBox)e.Item.FindControl("tb_morada");
                if (dr["morada"] != DBNull.Value)
                {
                    tbMorada.Text = dr["morada"].ToString();
                }
                else
                {
                    tbMorada.Enabled = false;
                }

                ((CheckBox)e.Item.FindControl("cb_ativo")).Checked = (bool)dr["ativo"];

                CheckBox cbAtivo = (CheckBox)e.Item.FindControl("cb_ativo");
                cbAtivo.CssClass = (bool)dr["ativo"] ? "form-control checkbox-true" : "form-control checkbox-false";



                ((ImageButton)e.Item.FindControl("btn_grava")).CommandArgument = dr["id_utilizador"].ToString();
                ((ImageButton)e.Item.FindControl("btn_delete")).CommandArgument = dr["id_utilizador"].ToString();
            }
        }

        protected void rpt_lista_de_utilizadores_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // variável para armazenar erro
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

                        // Obter o e-mail do formulário
                        string email = ((TextBox)e.Item.FindControl("tb_email")).Text;
                        int idUtilizador = Convert.ToInt32(e.CommandArgument);

                        // Verificar se o e-mail já existe para outro utilizador
                        mycommand.CommandText = "SELECT COUNT(*) FROM tb_utilizadores WHERE email = @email AND id_utilizador <> @id_utilizador";
                        mycommand.Parameters.AddWithValue("@email", email);
                        mycommand.Parameters.AddWithValue("@id_utilizador", idUtilizador);
                        int count = Convert.ToInt32(mycommand.ExecuteScalar());

                        Label lblErrorMessage = (Label)e.Item.FindControl("lblErrorMessage");
                        if (count > 0)
                        {
                            // E-mail já existe
                            lblErrorMessage.Text = "O e-mail informado já está sendo utilizado por outro utilizador.";
                            lblErrorMessage.Visible = true;
                        }
                        else
                        {
                            mycommand.Parameters.Clear();
                            lblErrorMessage.Visible = false;

                            // Atualiza as informações do utilizador
                            mycommand.CommandText =
                                "UPDATE tb_utilizadores SET perfil_id = @perfil_id, nome_completo = @nome_completo, email = @email, password = @password, nif = @nif, sexo = @sexo, data_nasc = @data_nasc, telemovel = @telemovel, morada = @morada, ativo = @ativo WHERE id_utilizador = @id_utilizador";
                            mycommand.Parameters.AddWithValue("@perfil_id",
                                ((DropDownList)e.Item.FindControl("ddl_perfil")).SelectedValue);
                            mycommand.Parameters.AddWithValue("@nome_completo",
                                ((TextBox)e.Item.FindControl("tb_nome")).Text);
                            mycommand.Parameters.AddWithValue("@email", ((TextBox)e.Item.FindControl("tb_email")).Text);

                            // Obtém as referências para os controles TextBox
                            TextBox tbPassword = (TextBox)e.Item.FindControl("tb_password");
                            TextBox tbNif = (TextBox)e.Item.FindControl("tb_nif");
                            TextBox tbSexo = (TextBox)e.Item.FindControl("tb_sexo");
                            TextBox tbDataNasc = (TextBox)e.Item.FindControl("tb_data_nasc");
                            TextBox tbTelemovel = (TextBox)e.Item.FindControl("tb_telemovel");
                            TextBox tbMorada = (TextBox)e.Item.FindControl("tb_morada");

                            // Adiciona os valores aos parâmetros apenas se os campos não estiverem vazios
                            mycommand.Parameters.AddWithValue("@password",
                                string.IsNullOrEmpty(tbPassword.Text) ? DBNull.Value :(object)EncryptString(tbPassword.Text));
                            mycommand.Parameters.AddWithValue("@nif",
                                string.IsNullOrEmpty(tbNif.Text) ? DBNull.Value : (object)tbNif.Text);
                            mycommand.Parameters.AddWithValue("@sexo",
                                string.IsNullOrEmpty(tbSexo.Text) ? DBNull.Value : (object)tbSexo.Text);
                            mycommand.Parameters.AddWithValue("@data_nasc",
                                string.IsNullOrEmpty(tbDataNasc.Text) ? DBNull.Value : (object)tbDataNasc.Text);
                            mycommand.Parameters.AddWithValue("@telemovel",
                                string.IsNullOrEmpty(tbTelemovel.Text) ? DBNull.Value : (object)tbTelemovel.Text);
                            mycommand.Parameters.AddWithValue("@morada",
                                string.IsNullOrEmpty(tbMorada.Text) ? DBNull.Value : (object)tbMorada.Text);
                            mycommand.Parameters.AddWithValue("@ativo",
                                ((CheckBox)e.Item.FindControl("cb_ativo")).Checked);
                            mycommand.Parameters.AddWithValue("@id_utilizador", Convert.ToInt32(e.CommandArgument));

                            CheckBox cbAtivo = (CheckBox)e.Item.FindControl("cb_ativo");
                            cbAtivo.CssClass = ((CheckBox)e.Item.FindControl("cb_ativo")).Checked
                                ? "form-control checkbox-true"
                                : "form-control checkbox-false";

                            mycommand.ExecuteNonQuery();
                        }

                    }
                    else if (e.CommandName.Equals("btn_apaga"))
                    {
                        // Deleta o utilizador
                        mycommand.CommandText = "DELETE FROM tb_utilizadores WHERE id_utilizador = @id_utilizador";
                        mycommand.Parameters.AddWithValue("@id_utilizador", Convert.ToInt32(e.CommandArgument));
                        mycommand.ExecuteNonQuery();
                        Response.Redirect(Request.RawUrl);
                    }
                    myConn.Close();

                }
                catch (Exception ex)
                {
                    errorMessage = "Ocorreu um erro inesperado: " + ex.Message;

                }

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", $"alert('{errorMessage}');", true);
                }

            }


        }


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



        public static string DecryptString(string Message)
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



            // Step 3. Setup the decoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;



            // Step 4. Convert the input string to a byte[]



            Message = Message.Replace("KKK", "+");
            Message = Message.Replace("JJJ", "/");
            Message = Message.Replace("III", "\\");



            byte[] DataToDecrypt = Convert.FromBase64String(Message);



            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }



            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);
        }



    }
}