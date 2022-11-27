using FastAop;
using static FastAop.FastAop;

namespace FastData.Model
{
    public class ConfigRepository
    {
        public FastAopAttribute Aop { get; set; }

        public string NameSpaceModel { get; set; }

        public string NameSpaceServie { get; set; }

        public WebType webType { get; set; }

        public ServiceLifetime ServiceLifetime { get; set; } = ServiceLifetime.Scoped;
    }
}