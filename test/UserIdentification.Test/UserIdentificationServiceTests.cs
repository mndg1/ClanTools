using Microsoft.Extensions.Logging;
using UserIdentification.Data;
using UserIdentification.Entities;

namespace UserIdentification.Test;

public class UserIdentificationServiceTests
{
	private readonly IUserIdenificationService _userIdenificationService;
    private readonly IUserIdentificationDataService _userIdentificationDataServiceStub;
    private readonly IGuidProvider _guidProviderStub;

    private readonly UserIdEntity _newUserIdEntity;
    private readonly UserIdEntity _existingUserIdEntity;

    private const string NEW_USER_NAME = "New User";
    private const string EXISTING_USER_NAME = "Existing User";
    private const string OLD_USER_NAME = "Old User";

    private const string NEW_USER_ID = "11111111-1111-1111-1111-111111111111";
    private const string EXISTING_USER_ID = "22222222-2222-2222-2222-222222222222";
    private const string NAME_CHANGE_ID = "33333333-3333-3333-3333-333333333333";

	public UserIdentificationServiceTests()
    {
        _userIdentificationDataServiceStub = Substitute.For<IUserIdentificationDataService>();
		_userIdentificationDataServiceStub.GetUserId(EXISTING_USER_ID).Returns(_existingUserIdEntity);

		_guidProviderStub = Substitute.For<IGuidProvider>();
        var loggerStub = Substitute.For <ILogger<UserIdentificationService>>();

        _userIdenificationService = new UserIdentificationService(_userIdentificationDataServiceStub, _guidProviderStub, loggerStub);

        _newUserIdEntity = new UserIdEntity(NEW_USER_NAME, NEW_USER_ID);
		_existingUserIdEntity = new UserIdEntity(EXISTING_USER_NAME, EXISTING_USER_ID);
	}

    [Fact]
    public async Task RegisterNewUser_WithNewUserName_CallsDataStore()
    {
        // Arrange
        _guidProviderStub.CreateGuid().Returns(Guid.Parse(NEW_USER_ID));

        // Act
        await _userIdenificationService.RegisterNewUser(NEW_USER_NAME);

        // Assert
        await _userIdentificationDataServiceStub.Received().StoreUser(_newUserIdEntity);
    }

    [Fact]
    public async Task RegisterNewUser_WithExistingUserName_DoesNotCallDataStore()
    {
		// Arrange
		_userIdentificationDataServiceStub.GetUserId(EXISTING_USER_ID).Returns(_existingUserIdEntity);

		// Act
		await _userIdenificationService.RegisterNewUser(EXISTING_USER_NAME);

        // Assert
        await _userIdentificationDataServiceStub.DidNotReceive().StoreUser(_existingUserIdEntity);
    }

    [Fact]
    public async Task GetUserId_WithExistingUserName_ReturnsNonNull()
    {
		// Arrange 
		_userIdentificationDataServiceStub.GetUserId(EXISTING_USER_NAME).Returns(_existingUserIdEntity);

		// Act
		var existingUserGuid = await _userIdenificationService.GetUserId(EXISTING_USER_NAME);

        // Assert
        existingUserGuid.Should().NotBeNull();
    }

    [Fact]
    public async Task GetUserId_NoneExistingUserName_ShouldReturnNull()
    {
        // Arrange
        const string NONE_EXISTING_USER_NAME = "None Existent User";

        // Act
        var resultGuid = await _userIdenificationService.GetUserId(NONE_EXISTING_USER_NAME);

        // Assert
        resultGuid.Should().BeNull();
    }

    [Fact]
    public async Task GetUserId_WithReturnedInvalidGuid_ReturnsNull()
    {
        // Arrange
        const string INVALID_GUID = "I am not a parsable guid";
        var invalidUserIdEntity = new UserIdEntity(EXISTING_USER_NAME, INVALID_GUID);
        _userIdentificationDataServiceStub.GetUserId(EXISTING_USER_NAME).Returns(invalidUserIdEntity);

        // Act
        var resultGuid = await _userIdenificationService.GetUserId(EXISTING_USER_NAME);

        // Assert
        resultGuid.Should().BeNull();
    }

    [Fact]
    public async Task UpdateUserName_WithValidData_ShouldCallDataStore()
    {
        // Arrange
        var changingUserIdEntity = new UserIdEntity(NEW_USER_NAME, NAME_CHANGE_ID);
        _userIdentificationDataServiceStub.GetUserId(OLD_USER_NAME).Returns(changingUserIdEntity);

        // Act
        await _userIdenificationService.UpdateUserName(OLD_USER_NAME, NEW_USER_NAME);

        // Assert
        await _userIdentificationDataServiceStub.Received().StoreUser(changingUserIdEntity);
    }

    [Theory]
    [InlineData("Non Existing Name", NEW_USER_NAME)]
    [InlineData(OLD_USER_NAME, EXISTING_USER_NAME)]
    public async Task UpdateUserName_WithNonCompatibleData_DoesNotCallDataStore(string oldName, string newName)
    {
		// Arrange
		var changingUserIdEntity = new UserIdEntity(OLD_USER_NAME, NAME_CHANGE_ID);
		_userIdentificationDataServiceStub.GetUserId(EXISTING_USER_NAME).Returns(_existingUserIdEntity);
		_userIdentificationDataServiceStub.GetUserId(OLD_USER_NAME).Returns(changingUserIdEntity);

		// Act
        await _userIdenificationService.UpdateUserName(oldName, newName);

		// Assert
        await _userIdentificationDataServiceStub.DidNotReceive().StoreUser(Arg.Any<UserIdEntity>());
	}
}
