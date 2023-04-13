namespace Game.Input
{
    public interface IInputControlMenu
    {
        InputButton ConfirmButton { get; }
        InputButton BackButton { get; }
        InputButton MenuButton { get; }
        InputButton OptionButton { get; }
    }
}