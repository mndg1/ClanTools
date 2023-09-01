using Microsoft.Extensions.Options;

namespace Application;

public class ApplicationManager : IApplicationManager
{
	private IEnumerable<IApplication> _applications;
	private ApplicationsConfiguration _applicationsConfig;

	public ApplicationManager(
		IEnumerable<IApplication> applications, 
		IOptions<ApplicationsConfiguration> applicationsConfig)
	{
		_applications = applications;
		_applicationsConfig = applicationsConfig.Value;
	}

	public async Task StartApplicationsAsync()
	{
		var startupNames = _applicationsConfig.StartupApplications
			.Where(option => option.Value)
			.Select(option => option.Key);

		var startupTasks = _applications
			.Where(application => startupNames.Contains(application.Name))
			.Select(application => application.StartAsync());

		await Task.WhenAll(startupTasks);
	}
}
