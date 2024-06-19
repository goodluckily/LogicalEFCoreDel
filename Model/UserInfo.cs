using System.ComponentModel.DataAnnotations;
using LogicalEFCoreDel.Extensions;

namespace LogicalEFCoreDel.Model
{
    public class UserInfo: IEntityBase
    {
        public string? Name {  get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Account { get; set; }
        public int? Age { get; set; }
    }
}
