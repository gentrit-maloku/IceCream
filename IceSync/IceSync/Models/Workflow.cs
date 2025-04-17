namespace IceSync.Models
{
    public sealed class Workflow
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string MultiExecBehavior { get; set; }
    }
}
