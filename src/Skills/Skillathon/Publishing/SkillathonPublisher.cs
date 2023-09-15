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

	public async Task PublishSkillathonAsync(SkillathonEvent skillathon, EventMessageStatus status)
	{
		var message = new SkillathonEventMessage(skillathon, status);

		await _bus.Publish(message);
	}
}
