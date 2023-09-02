using FluentAssertions;
using Microsoft.Extensions.Options;

namespace Application.Test;

public class ApplicationManagerTests
{

    [Theory]
	[InlineData(true)]
	[InlineData(false)]
	public async Task StartApplicationsAsync_ApplicationShouldStartBasedOnSetting(bool shouldStart)
	{
		// Arrange
		var mockApplication = new ApplicationFake();
		var stubApplications = new List<IApplication>()
		{
			mockApplication
		};
		var stubConfiguration = CreateSingleAppOptions(shouldStart);
		var applicationManager = new ApplicationManager(stubApplications, stubConfiguration);

		// Act
		await applicationManager.StartApplicationsAsync();

		// Assert
		mockApplication.HasStarted.Should().Be(shouldStart);
	}

	private IOptions<ApplicationsConfiguration> CreateSingleAppOptions(bool shouldStart)
	{
		var applicationsConfig = new ApplicationsConfiguration()
		{
			StartupApplications =
			{
				{ "Fake", shouldStart }
			}
		};

		var options = Options.Create(applicationsConfig);

		return options;
	}
}
