namespace MexxarTaskTracker.Domain
{
    public class BaseEntityDto
    {
        public long Id { get; set; }

        public DateTime? SysCreatedOn { get; set; }

        public string? SysCreatedBy { get; set; }

        public DateTime? SysUpdatedOn { get; set; }

        public string? SysUpdatedBy { get; set; }

        public DateTime? SysDeletedOn { get; set; }

        public string? SysDeletedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
