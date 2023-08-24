using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MexxarTaskTracker.Domain
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }

        [StringLength(128)]
        public DateTime? SysCreatedOn { get; set; }

        [StringLength(128)]
        public string? SysCreatedBy { get; set; }

        [StringLength(128)]
        public DateTime? SysUpdatedOn { get; set; }

        [StringLength(128)]
        public string? SysUpdatedBy { get; set; }

        [StringLength(128)]
        public DateTime? SysDeletedOn { get; set; }

        [StringLength(128)]
        public string? SysDeletedBy { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
