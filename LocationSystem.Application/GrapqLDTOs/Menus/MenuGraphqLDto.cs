namespace LocationSystem.Application.GrapqLDTOs.Menus
{
    public class MenuGraphqLDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
        public int Level { get; set; }
        public bool IsBackEnd { get; set; }
        public Guid? ParentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
