using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhuang.Data.Annotations;

namespace Zhuang.Data.Test
{
    [Table("Sys_Product")]
    public partial class SysProduct
    {
        [Key]
        [AutoGenerate]
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string BarCode { get; set; }
        public string SearchCode { get; set; }
        public string Unit { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }

        public string RecordStatus { get; set; }
        public string CreatedById { get; set; }
        public string ModifiedById { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}
