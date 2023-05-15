using System;
using System.Collections.Generic;
using System.Text.Json;
using MCST_Message.Data;
using MCST_Message.Data.DTOs;
using MCST_Message.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace MCST_Message.Domain
{
	public class MessageService
	{
		private readonly MessageRepository repo;
        private readonly IMessageController controller;

        public MessageService(MessageRepository _messageRepository, IMessageController controller)
		{
			repo = _messageRepository;
            this.controller = controller;
        }

		public bool NewMessage(Models.Message message)
		{
            if (message.IsValid())
            {
                controller.NewMessageOverWS(message);
                var metaData = BsonSerializer.Deserialize<BsonDocument>((message.MetaData).ToString());
            
			    repo.InsertMessage(new MessageDTO(message.Type, message.Source, message.Content, metaData, message.CreationTime, message.Identifier));
                return true;
            } else
            {
                return false;
            }

		}

		public List<Models.Message> GetAll(int page, int pageSize)
		{
			List<MessageDTO> messageDTOs = repo.GetAllMessages(page, pageSize);
            List<Models.Message> messages = new List<Models.Message>();
			foreach (var messageDTO in messageDTOs)
			{
                Models.Message message = new Models.Message
				{
					Identifier = messageDTO.Identifier,
					Content = messageDTO.Content,
                    MetaData = JsonSerializer.Deserialize<JsonElement>(messageDTO.MetaData.ToJson<BsonDocument>()),
					Source = messageDTO.Source,
					Type = messageDTO.Type
				};
                message.OverrideCreationTime(messageDTO.CreationTime);
                messages.Add(message);
            }
			return messages;
		}

        public List<Models.Message> GetBySource(int page, int pageSize, MessageSource source)
        {
            List<MessageDTO> messageDTOs = repo.GetAllMessagesBySource(page, pageSize, source);
            List<Models.Message> messages = new List<Models.Message>();
            foreach (var messageDTO in messageDTOs)
            {
                Models.Message message = new Models.Message
                {
                    Identifier = messageDTO.Identifier,
                    Content = messageDTO.Content,
                    MetaData = JsonSerializer.Deserialize<JsonElement>(messageDTO.MetaData.ToJson<BsonDocument>()),
                    Source = messageDTO.Source,
                    Type = messageDTO.Type
                };
                message.OverrideCreationTime(messageDTO.CreationTime);
                messages.Add(message);
            }
            return messages;
        }

        public List<Models.Message> GetByIdentifiers(int page, int pageSize, string[] identifiers)
        {
            List<MessageDTO> messageDTOs = repo.GetAllMessagesByIdentifiers(page, pageSize, identifiers);
            List<Models.Message> messages = new List<Models.Message>();
            foreach (var messageDTO in messageDTOs)
            {
                Models.Message message = new Models.Message
                {
                    Identifier = messageDTO.Identifier,
                    Content = messageDTO.Content,
                    MetaData = JsonSerializer.Deserialize<JsonElement>(messageDTO.MetaData.ToJson<BsonDocument>()),
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