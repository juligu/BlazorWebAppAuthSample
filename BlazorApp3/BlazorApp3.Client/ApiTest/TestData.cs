namespace BlazorApp3.Client.ApiTest;
public sealed class TestData(int id, string name)
{
    public int Id { get; set; } = id; 
    public string Name { get; set;} = name;
}
