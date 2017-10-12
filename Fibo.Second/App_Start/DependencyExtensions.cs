using Fibo.Logging;
using Fibo.Calculator;
using Fibo.Processing;
using Fibo.Storage;
using Fibo.Transport;
using Fibo.Transport.Rabbit;
using System.Configuration;
using Autofac;

namespace Fibo.Second
{
    public static class DependencyExtensions
    {
        public static ContainerBuilder RegisterSecond(this ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(RabbitSender<>))
                .As(typeof(ISender<>))
                .SingleInstance()
                .WithParameters(new[]
                {
                    new NamedParameter("hostUri", ConfigurationManager.AppSettings["rabbitHost"]),
                    new NamedParameter("username", ConfigurationManager.AppSettings["rabbitUsername"]),
                    new NamedParameter("password", ConfigurationManager.AppSettings["rabbitPassword"])
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

            return builder;
        }
    }
}
