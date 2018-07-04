using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamHelper
{
    class InvalidMessageException :  Exception
    {
        public InvalidMessageException(object message) : base(message.ToString())
        {
        }
    }
}
