using System;

namespace GamesWithGravitas.ServiceConstruct
{
    public class ServiceConstructAttribute : Attribute
    {
        public string MethodName { get; set; } = "ServiceConstruct";
    }
}

