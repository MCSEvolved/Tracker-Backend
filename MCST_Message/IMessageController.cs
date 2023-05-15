using System;
using MCST_Message.Models;

namespace MCST_Message
{
	public interface IMessageController
	{
		void NewMessageOverWS(Message message);
	}
}

