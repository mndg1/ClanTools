using SkillathonEvent.Models;

namespace SkillathonEvent.Processing;

internal interface ISkillathonProcessor
{
    Task ProcessAsync(IEnumerable<Skillathon> skillathons);
}
