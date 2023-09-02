using Skills.Models;

namespace Skills.Data;

public interface ISkillDataRetriever
{
    Task<SkillSet> GetSkillSetAsync(string userName);

    Task<IEnumerable<SkillSet>> GetSkillSetsAsync(IEnumerable<string> userNames);
}
