namespace Game.Input
{
    public enum InputSchema
    {
        Undefined,
        Menus,
        Gameplay
    }
    
    public delegate void InputSchemaDelegate(InputSchema schema);
}