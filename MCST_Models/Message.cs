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
        public MessageType Type { get; set; }

        public MessageSource Source { get; set; }

        public string Content { get; set; } = default!;

		public JsonElement? MetaData { get; set; }

        public long CreationTime { get; private set; } = DateTimeOffset.Now.ToUnixTimeSeconds();

        public string Identifier { get; set; } = "Unknown";


        public bool IsValid()
        {
			return Type != MessageType.INVALID && Source != MessageSource.INVALID && Content != null && CreationTime > 0;
        }

		public void OverrideCreationTime(long creationTime)
		{
			CreationTime = creationTime;
		}
	}
}

