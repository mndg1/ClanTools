using SkillathonEvent.Models;

namespace SkillathonEvent.Data;

internal interface ISkillathonDataStore
{
	Task StoreSkillathonAsync(Skillathon skillathon);

	Task<Skillathon?> GetSkillathonAsync(string eventName);

	Task<IEnumerable<Skillathon>> GetSkillathonsAsync();

	Task DeleteSkillathonAsync(string eventName);
}
