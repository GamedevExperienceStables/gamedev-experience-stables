namespace Game.Level
{
    public class LocationStateStoreFact
    {
        // ReSharper disable once MemberCanBeMadeStatic.Global
        public void Restore(LocationPersistenceFact data, ILocationContext context)
        {
            var facts = context.FindAll<ILocationPersistenceFact>();
            foreach (ILocationPersistenceFact fact in facts)
            {
                if (data.Contains(fact.Id))
                    fact.Confirm();
            }
        }

        // ReSharper disable once MemberCanBeMadeStatic.Global
        public void Store(LocationPersistenceFact data, ILocationContext context)
        {
            data.Clear();

            var facts = context.FindAll<ILocationPersistenceFact>();
            foreach (ILocationPersistenceFact fact in facts)
            {
                if (fact.IsConfirmed)
                    data.AddFact(fact.Id);
            }
        }
    }
}