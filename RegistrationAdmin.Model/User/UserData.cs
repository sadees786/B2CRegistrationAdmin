using System.Collections.Generic;

namespace RegistrationAdmin.Models.User
{
    public class UserData
    {
        public bool accountEnabled { get; set; }
        public string creationType { get; set; }
        public string displayName { get; set; }
        public PasswordProfile passwordProfile { get; set; }
        public string givenName { get; set; }
        public string surname { get; set; }
        public List<signInNames> signInNames { get; set; }

    }
}
