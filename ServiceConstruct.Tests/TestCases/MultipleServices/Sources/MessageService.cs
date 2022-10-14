using Goodbye;
using Hello;
using GamesWithGravitas.ServiceConstruct;

namespace Message
{
    public partial class MessageService
    {
        private readonly HelloService _helloService;
        private readonly GoodbyeService _goodbyeService;

        [ServiceConstruct]
        public MessageService(HelloService helloService, GoodbyeService goodbyeService)
        {
            _helloService = helloService;
            _goodbyeService = goodbyeService;
        }
    }
}