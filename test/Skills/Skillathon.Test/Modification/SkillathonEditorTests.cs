using AutoFixture.Xunit2;
using SharedTestResources;
using SkillathonEvent.Data;
using SkillathonEvent.Models;
using SkillathonEvent.Modification;

namespace SkillathonEvent.Test.Modification;

public class SkillathonEditorTests
{
	private readonly ISkillathonEditor _skillathonEditor;
	private readonly ISkillathonDataStore _skillathonDataStore;

    public SkillathonEditorTests()
    {
        _skillathonDataStore = Substitute.For<ISkillathonDataStore>();

		_skillathonEditor = new SkillathonEditor(_skillathonDataStore);
    }

    [Theory, ClanToolsAutoData]
	internal async Task SetStartDateAsync_ShouldUpdateStartDate(
		string eventName,
		DateOnly date)
	{
		// Arrange
		var skillathon = new Skillathon()
		{
			EventName = eventName,
		};

		_skillathonDataStore.GetSkillathonAsync(eventName).Returns(skillathon);

		// Act
		await _skillathonEditor.SetStartDateAsync(eventName, date);

		// Assert
		skillathon.StartDate.Should().Be(date);
	}

	[Theory, ClanToolsAutoData]
	internal async Task SetEndDateAsync_ShouldUpdateStartDate(
		string eventName,
		DateOnly date)
	{
		// Arrange
		var skillathon = new Skillathon()
		{
			EventName = eventName,
		};

		_skillathonDataStore.GetSkillathonAsync(eventName).Returns(skillathon);

		// Act
		await _skillathonEditor.SetEndDateAsync(eventName, date);

		// Assert
		skillathon.EndDate.Should().Be(date);
	}
}
