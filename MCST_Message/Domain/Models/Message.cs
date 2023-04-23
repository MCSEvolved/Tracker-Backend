using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace MCST_Message.Models
{
	public enum MessageSource
	{
		Service,
		Computer
	}
	public enum MessageType
	{
		Error,
		Warning,
		Info,
		Debug
	}

	public class Message
	{
        //public Message(MessageType type, string content, long creationTime, dynamic metaData)
        //{
        //    Type = type;
        //    Content = content;
        //    MetaData = metaData;
        //    CreationTime = creationTime;
        //}
        [Required]
        public MessageType Type { get; set; }

        [Required]
        public MessageSource Source { get; set; }

        [Required]
        public string? Content { get; set; }

		public JsonElement? MetaData { get; set; }

        [Required]
        public long CreationTime { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds();

        [Required]
        public string Identifier { get; set; } = "Unknown";
	}
}

