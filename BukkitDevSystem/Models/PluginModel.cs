using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BukkitDevSystem.Models
{
    public partial class PluginModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string NomePlugin { get; set; }
        [Required]
        public string AutorPlugin { get; set; }
        [Required]
        public string VersaoPlugin { get; set; }
        [Required]
        public string TipoPlugin { get; set; }
        [Required]
        public string PrecoPlugin { get; set; }
        [Required]
        public string DescricaoPlugin { get; set; }
        [Required]
        public bool ImagemPadraoPersonalizada { get; set; }
    }
}
