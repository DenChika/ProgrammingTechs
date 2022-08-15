using System.Net.Http.Json;
using System.Text.Json;
using Parser.ParserResults.Entities;

namespace Parser.ParserResults.ClientGens
{
    public class CatClient
    {
        private static readonly HttpClient _client = new HttpClient();
        public static async Task<LinkedList<Cat>> getAll()
        {
            var jsonContent = JsonContent.Create("");
            var response = await _client.GetAsync($"http://localhost:8080/getAll" + $"");
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LinkedList<Cat>>(responseContent);
        }

        public static async Task<Cat> createCat(string name, string date, string breed, string color)
        {
            var jsonContent = JsonContent.Create("");
            var response = await _client.PostAsync($"http://localhost:8080/createCat" + $"?name={name}&date={date}&breed={breed}&color={color}", jsonContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Cat>(responseContent);
        }

        public static async Task<Cat> findByName(string name)
        {
            var jsonContent = JsonContent.Create("");
            var response = await _client.GetAsync($"http://localhost:8080/findByName" + $"?name={name}");
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Cat>(responseContent);
        }

        public static async Task<LinkedList<Cat>> getFriends(Cat cat)
        {
            var jsonContent = JsonContent.Create(cat);
            var response = await _client.GetAsync($"http://localhost:8080/getFriends" + $"");
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LinkedList<Cat>>(responseContent);
        }

        public static async Task<LinkedList<Cat>> findByColor(string color)
        {
            var jsonContent = JsonContent.Create("");
            var response = await _client.GetAsync($"http://localhost:8080/findByColor" + $"?color={color}");
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LinkedList<Cat>>(responseContent);
        }

        public static async Task<LinkedList<Cat>> findByDate(string date)
        {
            var jsonContent = JsonContent.Create("");
            var response = await _client.GetAsync($"http://localhost:8080/findByDate" + $"?date={date}");
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LinkedList<Cat>>(responseContent);
        }

        public static async Task<LinkedList<Cat>> findByBreed(string breed)
        {
            var jsonContent = JsonContent.Create("");
            var response = await _client.GetAsync($"http://localhost:8080/findByBreed" + $"?breed={breed}");
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LinkedList<Cat>>(responseContent);
        }

        public static async Task<Cat> deleteCat(Cat cat)
        {
            var jsonContent = JsonContent.Create(cat);
            var response = await _client.DeleteAsync($"http://localhost:8080/deleteCat" + $"");
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Cat>(responseContent);
        }
    }
}