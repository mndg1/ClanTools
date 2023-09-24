using JsonFlatFileDataStore;

namespace Shared;

public interface INamedDataStore
{
	string FileName { get; init; }

	IDataStore DataStore { get; init; }
}
