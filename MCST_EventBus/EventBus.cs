using System;
namespace MCST_EventBus
{
	public delegate Task ComputerCommand(int computerId, string command);

	public class EventBus
	{
		public event ComputerCommand? NewComputerCommand;

		public void PublishComputerCommand(int computerId, string command)
		{
			NewComputerCommand?.Invoke(computerId, command);
		}
	}
}

