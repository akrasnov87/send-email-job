using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Email.Models
{
    [Table("cv_house_mapping", Schema = "dbo")]
    public class HouseMapping
    {
        [Key]
        public Guid id { get; set; }

        public int f_main_division { get; set; }

        public int f_subdivision { get; set; }

        public int f_division { get; set; }

        /// <summary>
        /// Округ
        /// </summary>
        public int n_gos_subdivision { get; set; }

        /// <summary>
        /// УИК
        /// </summary>
        public int n_uik { get; set; }
    }
}
