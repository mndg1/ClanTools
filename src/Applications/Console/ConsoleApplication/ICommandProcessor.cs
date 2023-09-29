namespace ConsoleApplication;

internal interface ICommandProcessor
{
	Task ProcessCommandsAsync(CancellationToken cancellationToken);
}
