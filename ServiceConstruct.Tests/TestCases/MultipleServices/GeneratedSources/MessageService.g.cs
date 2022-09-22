namespace Message
{
    public partial class MessageService
    {
        public static MessageService ServiceConstruct(global::System.IServiceProvider serviceProvider)
        {
            return new MessageService(
                (Hello.HelloService)serviceProvider.GetService(typeof(Hello.HelloService)),
                (Goodbye.GoodbyeService)serviceProvider.GetService(typeof(Goodbye.GoodbyeService)));
        }
    }
}