using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MCST_Controller.SignalRHubs
{
	[Authorize]
	public class AuthorizedClientHub: Hub
	{
		public AuthorizedClientHub()
		{
		}
	}
}

