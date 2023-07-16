using System;
namespace MCST_Models
{
	public class ComputerCommand
	{
        public ComputerCommand(int computerId, string command)
        {
            ComputerId = computerId;
            Command = command;
        }


        public int ComputerId { get; private set; }
		public string Command { get; private set; }
	}
}

