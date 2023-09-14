using Skillathon.Models;

namespace Skillathon.Data;

internal interface ISkillathonDataService
{
	Task StoreSkillathonAsync(SkillathonEvent skillathon);

	Task<SkillathonEvent?> GetSkillathonAsync(string eventName);

	Task DeleteSkillathonAsync(string eventName);
}
