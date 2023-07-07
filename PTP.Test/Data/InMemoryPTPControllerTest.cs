using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PTP.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Test.Data
{
    public class InMemoryPTPControllerTest 
    {
        private readonly DbContextOptions<PTPContext> _contextOptions;
        public InMemoryPTPControllerTest()
        {
            _contextOptions = new DbContextOptionsBuilder<PTPContext>()
                .UseInMemoryDatabase("PTPControllerTest")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new PTPContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.AddRange();

            context.SaveChanges();
        }

    }
}
