using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpConnectionHandler
{
    public class ClientStatusChanged : ValueChangedMessage<bool>
    {
        public ClientStatusChanged(bool value) : base(value)
        {
        }
    }
}
