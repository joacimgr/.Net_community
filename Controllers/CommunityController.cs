using System;
using System.Collections.Generic;
using DistroLabCommunity.Models;
using DistroLabCommunity.ViewModel.Community;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DistroLabCommunity.Controllers {

    /// <summary>
    /// Controller for community related actions
    /// </summary>
    [Authorize]
    public class CommunityController : Controller {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserHandler _userHandler;
        private readonly IMessageHandler _messageHandler;

        public CommunityController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUserHandler userHandler, IMessageHandler messageHandler) {
            _userManager = userManager;
            _signInManager = signInManager;
            _userHandler = userHandler;
            _messageHandler = messageHandler;
        }

        /// <summary>
        /// Welcome page for logged in users
        /// Displays:
        ///     -Username
        ///     -Last login time
        ///     -Number of logins the last 30 days
        ///     -Number of unread messages
        /// </summary>
        public IActionResult Welcome() {
            string userId = _userManager.GetUserId(HttpContext.User);
            List<UserLogin> userLogins = _userHandler.GetUserLoginsById(userId);
            List<User> user = _userHandler.GetUserByIdList(userId);
            List<Message> messages = _messageHandler.GetMessagesByReceiverId(userId);
            int unreadMessages = 0;
            foreach (Message m in messages) {
                if (!m.Opened) {
                    unreadMessages++;
                }
            }
            var welcomeViewModel = new WelcomeVM {
                Username = user[0].Username,
                LoginsLast30Days = GetNumberOfLoginsLast30Days(userLogins),
                LastLogin = userLogins[userLogins.Count - 1].Login,
                UnreadMessages = unreadMessages // TODO: Make sure this works
            };
            return View(welcomeViewModel);
        }

        /// <summary>
        /// This action returns a count of how many logins a user has made within the last
        /// 30 days.
        /// </summary>
        /// <param name="Logins"></param>
        /// <returns>int</returns>
        private int GetNumberOfLoginsLast30Days(List<UserLogin> Logins) {
            int n = 0;
            foreach (UserLogin userLogin in Logins) {
                if (userLogin.Login.AddDays(30) >= DateTime.Now) {
                    n++;
                }
            }
            return n;
        }

        /// <summary>
        /// This action fetches all users currently in the Db and returns 
        /// all but the user invoking this action
        /// </summary>
        /// <returns></returns>
        public IActionResult SendMessage() {
            List<User> allUsers = _userHandler.GetAllUsersList();
            List<SendMessageUserVM> allUserVMs = new List<SendMessageUserVM>();
            foreach (User user in allUsers) {
                if (user.UserID != _userManager.GetUserId(HttpContext.User)) {
                    allUserVMs.Add(new SendMessageUserVM {
                        UserID = user.UserID,
                        Username = user.Username
                    });
                }
            }
            SendMessageVM sendMessage = new SendMessageVM {
                Users = allUserVMs
            };
            return View(sendMessage);
        }

        /// <summary>
        /// This action try to create a message with its given parameters.
        /// if receiver exists and if entered title and text has a valid format, the action
        /// try to add it to Db. To get the message Id, the message has to be created and then 
        /// fetched from db. Information about the message is then shown to the sender.
        /// Error messages is created and shown to the user
        /// if some operation neccesary to send the message failed.
        /// </summary>
        /// <param name="receiverName"></param>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendMessage(string receiverName, string title, string text) {
            if (_userHandler.UsernameExists(receiverName)) {
                List<User> resultReceiver = _userHandler.GetUserByUsernameList(receiverName);
                List<User> resultSender = _userHandler.GetUserByIdList(_userManager.GetUserId(HttpContext.User));
                if (resultReceiver.Count == 1 && resultSender.Count == 1) {
                    User receiver = resultReceiver[0];
                    User sender = resultSender[0];
                    if (_messageHandler.AddMessage(title, text, sender, receiver)) {
                        List<Message> messages = _messageHandler.GetMessagesBySenderId(sender.UserID);
                        
                        if (messages.Count > 0) {
                            Message m = messages[0];
                            foreach (Message message in messages) {
                                if(message.TimeStamp > m.TimeStamp) {
                                    m = message;
                                }
                            }
                            TempData["sendMessageStatus"] = "OK";
                            TempData["sendMessageStatusMessage"] = "Message number " + m.MessageID + " sent to " + m.ReceiverUser.Username + " at " + m.TimeStamp + ".";
                        }
                    } else {
                        TempData["sendMessageStatus"] = "ERROR";
                        TempData["sendMessageStatusMessage"] = "Message not sent - All fields has to be set";
                    }
                }
                else {
                    //Unable to fetch sender or receiver from DB
                    TempData["sendMessageStatus"] = "ERROR";
                    TempData["sendMessageStatusMessage"] = "Error - Try again";
                }
            }
            else {
                //User does not exist
                TempData["sendMessageStatus"] = "ERROR";
                TempData["sendMessageStatusMessage"] = "Unable to find user";
            }
            return RedirectToAction("SendMessage");
        }

        /// <summary>
        /// This action will create an InboxVM where receiving user is current user.
        /// </summary>
        /// <returns>InboxVM</returns>
        public IActionResult Inbox() {
            string UserID = _userManager.GetUserId(HttpContext.User);
            List<User> users = _userHandler.GetAllUsersList();
            List<Message> messages = _messageHandler.GetMessagesByReceiverId(UserID);

            List<InboxUserVM> vmUsers = new List<InboxUserVM>();
            foreach (User user in users) {
                if (user.UserID != _userManager.GetUserId(HttpContext.User)) {
                    if (hasMessagesFromUser(user, messages)) {
                        List<InboxMessageVM> vmMessages = new List<InboxMessageVM>();
                        int numberOfUnreadMessagesFromUser = 0;
                        foreach(Message m in messages) {
                            if(m.SenderUser.UserID == user.UserID) {
                                if (!m.Removed) {
                                    if (!m.Opened) {
                                        numberOfUnreadMessagesFromUser++;
                                    }
                                    vmMessages.Add(new InboxMessageVM {
                                        MessageID = m.MessageID,
                                        Sender = m.SenderUser.Username,
                                        TimeStamp = m.TimeStamp,
                                        Title = m.Title,
                                        Opened = m.Opened,
                                        Removed = m.Removed
                                    });
                                }
                            }
                        }
                        vmUsers.Add(new InboxUserVM {
                            Messages = vmMessages,
                            Username = user.Username,
                            TotalNumberOfUnread = numberOfUnreadMessagesFromUser
                        });
                    }
                }
            }

            int numberOfReadMessages = 0;
            int numberOfRemovedMessages = 0;
            foreach (Message message in messages) {
                if (message.Opened) {
                    numberOfReadMessages++;
                }
                if (message.Removed) {
                    numberOfRemovedMessages++;
                }
            }
            
            var inbox = new InboxVM {
                Users = vmUsers,
                TotalNumberOfMessages = messages.Count,
                TotalNumberOfOpened = numberOfReadMessages,
                TotalNumberOfRemoved = numberOfRemovedMessages
            };
            return View(inbox);
        }

        private bool hasMessagesFromUser(User user, List<Message> list) {
            foreach (Message message in list) {
                if (message.SenderUser.Equals(user) && !message.Removed) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Fetches a message and returns a viewmodel of that message.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>ViewMessageVM</returns>
        public IActionResult ViewMessage(int messageId) {
            if (_messageHandler.SetMessageAsOpened(messageId)) {
                List<Message> result = _messageHandler.GetMessagesByMessageId(messageId);
                if (result.Count == 1) {
                    Message message = result[0];
                    string UserID = _userManager.GetUserId(HttpContext.User);
                    if (message.ReceiverUser.UserID == UserID) {
                        var viewMessage = new ViewMessageVM {
                            MessageID = messageId,
                            Title = message.Title,
                            Sender = message.SenderUser.Username,
                            Text = message.Text
                        };
                        return View(viewMessage);
                    }
                    else {
                        return Unauthorized();
                    }
                }
            }
            return NotFound();
        }

        /// <summary>
        /// Removes message by messageId
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public IActionResult RemoveMessage(int messageId) {
            if (_messageHandler.removeMessageByMid(messageId)) {
                TempData["removedMessageStatus"] = "Delete successful";
            } else {
                TempData["removedMessageStatus"] = "Delete unsuccessful";
            }
            return RedirectToAction("Inbox");
        }
    }
}
