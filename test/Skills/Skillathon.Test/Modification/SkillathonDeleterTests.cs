using AutoFixture.Xunit2;
using SharedTestResources;
using SkillathonEvent.Data;
using SkillathonEvent.Modification;

namespace SkillathonEvent.Test.Modification;

public class SkillathonDeleterTests
{
	[Theory, ClanToolsAutoData]
	internal async Task DeleteSkillathonAsync_ShouldCallDeleteFromDatastore(
		string eventName,
		[Frozen] ISkillathonDataStore skillathonDataStoreMock)
	{
		// Arrange
		var skillathonDeleter = new SkillathonDeleter(skillathonDataStoreMock);

		// Act
		await skillathonDeleter.DeleteSkillathonAsync(eventName);

		// Assert
		await skillathonDataStoreMock.Received().DeleteSkillathonAsync(Arg.Any<string>());
	}
}
