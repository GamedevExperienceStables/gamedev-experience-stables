namespace Game.Inventory
{
    
    public abstract class ConsumableDefinition : ItemDefinition, IItemExecutableDefinition
    {
        public virtual bool CanExecute(ItemExecutionContext context) 
            => true;

        public virtual void Execute(ItemExecutionContext context)
        {
        }
    }
}