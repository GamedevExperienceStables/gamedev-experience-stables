namespace Game.Input
{
    public interface IInputControlMenu
    {
        InputButton BackButton { get; }
        InputButton MenuButton { get; }
        InputButton OptionButton { get; }
    }
}