using Microsoft.AspNetCore.Identity;

namespace App.webui.identity
{
    public class User:IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        

    }
}