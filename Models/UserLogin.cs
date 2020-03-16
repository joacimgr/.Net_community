using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistroLabCommunity.Models {

    /// <summary>
    /// Database class handling the date of user logins.
    /// </summary>
    public class UserLogin {

        [Key, Column(Order = 0)]
        public DateTime Login { get; set; }
        [Key, Column(Order = 1)]
        [Required]
        public User User { get; set; }
    }
}
