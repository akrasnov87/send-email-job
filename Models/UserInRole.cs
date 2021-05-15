using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Email.Models
{
    [Table("pd_userinroles", Schema = "core")]
    public class UserInRoles
    {
        [Key]
        public int id { get; set; }

        /// <summary>
        /// пользователь
        /// </summary>
        public int f_user { get; set; }

        /// <summary>
        /// роль
        /// </summary>
        public int f_role { get; set; }

        /// <summary>
        /// Признак удаления
        /// </summary>
        public bool sn_delete { get; set; }
    }
}
