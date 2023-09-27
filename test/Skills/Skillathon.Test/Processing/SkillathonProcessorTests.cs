using NSubstitute;
using Shared;
using SharedTestResources;
using SkillathonEvent.Models;
using SkillathonEvent.Modification;
using SkillathonEvent.Processing;

namespace SkillathonEvent.Test.Processing;

public class SkillathonProcessorTests
{
	private readonly ISkillathonProcessor _skillathonProcessor;

	private readonly ITimeProvider _timeProviderStub;

	public SkillathonProcessorTests()
	{
		var skillathonUpdaterStub = Substitute.For<ISkillathonUpdater>();
		_timeProviderStub = Substitute.For<ITimeProvider>();

		_skillathonProcessor = new SkillathonProcessor(skillathonUpdaterStub, _timeProviderStub);
	}

	[Theory, ClanToolsAutoData]
	public async Task ProcessAsync_ShouldUpdateStartState(Skillathon skillathon)
	{
		// Arrange
		skillathon.State = SkillathonState.Waiting;
		var startDate = skillathon.StartDate!.Value;
		var currentDate = new DateTime(startDate.Year, startDate.Month, startDate.Day + 1, 0, 0, 0, DateTimeKind.Utc);
		_timeProviderStub.UtcNow.Returns(currentDate);
		var skillathonsToProcess = new List<Skillathon>() { skillathon };

		// Act
		await _skillathonProcessor.ProcessAsync(skillathonsToProcess);

		// Assert
		skillathon.State.Should().Be(SkillathonState.Active);
	}

	[Theory, ClanToolsAutoData]
	public async Task ProcessAsync_ShouldUpdateEndState(Skillathon skillathon)
	{
		// Arrange
		skillathon.State = SkillathonState.Active;
		var endDate = skillathon.EndDate!.Value;
		var currentDate = new DateTime(endDate.Year, endDate.Month, endDate.Day + 1, 0, 0, 0, DateTimeKind.Utc);
		_timeProviderStub.UtcNow.Returns(currentDate);
		var skillathonsToProcess = new List<Skillathon>() { skillathon };

		// Act
		await _skillathonProcessor.ProcessAsync(skillathonsToProcess);

		// Assert
		skillathon.State.Should().Be(SkillathonState.Ended);
	}
}
