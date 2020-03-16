using System;
using System.Collections.Generic;

namespace DistroLabCommunity.ViewModel.Community {

    /// <summary>
    /// Inbox page view model class handling:
    ///     -List of users that has sent a non-removed message to the user
    ///     -The total number of messages sent to the user (removed or not)
    ///     -The total number of removed messages earlier sent to the user
    ///     -The total number of messages the user has opened and read
    /// </summary>
    public class InboxVM {
        public List<InboxUserVM> Users { get; set; }
        public int TotalNumberOfMessages { get; set; }
        public int TotalNumberOfRemoved { get; set; }
        public int TotalNumberOfOpened { get; set; }
    }

    /// <summary>
    /// Inbox page view model class handling:
    ///     -List of messages sent by a certain user
    ///     -Name of the sender
    ///     -The total number of unread messages from this sender
    /// </summary>
    public class InboxUserVM {
        public List<InboxMessageVM> Messages { get; set; }
        public string Username { get; set; }
        public int TotalNumberOfUnread { get; set; }
    }

    /// <summary>
    /// Inbox page view model class handling:
    ///     -MessageID of a message sent to the user
    ///     -Sender name of this message
    ///     -The time and date the message got sent
    ///     -The title of the message
    ///     -Boolean value if the message has been opened
    ///     -Boolean value if the message has been removed
    /// </summary>
    public class InboxMessageVM {
        public int MessageID { get; set; }
        public string Sender { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Title { get; set; }
        public bool Opened { get; set; }
        public bool Removed { get; set; }
    }
}
