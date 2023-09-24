using Shared;
using Skillathon.Models;
using SkillHistory;
using Skills;
using UserIdentification;

namespace Skillathon;

internal class SkillathonProcessor : ISkillathonProcessor
{
	private readonly IUserIdentificationService _userIdenificationService;
	private readonly ISkillService _skillService;
	private readonly ISkillHistoryService _skillHistoryService;
	private readonly ITimeProvider _timeProvider;

	public SkillathonProcessor(
		IUserIdentificationService userIdenificationService,
		ISkillService skillService,
		ISkillHistoryService skillHistoryService,
		ITimeProvider timeProvider)
	{
		_userIdenificationService = userIdenificationService;
		_skillService = skillService;
		_skillHistoryService = skillHistoryService;
		_timeProvider = timeProvider;
	}

	public async Task<IEnumerable<SkillathonEvent>> ProcessAsync(IEnumerable<SkillathonEvent> skillathons)
	{
		foreach (var skillathon in skillathons)
		{
			var hasUpdated = UpdateState(skillathon);

			if (!hasUpdated)
			{
				continue;
			}

			await UpdateSkillathonAsync(skillathon);
		}

		return skillathons;
	}

	private bool UpdateState(SkillathonEvent skillathon)
	{
		var updated = false;

		if (ShouldStart(skillathon))
		{
			skillathon.State = SkillathonState.Active;
			updated = true;
		}
		else if (ShouldEnd(skillathon))
		{
			skillathon.State = SkillathonState.Ended;
			updated = true;
		}

		return updated;
	}

	private async Task UpdateSkillathonAsync(SkillathonEvent skillathon)
	{
		if (skillathon.State != SkillathonState.Active)
		{
			return;
		}

		foreach (var participantData in skillathon.Participants)
		{
			var name = participantData.Name;

			var skillSet = await _skillService.GetSkillSetAsync(name);
			var userId = await _userIdenificationService.GetUserId(name) 
				?? await _userIdenificationService.RegisterNewUser(name);

			await _skillHistoryService.StoreCurrentDataAsync(userId.Value, skillSet);

			var startDate = skillathon.StartDate!.Value.ToDateTime(TimeOnly.MinValue);
			var historicData = await _skillHistoryService.GetSkillHistorySinceAsync(userId.Value, startDate);

			participantData.ExperienceHistory = historicData.SkillHistory.ToDictionary(
				historicEntry => historicEntry.Key,
				historicEntry => historicEntry.Value.Skills[skillathon.SkillName.ToLower()].Experience);
		}

		skillathon.LastUpdateTime = _timeProvider.UtcNow;
	}

	private bool ShouldStart(SkillathonEvent skillathon)
	{
		if (skillathon.State != SkillathonState.Waiting)
		{
			return false;
		}

		if (skillathon.StartDate is null)
		{
			return false;
		}

		var currentDate = DateOnly.FromDateTime(_timeProvider.UtcNow);

		return skillathon.StartDate <= currentDate;
	}

	private bool ShouldEnd(SkillathonEvent skillathon)
	{
		if (skillathon.EndDate is null)
		{
			return false;
		}

		var currentDate = DateOnly.FromDateTime(_timeProvider.UtcNow);

		return skillathon.EndDate <= currentDate;
	}
}
