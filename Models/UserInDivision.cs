using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Email.Models
{
    [Table("pd_userindivisions", Schema = "core")]
    public class UserInDivision
    {
        [Key]
        public int id { get; set; }

        /// <summary>
        /// пользователь
        /// </summary>
        public int f_user { get; set; }

        /// <summary>
        /// Главное подразделение
        /// </summary>
        public int? f_division { get; set; }

        /// <summary>
        /// Подразделение
        /// </summary>
        public int? f_subdivision { get; set; }

        /// <summary>
        /// УИК
        /// </summary>
        public int? f_uik { get; set; }

        /// <summary>
        /// Округ
        /// </summary>
        public int? n_gos_subdivision { get; set; }

        /// <summary>
        /// Признак удаления
        /// </summary>
        public bool sn_delete { get; set; }
    }
}
