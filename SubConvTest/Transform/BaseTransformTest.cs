using System.Collections.Generic;

namespace SubConvTest.Transform
{
    public abstract class BaseTransformTest
    {
        protected static IEnumerable<T> ToEnumerable<T>(params T[] data) => data;
    }
}
