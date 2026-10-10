using Microsoft.AspNetCore.Identity;
using MedicaidEmploymentVerificationApplication.Models;

namespace MedicaidEmploymentVerificationApplication.Utilities
{
    public static class DataSeeder
    {
        public static async Task SeedIdentities(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            string name = configuration["SeedEmployee:Username"];
            if (await userManager.FindByNameAsync(name) == null)
            {
                string password = configuration["SeedEmployee:Password"] ??
                    throw new InvalidOperationException("DataSeeder could not find Employee Password. SeedEmployee:Password is not configured");

                var user = new User
                {
                    FirstName = configuration["SeedEmployee:FirstName"],
                    LastName = configuration["SeedEmployee:LastName"],
                    UserName = configuration["SeedEmployee:Username"],
                };

                var result = await userManager.CreateAsync(user, password);

                if(!result.Succeeded)
                {
                    throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
                }
                await userManager.AddToRoleAsync(user, "Employee");
            }

            string applicantName = configuration["SeedApplicant:Username"];
            if (await userManager.FindByNameAsync(applicantName) == null)
            {
                string password = configuration["SeedApplicant:Password"] ??
                    throw new InvalidOperationException("DataSeeder could not find Employee Password. SeedEmployee:Password is not configured");

                var user = new User
                {
                    FirstName = configuration["SeedApplicant:FirstName"],
                    LastName = configuration["SeedApplicant:LastName"],
                    UserName = configuration["SeedApplicant:Username"],
                };

                var result = await userManager.CreateAsync(user, password);

                if(!result.Succeeded)
                {
                    throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
                }
                await userManager.AddToRoleAsync(user, "Applicant");
            }

            
        }
    }
}
