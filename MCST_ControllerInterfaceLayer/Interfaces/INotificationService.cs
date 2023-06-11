
using System;
using MCST_Models;

namespace MCST_ControllerInterfaceLayer.Interfaces
{
	public interface INotificationService
	{
		Task<bool> SendNotification(Notification notification);
	}
}

