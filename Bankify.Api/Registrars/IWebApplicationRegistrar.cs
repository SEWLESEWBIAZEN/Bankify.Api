﻿
namespace Bankify.Api.Registrars
{
    public interface IWebApplicationRegistrar:IRegistrar
    {
        public void RegisterPipelineComponents(WebApplication app);
    }
   
}
