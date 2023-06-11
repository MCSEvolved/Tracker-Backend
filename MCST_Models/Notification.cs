using System;
namespace MCST_Models
{
	public class Notification
	{
		public string Title { get; set; } = default!;
		public string Body { get; set; } = default!;
		public string Topic { get; set; } = default!;

		public bool isValid()
		{
			return Title != null && Body != null && Topic != null;
		}

    }
}

