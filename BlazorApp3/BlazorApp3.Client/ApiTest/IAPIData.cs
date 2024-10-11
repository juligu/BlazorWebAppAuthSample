namespace BlazorApp3.Client.ApiTest;

public interface IAPIData
{
    public Task<IEnumerable<TestData>> GetTestDataAsync();

    public Task<IEnumerable<TestData>> GetTestRemoteDataAsync();
}
