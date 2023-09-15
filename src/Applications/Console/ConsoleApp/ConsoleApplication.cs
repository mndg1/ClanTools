using Application;
using Microsoft.Extensions.Logging;

namespace ConsoleApp
{
	public class ConsoleApplication : IApplication
	{
		private readonly ILogger<ConsoleApplication> _logger;

		public string Name => "ConsoleApp";

		public ConsoleApplication(ILogger<ConsoleApplication> logger)
		{
			_logger = logger;
		}

		public Task StartAsync()
		{
			_logger.LogInformation("Console App started");

			return Task.CompletedTask;
		}
	}
}