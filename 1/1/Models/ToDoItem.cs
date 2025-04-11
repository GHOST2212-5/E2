using System;
using System.ComponentModel.DataAnnotations;

public class ToDoItem
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Название задачи обязательно")]
    [StringLength(200, ErrorMessage = "Название задачи не должно превышать 200 символов")]
    public required string Title { get; set; }

    public bool IsCompleted { get; set; }

    [Required(ErrorMessage = "Необходимо выбрать корректную дату.")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? Deadline { get; set; }
}