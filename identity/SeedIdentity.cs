using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace App.webui.identity
{
    public static class SeedIdentity
    {
        public static async Task Seed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {

            var username = config["Data:AdminUser:username"];
            var password = config["Data:AdminUser:password"];
            var email = config["Data:AdminUser:email"];
            var role = config["Data:AdminUser:role"];

                if(await userManager.FindByNameAsync(username)==null){

                    await roleManager.CreateAsync(new IdentityRole(role));
                    var user = new User(){
                        UserName=username,
                        Email=email,
                        FirstName="admin",
                        LastName="admin",
                        EmailConfirmed=true
                    };

                    var result = await userManager.CreateAsync(user,password);

                    if(result.Succeeded){
                        await userManager.AddToRoleAsync(user,role);
                    }
                }
        }
    }
}