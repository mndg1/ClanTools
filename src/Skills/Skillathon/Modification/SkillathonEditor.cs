using MassTransit;
using MessagingContracts.Skills.Skillathon;
using Skillathon.Data;

namespace Skillathon.Modification;

internal class SkillathonEditor : ISkillathonEditor, IConsumer<EditSkillathon>
{
	private readonly ISkillathonDataStore _skillathonDataService;

	public SkillathonEditor(ISkillathonDataStore skillathonDataService)
	{
		_skillathonDataService = skillathonDataService;
	}

	public async Task SetStartDateAsync(string eventName, DateOnly? date)
	{
		if (!date.HasValue)
		{
			return;
		}

		var skillathon = await _skillathonDataService.GetSkillathonAsync(eventName);

		if (skillathon is null) 
		{
			return;
		}

		skillathon.StartDate = date;
		await _skillathonDataService.StoreSkillathonAsync(skillathon);
	}

	public async Task SetEndDateAsync(string eventName, DateOnly? date)
	{
		if (!date.HasValue)
		{
			return;
		}

		var skillathon = await _skillathonDataService.GetSkillathonAsync(eventName);

		if (skillathon is null)
		{
			return;
		}

		skillathon.EndDate = date;
		await _skillathonDataService.StoreSkillathonAsync(skillathon);
	}

	public async Task Consume(ConsumeContext<EditSkillathon> context)
	{
		var command = context.Message;

		await SetStartDateAsync(command.EventName, command.StartDate);
		await SetEndDateAsync(command.EventName, command.EndDate);
	}
}
