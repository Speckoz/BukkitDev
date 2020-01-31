using System;

namespace Web.Exceptions
{
    /// <summary>
    /// Exceções relacionada à API
    /// </summary>
    public class APIException : Exception
    {
        private static readonly string _msg =
           "Houve com erro ao tentar se conectar na API, " +
           "verifique se sua API está online e os dados estão configurados corretamente no appsettings.json";

        public APIException() : base(_msg)
        {
        }
    }
}