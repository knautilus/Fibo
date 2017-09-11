using Fibo.Logging;
using Fibo.Processing;
using Fibo.Storage;
using Fibo.Transport;
using Fibo.Transport.Rabbit;
using StructureMap;
using System.Configuration;

public class DependencyConfig : Registry
{
    public DependencyConfig()
    {
        Scan(scan => {
            scan.TheCallingAssembly();
            scan.WithDefaultConventions();
        });
        For(typeof(ISender<>)).Singleton().Use(typeof(RabbitSender<>))
            .Ctor<string>("hostUri").Is(ConfigurationManager.AppSettings["rabbitHost"])
            .Ctor<string>("username").Is(ConfigurationManager.AppSettings["rabbitUsername"])
            .Ctor<string>("password").Is(ConfigurationManager.AppSettings["rabbitPassword"]);
        For(typeof(ICalculator<>)).Singleton().Use(typeof(FibonacciCalculator));
        For(typeof(IStorage<,>)).Singleton().Use(typeof(DictionaryStorage<,>));
        For<ILogger>().Singleton().Use<Log4NetLogger>();
    }
}
