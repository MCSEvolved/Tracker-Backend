using System;
using System.Runtime.ConstrainedExecution;
using MCST_Message.Data.DTOs;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using MCST_Enums;
using Microsoft.Extensions.Configuration;

namespace MCST_Message.Data
{
	public class MessageRepository
	{
        private readonly IMongoCollection<MessageDTO> _messages;

        public MessageRepository(IMongoClient client, IConfiguration config)
		{
            var database = client.GetDatabase(config["MongoDb:DbName"]);
            var collection = database.GetCollection<MessageDTO>("messages");

            _messages = collection;
        }

        public async Task InsertMessage(MessageDTO message)
        {
            await _messages.InsertOneAsync(message);
        }

        public async Task<List<MessageDTO>> GetAllMessages(int page, int pageSize)
        {
            var sort = Builders<MessageDTO>.Sort.Descending("CreationTime");
            List<MessageDTO> messages = await _messages
                .Find(new BsonDocument())
                .Sort(sort)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
            return messages;
        }

        public async Task<List<MessageDTO>> GetAllMessagesBySource(int page, int pageSize, MessageSource source)
        {
            var sort = Builders<MessageDTO>.Sort.Descending("CreationTime");
            List<MessageDTO> messages = await _messages
                .Find(new BsonDocument("Source", source))
                .Sort(sort)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
            return messages;
        }

        public async Task<List<MessageDTO>> GetAllMessagesBySourceIds(int page, int pageSize, string[] sourceIds)
        {
            var sort = Builders<MessageDTO>.Sort.Descending("CreationTime");
            List<MessageDTO> messages = await _messages
                .Find(model => sourceIds.Contains(model.SourceId))
                .Sort(sort)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
            return messages;
        }
    }
}

