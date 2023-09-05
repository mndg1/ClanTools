using Microsoft.Extensions.Options;
using Skills.Models;
using System.Text.Json;

namespace Skills.Data;

internal class HiScoresLiteService : RuneScapeApiService, ISkillDataRetriever
{
	public HiScoresLiteService(
		IHttpClientFactory httpClientFactory,
		IOptions<SkillsConfiguration> skillsConfig)
		: base(httpClientFactory, skillsConfig)
	{
	}

	public async Task<SkillSet> GetSkillSetAsync(string userName)
	{
		var url = GetUrl(userName);
		var apiResult = await PerformRequest<List<int[]>>(url);

		if (!apiResult.Successful)
		{
			apiResult = await Retry<List<int[]>>(url);
		}

		return ConstructSkillSet(apiResult.Result);
	}

	public async Task<IEnumerable<SkillSet>> GetSkillSetsAsync(IEnumerable<string> userNames)
	{
		var skillTasks = userNames.Select(GetSkillSetAsync);

		var results = await Task.WhenAll(skillTasks);

		return results;
	}

	private SkillSet ConstructSkillSet(IList<int[]> data)
	{
		var skillSet = new SkillSet(_skillsConfig.SkillNames);

		if (!IsValidData(data))
		{
			return skillSet;
		}

		for (int i = 0; i < _skillsConfig.SkillNames.Count; i++)
		{
			var skillName = _skillsConfig.SkillNames[i];
			var level = data[i][1];
			var experience = data[i][2];

			skillSet.Skills[skillName] = new Skill(skillName, level, experience);
		}

		return skillSet;
	}

	private static string GetUrl(string userName)
	{
		return $"https://secure.runescape.com/m=hiscore/index_lite.ws?player={userName}";
	}

	private bool IsValidData(IList<int[]> data)
	{
		return data.Count >= _skillsConfig.SkillNames.Count;
	}
}
