﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Norris.Data.Data.Entities
{
    public class User : IdentityUser
    {
        List<User> Friends;
        List<GameSession> gameSessions;
    }
}
