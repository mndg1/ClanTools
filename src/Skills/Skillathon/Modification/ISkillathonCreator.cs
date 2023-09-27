using SkillathonEvent.Models;

namespace SkillathonEvent.Modification;

internal interface ISkillathonCreator
{
	Task<Skillathon> CreateSkillathonAsync(string eventName, string skillName, DateOnly? startDate = null, DateOnly? endDate = null);
}
