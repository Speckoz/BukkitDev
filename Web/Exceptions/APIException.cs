using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Exceptions
{
    /// <summary>
    /// Exceções relacionada à API
    /// </summary>
    public class APIException : Exception
    {
        private static string msg = 
           "Houve com erro ao tentar se conectar na API, " +
           "verifique se sua API está online e os dados estão configurados corretamente no appsettings.json";
        public APIException() : base(msg)
        {
        }
    }
}
