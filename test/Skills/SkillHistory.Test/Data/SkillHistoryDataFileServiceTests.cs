using JsonFlatFileDataStore;
using Microsoft.Extensions.Logging;
using NSubstitute.ExceptionExtensions;
using Skills.Models;

namespace SkillHistory.Test.Data;

public class SkillHistoryDataFileServiceTests
{
    private readonly IDataStore _dataStoreStub;
    private readonly SkillHistoryDataFileService _skillHistoryDataFileService;

    private const string NONE_EXISTENT_ID = "00000000-0000-0000-0000-000000000000";
    private const string EXISTING_ID = "11111111-1111-1111-1111-111111111111";

    public SkillHistoryDataFileServiceTests()
    {
        _dataStoreStub = Substitute.For<IDataStore>();
        _dataStoreStub.GetItem<HistoricSkillData>(NONE_EXISTENT_ID)
            .Throws(new KeyNotFoundException());
        _dataStoreStub.GetItem<HistoricSkillData>(EXISTING_ID)
            .Returns(new HistoricSkillData(new Dictionary<DateOnly, SkillSet>()));

        var logger = Substitute.For<ILogger<SkillHistoryDataFileService>>();

        _skillHistoryDataFileService = new(_dataStoreStub, logger);
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
