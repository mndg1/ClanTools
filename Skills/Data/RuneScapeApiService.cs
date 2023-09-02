namespace Skills.Data;

public abstract class RuneScapeApiService
{
	private readonly IHttpClientFactory _httpClientFactory;

	protected RuneScapeApiService(IHttpClientFactory httpClientFactory)
	{
		_httpClientFactory = httpClientFactory;
	}

	internal async Task<string> PerformRequest(string url)
	{
		using var httpClient = _httpClientFactory.CreateClient();
		using var response = await httpClient.GetAsync(url);

		if (!response.IsSuccessStatusCode)
		{
			// TODO: Mark for retry?
		}

		return await response.Content.ReadAsStringAsync();
	}
}
