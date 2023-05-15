using System;
namespace MCST_Command
{
	public interface ICommandController
	{
		void SendCommandToComputer(int computerId, string command);
		

    }
}

