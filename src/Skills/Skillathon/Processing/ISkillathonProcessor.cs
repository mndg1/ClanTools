using Skillathon.Models;

namespace Skillathon.Processing;

internal interface ISkillathonProcessor
{
    Task ProcessAsync(IEnumerable<SkillathonEvent> skillathons);
}
