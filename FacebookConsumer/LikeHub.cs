using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace FacebookConsumer
{
    public class LikeHub : Hub
    {
        public void SendLike(int postId, int userId)
        {
            Clients.All.newLike(postId,userId);
        }
    }
}