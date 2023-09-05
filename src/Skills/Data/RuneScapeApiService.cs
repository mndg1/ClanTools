using Microsoft.Extensions.Options;
using Skills.Models;
using System.Text.Json;

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

	internal async Task<ApiResult<T>> PerformRequest<T>(string url)
	{
		using var httpClient = _httpClientFactory.CreateClient();
		using var response = await httpClient.GetAsync(url);

		var result = await JsonSerializer.DeserializeAsync<T>(response.Content.ReadAsStream());
		var apiResult = new ApiResult<T>(response.IsSuccessStatusCode, result!);

		return apiResult;
	}

	protected async Task<ApiResult<T>> Retry<T>(string url)
	{
		int retryCount = 0;
		var apiResult = new ApiResult<T>(false, default!);

		while (retryCount < _skillsConfig.ApiRetryAmount)
		{
			retryCount++;

			apiResult = await PerformRequest<T>(url);

			if (apiResult.Successful)
			{
				break;
			}
		}

		return apiResult;
	}
}
