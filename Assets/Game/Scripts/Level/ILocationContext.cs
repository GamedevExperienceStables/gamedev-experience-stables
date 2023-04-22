using System.Collections.Generic;

namespace Game.Level
{
    public interface ILocationContext
    {
        ILocationBounds LocationBounds { get; }
        IList<T> FindAll<T>();
        LocationPoint FindLocationPoint(ILocationPointKey locationPoint);
    }
}