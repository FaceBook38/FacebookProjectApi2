using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FaceBookAPI.Models.FaceBook;
using FaceBookAPI.Models.SignalRModels;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace FaceBookAPI.SignalR
{
    [HubName("Chat")]
    public class ChatHub : Hub
    {
        FacebookContext facebookContext = new FacebookContext();
        static List<ConnectedUsers> ConnectedUsers = new List<ConnectedUsers>();
        
        public void Connect(int userId)
        {
            string cId = Context.ConnectionId;
            //check if connection id exists
            if(ConnectedUsers.Count(u => u.connectionId == cId) == 0)
            {
                ConnectedUsers.Add(new ConnectedUsers() { userId = userId, connectionId = cId });
                //client side method
                Clients.AllExcept(cId).NewUserOnline(userId, cId);
            }

        }

        public void LoadFriendMessages(int senderId,int recieverId)
        {
           List<UsersMessage> usersMessages = facebookContext.UsersMessages.Where(u => u.sender_id == senderId && u.reciver_id == recieverId).ToList();
            
            Clients.Caller.LoadMessages(usersMessages);
        }
        //Server Method (implement in client and send data to server)
        public void Sendmessage(string message, int receiverId, int senderId, DateTime dt)
        {
            //implement in client to get called by server and executed in client

            //recevier connection id
            ConnectedUsers receiver = ConnectedUsers.FirstOrDefault(c => c.userId == receiverId);
            if(receiver!=null)
            {
                Clients.Client(receiver.connectionId).Send(message, dt);
            }
            //
            Clients.Caller.Send(message,dt);

            //save message in DB
            facebookContext.UsersMessages.Add(new UsersMessage() { message_content = message, sender_id = senderId, reciver_id = receiverId, date = dt, deleted = false,
                read = false});
            facebookContext.SaveChanges();
            
        }
    }
}