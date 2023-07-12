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

        private async Task SendNotification(Message message)
        {
            if (message.Type == MessageType.Error)
            {
                Notification notification = new()
                {
                    Title = $"{message.Source} {message.SourceId} sent an Error!",
                    Body = message.Content,
                    Topic = "tracker_error"
                };
                await notificationService.SendNotification(notification);
            }
            else if (message.Type == MessageType.Warning)
            {
                Notification notification = new()
                {
                    Title = $"{message.Source} {message.SourceId} sent a Warning!",
                    Body = message.Content,
                    Topic = "tracker_warning"
                };
                await notificationService.SendNotification(notification);
            }
            else if (message.Type == MessageType.OutOfFuel)
            {
                Notification notification = new()
                {
                    Title = "Turtle Out of Fuel!",
                    Body = $"Turtle {message.SourceId} has run out of fuel and needs help!",
                    Topic = "tracker_out-of-fuel"
                };
                await notificationService.SendNotification(notification);
            }
        }

		public async Task<bool> NewMessage(Message message)
		{
            if (message.IsValid())
            {
                var metaData = BsonSerializer.Deserialize<BsonDocument>(message.MetaData.ToString());
                MessageDTO dto = new MessageDTO(message.Type, message.Source, message.Content, metaData, message.CreationTime, message.SourceId);
                await repo.InsertMessage(dto);
                message.OverrideId(dto.Id);
                await clientWsService.NewMessageOverWS(message);
                await SendNotification(message);
                return true;
            } else
            {
                return false;
            }

		}

		public async Task<List<Message>> GetAll(int page, int pageSize)
		{
			List<MessageDTO> messageDTOs = await repo.GetAllMessages(page, pageSize);
            List<Message> messages = new List<Message>();
			foreach (var messageDTO in messageDTOs)
			{
                Message message = new Message
				{
					SourceId = messageDTO.SourceId,
					Content = messageDTO.Content,
                    MetaData = JsonSerializer.Deserialize<JsonElement>(messageDTO.MetaData.ToJson()),
					Source = messageDTO.Source,
					Type = messageDTO.Type
				};
                message.OverrideCreationTime(messageDTO.CreationTime);
                message.OverrideId(messageDTO.Id);
                messages.Add(message);
            }
			return messages;
		}


        //SUPPORT FOR MULTIPLE SOURCES
        public async Task<List<Message>> GetBySource(int page, int pageSize, MessageSource source)
        {
            List<MessageDTO> messageDTOs = await repo.GetAllMessagesBySource(page, pageSize, source);
            List<Message> messages = new List<Message>();
            foreach (var messageDTO in messageDTOs)
            {
                Message message = new Message
                {
                    SourceId = messageDTO.SourceId,
                    Content = messageDTO.Content,
                    MetaData = JsonSerializer.Deserialize<JsonElement>(messageDTO.MetaData.ToJson()),
                    Source = messageDTO.Source,
                    Type = messageDTO.Type
                };
                message.OverrideCreationTime(messageDTO.CreationTime);
                message.OverrideId(messageDTO.Id);
                messages.Add(message);
            }
            return messages;
        }

        public async Task<List<Message>> GetBySourceIds(int page, int pageSize, string[] sourceIds)
        {
            List<MessageDTO> messageDTOs = await repo.GetAllMessagesBySourceIds(page, pageSize, sourceIds);
            List<Message> messages = new List<Message>();
            foreach (var messageDTO in messageDTOs)
            {
                Message message = new Message
                {
                    SourceId = messageDTO.SourceId,
                    Content = messageDTO.Content,
                    MetaData = JsonSerializer.Deserialize<JsonElement>(messageDTO.MetaData.ToJson()),
                    Source = messageDTO.Source,
                    Type = messageDTO.Type
                };
                message.OverrideCreationTime(messageDTO.CreationTime);
                message.OverrideId(messageDTO.Id);
                messages.Add(message);
            }
            return messages;
        }

        public async Task<List<Message>> GetByFilters(int page, int pageSize, MessageFilter filters) {
            List<MessageDTO> messageDTOs = await repo.GetMessagesByFilters(page, pageSize, filters);
            List<Message> messages = new List<Message>();
            foreach (var messageDTO in messageDTOs)
            {
                Message message = new Message
                {
                    SourceId = messageDTO.SourceId,
                    Content = messageDTO.Content,
                    MetaData = JsonSerializer.Deserialize<JsonElement>(messageDTO.MetaData.ToJson()),
                    Source = messageDTO.Source,
                    Type = messageDTO.Type
                };
                message.OverrideCreationTime(messageDTO.CreationTime);
                message.OverrideId(messageDTO.Id);
                messages.Add(message);
            }
            return messages;
        }

        public async Task<long> GetAmountByFilters(MessageFilter filters) {
            return await repo.GetAmountByFilters(filters);
        }
    }
}