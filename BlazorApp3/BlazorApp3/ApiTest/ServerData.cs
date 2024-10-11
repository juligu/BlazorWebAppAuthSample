using BlazorApp3.Client.ApiTest;

namespace BlazorApp3.ApiTest
{
    public class ServerData : IClientData
    {
        public async Task<IEnumerable<TestData>> GetData()
        {
            return Enumerable.Range(1, 5).Select(index =>
                new TestData(id: index, name: $"Item{index}")
            ).ToArray();
        }
    }
}
