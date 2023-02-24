namespace Game.Input
{
    public interface IHistoryHandler<in T>
    {
        void PushState(T state);
        void ReplaceState(T state);
        void Back(int depth = 1);
    }
}