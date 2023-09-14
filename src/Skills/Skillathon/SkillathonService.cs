using Microsoft.Extensions.Logging;
using Skillathon.Data;
using Skillathon.Models;
using Skills;

namespace Skillathon;

internal class SkillathonService : ISkillathonService
{
	private readonly ISkillathonDataService _skillathonDataService;
	private readonly ISkillService _skillService;
	private readonly ILogger<SkillathonService> _logger;

	public SkillathonService(
		ISkillathonDataService skillathonDataService,
		ISkillService skillService,
		ILogger<SkillathonService> logger)
	{
		_skillathonDataService = skillathonDataService;
		_skillService = skillService;
		_logger = logger;
	}

	public async Task<SkillathonEvent> CreateSkillathonAsync(string eventName, string skillName)
	{
		var creationFailed = false;
		var existingEvent = await GetSkillathonAsync(eventName);

		if (existingEvent is not null)
		{
			_logger.LogWarning("Could not create Skillathon event {eventName} because an event with that name already exists.", eventName);
			creationFailed = true;
		}

		if (!IsExistingSkill(skillName, out var actualName)) 
		{
			_logger.LogError("Could not create Skillathon event because {skillName} is not an actual skill.", skillName);
			creationFailed = true;
		}

		if (creationFailed)
		{
			return null!;
		}

		var skillathon = new SkillathonEvent(eventName, actualName);
		await _skillathonDataService.StoreSkillathonAsync(skillathon);

		return skillathon;
	}

	public Task<SkillathonEvent> GetSkillathonAsync(string eventName)
	{
		var skillathon = _skillathonDataService.GetSkillathonAsync(eventName);

		if (skillathon is null)
		{
			_logger.LogError("No Skillathon event exists with name {eventName}.", eventName);
			return null!;
		}

		return skillathon;
	}

	public async Task DeleteSkillathonAsync(string eventName)
	{
		await _skillathonDataService.DeleteSkillathonAsync(eventName);
	}

	private bool IsExistingSkill(string skillToCheck, out string actualName)
	{
		actualName = _skillService.GetSkillNames().First(skillName => skillName.Equals(skillToCheck, StringComparison.OrdinalIgnoreCase));
		
		return !string.IsNullOrWhiteSpace(actualName);
	}
}
