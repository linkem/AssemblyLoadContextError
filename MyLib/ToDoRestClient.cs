using System.Net.Http;
using System.Threading.Tasks;

namespace MyLib
{
    public class ToDoRestClient
    {
        public static async Task<TodoModel> GetToDoAsync()
        {
            var client = new HttpClient();
            var response = await client.GetAsync("https://jsonplaceholder.typicode.com/todos/1");
            var strResponse = await response.Content.ReadAsStringAsync();

            var deserialized = System.Text.Json.JsonSerializer.Deserialize<TodoModel>(strResponse);
            return deserialized;
        }
    }
    public class TodoModel
    {
        public int UserId { get; set; }
    }
}