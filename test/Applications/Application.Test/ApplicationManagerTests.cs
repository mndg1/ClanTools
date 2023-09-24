using Microsoft.Extensions.Options;

namespace Application.Test;

public class ApplicationManagerTests
{
	private readonly IApplication _mockApplication;
	private readonly IEnumerable<IApplication> _stubApplications;

	private const string MOCK_APPLICATION_NAME = "Fake";

    public ApplicationManagerTests()
    {
        _mockApplication = Substitute.For<IApplication>();
		_mockApplication.Name.Returns(MOCK_APPLICATION_NAME);

		_stubApplications = new List<IApplication>()
		{
			_mockApplication
		};
	}

    [Fact]
	public async Task StartApplicationsAsync_ApplicationMarkedForStart_ReceivedStartCall()
	{
		// Arrange
		var stubConfiguration = CreateSingleAppOptions(true);
		var applicationManager = new ApplicationManager(_stubApplications, stubConfiguration);

		// Act
		await applicationManager.StartApplicationsAsync();

		// Assert
		await _mockApplication.Received().StartAsync();
	}

	[Fact]
	public async Task StartApplicationsAsync_ApplicationNotMarkedForStart_DidNotReceiveStartCall()
	{
		// Arrange
		var stubConfiguration = CreateSingleAppOptions(false);
		var applicationManager = new ApplicationManager(_stubApplications, stubConfiguration);

		// Act
		await applicationManager.StartApplicationsAsync();

		// Assert
		await _mockApplication.DidNotReceive().StartAsync();
	}

	private IOptions<ApplicationsConfiguration> CreateSingleAppOptions(bool shouldStart)
	{
		var applicationsConfig = new ApplicationsConfiguration()
		{
			StartupApplications =
			{
				{ MOCK_APPLICATION_NAME, shouldStart }
			}
		};

		var options = Options.Create(applicationsConfig);

		return options;
	}
}
