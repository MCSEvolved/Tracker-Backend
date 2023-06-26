using System;
using System.IO;
using FirebaseAdmin.Auth;
using MCST_ControllerInterfaceLayer.Interfaces;
using MCST_Models;
using MCST_Notification.Domain.Models;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace MCST_Notification.Domain
{
	public class NotificationService: INotificationService
	{
        private readonly string baseUrl = "https://api.mcsynergy.nl/";
        private readonly IAuthService authService;
        private readonly IConfiguration configuration;

        public NotificationService(IAuthService _authService, IConfiguration _configuration)
        {
            this.authService = _authService;
            this.configuration = _configuration;
        }

        public async Task<bool> SendNotification(Notification notification)
		{
            var enabled = configuration.GetValue<bool>("EnableNotifications");

            if (!enabled) {
                return false;
            }

            if (!notification.isValid())
            {
                return false;
            }

            string idToken = await authService.RequestIdToken();

            var client = new RestClient(baseUrl);
            var request = new RestRequest("notifications/send");
            request.AddQueryParameter("topic", notification.Topic);
            request.AddHeader("Authorization", idToken);
            request.AddJsonBody(new NotificationJsonModel { body = notification.Body, title = notification.Title });

            var response = await client.PostAsync(request);
            return response.IsSuccessful;
        }
	}
}

