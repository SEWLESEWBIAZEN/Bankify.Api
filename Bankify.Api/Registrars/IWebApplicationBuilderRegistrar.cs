﻿namespace Bankify.Api.Registrars
{
    public interface IWebApplicationBuilderRegistrar : IRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder);

    }
}
