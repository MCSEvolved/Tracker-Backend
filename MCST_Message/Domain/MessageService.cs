using System.Text.Json;
using MCST_ControllerInterfaceLayer.Interfaces;
using MCST_Enums;
using MCST_Message.Data;
using MCST_Message.Data.DTOs;
using MCST_Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace MCST_Message.Domain
{
    public class MessageService
	{
		private readonly MessageRepository repo;
        private readonly IWsService clientWsService;
        private readonly INotificationService notificationService;

        public MessageService(MessageRepository _messageRepository, IWsService _clientWsService, INotificationService _notificationService)
		{
			repo = _messageRepository;
            this.clientWsService = _clientWsService;
            this.notificationService = _notificationService;
        }

        private void SendNotification(Message message)
        {
            if (message.Type == MessageType.Error)
            {
                Notification notification = new()
                {
                    Title = $"{message.Source} {message.Identifier} sent an Error!",
                    Body = message.Content,
                    Topic = "tracker_error"
                };
                notificationService.SendNotification(notification);
            }
            else if (message.Type == MessageType.Warning)
            {
                Notification notification = new()
                {
                    Title = $"{message.Source} {message.Identifier} sent a Warning!",
                    Body = message.Content,
                    Topic = "tracker_warning"
                };
                notificationService.SendNotification(notification);
            }
            else if (message.Type == MessageType.OutOfFuel)
            {
                Notification notification = new()
                {
                    Title = "Turtle Out of Fuel!",
                    Body = $"Turtle {message.Identifier} has run out of fuel and needs help!",
                    Topic = "tracker_out-of-fuel"
                };
                notificationService.SendNotification(notification);
            }
        }

		public bool NewMessage(Message message)
		{
            if (message.IsValid())
            {
                clientWsService.NewMessageOverWS(message);
                var metaData = BsonSerializer.Deserialize<BsonDocument>((message.MetaData).ToString());
            
			    repo.InsertMessage(new MessageDTO(message.Type, message.Source, message.Content, metaData, message.CreationTime, message.Identifier));
                SendNotification(message);
                return true;
            } else
            {
                return false;
            }

		}

		public List<Message> GetAll(int page, int pageSize)
		{
			List<MessageDTO> messageDTOs = repo.GetAllMessages(page, pageSize);
            List<Message> messages = new List<Message>();
			foreach (var messageDTO in messageDTOs)
			{
                Message message = new Message
				{
					Identifier = messageDTO.Identifier,
					Content = messageDTO.Content,
                    MetaData = JsonSerializer.Deserialize<JsonElement>(messageDTO.MetaData.ToJson()),
					Source = messageDTO.Source,
					Type = messageDTO.Type
				};
                message.OverrideCreationTime(messageDTO.CreationTime);
                messages.Add(message);
            }
			return messages;
		}


        //SUPPORT FOR MULTIPLE SOURCES
        public List<Message> GetBySource(int page, int pageSize, MessageSource source)
        {
            List<MessageDTO> messageDTOs = repo.GetAllMessagesBySource(page, pageSize, source);
            List<Message> messages = new List<Message>();
            foreach (var messageDTO in messageDTOs)
            {
                Message message = new Message
                {
                    Identifier = messageDTO.Identifier,
                    Content = messageDTO.Content,
                    MetaData = JsonSerializer.Deserialize<JsonElement>(messageDTO.MetaData.ToJson()),
                    Source = messageDTO.Source,
                    Type = messageDTO.Type
                };
                message.OverrideCreationTime(messageDTO.CreationTime);
                messages.Add(message);
            }
            return messages;
        }

        public List<Message> GetByIdentifiers(int page, int pageSize, string[] identifiers)
        {
            List<MessageDTO> messageDTOs = repo.GetAllMessagesByIdentifiers(page, pageSize, identifiers);
            List<Message> messages = new List<Message>();
            foreach (var messageDTO in messageDTOs)
            {
                Message message = new Message
                {
                    Identifier = messageDTO.Identifier,
                    Content = messageDTO.Content,
                    MetaData = JsonSerializer.Deserialize<JsonElement>(messageDTO.MetaData.ToJson()),
                    Source = messageDTO.Source,
                    Type = messageDTO.Type
                };
                message.OverrideCreationTime(messageDTO.CreationTime);
                messages.Add(message);
            }
            return messages;
        }
    }
}