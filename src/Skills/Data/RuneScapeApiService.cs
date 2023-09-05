using Microsoft.Extensions.Options;
using Skills.Models;

namespace Skills.Data;

internal abstract class RuneScapeApiService
{
	private readonly IHttpClientFactory _httpClientFactory;
	protected readonly SkillsConfiguration _skillsConfig;

	protected RuneScapeApiService(IHttpClientFactory httpClientFactory, IOptions<SkillsConfiguration> skillsConfig)
	{
		_httpClientFactory = httpClientFactory;
		_skillsConfig = skillsConfig.Value;
	}

	internal async Task<ApiResult> PerformRequest(string url)
	{
		using var httpClient = _httpClientFactory.CreateClient();
		using var response = await httpClient.GetAsync(url);

		var result = await response.Content.ReadAsStringAsync();
		var apiResult = new ApiResult(response.IsSuccessStatusCode, result);

		return apiResult;
	}

	protected async Task<ApiResult> Retry(string url)
	{
		int retryCount = 0;
		var apiResult = new ApiResult(false, string.Empty);
		var retryInterval = _skillsConfig.ApiRetryInterval;

		while (retryCount < _skillsConfig.ApiRetryAmount)
		{
			retryCount++;

			await Task.Delay(TimeSpan.FromSeconds(retryInterval));
			apiResult = await PerformRequest(url);

			if (apiResult.Successful)
			{
				break;
			}

			retryInterval *= _skillsConfig.ApiRetryExponent;
		}

		return apiResult;
	}
}
