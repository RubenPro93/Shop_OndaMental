using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace lojavirtualOndaMental
{
    public partial class recuperar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        //funçao para gerar as palavaras-passe aleatoria
        private string RandomSenha(int length)
        {
            const string palavras = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();
            char[] caracteres = new char[length];

            for (int i = 0; i < length; i++)
            {
                caracteres[i] = palavras[random.Next(palavras.Length)];
            }

            return new string(caracteres);
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


        //decryp
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



        protected void btn_trocar_pw_Click(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;  //resolver erro de registo linha 153

            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings
              ["lojavirtualOndaMentalConnectionString"].ConnectionString);

            SqlCommand myCommand = new SqlCommand();

            // gerar uma nova palavra-passe e encriptar ela
            string novaPalavra_passe = RandomSenha(12); // Supondo que você queira uma senha de 12 caracteres
            string encryptarSenha = EncryptString(novaPalavra_passe);

            //Inicio 
            myCommand.Parameters.AddWithValue("@email", tb_email.Text);
            myCommand.Parameters.AddWithValue("@pw", encryptarSenha);


            SqlParameter valor = new SqlParameter();
            valor.ParameterName = "@retorno";
            valor.Direction = ParameterDirection.Output;
            valor.SqlDbType = SqlDbType.Int;  //0 caso seja inserido e 1 caso n
            myCommand.Parameters.Add(valor);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "recuperar_pw";

            myCommand.Connection = myConn;
            myConn.Open();
            myCommand.ExecuteNonQuery();

            int resposta = Convert.ToInt32(myCommand.Parameters["@retorno"].Value);


            myConn.Close();

            if (resposta == 1) //verifica se o retorno é correto com a BD, se for 1 é sim e caso 0 não. Seguindo as regras da SP da base de dados..
            {
                string utilizador = ConfigurationManager.AppSettings["SMTP_USER"];
                string pw = ConfigurationManager.AppSettings["SMTP_PASS"];

                MailMessage mail = new MailMessage();
                SmtpClient servidor = new SmtpClient();

                mail.From = new MailAddress("esther.candido.t0123765@edu.atec.pt");

                mail.From = new MailAddress(utilizador);  //meu email q manda
                mail.To.Add(new MailAddress(tb_email.Text)); //o email q recebe
                mail.Subject = "NOVA PALAVRA-PASSE LOJA ONDAMENTAL";  //assunto do email
                mail.IsBodyHtml = true;

                //fica na mensagem, corpo do email
                mail.Body = $"Sua nova senha é: {novaPalavra_passe}";

                servidor.Host = ConfigurationManager.AppSettings["SMTP_HOST"];
                servidor.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_PORT"]);


                servidor.Credentials = new NetworkCredential(utilizador, pw);

                servidor.EnableSsl = true;

                servidor.Send(mail);

                lbl_mensagem.Text = "Nova Palavra-Passe enviado para o e-mail!!!";
            }
            else if (resposta == 2)
            {
                lbl_mensagem.Text = "Utilizador inativo, recuperar palavra-passe não é possivel!";

            }
            else
            {
                lbl_mensagem.Text = "E-mail não encontrado na base de dados!!";
            }
        }
    }
}