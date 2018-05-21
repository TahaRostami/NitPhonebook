using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nit.Phonebook.Models.Data
{
    [Table("ChangeTracingkInformation")]
    public partial class ChangeTracingkInformation
    {
        public int Id { get; set; }

        [Required]
        public string TableName { get; set; }

        [Required]
        public string Action { get; set; }


        public string LastTime { get; set; }

        public string LastId { get; set; }

    }
}
