using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business
{
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            try
            {
                await AddInterestRule(app);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private static async Task AddInterestRule(IApplicationBuilder app)
        {
            using(var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<Context>();

                if (!await context.Interest.AnyAsync())
                {
                    context.Interest.AddRange(new List<InterestRule>
                    {
                        new InterestRule { DelayDays = 3, Penalty = 0.02m, InterestPerDay = 0.001m },
                        new InterestRule { DelayDays = 4, Penalty = 0.03m, InterestPerDay = 0.002m },
                        new InterestRule { DelayDays = 6, Penalty = 0.05m, InterestPerDay = 0.003m }
                    });

                    await context.SaveChangesAsync();
                }
            }
        }

    }
}
