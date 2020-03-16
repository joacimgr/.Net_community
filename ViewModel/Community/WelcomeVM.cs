using System;

namespace DistroLabCommunity.ViewModel.Community {

    /// <summary>
    /// Welcome page view model class handling:
    ///     -Username to be displayed 
    ///     -Number of logins past 30 days
    ///     -Time and date of the last login
    ///     -Number of unread messages
    /// </summary>
    public class WelcomeVM {
        public string Username { get; set; }
        public int LoginsLast30Days { get; set; }
        public DateTime LastLogin { get; set; }
        public int UnreadMessages { get; set; }
    }
}
