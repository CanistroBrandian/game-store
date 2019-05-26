using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Web.Services.Abstract
{
    public interface IAuthProvider
    {
        bool Authenticate(string username, string password);
    }
}