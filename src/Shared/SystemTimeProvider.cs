namespace Shared;

internal class SystemTimeProvider : ITimeProvider
{
	public DateTime UtcNow => DateTime.UtcNow;
}
