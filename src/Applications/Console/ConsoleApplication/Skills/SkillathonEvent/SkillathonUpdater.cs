using MassTransit;
using MessagingContracts.Skills.Skillathon;

namespace ConsoleApplication.Skills.SkillathonEvent;

internal class SkillathonUpdater : ISkillathonUpdater
{
	private readonly IBus _bus;

	public SkillathonUpdater(IBus bus)
	{
		_bus = bus;
	}

	public async Task Update(string eventName)
	{
		var updateCommand = new UpdateSkillathon(eventName);

		await _bus.Publish(updateCommand);
	}
}
