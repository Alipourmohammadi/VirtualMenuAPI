using Microsoft.AspNetCore.Identity;
using VirtualMenuAPI.Data.Helpers;

namespace VirtualMenuAPI.Data
{

  public class AppDbInitializer
  {
    public static async Task SeedRolesToDb(IApplicationBuilder applicationBuilder)
    {
      using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
      {
        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync(UserRoles.Customer))
          await roleManager.CreateAsync(new IdentityRole(UserRoles.Customer));

        if (!await roleManager.RoleExistsAsync(UserRoles.Barista))
          await roleManager.CreateAsync(new IdentityRole(UserRoles.Barista));

        if (!await roleManager.RoleExistsAsync(UserRoles.Manager))
          await roleManager.CreateAsync(new IdentityRole(UserRoles.Manager));

      }
    }
  }

}
