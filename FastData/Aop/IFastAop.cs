using System;

namespace FastData.Aop
{
    public interface IFastAop
    {
        event EventHandler<AfterEventArgs> After;
        EventHandler<BeforeEventArgs> BeforeHandler { get; }

        event EventHandler<BeforeEventArgs> Before;
        EventHandler<AfterEventArgs> AfterHandler { get; }
    }
}
