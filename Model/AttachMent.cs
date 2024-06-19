using System.ComponentModel.DataAnnotations;
using LogicalEFCoreDel.Extensions;

namespace LogicalEFCoreDel.Model
{
    // 模拟数据库实体
    public class AttachMent: IEntityBase
    {
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public int? FileSize { get; set; }
        public string? FilePath { get; set; }
        public DateTime? UploadDate { get; set; }
    }
}
