using Skillathon.Models;

namespace Skillathon;

internal interface ISkillathonProcessor
{
	Task<IEnumerable<SkillathonEvent>> ProcessAsync(IEnumerable<SkillathonEvent> skillathons);
}
