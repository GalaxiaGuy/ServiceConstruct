using Hello;

namespace Message
{
    public partial class MessageService
    {
        private readonly HelloService _helloService;

        [ServiceConstruct.ServiceConstruct]
        public MessageService(HelloService helloService)
        {
            _helloService = helloService;
        }
    }
}