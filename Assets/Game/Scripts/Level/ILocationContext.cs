using System.Collections.Generic;

namespace Game.Level
{
    public interface ILocationContext
    {
        ILocationDefinition Location { get; }
        ILocationBounds LocationBounds { get; }
        IList<T> FindAll<T>();
        LocationPoint FindLocationPoint(ILocationPointKey locationPointKey);
    }
}