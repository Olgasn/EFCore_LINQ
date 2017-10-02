﻿using Microsoft.EntityFrameworkCore;

namespace Fuels.Models
{
    public class FuelsContext: DbContext
    {
        public FuelsContext(DbContextOptions<FuelsContext> options): base(options)
        {
        }
        public DbSet<Fuel> Fuels { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Tank> Tanks { get; set; }
    }
}
