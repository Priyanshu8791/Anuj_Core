using System;
using System.Collections.Generic;

#nullable disable

namespace Anuj_Core.DB_Context
{
    public partial class Login
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Passward { get; set; }
    }
}
