using Microsoft.Extensions.Logging;
using SharedTestResources;
using SkillathonEvent.Data;
using SkillathonEvent.Models;
using SkillathonEvent.Modification;
using Skills;

namespace SkillathonEvent.Test.Modification;

public class SkillathonCreatorTests
{
	private readonly ISkillathonCreator _skillathonCreator;
	private readonly ISkillService _skillServiceStub;
	private readonly ISkillathonDataStore _skillathonDataStoreStub;

	public SkillathonCreatorTests()
	{
		_skillServiceStub = Substitute.For<ISkillService>();
		_skillathonDataStoreStub = Substitute.For<ISkillathonDataStore>();
		var loggerStub = Substitute.For<ILogger<SkillathonCreator>>();

		_skillathonCreator = new SkillathonCreator(_skillServiceStub, _skillathonDataStoreStub, loggerStub);
	}

	[Theory, ClanToolsAutoData]
	public async Task CreateSkillathonAsync_WithNewEventData_ShouldReturnSkillathon(
		string eventName,
		string skillName,
		DateOnly startDate,
		DateOnly endDate)
	{
		// Arrange
		_skillathonDataStoreStub.GetSkillathonsAsync().Returns(x => Task.FromResult(Enumerable.Empty<Skillathon>()));
		_skillServiceStub.GetSkillNames().Returns(new List<string>() { skillName });

		// Act
		var skillathon = await _skillathonCreator.CreateSkillathonAsync(eventName, skillName, startDate, endDate);

		// Assert
		skillathon.Should().NotBeNull();
	}

	[Theory, ClanToolsAutoData]
	public async Task CreateSkillathonAsync_WithExistingEventData_ShouldReturnExistingSkillathon(
		string eventName,
		string skillName,
		DateOnly startDate,
		DateOnly endDate)
	{
		// Arrange
		var existingSkillathon = (Skillathon?) new Skillathon(eventName, skillName);
		_skillathonDataStoreStub.GetSkillathonAsync(eventName).Returns(x => Task.FromResult(existingSkillathon));
		_skillServiceStub.GetSkillNames().Returns(new List<string>() { skillName });

		// Act
		var skillathon = await _skillathonCreator.CreateSkillathonAsync(eventName, skillName, startDate, endDate);

		// Assert
		skillathon.Should().BeSameAs(existingSkillathon);
	}

	[Theory, ClanToolsAutoData]
	public async Task CreateSkillathonAsync_WithInvalidSkillName_ShouldReturnNull(
		string eventName,
		string skillName,
		DateOnly startDate,
		DateOnly endDate)
	{
		// Arrange
		_skillathonDataStoreStub.GetSkillathonsAsync().Returns(x => Task.FromResult(Enumerable.Empty<Skillathon>()));
		_skillServiceStub.GetSkillNames().Returns(x => Enumerable.Empty<string>().ToList());

		// Act
		var skillathon = await _skillathonCreator.CreateSkillathonAsync(eventName, skillName, startDate, endDate);

		// Assert
		skillathon.Should().BeNull();
	}
}
