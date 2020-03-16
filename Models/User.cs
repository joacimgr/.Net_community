using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistroLabCommunity.Models {

    /// <summary>
    /// Database class representing a Community user.
    /// </summary>
    public class User {

        [Key]
        public string UserID { get; set; }
        [Required]
        [MaxLength(20)]
        public string Username { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Created { get; set; }
        public ICollection<UserLogin> AccountActivity { get; set; }

        //https://www.loginworks.com/blogs/use-foreign-key-inverse-property-entity-framework/

        [InverseProperty("SenderUser")]
        public virtual ICollection<Message> SentMessages { get; set; }

        [InverseProperty("ReceiverUser")]
        public virtual ICollection<Message> ReceivedMessages { get; set; }

        public override bool Equals(object obj){
            var other = obj as User;
            if (other == null)
                return false;
            if (UserID != other.UserID)
                return false;
            return true;
        }
    }
}