using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistroLabCommunity.Models {

    /// <summary>
    /// Database class representing a Community message.
    /// </summary>
    public class Message {

        [Key]
        public int MessageID { get; set; }
        [Required]
        [MaxLength(55)]
        public string Title { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Text { get; set; }
        public bool Opened { get; set; }
        public bool Removed { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime TimeStamp { get; set; }

        public virtual User SenderUser { get; set; }
        public virtual User ReceiverUser { get; set; }
    }
}
