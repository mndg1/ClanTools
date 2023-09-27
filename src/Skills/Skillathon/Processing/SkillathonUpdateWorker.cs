using MassTransit.Initializers;
using Microsoft.Extensions.Hosting;
using SkillathonEvent.Data;
using SkillathonEvent.Models;

namespace SkillathonEvent.Processing;

internal class SkillathonUpdateWorker : BackgroundService
{
    private readonly ISkillathonDataStore _skillathonDataService;
    private readonly ISkillathonProcessor _skillathonProcessor;

    private const int UPDATE_INTERVAL_MILLIS = 3_600_000;

    public SkillathonUpdateWorker(
        ISkillathonDataStore skillathonDataService,
        ISkillathonProcessor skillathonProcessor)
    {
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

            await Task.Delay(UPDATE_INTERVAL_MILLIS);
        }
    }
}
