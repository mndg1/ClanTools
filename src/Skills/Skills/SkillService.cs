using Microsoft.Extensions.Options;
using Skills.Data;
using Skills.Models;

namespace Skills;

internal class SkillService : ISkillService
{
	private readonly ISkillDataRetriever _skillDataRetriever;
	private readonly SkillsConfiguration _skillsConfig;

	public SkillService(ISkillDataRetriever skillDataRetriever, IOptions<SkillsConfiguration> skillsConfig)
	{
		_skillDataRetriever = skillDataRetriever;
		_skillsConfig = skillsConfig.Value;
	}

	public Task<SkillSet> GetSkillSetAsync(string userName)
	{
		return _skillDataRetriever.GetSkillSetAsync(userName);
	}

	public IList<string> GetSkillNames()
	{
		return _skillsConfig.SkillNames;
	}
}
