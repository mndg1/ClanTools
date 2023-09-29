using ConsoleApplication.Skills.SkillathonEvent.Models;
using MassTransit;
using MessagingContracts.Skills.Skillathon;

namespace ConsoleApplication.Skills.SkillathonEvent;

internal class SkillathonUpdateProcessor : ISkillathonUpdateProcessor, IConsumer<SkillathonUpdated>
{
	public async Task Consume(ConsumeContext<SkillathonUpdated> context)
	{
		var eventName = context.Message.EventName;
		var skillName = context.Message.SkillName;
		var participants = context.Message.Participants.Select(x => new Participant(x.Name, x.ExperienceHistory));

		var skillathon = new Skillathon(eventName, skillName, participants);

		await ProcessSkillathon(skillathon);
	}

	public Task ProcessSkillathon(Skillathon skillathon)
	{
		foreach (var p in skillathon.Participants)
		{
			Console.WriteLine(p.UserName);
		}

		return Task.CompletedTask;
	}
}
