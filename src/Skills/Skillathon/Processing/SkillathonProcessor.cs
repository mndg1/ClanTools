using Shared;
using Skillathon.Modification;
using Skillathon.Models;

namespace Skillathon.Processing;

internal class SkillathonProcessor : ISkillathonProcessor
{
    private readonly ISkillathonUpdater _skillathonUpdater;
    private readonly ITimeProvider _timeProvider;

    public SkillathonProcessor(
        ISkillathonUpdater skillathonUpdater,
        ITimeProvider timeProvider)
    {
        _skillathonUpdater = skillathonUpdater;
        _timeProvider = timeProvider;
    }

    public async Task ProcessAsync(IEnumerable<SkillathonEvent> skillathons)
    {
        foreach (var skillathon in skillathons)
        {
            var hasUpdated = UpdateState(skillathon);

            if (!hasUpdated)
            {
                continue;
            }

            await _skillathonUpdater.UpdateSkillathonAsync(skillathon.EventName);
        }
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
