public partial class MessageService
{
    public static MessageService ServiceConstruct(global::System.IServiceProvider serviceProvider)
    {
        return new MessageService((HelloService)serviceProvider.GetService(typeof(HelloService)));
    }
}