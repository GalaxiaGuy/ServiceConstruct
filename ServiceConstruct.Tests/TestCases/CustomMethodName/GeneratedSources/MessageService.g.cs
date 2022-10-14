public partial class MessageService
{
    public static MessageService Create(global::System.IServiceProvider serviceProvider)
    {
        return new MessageService((HelloService)serviceProvider.GetService(typeof(HelloService)));
    }
}