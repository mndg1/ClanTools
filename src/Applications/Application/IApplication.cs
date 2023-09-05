namespace Application;

public interface IApplication
{
	string Name { get; }

	Task StartAsync();
}
