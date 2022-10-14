using GamesWithGravitas.ServiceConstruct;

public partial class MessageService
{
    private readonly HelloService _helloService;

    [ServiceConstruct(MethodName = "Create")]
    public MessageService(HelloService helloService)
    {
        _helloService = helloService;
    }
}