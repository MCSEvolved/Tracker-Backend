using System;
using MCST_Message.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace MCST_Message.Data.DTOs
{
	public class MessageDTO
	{
        public MessageDTO(MessageType type, MessageSource source, string content, BsonDocument? metaData, long creationTime, string identifier)
        {
            Type = type;
            Source = source;
            Content = content;
            MetaData = metaData;
            CreationTime = creationTime;
            Identifier = identifier;
        }

        public MessageDTO(string? id, MessageType type, MessageSource source, string content, BsonDocument? metaData, long creationTime, string identifier)
        {
            Id = id;
            Type = type;
            Source = source;
            Content = content;
            MetaData = metaData;
            CreationTime = creationTime;
            Identifier = identifier;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public MessageType Type { get; set; }
        public MessageSource Source { get; set; }
        [BsonRequired]
        public string Content { get; set; }

        public BsonDocument? MetaData { get; set; }
        public long CreationTime { get; set; }
        public string Identifier { get; set; }
    }
}

