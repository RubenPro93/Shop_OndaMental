using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;

namespace lojavirtualOndaMental
{
    public partial class registro_utilizador : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        //Codigo encriptado
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



        protected void tb_registrar_Click1(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;  //resolver erro de registo linha 153

            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings
             ["lojavirtualOndaMentalConnectionString"].ConnectionString);



            SqlCommand myCommand = new SqlCommand();



            myCommand.Parameters.AddWithValue("@nome", tb_nome.Text);
            myCommand.Parameters.AddWithValue("@email", tb_email.Text);
            myCommand.Parameters.AddWithValue("@pw", EncryptString(tb_pw.Text));
            myCommand.Parameters.AddWithValue("@nif", tb_nif.Text);
            myCommand.Parameters.AddWithValue("@sexo", ddl_sexo.SelectedValue.ToString());
            myCommand.Parameters.AddWithValue("@nascimento", tb_nascimento.Text);
            myCommand.Parameters.AddWithValue("@telemovel", tb_telefone.Text);
            myCommand.Parameters.AddWithValue("@morada", tb_morada.Text);




            SqlParameter valor = new SqlParameter();
            valor.ParameterName = "@retorno";
            valor.Direction = ParameterDirection.Output;
            valor.SqlDbType = SqlDbType.Int;  //0 caso seja inserido e 1 caso n
            myCommand.Parameters.Add(valor);


            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "inserir_registro";


            myCommand.Connection = myConn;
            myConn.Open();
            myCommand.ExecuteNonQuery();

            int resposta = Convert.ToInt32(myCommand.Parameters["@retorno"].Value);
            myConn.Close();

            if (resposta == 1)
            {
                string utilizador = ConfigurationManager.AppSettings["SMTP_USER"];
                string pw = ConfigurationManager.AppSettings["SMTP_PASS"];

                MailMessage mail = new MailMessage();
                SmtpClient servidor = new SmtpClient();

                mail.From = new MailAddress("esther.candido.t0123765@edu.atec.pt");

                mail.From = new MailAddress(utilizador);  //meu email q manda
                mail.To.Add(new MailAddress(tb_email.Text)); //o email q recebe
                mail.Subject = "CADASTRO - ATIVE SUA CONTA!";  //assunto do email
                mail.IsBodyHtml = true;

                //fica na mensagem, corpo do email
                mail.Body = "Para ativar a conta deste e-mail clique <a href='https://localhost:44320/ativacao.aspx?email=" + EncryptString(tb_email.Text) + "'>aqui</a>";

                servidor.Host = ConfigurationManager.AppSettings["SMTP_HOST"];
                servidor.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_PORT"]);


                servidor.Credentials = new NetworkCredential(utilizador, pw);

                servidor.EnableSsl = true;

                servidor.Send(mail);


                lbl_mensagem.Text = "E-mail adicionado, foi enviado email para ativação da conta";
            }

            else
            {
                lbl_mensagem.Text = "E-mail ja existe em nossa Base de Dados!";
            }
           
        }
    }
}