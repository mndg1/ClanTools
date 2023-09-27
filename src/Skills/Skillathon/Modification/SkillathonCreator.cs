using MassTransit;
using MessagingContracts.Skills.Skillathon;
using Microsoft.Extensions.Logging;
using Skillathon.Data;
using Skillathon.Models;
using Skills;

namespace Skillathon.Modification;

internal class SkillathonCreator : ISkillathonCreator, IConsumer<CreateSkillathon>
{
	private readonly ISkillService _skillService;
	private readonly ISkillathonDataStore _skillathonDataService;
	private readonly ILogger<SkillathonCreator> _logger;

	public SkillathonCreator(
		ISkillService skillService,
		ISkillathonDataStore skillathonDataService,
		ILogger<SkillathonCreator> logger)
	{
		_skillService = skillService;
		_skillathonDataService = skillathonDataService;
		_logger = logger;
	}

	public async Task<SkillathonEvent> CreateSkillathonAsync(string eventName, string skillName, DateOnly? startDate = null, DateOnly? endDate = null)
	{
		var existingEvent = await _skillathonDataService.GetSkillathonAsync(eventName);
		
		if (existingEvent is not null)
		{
			_logger.LogWarning("Could not create Skillathon event {eventName} because an event with that name already exists.", eventName);
			return existingEvent;
		}

		if (!IsValidSkillName(skillName, out var actualSkillName))
		{
			_logger.LogError("Could not create Skillathon event {eventName} because {skillName} is not an actual skill.", eventName, skillName);
			return null!;
		}

		return new SkillathonEvent()
		{
			EventName = eventName,
			SkillName = actualSkillName!,
			StartDate = startDate,
			EndDate = endDate
		};
	}

	public async Task Consume(ConsumeContext<CreateSkillathon> context)
	{
		var command = context.Message;
		await CreateSkillathonAsync(command.EventName, command.SkillName, command.StartDate, command.EndDate);
	}

	private bool IsValidSkillName(string skillName, out string? actualName)
	{
		actualName = _skillService.GetSkillNames()
			.FirstOrDefault(name => name.Equals(skillName, StringComparison.OrdinalIgnoreCase));

		return !string.IsNullOrWhiteSpace(actualName);
	}
}
