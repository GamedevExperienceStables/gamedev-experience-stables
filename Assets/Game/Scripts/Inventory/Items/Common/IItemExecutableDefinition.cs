namespace Game.Inventory
{
    public interface IItemExecutableDefinition 
    {
        bool CanExecute(ItemExecutionContext context);
        void Execute(ItemExecutionContext context);
    }
}