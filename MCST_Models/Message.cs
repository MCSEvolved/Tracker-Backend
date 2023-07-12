using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using MCST_Enums;
using Newtonsoft.Json.Converters;

namespace MCST_Models
{
	public class Message
	{
		public string? Id { get; private set; }

        public MessageType Type { get; set; }

        public MessageSource Source { get; set; }

        public string Content { get; set; } = default!;

		public JsonElement? MetaData { get; set; } = JsonDocument.Parse("{}").RootElement;

        public long CreationTime { get; private set; } = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        public string SourceId { get; set; } = "Unknown";


        public bool IsValid()
        {
			return Type != MessageType.INVALID && Source != MessageSource.INVALID && Content != null && CreationTime > 0;
        }

		public void OverrideCreationTime(long creationTime)
		{
			CreationTime = creationTime;
		}

		public void OverrideId(string? id) {
			Id = id;
		}
	}
}

