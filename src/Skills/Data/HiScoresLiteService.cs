using Microsoft.Extensions.Options;
using Skills.Models;
using System.Text.Json;

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
			// TODO: Log
		}

		return ConstructSkillSet(apiResult);
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
			var skillData = JsonSerializer.Deserialize<int[]>(result[i]);

			if (skillData == null)
			{
				skillData = new int[3];
				// TODO: Log
			}

			data.Add(skillData);
		}

		return data;
	}
}
