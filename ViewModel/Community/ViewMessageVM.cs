namespace DistroLabCommunity.ViewModel.Community {

    /// <summary>
    /// ViewMessage page view model class handling:
    ///     -MessageID to be used for removal of message
    ///     -Title of message
    ///     -Sender of message
    ///     -Text body of message
    /// </summary>
    public class ViewMessageVM {
        public int MessageID { get; set; }
        public string Title { get; set; }
        public string Sender { get; set; }
        public string Text { get; set; }
    }
}
