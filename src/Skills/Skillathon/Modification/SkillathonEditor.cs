using MassTransit;
using MessagingContracts.Skills.Skillathon;
using SkillathonEvent.Data;

namespace SkillathonEvent.Modification;

internal class SkillathonEditor : ISkillathonEditor, IConsumer<EditSkillathon>
{
	private readonly ISkillathonDataStore _skillathonDataStore;

	public SkillathonEditor(ISkillathonDataStore skillathonDataStore)
	{
		_skillathonDataStore = skillathonDataStore;
	}

	public async Task SetStartDateAsync(string eventName, DateOnly? date)
	{
		if (!date.HasValue)
		{
			return;
		}

		var skillathon = await _skillathonDataStore.GetSkillathonAsync(eventName);

		if (skillathon is null) 
		{
			return;
		}

		skillathon.StartDate = date;
		await _skillathonDataStore.StoreSkillathonAsync(skillathon);
	}

	public async Task SetEndDateAsync(string eventName, DateOnly? date)
	{
		if (!date.HasValue)
		{
			return;
		}

		var skillathon = await _skillathonDataStore.GetSkillathonAsync(eventName);

		if (skillathon is null)
		{
			return;
		}

		skillathon.EndDate = date;
		await _skillathonDataStore.StoreSkillathonAsync(skillathon);
	}

	public async Task Consume(ConsumeContext<EditSkillathon> context)
	{
		var command = context.Message;

		await SetStartDateAsync(command.EventName, command.StartDate);
		await SetEndDateAsync(command.EventName, command.EndDate);
	}
}
