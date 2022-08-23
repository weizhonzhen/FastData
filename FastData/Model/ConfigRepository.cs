using FastAop;

namespace FastData.Model
{
    public class ConfigRepository
    {
        public FastAopAttribute Aop { get; set; }

        public string NameSpaceModel { get; set; }

        public string NameSpaceServie { get; set; }

        public FastAop.FastAop.WebType webType { get; set; }
    }
}
