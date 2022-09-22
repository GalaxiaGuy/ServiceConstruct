namespace Test
{
    public partial class MessageService
    {
        public static MessageService ServiceConstruct(System.IServiceProvider serviceProvider)
        {
            return new MessageService((HelloService)serviceProvider.GetService(typeof(HelloService)));
        }
    }
}