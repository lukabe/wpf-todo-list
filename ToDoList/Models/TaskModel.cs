namespace ToDoList.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class TaskModel
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Note { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsDone { get; set; }

        public int? ProjectId { get; set; }

        public virtual ProjectModel Project { get; set; }
    }
}
