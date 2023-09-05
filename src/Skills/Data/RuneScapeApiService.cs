using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Skills.Models;

namespace Skills.Data;

internal abstract class RuneScapeApiService
{
	private readonly IHttpClientFactory _httpClientFactory;
	protected readonly SkillsConfiguration _skillsConfig;
	protected readonly ILogger<RuneScapeApiService> _logger;

	protected RuneScapeApiService(
		IHttpClientFactory httpClientFactory,
		IOptions<SkillsConfiguration> skillsConfig,
		ILogger<RuneScapeApiService> logger)
	{
		_httpClientFactory = httpClientFactory;
		_skillsConfig = skillsConfig.Value;
		_logger = logger;
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
		_logger.LogInformation("Retrying API call for {url}.", url);

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
				_logger.LogInformation("Retrying was successful for {url}.", url);
				break;
			}

			retryInterval *= _skillsConfig.ApiRetryExponent;
		}

		return apiResult;
	}
}
