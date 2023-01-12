using System;

namespace Game.UI
{
    public interface IFaderScreen
    {
        void FadeIn(TimeSpan duration);
        void FadeIn(float opacity, TimeSpan duration);
        void FadeOut();
        void FadeOut(TimeSpan duration);
    }
}