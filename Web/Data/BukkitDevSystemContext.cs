using System;
using Speckoz.BukkitDev.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Speckoz.BukkitDev.Models
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
