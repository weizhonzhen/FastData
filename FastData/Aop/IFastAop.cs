using System;

namespace FastData.Aop
{
    public interface IFastAop
    {
        event EventHandler<AfterEventArgs> After;
        EventHandler<AfterEventArgs> AfterHandler { get; }

        EventHandler<BeforeEventArgs> BeforeHandler { get; }

        event EventHandler<BeforeEventArgs> Before;

        EventHandler<MapEventArgs> MapHandler { get; }

        event EventHandler<MapEventArgs> Map;
    }
}
