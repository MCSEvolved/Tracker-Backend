using System;
using MCST_Enums;

namespace MCST_Models
{
	public class MessageFilter
	{
		public MessageType[]? Types { get; set; } = null;
		public MessageSource[]? Sources { get; set; } = null;
		public long? BeginRange { get; set; } = null;
		public long? EndRange { get; set; } = null;
		public string[]? SourceIds { get; set; } = null;
	}
}

