using Skillathon.Models;

namespace Skillathon.Publishing;

internal class SkillathonPublisher : ISkillathonPublisher
{
	public ISet<ISkillathonSubscriber> Subscribers { get; init; } = new HashSet<ISkillathonSubscriber>();

	public async Task PublishSkillathonStart(SkillathonEvent skillathon)
	{
		var publishTasks = Subscribers.Select(sub => sub.ConsumeSkillathonStart(skillathon));

		await Task.WhenAll(publishTasks);
	}

	public async Task PublishSkillathonUpdate(SkillathonEvent skillathon)
	{
		var publishTasks = Subscribers.Select(sub => sub.ConsumeSkillathonUpdate(skillathon));

		await Task.WhenAll(publishTasks);
	}

	public async Task PublishSkillathonEnd(SkillathonEvent skillathon)
	{
		var publishTasks =  Subscribers.Select(sub => sub.ConsumeSkillathonEnd(skillathon));
		
		await Task.WhenAll(publishTasks);
	}
}
