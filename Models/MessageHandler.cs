using DistroLabCommunity.Data;
using System.Collections.Generic;
using System.Linq;

namespace DistroLabCommunity.Models {

    /// <summary>
    /// Class for handling CommunityContext messages.
    /// Contains methods for adding, fetching, updating and deleting data
    /// in the messages table.
    /// </summary>
    public class MessageHandler : IMessageHandler {

        private CommunityContext _communityContext;

        public MessageHandler(CommunityContext communityContext) {
            _communityContext = communityContext;
        }

        /// <summary>
        /// Returns all messages from the database that has a receiver with a matching UserID
        /// </summary>
        /// <param name="userId">Receiver UserID</param>
        /// <returns>A list of messages</returns>
        public List<Message> GetMessagesByReceiverId(string userId) {
            var messages = _communityContext.Messages.Where(b => b.ReceiverUser.UserID.Equals(userId));
            List<Message> m = messages.Select(a => new Message {
                MessageID = a.MessageID,
                Title = a.Title,
                Text = a.Text,
                Opened = a.Opened,
                Removed = a.Removed,
                TimeStamp = a.TimeStamp,
                SenderUser = a.SenderUser,
                ReceiverUser = a.ReceiverUser
            }).ToList();
            return m;
        }

        /// <summary>
        /// Returns all messages from the database that has a sender with a matching UserID
        /// </summary>
        /// <param name="userId">Sender UserID</param>
        /// <returns>A list of messages</returns>
        public List<Message> GetMessagesBySenderId(string userId) {
            var messages = _communityContext.Messages.Where(b => b.SenderUser.UserID.Equals(userId));
            List<Message> m = messages.Select(a => new Message {
                MessageID = a.MessageID,
                Title = a.Title,
                Text = a.Text,
                Opened = a.Opened,
                Removed = a.Removed,
                TimeStamp = a.TimeStamp,
                SenderUser = a.SenderUser,
                ReceiverUser = a.ReceiverUser
            }).ToList();
            return m;
        }

        /// <summary>
        /// Adds a new message to the database
        /// </summary>
        /// <param name="title">Message title</param>
        /// <param name="text">Message body</param>
        /// <param name="senderUser">Sender of the message</param>
        /// <param name="receiverUser">Receiver of the message</param>
        /// <returns>True or false depending on if the operation was successful</returns>
        public bool AddMessage(string title, string text, User senderUser, User receiverUser) {
            try {
                Message newMessage = new Message();
                newMessage.Title = title;
                newMessage.Text = text;
                newMessage.SenderUser = senderUser;
                newMessage.ReceiverUser = receiverUser;
                if (!senderUser.Equals(receiverUser)) {
                    _communityContext.Attach(receiverUser);
                }
                _communityContext.Attach(senderUser);
                _communityContext.Add(newMessage);
                _communityContext.SaveChanges();
                return true;
            }
            catch {
                return false;
            }
        }

        /// <summary>
        /// Returns all messages from the database with a matching messageId
        /// </summary>
        /// <param name="messageId">Message ID</param>
        /// <returns>A list of messages</returns>
        public List<Message> GetMessagesByMessageId(int messageId) {
            var messages = _communityContext.Messages.Where(b => b.MessageID == messageId);
            List<Message> m = messages.Select(a => new Message {
                MessageID = a.MessageID,
                Title = a.Title,
                Text = a.Text,
                Opened = a.Opened,
                Removed = a.Removed,
                TimeStamp = a.TimeStamp,
                SenderUser = a.SenderUser,
                ReceiverUser = a.ReceiverUser
            }).ToList();
            return m;
        }

        /// <summary>
        /// Attempts to set the message with a matching messageId's opened status to true 
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>True or false depending on if the operation was successful</returns>
        public bool SetMessageAsOpened(int messageId) {
            var result = _communityContext.Messages.SingleOrDefault(m => m.MessageID == messageId);
            if (result != null) {
                try {
                    result.Opened = true;
                    _communityContext.SaveChanges();
                    return true;
                }
                catch {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Attemts to set the message with matching messageId's remove status to true
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>bool depending on if the operation was successful</returns>
        public bool removeMessageByMid(int messageId) {
            var result = _communityContext.Messages.SingleOrDefault(m => m.MessageID == messageId);
            if (result != null) {
                try {
                    result.Removed = true;
                    _communityContext.SaveChanges();
                    return true;
                }
                catch {
                    return false;
                }
            }
            return false;
        }
    }
}
