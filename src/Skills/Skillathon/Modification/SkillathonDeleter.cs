using MassTransit;
using MessagingContracts.Skills.Skillathon;
using SkillathonEvent.Data;

namespace SkillathonEvent.Modification;

internal class SkillathonDeleter : ISkillathonDeleter, IConsumer<DeleteSkillathon>
{
	private readonly ISkillathonDataStore _skillathonDataStore;

	public SkillathonDeleter(ISkillathonDataStore skillathonDataStore)
	{
		_skillathonDataStore = skillathonDataStore;
	}

	public async Task DeleteSkillathonAsync(string eventName)
	{
		await _skillathonDataStore.DeleteSkillathonAsync(eventName);
	}

	public async Task Consume(ConsumeContext<DeleteSkillathon> context)
	{
		await DeleteSkillathonAsync(context.Message.EventName);
	}
}
