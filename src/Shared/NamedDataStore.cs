using JsonFlatFileDataStore;

namespace Shared;

public class NamedDataStore : INamedDataStore
{
	public string FileName { get; init; }
	public	IDataStore DataStore { get; init; }

	public NamedDataStore(string fileName)
	{
		FileName = fileName;
		DataStore = new DataStore(fileName);
	}
}