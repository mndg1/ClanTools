using JsonFlatFileDataStore;
using Shared;
using SharedTestResources;
using SkillathonEvent.Data;
using SkillathonEvent.Models;

namespace SkillathonEvent.Test.Data;

public class SkillathonFileDataStoreTests
{
	private readonly ISkillathonDataStore _skillathonDataStore;
	private readonly IDataStore _dataStoreStub;
	private readonly IDocumentCollection<Skillathon> _documentCollectionStub;

	public SkillathonFileDataStoreTests()
	{
		_dataStoreStub = Substitute.For<IDataStore>();
		var namedDataStoreStub = Substitute.For<INamedDataStore>();
		namedDataStoreStub.FileName.Returns(SkillathonFileDataStore.FILE_NAME);
		namedDataStoreStub.DataStore.Returns(_dataStoreStub);

		_skillathonDataStore = new SkillathonFileDataStore(new List<INamedDataStore> { namedDataStoreStub });

		_documentCollectionStub = Substitute.For<IDocumentCollection<Skillathon>>();
	}

	[Theory, ClanToolsAutoData]
	public async Task StoreSkillathonAsync_WithNewSkillathon_ShouldCallInsert(Skillathon skillathon)
	{
		// Arrange
		_documentCollectionStub.AsQueryable().Returns(Enumerable.Empty<Skillathon>().AsQueryable());
		_dataStoreStub.GetCollection<Skillathon>().Returns(_documentCollectionStub);

		// Act
		await _skillathonDataStore.StoreSkillathonAsync(skillathon);

		// Assert
		await _dataStoreStub.GetCollection<Skillathon>().Received().InsertOneAsync(skillathon);
	}

	[Theory, ClanToolsAutoData]
	public async Task GetSkillathonAsync_WithExistingName_ReturnsStoredSkillathon(Skillathon skillathon)
	{
		// Arrange
		var existingList = new List<Skillathon>() { skillathon };
		_documentCollectionStub.AsQueryable().Returns(existingList.AsQueryable());
		_dataStoreStub.GetCollection<Skillathon>().Returns(_documentCollectionStub);

		// Act
		var retrievedSkillathon = await _skillathonDataStore.GetSkillathonAsync(skillathon.EventName);

		// Assert
		retrievedSkillathon.Should().BeSameAs(skillathon);
	}

	[Fact]
	public async Task GetSkillathonAsync_WithNoExistingName_ReturnsNoSkillathons()
	{
		// Arrange
		const string NONE_PRESENT_NAME = "NotStored";
		_documentCollectionStub.AsQueryable().Returns(Enumerable.Empty<Skillathon>().AsQueryable());
		_dataStoreStub.GetCollection<Skillathon>().Returns(_documentCollectionStub);

		// Act
		var retrievedSkillathon = await _skillathonDataStore.GetSkillathonAsync(NONE_PRESENT_NAME);

		// Assert
		retrievedSkillathon.Should().BeNull();
	}

	[Theory, ClanToolsAutoData]
	public async Task GetSkillathonsAsync_ReturnsAllSkillathons(IEnumerable<Skillathon> skillathons)
	{
		// Arrange
		_documentCollectionStub.AsQueryable().Returns(skillathons.AsQueryable());
		_dataStoreStub.GetCollection<Skillathon>().Returns(_documentCollectionStub);

		// Act
		var retrievedSkillathons = await _skillathonDataStore.GetSkillathonsAsync();

		// Assert
		retrievedSkillathons.Should().BeEquivalentTo(skillathons);
	}
}
