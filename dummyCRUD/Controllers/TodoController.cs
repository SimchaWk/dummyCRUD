using DummyCRUD.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace DummyCRUD.Controllers
{
    public class TodoController(IHttpClientFactory clientFactory) : Controller
    {
        public async Task<IActionResult> Index()
        {
            HttpClient httpClient = clientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync("https://dummyjson.com/todos");

            if (!response.IsSuccessStatusCode) { return RedirectToAction("Index"); }

            string content = await response.Content.ReadAsStringAsync();
            TodosModel? todos = JsonSerializer.Deserialize<TodosModel>(
                content,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
            );
            return View(todos.todos);
        }

        public async Task<IActionResult> Details(int id)
        {
            HttpClient httpClient = clientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync("https://dummyjson.com/todos");

            if (!response.IsSuccessStatusCode) { return RedirectToAction("Index"); }

            string content = await response.Content.ReadAsStringAsync();
            TodosModel? todos = JsonSerializer.Deserialize<TodosModel>(
                content,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
            );
            TodoModel? todo = todos.todos.FirstOrDefault(t => t.Id == id);

            if (todo == null) { return RedirectToAction("Index"); }

            return View(todo);
        }

        public async Task<IActionResult> Delete(int id)
        {
            HttpClient httpClient = clientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.DeleteAsync($"https://dummyjson.com/todos/{id}");

            if (!response.IsSuccessStatusCode) { return RedirectToAction("Index"); }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(TodoModel updatedTodo)
        {
            HttpClient httpClient = clientFactory.CreateClient();

            string jsonTodo = JsonSerializer.Serialize(updatedTodo);
            StringContent content = new StringContent(jsonTodo, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PutAsync($"https://dummyjson.com/todos/{updatedTodo.Id}", content);

            if (!response.IsSuccessStatusCode) { return RedirectToAction("Index"); }

            return RedirectToAction("Details", new { id = updatedTodo.Id });
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TodoModel newTodo)
        {
            HttpClient httpClient = clientFactory.CreateClient();

            string jsonTodo = JsonSerializer.Serialize(newTodo);
            StringContent content = new StringContent(jsonTodo, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync("https://dummyjson.com/todos", content);

            if (!response.IsSuccessStatusCode) { return RedirectToAction("Index"); }

            return RedirectToAction("Details", new { id = newTodo.Id });
        }
    }
}
