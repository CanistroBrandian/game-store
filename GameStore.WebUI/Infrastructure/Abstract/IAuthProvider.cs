using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.WebUI.Infrastructure.Abstruct
{
    public interface IAuthProvider
    {
        bool Authenticate(string username, string password);
    }
}