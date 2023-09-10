using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Skills.Models;

namespace Skills.Data;

internal class HiScoresLiteService : RuneScapeApiService, ISkillDataRetriever
{
	private const int LEVEL_INDEX = 1;
	private const int EXPERIENCE_INDEX = 2;

	public HiScoresLiteService(
		IHttpClientFactory httpClientFactory,
		IOptions<SkillsConfiguration> skillsConfig,
		ILogger<HiScoresLiteService> logger)
		: base(httpClientFactory, skillsConfig, logger)
	{
	}

	public async Task<SkillSet> GetSkillSetAsync(string userName)
	{
		_logger.LogInformation("Getting skill data for {userName}", userName);
		var url = GetUrl(userName);
		var apiResult = await PerformRequest(url);

		if (!apiResult.Successful)
		{
			_logger.LogError("Initial API call for {userName} failed.", userName);
			apiResult = await Retry(url);
		}

		return ConstructSkillSet(apiResult);
	}

	private SkillSet ConstructSkillSet(ApiResult apiResult)
	{
		var skillSet = new SkillSet(_skillsConfig.SkillNames);

		if (!apiResult.Successful)
		{
			_logger.LogWarning("Could not create skill set with results from an unsuccessful API call.");
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
