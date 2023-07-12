using MCST_Message.Data.DTOs;
using MongoDB.Driver;
using MongoDB.Bson;
using MCST_Enums;
using Microsoft.Extensions.Configuration;
using MCST_Models;

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

        private FilterDefinition<MessageDTO> GetMessageFilter(MessageFilter filters) {

            var builder = Builders<MessageDTO>.Filter;
            var filter = builder.Empty;

            if (filters.Types != null)
            {
                var typesFilter = builder.In(x => x.Type, filters.Types);
                filter &= typesFilter;
            }

            if (filters.Sources != null)
            {
                var sourcesFilter = builder.In(x => x.Source, filters.Sources);
                filter &= sourcesFilter;
            }

            if (filters.BeginRange != null)
            {
                var beginRangeFilter = builder.Gte(x => x.CreationTime, filters.BeginRange);
                filter &= beginRangeFilter;
            }

            if (filters.EndRange != null)
            {
                var beginRangeFilter = builder.Lte(x => x.CreationTime, filters.EndRange);
                filter &= beginRangeFilter;
            }

            if (filters.SourceIds != null)
            {
                var sourceIdsFilter = builder.In(x => x.SourceId, filters.SourceIds);
                filter &= sourceIdsFilter;
            }



            return filter;
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

        public async Task<List<MessageDTO>> GetMessagesByFilters(int page, int pageSize, MessageFilter filters)
        {
            var sort = Builders<MessageDTO>.Sort.Descending("CreationTime");
            List<MessageDTO> messages = await _messages
                .Find(GetMessageFilter(filters))
                .Sort(sort)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
            return messages;
        }

        public async Task<long> GetAmountByFilters(MessageFilter filters)
        {
            long amount = await _messages.CountDocumentsAsync(GetMessageFilter(filters));
            return amount;
        }
    }
}

