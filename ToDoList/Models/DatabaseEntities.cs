namespace ToDoList.Models
{
    using System.Data.Entity;

    public partial class DatabaseEntities : DbContext
    {
        public DatabaseEntities()
            : base("name=DatabaseConnection")
        {
        }

        public virtual DbSet<ProjectModel> Projects { get; set; }
        public virtual DbSet<TaskModel> Tasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
