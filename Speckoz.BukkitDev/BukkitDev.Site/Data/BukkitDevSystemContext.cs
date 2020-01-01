using Microsoft.EntityFrameworkCore;

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