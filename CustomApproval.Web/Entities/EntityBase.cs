using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomApproval.Web.Entities
{
    public class EntityBase
    {
        public string Id { get; set; }

        public DateTime LastUpdatedTime { get; set; }
    }
}
