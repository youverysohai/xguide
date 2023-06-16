using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X_Guide.MVVM.Model;
using XGuideSQLiteDB.Models;

namespace X_Guide.MessageToken
{
    internal class UserLoginChanged : ValueChangedMessage<User>
    {
        public UserLoginChanged(User value) : base(value)
        {
        }
    }
}
