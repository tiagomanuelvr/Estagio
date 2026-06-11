using MvcMovie.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MvcMovie_API.Exe;

public class Scheduler
{
    public void PostDataDefault()
    {
        PostData().Wait();
    }
    public static async Task PostData()
    {
        using (var client = new HttpClient())
        {
            DateTime dateParse;
            DateTime.TryParse("1989-2-12", out dateParse);
            var endpoint = new Uri("https://localhost:7240/api/moviesApi/loadMovie");

            var newPost = new Movie()
            {
                Title = "Scheduler Post Data",
                ReleaseDate = dateParse,
                Genre = "Comedy",
                Price = 17,
                Rating = "R",
            };
            var newPostJson = JsonConvert.SerializeObject(newPost);

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(JsonConvert.SerializeObject(newPost), Encoding.UTF8, "application/json");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Headers.Add("X-API-KEY", "QXBpS2V5TWlkZGxld2FyZQ==");

            //var formContent = new MultipartFormDataContent();
            //formContent.Add(new StringContent(newPostJson, Encoding.UTF8, MediaTypeNames.Multipart.FormData), "movie");
            //new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("movie", newPostJson) });
            var result = await client.SendAsync(request);

            //if (result.IsSuccessStatusCode)
            //{
            //    File.WriteAllText(@"C:\Users\tiago.ramalho\Documents\Test\test.json", result.Content.ReadAsStringAsync().Result) ;
            //}

            //await File.WriteAllTextAsync(@"C:\Users\tiago.ramalho\Documents\Test\test.json", newPostJson);
        }
    }
}
