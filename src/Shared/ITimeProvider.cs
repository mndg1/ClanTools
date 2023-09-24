namespace Shared;

public interface ITimeProvider
{
	DateTime UtcNow { get; }
}
