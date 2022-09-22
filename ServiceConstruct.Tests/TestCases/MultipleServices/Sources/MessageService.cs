using Goodbye;
using Hello;

namespace Message
{
    public partial class MessageService
    {
        private readonly HelloService _helloService;
        private readonly GoodbyeService _goodbyeService;

        [ServiceConstruct.ServiceConstruct]
        public MessageService(HelloService helloService, GoodbyeService goodbyeService)
        {
            _helloService = helloService;
            _goodbyeService = goodbyeService;
        }
    }
}