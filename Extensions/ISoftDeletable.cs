namespace LogicalEFCoreDel.Extensions
{
    public interface ISoftDeletable
    {
        bool? IsDeleted { get; set; }
    }
}
