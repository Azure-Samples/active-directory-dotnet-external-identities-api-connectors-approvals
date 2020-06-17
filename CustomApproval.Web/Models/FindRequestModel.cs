using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomApproval.Web.Models
{
    public class FindRequestModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
