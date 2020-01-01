using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Speckoz.BukkitDev.Models
{
    /// <summary>
    /// Representa o modelo de uma licença
    /// </summary>
    public class LicencaModel
    {
        /// <summary>
        /// ID Unico da licença
        /// </summary>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// ID do cliente que possui a licença
        /// </summary>
        [Required]
        public string ClienteId { get; set; }
        /// <summary>
        /// ID Unico do plugin
        /// </summary>
        [Required]
        public string PluginId { get; set; }
        /// <summary>
        /// Chave da licença
        /// </summary>
        [Required]
        public string LicencaKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string LicencaGlobal { get; set; }
        /// <summary>
        /// Data de criação da licença
        /// </summary>
        [Required]
        public string DataCriacao { get; set; }
        /// <summary>
        /// Horário de criacao da licença
        /// </summary>
        [Required]
        public string HorarioCriacao { get; set; }
        /// <summary>
        /// Indica se a licença está suspensa
        /// </summary>
        [Required]
        public string LicencaSuspenso { get; set; }
    }
}
