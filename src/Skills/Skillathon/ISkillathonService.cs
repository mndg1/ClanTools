using Skillathon.Models;

namespace Skillathon;

public interface ISkillathonService
{
	Task<SkillathonEvent> CreateSkillathonAsync(string eventName, string skillName, DateOnly? startDate = null, DateOnly? endDate = null);

	Task<SkillathonEvent> GetSkillathonAsync(string eventName);

	Task UpdateSkillathonAsync(SkillathonEvent skillathon);

	Task DeleteSkillathonAsync(string eventName);
}
