using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace lojavirtualOndaMental
{
    public partial class alterar_pw : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["logado"] != "Sim")
            {
                Response.Redirect("login.aspx");
            }
            
        }

        //encriptação
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

        protected void tb_alterar_pw_Click(object sender, EventArgs e)
        {
            //Verificacao senha forte
            Regex maiusculas = new Regex("[A-Z]");
            Regex minusculas = new Regex("[a-z]");
            Regex numeros = new Regex("[0-9]");
            Regex especiais = new Regex("[^A-Za-z0-9]");
            Regex plica = new Regex("'");


            bool forte = true;

            if (tb_pw_nova.Text.Length < 6)
            {
                forte = false;
            }
            if (maiusculas.Matches(tb_pw_nova.Text).Count == 0)
            {
                forte = false;
            }
            if (minusculas.Matches(tb_pw_nova.Text).Count == 0)
            {
                forte = false;
            }
            if (numeros.Matches(tb_pw_nova.Text).Count == 0)
            {
                forte = false;
            }
            if (especiais.Matches(tb_pw_nova.Text).Count == 0)
            {
                forte = false;
            }
            if (plica.Matches(tb_pw_nova.Text).Count > 0)
            {
                forte = false;
            }



            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings
               ["lojavirtualOndaMentalConnectionString"].ConnectionString);



            SqlCommand myCommand = new SqlCommand();


            if (forte)
            {

                lbl_mensagemFRACA.Visible = false;

                myCommand.Parameters.AddWithValue("@email", Session["email"].ToString());
                myCommand.Parameters.AddWithValue("@pw_antiga", EncryptString(tb_pw_antiga.Text));
                myCommand.Parameters.AddWithValue("@pw_nova", EncryptString(tb_pw_nova.Text));

                SqlParameter valor = new SqlParameter();
                valor.ParameterName = "@retorno";
                valor.Direction = ParameterDirection.Output;
                valor.SqlDbType = SqlDbType.Int;  
                myCommand.Parameters.Add(valor);



                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = "alterar_pw";



                myCommand.Connection = myConn;
                myConn.Open();
                myCommand.ExecuteNonQuery();

                int resposta = Convert.ToInt32(myCommand.Parameters["@retorno"].Value);


                myConn.Close();

                if (resposta == 1) //verifica se o retorno é correto com a BD, se for 1 é sim e caso 0 não. Seguindo as regras da SP da base de dados..
                {

                    lbl_mensagem.Text = "Palavra-Passe alterado com sucesso!!!";

                }
                else
                {
                    lbl_mensagem.Text = "Utilizador e/ou palavra-passe errados!";
                }

            }//fim if (Forte)
            else
            {
                lbl_mensagemFRACA.Text = "Senha fraca - Utilize Maiuscula, Minuscula e Numeros";
            }
        }
    }
}