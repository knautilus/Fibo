using Fibo.Logging;
using Fibo.Calculator;
using Fibo.Storage;
using Fibo.Transport;
using Fibo.Transport.Rabbit;
using Fibo.Transport.Rest;
using StructureMap;
using System.Configuration;

namespace Fibo.First
{
    public class DependencyConfig : Registry
    {
        public DependencyConfig()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
            For(typeof(IConsumer<>)).Singleton().Use(typeof(RabbitConsumer<>))
                .Ctor<string>("hostUri").Is(ConfigurationManager.AppSettings["rabbitHost"])
                .Ctor<string>("username").Is(ConfigurationManager.AppSettings["rabbitUsername"])
                .Ctor<string>("password").Is(ConfigurationManager.AppSettings["rabbitPassword"])
                .Ctor<string>("queueName").Is(ConfigurationManager.AppSettings["rabbitQueue"]);
            For(typeof(ISender<>)).Singleton().Use(typeof(RestSender<>))
                .Ctor<string>("baseUrl").Is(ConfigurationManager.AppSettings["restUrl"])
                .Ctor<string>("resource").Is(ConfigurationManager.AppSettings["restResource"]);
            For(typeof(ICalculator<>)).Singleton().Use(typeof(FibonacciCalculator));
            For(typeof(IStorage<,>)).Singleton().Use(typeof(DictionaryStorage<,>));
            For<ILogger>().Singleton().Use<Log4NetLogger>();
        }
    }
}
