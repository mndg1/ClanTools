namespace Application;

public class ApplicationsConfiguration
{
	public const string SECTION_NAME = "Applications";

	public IDictionary<string, bool> StartupApplications { get; set; } = new Dictionary<string, bool>();
}
