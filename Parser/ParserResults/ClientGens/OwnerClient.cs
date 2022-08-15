using System.Net.Http.Json;
using System.Text.Json;
using Parser.ParserResults.Entities;

namespace Parser.ParserResults.ClientGens
{
    public class OwnerClient
    {
        private static readonly HttpClient _client = new HttpClient();
        public static async Task<LinkedList<Owner>> getAll()
        {
            var jsonContent = JsonContent.Create("");
            var response = await _client.GetAsync($"http://localhost:8080/getAll" + $"");
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LinkedList<Owner>>(responseContent);
        }

        public static async Task<Owner> createOwner(string name, string date)
        {
            var jsonContent = JsonContent.Create("");
            var response = await _client.PostAsync($"http://localhost:8080/createOwner" + $"?name={name}&date={date}", jsonContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Owner>(responseContent);
        }

        public static async Task<Owner> findByName(string name)
        {
            var jsonContent = JsonContent.Create("");
            var response = await _client.GetAsync($"http://localhost:8080/findByName" + $"?name={name}");
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Owner>(responseContent);
        }

        public static async Task<LinkedList<Owner>> findByDate(string date)
        {
            var jsonContent = JsonContent.Create("");
            var response = await _client.GetAsync($"http://localhost:8080/findByDate" + $"?date={date}");
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LinkedList<Owner>>(responseContent);
        }

        public static async Task<Owner> deleteOwner(Owner owner)
        {
            var jsonContent = JsonContent.Create(owner);
            var response = await _client.DeleteAsync($"http://localhost:8080/deleteCat" + $"");
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Owner>(responseContent);
        }
    }
}