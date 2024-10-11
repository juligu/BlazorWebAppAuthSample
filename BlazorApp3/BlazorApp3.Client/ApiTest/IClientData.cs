namespace BlazorApp3.Client.ApiTest
{
    public interface IClientData
    {
        Task<IEnumerable<TestData>> GetData();
    }
}