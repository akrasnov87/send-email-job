using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Email.Models
{
    [Table("pd_roles", Schema = "core")]
    public class Role
    {
        [Key]
        public int id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string c_name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string c_description { get; set; }

        /// <summary>
        /// Вес
        /// </summary>
        public int n_weight { get; set; }

        /// <summary>
        /// Признак удаления
        /// </summary>
        public bool sn_delete { get; set; }
    }
}
