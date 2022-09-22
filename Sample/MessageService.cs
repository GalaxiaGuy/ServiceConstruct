using ServiceConstruct;

namespace Sample;

public partial class MessageService : IMessageService
{
    private readonly IHelloService _helloService;

    [ServiceConstruct]
    public MessageService(IHelloService helloService)
    {
        _helloService = helloService;
    }

    public string Message() => _helloService.HelloWorld();
}