using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using ASPSnippets.GoogleAPI;
using Newtonsoft.Json;

namespace lojavirtualOndaMental
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

           
            GoogleConnect.ClientId = "804760204150-r9v4hbm9kh2dj7guf6839bkskenmru8v.apps.googleusercontent.com";
            GoogleConnect.ClientSecret = "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";
            GoogleConnect.RedirectUri = "https://localhost:44320/Default.aspx";

            if (!string.IsNullOrEmpty(Request.QueryString["code"]))
            {
                string code = Request.QueryString["code"];

                try
                {
                    var tokenResponse = Task.Run(() => ExchangeCodeForTokenAsync(code)).Result;
                    string json = Task.Run(() => FetchGoogleProfileAsync(tokenResponse.Access_token)).Result;

                    // Imprimir a resposta JSON para depuração
                    Console.WriteLine("Resposta JSON: " + json);

                    GoogleProfile profile = new JavaScriptSerializer().Deserialize<GoogleProfile>(json);

                
                    int id = VerificaOuRegistraUsuario(profile.Email, profile.Name);

                    if (id == 0)
                    {
                        // Se o ID do usuário for 0, significa que não foi possível verificar ou registrar o usuário
                        Session["erro_login"] = "Sim";
                        Response.Redirect("login.aspx");
                    }
                    else
                    {
                        // Se o ID do usuário for diferente de 0, significa que o usuário foi verificado ou registrado com sucesso
                        Session["utilizador_id"] = id;
                        Session["loginGoogle"] = code;
                        Session["logado"] = "Sim";
                        Response.Redirect("mostra_produtos.aspx");
                    }
                }
                catch (Exception ex)
                {
                    // Logar a exceção para diagnóstico
                    Console.WriteLine("Erro: " + ex.Message);
                }
            }
            if (Convert.ToString(Session["logado"]) == "Não")
            {
                Response.Redirect("login.aspx");
            }

            if (Request.QueryString["error"] == "access_denied")
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('Access denied.')", true);
            }
        }

        private int VerificaOuRegistraUsuario(string email, string nome)
        {
            int utilizadorID = 0;

            using (SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["lojavirtualOndaMentalConnectionString"].ConnectionString))
            {
                SqlCommand myCommand = new SqlCommand("verifica_registra_loginGoogle", myConn);
                myCommand.CommandType = CommandType.StoredProcedure;

                // Adicionando parâmetros
                myCommand.Parameters.AddWithValue("@Email", email);
                myCommand.Parameters.AddWithValue("@Nome", nome);

                // Parâmetro de saída para o ID do usuário
                SqlParameter userIdParam = new SqlParameter("@UtilizadorID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                myCommand.Parameters.Add(userIdParam);

                // Abrir conexão e executar comando
                myConn.Open();
                myCommand.ExecuteNonQuery();

                // Obter o ID do utilizador
                // É importante verificar se o valor retornado não é DBNull
                utilizadorID = (userIdParam.Value != DBNull.Value) ? Convert.ToInt32(userIdParam.Value) : 0;
            } // A conexão é fechada automaticamente aqui

            return utilizadorID;
        }


        public class GoogleProfile
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }

            public string Picture { get; set; }

            public string Verified_Email { get; set; }
            public string Gender { get; set; }
            public string ObjectType { get; set; }
        }





   

        private async Task<TokenResponse> ExchangeCodeForTokenAsync(string code)
        {
            using (var client = new HttpClient())
            {
                var postData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("client_id", GoogleConnect.ClientId),
                    new KeyValuePair<string, string>("client_secret", GoogleConnect.ClientSecret),
                    new KeyValuePair<string, string>("redirect_uri", GoogleConnect.RedirectUri),
                    new KeyValuePair<string, string>("grant_type", "authorization_code")
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth2.googleapis.com/token")
                {
                    Content = new FormUrlEncodedContent(postData)
                };

                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TokenResponse>(json);
                }

                throw new Exception("Não foi possível trocar o código pelo token");
            }
        }

        private async Task<string> FetchGoogleProfileAsync(string accessToken)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                throw new Exception("Não foi possível obter o perfil do usuário");
            }
        }

        public class TokenResponse
        {
            public string Access_token { get; set; }
        }

        protected void Login(object sender, EventArgs e)
        {
            GoogleConnect.Authorize("profile", "email");
        }

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            GoogleConnect.Clear(Request.QueryString["code"]);
        }
    }
}
