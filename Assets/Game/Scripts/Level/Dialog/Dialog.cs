namespace Game.Level
{
    public class Dialog : IDialogDefinition
    {
        public Dialog(IDialogItem item)
        {
            Single = item;
        }

        public IDialogItem Single { get; }
    }
}