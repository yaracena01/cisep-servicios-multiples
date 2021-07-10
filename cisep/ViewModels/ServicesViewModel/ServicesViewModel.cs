using cisep.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cisep.ViewModels.ServicesViewModel
{
    public class ServicesViewModel
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [StringLength(2000, ErrorMessage = "Description cannot be longer than 40 characters.")]
        public string Description { get; set; }
        public string Photo { get; set; }
        public string Url { get; set; }
        public string UrlName { get; set; }
        public int Type_Services { get; set; }
        public IList<Services_Details> Services_Details { get; set; }
    }
}
