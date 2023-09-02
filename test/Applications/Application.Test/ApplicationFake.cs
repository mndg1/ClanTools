namespace Application.Test;

internal class ApplicationFake : IApplication
{
	public string Name => "Fake";
	public bool HasStarted {  get; set; }

	public Task StartAsync()
	{
		HasStarted = true;

		return Task.CompletedTask;
	}
}
