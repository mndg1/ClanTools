using Skillathon.Models;

namespace Skillathon;

public interface ISkillathonService
{
	Task<SkillathonEvent> CreateSkillathonAsync(string eventName, string skillName);

	Task<SkillathonEvent?> GetSkillathonAsync(string eventName);

	Task DeleteSkillathonAsync(string eventName);
}
