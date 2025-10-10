namespace Cracker_Shop.Models.CommonMasterModels
{
    public class UserRolePermission
    {
        public long ID { get; set; }
        public int? UserID { get; set; }
        public int RoleID { get; set; }
        public string? ModuleID { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public class ModuleDto
    {
        public long ModuleID { get; set; }
        public string Label { get; set; } = string.Empty;
        public string? Route { get; set; }
        public string? Icon { get; set; }
        public bool IsActive { get; set; }
        public long? ParentID { get; set; }  

        public List<ModuleDto> Children { get; set; } = new();
    }


}
