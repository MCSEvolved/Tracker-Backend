using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using FirebaseAdmin.Auth;
using MCST_Auth.Domain.Models;
using MCST_ControllerInterfaceLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace MCST_Auth.Domain
{
	public class AuthService: IAuthService
	{
        private readonly IConfiguration configuration;
        public AuthService(IConfiguration _configuration)
        {
            this.configuration = _configuration;
        }

        public async Task<string> RequestIdToken()
		{
            var uid = configuration["FirebaseUID"];
            var additionalClaims = new Dictionary<string, object>()
            {
                { "isService", true },
            };

            string customToken = await FirebaseAuth.DefaultInstance
                .CreateCustomTokenAsync(uid, additionalClaims);

            string idToken = await ConvertCustomToId(customToken);

            return idToken;
        }


        private async Task<string> ConvertCustomToId(string customToken)
        {
            string apiKey = configuration["FirebaseAPIKey"];

            var client = new RestClient("https://identitytoolkit.googleapis.com");
            var request = new RestRequest("v1/accounts:signInWithCustomToken");
            request.AddQueryParameter("key", apiKey);
            request.AddJsonBody(new RequestIdTokenModel { token = customToken });

            var response = await client.PostAsync(request);

            if (response.IsSuccessful && response.Content != null)
            {
                JsonNode? data = JsonSerializer.Deserialize<JsonNode>(response.Content);
                if (data != null)
                {
                    JsonNode? idTokenJson = data["idToken"];
                    if (idTokenJson != null)
                    {
                        string idToken = idTokenJson.ToString();
                        return idToken;
                    }
                    else
                    {
                        return "";
                    }
                } else
                {
                    return "";
                }
                
                
            }
            else {
                return "";
            }
        }
    }
}

