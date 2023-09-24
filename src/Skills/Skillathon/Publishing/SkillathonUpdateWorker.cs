using MassTransit.Initializers;
using Microsoft.Extensions.Hosting;
using Skillathon.Data;
using Skillathon.Models;

namespace Skillathon.Publishing;

internal class SkillathonUpdateWorker : BackgroundService
{
	private readonly ISkillathonPublisher _publisher;
	private readonly ISkillathonDataService _skillathonDataService;
	private readonly ISkillathonProcessor _skillathonProcessor;

	private const int UPDATE_INTERVAL_MILLIS = 3_600_000;

	public SkillathonUpdateWorker(
		ISkillathonPublisher publisher,
		ISkillathonDataService skillathonDataService,
		ISkillathonProcessor skillathonProcessor)
	{
		_publisher = publisher;
		_skillathonDataService = skillathonDataService;
		_skillathonProcessor = skillathonProcessor;
	}

	protected async override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			var skillathons = await _skillathonDataService.GetSkillathonsAsync();
			var activeSkillathons = skillathons.Where(skillathon => skillathon.State != SkillathonState.Ended);

			await _skillathonProcessor.ProcessAsync(activeSkillathons);

			await PublishAsync(activeSkillathons);
			await StoreAsync(activeSkillathons);

			await Task.Delay(UPDATE_INTERVAL_MILLIS);
		}
	}

	private async Task PublishAsync(IEnumerable<SkillathonEvent> skillathons)
	{
		var publishTasks = skillathons.Select(_publisher.PublishSkillathonAsync);
		await Task.WhenAll(publishTasks);
	}

	private async Task StoreAsync(IEnumerable<SkillathonEvent> skillathons)
	{
		var storeTasks = skillathons
				.Where(skillathon => skillathon.State != SkillathonState.Ended)
				.Select(_skillathonDataService.StoreSkillathonAsync);

		await Task.WhenAll(storeTasks);
	}
}
