using Skills.Models;

namespace Skills;

public interface ISkillService
{
	Task<SkillSet> GetSkillSetAsync(string userName);

	Task<IEnumerable<SkillSet>> GetSkillSetsAsync(IEnumerable<string> userNames);
}
