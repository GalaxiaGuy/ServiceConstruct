namespace Test
{
    public partial class MessageService
    {
        public static MessageService ServiceConstruct(global::System.IServiceProvider serviceProvider)
        {
            return new MessageService((Test.HelloService)serviceProvider.GetService(typeof(Test.HelloService)));
        }
    }
}