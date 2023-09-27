using MassTransit;
using MessagingContracts.Skills.Skillathon;
using Microsoft.Extensions.Logging;
using Shared;
using SkillathonEvent.Data;
using SkillathonEvent.Models;
using SkillathonEvent.Publishing;
using SkillHistory;
using Skills;
using UserIdentification;

namespace SkillathonEvent.Modification;

internal class SkillathonUpdater : ISkillathonUpdater, IConsumer<UpdateSkillathon>
{
    private readonly ISkillathonDataStore _skillathonDataService;
    private readonly IUserIdentificationService _userIdentificationService;
    private readonly ISkillService _skillService;
    private readonly ISkillHistoryService _skillHistoryService;
    private readonly ITimeProvider _timeProvider;
    private readonly ISkillathonPublisher _skillathonPublisher;
    private readonly ILogger<SkillathonUpdater> _logger;

    public SkillathonUpdater(
        ISkillathonDataStore skillathonDataService,
        IUserIdentificationService userIdentificationService,
        ISkillService skillService,
        ISkillHistoryService skillHistoryService,
        ITimeProvider timeProvider,
        ISkillathonPublisher skillathonPublisher,
        ILogger<SkillathonUpdater> logger)
    {
        _skillathonDataService = skillathonDataService;
        _userIdentificationService = userIdentificationService;
        _skillService = skillService;
        _skillHistoryService = skillHistoryService;
        _timeProvider = timeProvider;
        _skillathonPublisher = skillathonPublisher;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UpdateSkillathon> context)
    {
        var eventName = context.Message.EventName;
        await UpdateSkillathonAsync(eventName);
    }

    public async Task UpdateSkillathonAsync(string eventName)
    {
        var skillathon = await _skillathonDataService.GetSkillathonAsync(eventName);

        if (skillathon is null)
        {
            _logger.LogError("Could not update {eventName} because no Skillathon with that name was found.", eventName);
            return;
        }

        if (skillathon.State != SkillathonState.Active)
        {
            return;
        }

        foreach (var participantData in skillathon.Participants)
        {
            var name = participantData.Name;

            var skillSet = await _skillService.GetSkillSetAsync(name);
            var userId = await _userIdentificationService.GetUserId(name)
            ?? await _userIdentificationService.RegisterNewUser(name);

            await _skillHistoryService.StoreCurrentDataAsync(userId.Value, skillSet);

            var startDate = skillathon.StartDate!.Value.ToDateTime(TimeOnly.MinValue);
            var historicData = await _skillHistoryService.GetSkillHistorySinceAsync(userId.Value, startDate);

            participantData.ExperienceHistory = historicData.SkillHistory.ToDictionary(
                historicEntry => historicEntry.Key,
                historicEntry => historicEntry.Value.Skills[skillathon.SkillName.ToLower()].Experience);
        }

        skillathon.LastUpdateTime = _timeProvider.UtcNow;

        await _skillathonPublisher.PublishSkillathonEventAsync(skillathon, EventMessageStatus.Updated);
    }
}
