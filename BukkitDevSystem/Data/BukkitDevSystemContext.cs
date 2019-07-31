using System;
using BukkitDevSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BukkitDevSystem.Models
{
    public class BukkitDevSystemContext : DbContext
    {
        public BukkitDevSystemContext(DbContextOptions<BukkitDevSystemContext> options)
            : base(options)
        {
        }

        public DbSet<PluginModel> Pluginlist { get; set; }

    }
}
