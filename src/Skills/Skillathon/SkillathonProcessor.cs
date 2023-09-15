using MassTransit;
using Microsoft.TeamFoundation.Framework.Common;
using Skillathon.Models;
using Skillathon.Publishing;
using SkillHistory;
using Skills;
using UserIdentification;

namespace Skillathon;

internal class SkillathonProcessor : ISkillathonProcessor
{
	private readonly IBus _bus;
	private readonly IUserIdentificationService _userIdenificationService;
	private readonly ISkillService _skillService;
	private readonly ISkillHistoryService _skillHistoryService;
	private readonly ITimeProvider _timeProvider;

	public SkillathonProcessor(
		IBus bus,
		IUserIdentificationService userIdenificationService,
		ISkillService skillService,
		ISkillHistoryService skillHistoryService,
		ITimeProvider timeProvider)
	{
		_bus = bus;
		_userIdenificationService = userIdenificationService;
		_skillService = skillService;
		_skillHistoryService = skillHistoryService;
		_timeProvider = timeProvider;
	}

	public async Task<IEnumerable<SkillathonEvent>> ProcessAsync(IEnumerable<SkillathonEvent> skillathons)
	{
		foreach (var skillathon in skillathons)
		{
			if (ShouldStart(skillathon))
			{
				await StartAsync(skillathon);
			}
			else if (ShouldEnd(skillathon))
			{
				await EndAsync(skillathon);
			}
			else
			{
				await UpdateAsync(skillathon);
				var message = new SkillathonEventMessage(skillathon, EventMessageStatus.Update);
				await _bus.Publish(message);
			}
		}

		return skillathons;
	}

	private async Task StartAsync(SkillathonEvent skillathon)
	{
		var message = new SkillathonEventMessage(skillathon, EventMessageStatus.Start);

		skillathon.State = SkillathonState.Active;
		await UpdateAsync(skillathon);

		await _bus.Publish(message);
	}

	private async Task UpdateAsync(SkillathonEvent skillathon)
	{
		foreach (var participantName in  skillathon.ParticipantNames)
		{
			var userId = await _userIdenificationService.GetUserId(participantName) ?? await _userIdenificationService.RegisterNewUser(participantName);
			var skillSet = await _skillService.GetSkillSetAsync(participantName);

			await _skillHistoryService.StoreCurrentDataAsync(userId.Value, skillSet);
		}
	}

	private async Task EndAsync(SkillathonEvent skillathon)
	{
		var message = new SkillathonEventMessage(skillathon, EventMessageStatus.End);

		skillathon.State = SkillathonState.Ended;
		await UpdateAsync(skillathon);

		await _bus.Publish(message);
	}

	private bool ShouldStart(SkillathonEvent skillathon)
	{
		if (skillathon.State != SkillathonState.Waiting)
		{
			return false;
		}

		if (skillathon.StartTime is null)
		{
			return false;
		}

		var currentTime = _timeProvider.Now.ToUniversalTime();

		return skillathon.StartTime >= currentTime;
	}

	private bool ShouldEnd(SkillathonEvent skillathon)
	{
		if (skillathon.EndTime is null)
		{
			return false;
		}

		var currentTime = _timeProvider.Now.ToUniversalTime();

		return skillathon.EndTime <= currentTime;
	}
}
