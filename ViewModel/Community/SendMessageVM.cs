using System.Collections.Generic;

namespace DistroLabCommunity.ViewModel.Community {

    /// <summary>
    /// SendMessage view model class handling:
    ///     -List of possible receivers
    /// </summary>
    public class SendMessageVM {
        public List<SendMessageUserVM> Users { get; set; }
    }

    /// <summary>
    /// SendMessage view model class handling:
    ///     -Name of receiver
    ///     -UserId of receiver
    /// </summary>
    public class SendMessageUserVM {
        public string Username { get; set; }
        public string UserID { get; set; }
    }
}
