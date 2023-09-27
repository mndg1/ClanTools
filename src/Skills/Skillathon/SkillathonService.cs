using Microsoft.Extensions.Logging;
using Skillathon.Modification;
using Skillathon.Data;
using Skillathon.Models;

namespace Skillathon;

internal class SkillathonService : ISkillathonService
{
	private readonly ISkillathonDataStore _skillathonDataService;
	private readonly ISkillathonCreator _skillathonCreator;
	private readonly ILogger<SkillathonService> _logger;

	public SkillathonService(
		ISkillathonDataStore skillathonDataService,
		ISkillathonCreator skillathonCreator,
		ILogger<SkillathonService> logger)
	{
		_skillathonDataService = skillathonDataService;
		_skillathonCreator = skillathonCreator;
		_logger = logger;
	}

	public async Task<SkillathonEvent> CreateSkillathonAsync(string eventName, string skillName, DateOnly? startDate = null, DateOnly? endDate = null)
	{
		var skillathon = await _skillathonCreator.CreateSkillathonAsync(eventName, skillName, startDate, endDate);
		
		await _skillathonDataService.StoreSkillathonAsync(skillathon);

		return skillathon;
	}

	public async Task<SkillathonEvent> GetSkillathonAsync(string eventName)
	{
		var skillathon = await _skillathonDataService.GetSkillathonAsync(eventName);

		if (skillathon is null)
		{
			_logger.LogError("No Skillathon event exists with name {eventName}.", eventName);
			return null!;
		}

		return skillathon;
	}

	public async Task UpdateSkillathonAsync(SkillathonEvent skillathon)
	{
		await _skillathonDataService.StoreSkillathonAsync(skillathon);
	}

	public async Task DeleteSkillathonAsync(string eventName)
	{
		await _skillathonDataService.DeleteSkillathonAsync(eventName);
	}
}
