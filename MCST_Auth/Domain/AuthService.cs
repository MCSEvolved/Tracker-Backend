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
            configuration = _configuration;
        }

        public async Task<string> RequestIdToken()
		{
            var uid = configuration["FirebaseUID"];
            
            string customToken = await FirebaseAuth.DefaultInstance
                .CreateCustomTokenAsync(uid);

            string idToken = await ConvertCustomToId(customToken);

            return idToken;
        }


        private async Task<string> ConvertCustomToId(string customToken)
        {
            var client = new RestClient("https://api.mcsynergy.nl/auth");
            var request = new RestRequest("/exchange-custom-token");
            request.AddHeader("custom-token", customToken);
        
            var response = await client.PostAsync(request);

            if (!response.IsSuccessful || response.Content == null)
            {
                return "";
            }
            JsonNode? data = JsonSerializer.Deserialize<JsonNode>(response.Content);
            if (data == null)
            {
                return "";
            }
            JsonNode? idTokenJson = data["idToken"];
            if (idTokenJson == null)
            {
                return "";
            }
            String idToken = idTokenJson.ToString();
            Console.WriteLine(idToken);
            return idToken;

        }
    }
}

