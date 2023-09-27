using Skillathon.Models;

namespace Skillathon.Modification;

internal interface ISkillathonCreator
{
	Task<SkillathonEvent> CreateSkillathonAsync(string eventName, string skillName, DateOnly? startDate = null, DateOnly? endDate = null);
}
