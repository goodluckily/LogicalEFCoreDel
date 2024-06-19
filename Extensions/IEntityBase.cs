using System.ComponentModel.DataAnnotations;

namespace LogicalEFCoreDel.Extensions
{
    public class IEntityBase : ISoftDeletable
    {
        [Key]
        public string Id { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
