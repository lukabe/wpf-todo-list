namespace ToDoList.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class ProjectModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProjectModel()
        {
            Tasks = new HashSet<TaskModel>();
        }

        [Key]
        public int ProjectId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public bool IsDone { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskModel> Tasks { get; set; }
    }
}
