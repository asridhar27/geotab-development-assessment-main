using System.Threading.Tasks;

public interface IJokeService
{
    Task<string[]> GetRandomJokes(string category, int numberOfJokes);
    Task<string[]> GetCategories();
}
