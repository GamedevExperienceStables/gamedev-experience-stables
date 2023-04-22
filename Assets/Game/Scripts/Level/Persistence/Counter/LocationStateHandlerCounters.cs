namespace Game.Level
{
    public class LocationStateHandlerCounters
    {
        // ReSharper disable once MemberCanBeMadeStatic.Global
        public void Restore(LocationPersistenceInt data, ILocationContext context)
        {
            var counters = context.FindAll<ILocationPersistenceInt>();
            foreach (ILocationPersistenceInt counter in counters)
            {
                if (data.TryGetValue(counter.Id, out int value))
                    counter.Value = value;
            }
        }

        // ReSharper disable once MemberCanBeMadeStatic.Global
        public void Store(LocationPersistenceInt data, ILocationContext context)
        {
            foreach (ILocationPersistenceInt counter in context.FindAll<ILocationPersistenceInt>())
            {
                if (counter.IsDirty)
                    data.SetValue(counter.Id, counter.Value);
            }
        }
    }
}