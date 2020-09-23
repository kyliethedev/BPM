using System;
using System.Linq;
using Admin.Models.Login;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.Data
{
    public class DBInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationContext>>()))
            {
                // Look for any TmpTable Data
                if (context.TmpTable.Any())
                {
                    Console.WriteLine("DBInitiaal");
                    return;   // DB has been seeded
                }

                context.TmpTable.AddRange(
                    new TmpTable
                    {
                        seqno = 1234,
                        name  = "gwmyung"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
