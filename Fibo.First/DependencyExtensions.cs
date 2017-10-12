using Fibo.Logging;
using Fibo.Calculator;
using Fibo.Processing;
using Fibo.Storage;
using Fibo.Transport;
using Fibo.Transport.Rabbit;
using System.Configuration;
using Autofac;
using Fibo.Transport.Rest;

namespace Fibo.First
{
    public static class DependencyExtensions
    {
        public static ContainerBuilder RegisterFirst(this ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(RabbitConsumer<>))
                .As(typeof(IConsumer<>))
                .SingleInstance()
                .WithParameters(new[]
                {
                    new NamedParameter("hostUri", ConfigurationManager.AppSettings["rabbitHost"]),
                    new NamedParameter("username", ConfigurationManager.AppSettings["rabbitUsername"]),
                    new NamedParameter("password", ConfigurationManager.AppSettings["rabbitPassword"]),
                    new NamedParameter("queueName", ConfigurationManager.AppSettings["rabbitQueue"])
                });

            builder.RegisterGeneric(typeof(RestSender<>))
                .As(typeof(ISender<>))
                .SingleInstance()
                .WithParameters(new[]
                {
                    new NamedParameter("baseUrl", ConfigurationManager.AppSettings["restUrl"]),
                    new NamedParameter("resource", ConfigurationManager.AppSettings["restResource"])
                });

            builder.RegisterGeneric(typeof(DictionaryStorage<,>))
                .As(typeof(IStorage<,>))
                .SingleInstance();

            builder.RegisterType<FibonacciCalculator>()
                .As<ICalculator>()
                .SingleInstance();

            builder.RegisterType<Processor>()
                .As<IProcessor>()
                .SingleInstance();

            builder.RegisterType<Log4NetLogger>()
                .As<ILogger>()
                .SingleInstance();

            builder.RegisterType<Application>().As<IApplication>();

            return builder;
        }
    }
}
