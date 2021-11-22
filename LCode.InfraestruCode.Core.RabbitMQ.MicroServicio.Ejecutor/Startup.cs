using GreenPipes;
using LCode.InfraestruCode.Core.RabbitMQ.MicroServicio.Ejecutor.ConsumidorMQ;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NETCore.Base._3._0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCode.InfraestruCode.Core.RabbitMQ.MicroServicio.Ejecutor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            BaseConfiguracion BC = new BaseConfiguracion();
            string ServidorMQ = BC.ObtenerValor("ConfigMQ:RabbitMQ:Servidor");
            string MQ = BC.ObtenerValor("ConfigMQ:RabbitMQ:Cola");
            string UsuarioMQ = BC.ObtenerValor("ConfigMQ:RabbitMQ:Usuario");
            string ContraseniaMQ = BC.ObtenerValor("ConfigMQ:RabbitMQ:Contrasenia");
            services.AddMassTransit(x =>
            {
                x.AddConsumer<SolicitudServidorConsumidor>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.UseHealthCheck(provider);
                    cfg.Host(new Uri("rabbitmq://"+ ServidorMQ), h =>
                    {
                        h.Username(UsuarioMQ);
                        h.Password(ContraseniaMQ);
                    });
                    cfg.ReceiveEndpoint(MQ, ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<SolicitudServidorConsumidor>(provider);
                    });
                }));
            });
            services.AddMassTransitHostedService();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LCode.InfraestruCode.Core.RabbitMQ.MicroServicio.Ejecutor", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LCode.InfraestruCode.Core.RabbitMQ.MicroServicio.Ejecutor v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
