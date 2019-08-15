using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BukkitDevSystem.Models
{
    /// <summary>
    /// Representa o modelo de um plugin
    /// </summary>
    public partial class PluginModel
    {
        /// <summary>
        /// ID Unico do plugin
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Nome do plugin
        /// </summary>
        [Required]
        public string NomePlugin { get; set; }
        /// <summary>
        /// Nome do autor do plugin
        /// </summary>
        [Required]
        public string AutorPlugin { get; set; }
        /// <summary>
        /// Versão do plugin
        /// </summary>
        [Required]
        public string VersaoPlugin { get; set; }
        /// <summary>
        /// Tipo do plugin
        /// </summary>
        [Required]
        public string TipoPlugin { get; set; }
        /// <summary>
        /// Preço do plugin
        /// </summary>
        [Required]
        public string PrecoPlugin { get; set; }
        /// <summary>
        ///  Descricão do plugin
        /// </summary>
        [Required]
        public string DescricaoPlugin { get; set; }
        /// <summary>
        /// Indica se o plugin possui imagem padrão ou personalizada
        /// </summary>
        [Required]
        public bool ImagemPadraoPersonalizada { get; set; }
    }
}
