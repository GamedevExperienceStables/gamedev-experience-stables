#if UNITY_EDITOR
namespace Game.Level
{
    public partial class LocationPoint
    {
        public void SetPointKey(LocationPointKey point)
            => this.point = point;
    }
}
#endif