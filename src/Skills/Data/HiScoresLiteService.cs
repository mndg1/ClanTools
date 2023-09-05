using Microsoft.Extensions.Options;
using Skills.Models;

namespace Skills.Data;

internal class HiScoresLiteService : RuneScapeApiService, ISkillDataRetriever
{
	private const int LEVEL_INDEX = 1;
	private const int EXPERIENCE_INDEX = 2;

	public HiScoresLiteService(
		IHttpClientFactory httpClientFactory,
		IOptions<SkillsConfiguration> skillsConfig)
		: base(httpClientFactory, skillsConfig)
	{
	}

	public async Task<SkillSet> GetSkillSetAsync(string userName)
	{
		var url = GetUrl(userName);
		var apiResult = await PerformRequest(url);

		if (!apiResult.Successful)
		{
			apiResult = await Retry(url);
		}

		return ConstructSkillSet(apiResult);
	}

	public async Task<IEnumerable<SkillSet>> GetSkillSetsAsync(IEnumerable<string> userNames)
	{
		var skillTasks = userNames.Select(GetSkillSetAsync);

		var results = await Task.WhenAll(skillTasks);

		return results;
	}

	private SkillSet ConstructSkillSet(ApiResult apiResult)
	{
		var skillSet = new SkillSet(_skillsConfig.SkillNames);

		if (!apiResult.Successful)
		{
			return skillSet;
		}

		var data = DisectApiResult(apiResult);

		for (int i = 0; i < _skillsConfig.SkillNames.Count; i++)
		{
			var skillName = _skillsConfig.SkillNames[i];
			var level = data[i][LEVEL_INDEX];
			var experience = data[i][EXPERIENCE_INDEX];

			skillSet.Skills[skillName] = new Skill(skillName, level, experience);
		}

		return skillSet;
	}

	private static string GetUrl(string userName)
	{
		return $"https://secure.runescape.com/m=hiscore/index_lite.ws?player={userName}";
	}

	private IList<int[]> DisectApiResult(ApiResult apiResult)
	{
		var data = new List<int[]>();

		if (!apiResult.Successful)
		{
			return data;
		}

		var skillsAmount = _skillsConfig.SkillNames.Count;

		var result = apiResult.Result.Split('\n');

		for (int i = 0; i < skillsAmount; i++)
		{
			var skillData = result[i].Split(",");

			int.TryParse(skillData[LEVEL_INDEX], out var level);
			int.TryParse(skillData[EXPERIENCE_INDEX], out var experience);

			data.Add(new int[] { 0, level, experience });
		}

		return data;
	}
}
