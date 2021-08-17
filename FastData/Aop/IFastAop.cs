using System;

namespace FastData.Aop
{
    public interface IFastAop
    {
        void Map(MapContext context);

        void Before(BeforeContext context);

        void After(AfterContext context);

        void Exception(Exception ex, string name);
    }
}