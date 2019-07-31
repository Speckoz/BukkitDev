using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BukkitDevSystem.Models
{
    public class LicencaModel
    {

        [Required]
        public int Id { get; set; }
        [Required]
        public string ClienteId { get; set; }
        [Required]
        public string PluginId { get; set; }
        [Required]
        public string LicencaKey { get; set; }
        [Required]
        public string LicencaGlobal { get; set; }
        [Required]
        public string DataCriacao { get; set; }
        [Required]
        public string HorarioCriacao { get; set; }
        [Required]
        public string LicencaSuspenso { get; set; }
    }
}
