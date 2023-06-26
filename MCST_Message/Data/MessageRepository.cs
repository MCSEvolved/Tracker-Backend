using System;
using System.Runtime.ConstrainedExecution;
using MCST_Message.Data.DTOs;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using MCST_Enums;

namespace MCST_Message.Data
{
	public class MessageRepository
	{
        private readonly IMongoCollection<MessageDTO> _messages;

        public MessageRepository(IMongoClient client)
		{
            var database = client.GetDatabase("mcst_dev");
            var collection = database.GetCollection<MessageDTO>("messages");

            _messages = collection;
        }

        public void InsertMessage(MessageDTO message)
        {
            _messages.InsertOne(message);
        }

        public List<MessageDTO> GetAllMessages(int page, int pageSize)
        {
            var sort = Builders<MessageDTO>.Sort.Descending("CreationTime");
            List<MessageDTO> messages = _messages
                .Find(new BsonDocument())
                .Sort(sort)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToList();
            return messages;
        }

        public List<MessageDTO> GetAllMessagesBySource(int page, int pageSize, MessageSource source)
        {
            var sort = Builders<MessageDTO>.Sort.Descending("CreationTime");
            List<MessageDTO> messages = _messages
                .Find(new BsonDocument("Source", source))
                .Sort(sort)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToList();
            return messages;
        }

        public List<MessageDTO> GetAllMessagesBySourceIds(int page, int pageSize, string[] sourceIds)
        {
            var sort = Builders<MessageDTO>.Sort.Descending("CreationTime");
            List<MessageDTO> messages = _messages
                .Find(model => sourceIds.Contains(model.SourceId))
                .Sort(sort)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToList();
            return messages;
        }
    }
}

