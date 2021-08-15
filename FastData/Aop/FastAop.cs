using System;

namespace FastData.Aop
{
    public class FastAop : IFastAop
    {
        public EventHandler<BeforeEventArgs> BeforeHandler => Before;
        public EventHandler<AfterEventArgs> AfterHandler => After;

        public event EventHandler<AfterEventArgs> After;
        public event EventHandler<BeforeEventArgs> Before;
    }
}
