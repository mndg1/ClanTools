using Application;

namespace ConsoleApplication;

internal class ConsoleApp : IApplication
{
	private readonly ICommandProcessor _commandProcessor;

	public string Name => "ConsoleApp";

	public ConsoleApp(ICommandProcessor commandProcessor)
	{
		_commandProcessor = commandProcessor;
	}

	public async Task StartAsync()
	{
		using var tokenSource = new CancellationTokenSource();
		await _commandProcessor.ProcessCommandsAsync(tokenSource.Token);
	}
}
