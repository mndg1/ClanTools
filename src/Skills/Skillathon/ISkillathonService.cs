using SkillathonEvent.Models;

namespace SkillathonEvent;

public interface ISkillathonService
{
	Task<Skillathon> CreateSkillathonAsync(string eventName, string skillName, DateOnly? startDate = null, DateOnly? endDate = null);

	Task<Skillathon> GetSkillathonAsync(string eventName);

	Task UpdateSkillathonAsync(Skillathon skillathon);

	Task DeleteSkillathonAsync(string eventName);
}
