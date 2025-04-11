using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ToDoController : Controller
{
    private readonly AppDbContext _context;
    public ToDoController(AppDbContext context)
    {
        _context = context;
    }
    // /ToDo
    public async Task<IActionResult> Index()
    {
        var items = await _context.ToDoItems.ToListAsync();
        return View(items);
    }
    // /ToDo/Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ToDoItem item)
    {
        if (!ModelState.IsValid || item.Deadline == null)
        {
            ModelState.AddModelError("Deadline", "Необходимо выбрать корректную дату.");
            return View(item);
        }
        // Проверка
        string deadlineString = item.Deadline?.ToString("yyyy-MM-dd");
        if (string.IsNullOrEmpty(deadlineString) || deadlineString.Length != 10)
        {
            ModelState.AddModelError("Deadline", "Необходимо выбрать корректную дату.");
            return View(item);
        }
        _context.Add(item);
        await _context.SaveChangesAsync();

        TempData["Message"] = "Задача успешно добавлена!";
        TempData["MessageType"] = "success";
        return RedirectToAction(nameof(Index));
    }
    // /ToDo/Complete/5
    [HttpPost]
    public async Task<IActionResult> Complete(int id)
    {
        var item = await _context.ToDoItems.FindAsync(id);
        if (item == null) return NotFound();

        item.IsCompleted = true;
        _context.Update(item);
        await _context.SaveChangesAsync();

        TempData["Message"] = "Задача отмечена как выполненная!";
        TempData["MessageType"] = "success";
        return RedirectToAction(nameof(Index));
    }
    // /ToDo/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.ToDoItems.FindAsync(id);
        if (item == null) return NotFound();

        _context.ToDoItems.Remove(item);
        await _context.SaveChangesAsync();

        TempData["Message"] = "Задача успешно удалена!";
        TempData["MessageType"] = "danger";
        return RedirectToAction(nameof(Index));
    }
}