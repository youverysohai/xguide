using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManipulatorTcp
{

    public class JogDoneReceived : ValueChangedMessage<JogCommand>
    {
        public JogDoneReceived(JogCommand value) : base(value)
        {
        }
    }
}
