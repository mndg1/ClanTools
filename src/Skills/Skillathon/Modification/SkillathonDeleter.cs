using MassTransit;
using MessagingContracts.Skills.Skillathon;
using SkillathonEvent.Data;

namespace SkillathonEvent.Modification;

internal class SkillathonDeleter : ISkillathonDeleter, IConsumer<DeleteSkillathon>
{
	private readonly ISkillathonDataStore _skillathonDataService;

	public SkillathonDeleter(ISkillathonDataStore skillathonDataService)
	{
		_skillathonDataService = skillathonDataService;
	}

	public async Task DeleteSkillathonAsync(string eventName)
	{
		await _skillathonDataService.DeleteSkillathonAsync(eventName);
	}

	public async Task Consume(ConsumeContext<DeleteSkillathon> context)
	{
		await DeleteSkillathonAsync(context.Message.EventName);
	}
}
