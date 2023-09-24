using MassTransit;
using Skillathon.Models;

namespace Skillathon.Publishing;

internal class SkillathonPublisher : ISkillathonPublisher
{
	private readonly IBus _bus;

	public SkillathonPublisher(IBus bus)
	{
		_bus = bus;
	}

	public async Task PublishSkillathonAsync(SkillathonEvent skillathon)
	{
		var message = CreateMessage(skillathon);

		await _bus.Publish(message);
	}

	private SkillathonEventMessage CreateMessage(SkillathonEvent skillathon)
	{
		var eventName = skillathon.EventName;
		var skillName = skillathon.SkillName;
		var participantsData = skillathon.Participants;
		var terminate = skillathon.State == SkillathonState.Ended;

		return new SkillathonEventMessage(eventName, skillName, participantsData, terminate);
	}
}
