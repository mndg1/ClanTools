using Microsoft.Extensions.Logging;
using Skills.Models;

namespace SkillHistory.Test;

public class SkillHistoryServiceTests
{
	private readonly SkillHistoryService _skillHistoryService;
	private readonly Guid _userId = Guid.Parse(USER_ID);
	private readonly HistoricSkillData _historicSkillData;

	private const string USER_ID = "11111111-1111-1111-1111-111111111111";
	private const int DAY_LOWER_THAN_START = 1;
	private const int DAY_HIGHER_THAN_HIGHEST_ENTRY = 6;

	public SkillHistoryServiceTests()
	{
		_historicSkillData = CreateFakeHistoricData();

		var skillHistoryDataServiceStub = Substitute.For<ISkillHistoryDataService>();
		skillHistoryDataServiceStub.GetHistoricSkillDataAsync(_userId)
			.Returns(Task.FromResult(_historicSkillData));

		var loggerStub = Substitute.For<ILogger<SkillHistoryService>>();

		_skillHistoryService = new(skillHistoryDataServiceStub, loggerStub);
	}

	[Fact]
	public async Task GetSkillHistoryAsync_ReturnsSkillHistory()
	{
		// Act
		var historicData = await _skillHistoryService.GetSkillHistoryAsync(_userId);

		// Assert
		historicData.Should().NotBeNull();
	}

	[Fact]
	public async Task GetSkillHistorySinceAsync_ReturnsSkillHistoryWithCorrectDates()
	{
		// Arrange 
		var sinceDate = new DateTime(3, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Act
		var historicDataSinceDate = await _skillHistoryService.GetSkillHistorySinceAsync(_userId, sinceDate);

		// Assert
		var expectedDates = new DateOnly[]
		{
			new DateOnly(3, 1, 1),
			new DateOnly(4, 1, 1),
			new DateOnly(5, 1, 1),
		};

		historicDataSinceDate.SkillHistory.Keys.Should().BeEquivalentTo(expectedDates);
	}

	[Fact]
	public async Task GetSkillHistoryPeriodAsync_ReturnsSkillHistoryWithCorrectDates()
	{
		// Arrange 
		var periodStart = new DateTime(3, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		var periodEnd = new DateTime(5, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Act
		var historicDataPeriod = await _skillHistoryService.GetSkillHistoryPeriodAsync(_userId, periodStart, periodEnd);

		// Assert
		var expectedDates = new DateOnly[]
		{
			new DateOnly(3, 1, 1),
			new DateOnly(4, 1, 1),
			new DateOnly(5, 1, 1),
		};

		historicDataPeriod.SkillHistory.Keys.Should().BeEquivalentTo(expectedDates);
	}

	[Theory]
	[InlineData(DAY_LOWER_THAN_START)]
	[InlineData(DAY_HIGHER_THAN_HIGHEST_ENTRY)]
	public async Task GetSkillHistoryPeriodAsync_WithInvaldDates_ReturnsZeroDataPoints(int endDay)
	{
		// Arrange
		var periodStart = new DateTime(3, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		var periodEnd = new DateTime(1, 1, endDay, 0, 0, 0, DateTimeKind.Utc);

		// Act
		var historicDataInPeriod = await _skillHistoryService.GetSkillHistoryPeriodAsync(_userId, periodStart, periodEnd);

		// Assert
		historicDataInPeriod.SkillHistory.Count.Should().Be(0);
	}

	private HistoricSkillData CreateFakeHistoricData()
	{
		var dataMapping = new Dictionary<DateOnly, SkillSet>()
		{
			{ new DateOnly(1, 1, 1), null! },
			{ new DateOnly(2, 1, 1), null! },
			{ new DateOnly(3, 1, 1), null! },
			{ new DateOnly(4, 1, 1), null! },
			{ new DateOnly(5, 1, 1), null! },
		};

		return new HistoricSkillData(dataMapping);
	}
}
