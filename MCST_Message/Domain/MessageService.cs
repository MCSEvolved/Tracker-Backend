using System;
using System.Collections.Generic;
using System.Text.Json;
using MCST_Message.Data;
using MCST_Message.Data.DTOs;
using MCST_Message.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace MCST_Message
{
	public class MessageService
	{
		private readonly MessageRepository repo;
		public MessageService(MessageRepository _messageRepository)
		{
			repo = _messageRepository;
		}

		public void NewMessage(Message message)
		{
            var metaData = BsonSerializer.Deserialize<BsonDocument>((message.MetaData).ToString());
            
			repo.InsertMessage(new MessageDTO(message.Type, message.Source, message.Content, metaData, message.CreationTime, message.Identifier));

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
					CreationTime = messageDTO.CreationTime,
					Content = messageDTO.Content,
                    MetaData = JsonSerializer.Deserialize<JsonElement>(messageDTO.MetaData.ToJson()),
					Source = messageDTO.Source,
					Type = messageDTO.Type
				};
				messages.Add(message);
            }
			return messages;
		}

        public List<Message> GetBySource(int page, int pageSize, MessageSource source)
        {
            List<MessageDTO> messageDTOs = repo.GetAllMessagesBySource(page, pageSize, source);
            List<Message> messages = new List<Message>();
            foreach (var messageDTO in messageDTOs)
            {
                Message message = new Message
                {
                    Identifier = messageDTO.Identifier,
                    CreationTime = messageDTO.CreationTime,
                    Content = messageDTO.Content,
                    MetaData = JsonSerializer.Deserialize<JsonElement>(messageDTO.MetaData.ToJson()),
                    Source = messageDTO.Source,
                    Type = messageDTO.Type
                };
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
                    CreationTime = messageDTO.CreationTime,
                    Content = messageDTO.Content,
                    MetaData = JsonSerializer.Deserialize<JsonElement>(messageDTO.MetaData.ToJson()),
                    Source = messageDTO.Source,
                    Type = messageDTO.Type
                };
                messages.Add(message);
            }
            return messages;
        }
    }
}