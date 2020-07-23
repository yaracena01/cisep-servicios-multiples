using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cisep.Models
{
    [Table("Services")]
    public partial class Services
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the name")]

        public string Description { get; set; }
        public string Photo { get; set; }
        public string Url { get; set; }
        public string UrlName { get; set; }
        public int Type_Services { get; set; }
        public IList<services_detail> Services_Details { get; set; }

    }
}
