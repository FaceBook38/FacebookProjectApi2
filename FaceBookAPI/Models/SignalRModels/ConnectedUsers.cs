using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaceBookAPI.Models.SignalRModels
{
    public class ConnectedUsers
    {
        public int userId { get; set; }
        public string connectionId { get; set; }

    }
}