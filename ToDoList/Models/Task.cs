namespace ToDoList.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Task
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TaskId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Note { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsDone { get; set; }

        public int? ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }
}
