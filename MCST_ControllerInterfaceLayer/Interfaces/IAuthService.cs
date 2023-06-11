using System;
namespace MCST_ControllerInterfaceLayer.Interfaces
{
	public interface IAuthService
	{
		Task<string> RequestIdToken();
	}
}

