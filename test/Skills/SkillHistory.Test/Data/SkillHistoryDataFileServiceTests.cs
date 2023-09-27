using JsonFlatFileDataStore;
using Microsoft.Extensions.Logging;
using NSubstitute.ExceptionExtensions;
using Shared;
using Skills.Models;

namespace SkillHistory.Test.Data;

public class SkillHistoryDataFileServiceTests
{
	private readonly INamedDataStore _namedDataStoreStub;
	private readonly SkillHistoryDataFileService _skillHistoryDataFileService;

	private const string NONE_EXISTENT_ID = "00000000-0000-0000-0000-000000000000";
	private const string EXISTING_ID = "11111111-1111-1111-1111-111111111111";

	public SkillHistoryDataFileServiceTests()
	{
		_namedDataStoreStub = Substitute.For<INamedDataStore>();
		var dataStoreStub = Substitute.For<IDataStore>();
		dataStoreStub.GetItem<HistoricSkillData>(NONE_EXISTENT_ID)
			.Throws(new KeyNotFoundException());
		dataStoreStub.GetItem<HistoricSkillData>(EXISTING_ID)
			.Returns(new HistoricSkillData(new Dictionary<DateOnly, SkillSet>()));
		_namedDataStoreStub.DataStore.Returns(dataStoreStub);
		_namedDataStoreStub.FileName.Returns(SkillHistoryDataFileService.FILE_NAME);

		var logger = Substitute.For<ILogger<SkillHistoryDataFileService>>();

		_skillHistoryDataFileService = new(new List<INamedDataStore> { _namedDataStoreStub }, logger);
	}

	[Theory]
	[InlineData(EXISTING_ID)]
	[InlineData(NONE_EXISTENT_ID)]
	public async Task GetHistoricSkillDataAsync_Always_ReturnsSkillHistory(string idString)
	{
		// Arrange
		Guid id = Guid.Parse(idString);

		// Act
		var skillHistory = await _skillHistoryDataFileService.GetHistoricSkillDataAsync(id);

		// Assert
		skillHistory.Should().NotBeNull();
	}
}
