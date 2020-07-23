using cisep.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cisep.ViewModel
{
    public class ServicesViewModelCreate
    {
        
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string Photo { get; set; }
        public string Url { get; set; }
        public string UrlName { get; set; } 

        public int Type_Services { get; set; }
        public IList<services_detail> services_Details { get; set; }

    }
}
