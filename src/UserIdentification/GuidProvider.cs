namespace UserIdentification;

internal class GuidProvider : IGuidProvider
{
	public Guid CreateGuid()
	{
		return Guid.NewGuid();
	}
}
