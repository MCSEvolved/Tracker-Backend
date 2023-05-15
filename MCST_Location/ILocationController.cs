using System;
using MCST_Location.Domain.Models;

namespace MCST_Location
{
	public interface ILocationController
	{
		void NewLocationOverWS(Location location);
	}
}

