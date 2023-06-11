using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MCST_Controller.SignalRHubs
{
    [Authorize(Policy = "IsGuest")]
    public class ClientHub : Hub
	{
		
	}
}

