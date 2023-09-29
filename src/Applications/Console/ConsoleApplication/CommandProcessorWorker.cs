using ConsoleApplication.Skills.SkillathonEvent;
using Microsoft.Extensions.Hosting;

namespace ConsoleApplication;

public class CommandProcessorWorker : BackgroundService
{
	private readonly ISkillathonUpdater _updater;

	public CommandProcessorWorker(ISkillathonUpdater updater)
	{
		_updater = updater;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			var input = Console.ReadLine();

			if (input == null)
			{
				continue;
			}

			var split = input.Split(' ');

			await Delegate(split[0], split.Skip(1).ToList());

			await Task.Delay(1000);
		}
	}

	private async Task Delegate(string commandName, IList<string> args)
	{
		var task = commandName.ToLower() switch
		{
			"updateskillathon" => _updater.Update(args[0]),
			_ => throw new Exception()
		};

		await task;
	}
}
