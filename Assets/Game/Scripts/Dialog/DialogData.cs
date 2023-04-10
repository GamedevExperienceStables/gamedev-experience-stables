using Game.Level;

namespace Game.Dialog
{
    public struct DialogData
    {
        public DialogData(DialogDefinition definition)
            => Definition = definition;

        public DialogDefinition Definition { get; private set; }
    }
}