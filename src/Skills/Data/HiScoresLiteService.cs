using Microsoft.Extensions.Options;
using Skills.Models;

namespace Skills.Data;

public class HiScoresLiteService : RuneScapeApiService, ISkillDataRetriever
{
	private readonly SkillsConfiguration _skillsConfig;

	public HiScoresLiteService(
		IHttpClientFactory httpClientFactory,
		IOptions<SkillsConfiguration> skillsConfig)
		: base(httpClientFactory)
	{
		_skillsConfig = skillsConfig.Value;
	}

	public async Task<SkillSet> GetSkillSetAsync(string userName)
	{
		var skillSet = new SkillSet(_skillsConfig.SkillNames);

		var result = await PerformRequest(GetUrl(userName));

		if (string.IsNullOrWhiteSpace(result))
		{
			return skillSet;
		}

		var skillsData = result.Split('\n');

		for (int i = 0; i < _skillsConfig.SkillNames.Count; i++)
		{
			var skillName = _skillsConfig.SkillNames[i];
			var skillData = skillsData[i].Split(',');

			int.TryParse(skillData[1], out var level);
			int.TryParse(skillData[2], out var experience);

			var skill = new Skill(skillName, level, experience);
			skillSet.Skills[skill.Name] = skill;
		}

		return skillSet;
	}

	public async Task<IEnumerable<SkillSet>> GetSkillSetsAsync(IEnumerable<string> userNames)
	{
		var skillTasks = userNames.Select(GetSkillSetAsync);

		var results = await Task.WhenAll(skillTasks);

		return results;
	}

	private static string GetUrl(string userName)
	{
		return $"https://secure.runescape.com/m=hiscore/index_lite.ws?player={userName}";
	}
}
