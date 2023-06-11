using System;
namespace MCST_Auth.Domain.Models
{
	public class RequestIdTokenModel
	{
		public string token { get; set; } = default!;
		public bool returnSecureToken { get; private set; } = true;
	}
}

