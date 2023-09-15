using Microsoft.Extensions.Hosting;
using Microsoft.TeamFoundation.Framework.Common;
using Skillathon.Data;

namespace Skillathon.Publishing;

internal class SkillathonUpdateWorker : BackgroundService
{
	private readonly ISkillathonDataService _skillathonDataService;
	private readonly ISkillathonProcessor _skillathonProcessor;
	private readonly ITimeProvider _timeProvider;

	public SkillathonUpdateWorker(
		ISkillathonDataService skillathonDataService,
		ISkillathonProcessor skillathonProcessor,
		ITimeProvider timeProvider)
	{
		_skillathonDataService = skillathonDataService;
		_skillathonProcessor = skillathonProcessor;
		_timeProvider = timeProvider;
	}

	protected async override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			var skillathons = await _skillathonDataService.GetSkillathonsAsync();

			skillathons = await _skillathonProcessor.ProcessAsync(skillathons);

			var storeTasks = skillathons
				.Where(skillathon => skillathon.State != Models.SkillathonState.Ended)
				.Select(_skillathonDataService.StoreSkillathonAsync);

			await Task.WhenAll(storeTasks);

			await Task.Delay(CalculateTimeUntilNextUpdate());
		}
	}

	private int CalculateTimeUntilNextUpdate()
	{
		return 0;
	}
}
