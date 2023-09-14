using Skills.Models;

namespace Skills;

public interface ISkillService
{
	Task<SkillSet> GetSkillSetAsync(string userName);

	IList<string> GetSkillNames();
}
