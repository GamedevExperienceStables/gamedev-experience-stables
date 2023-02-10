namespace Game.UI
{
    public static class LayoutNames
    {
        public static class PauseMenu
        {
            public const string CONTAINER = "pause-menu";
            
            public const string BUTTON_MAIN_MENU = "button-exit";
            public const string BUTTON_RESTART = "button-restart";
            public const string BUTTON_RESUME = "button-resume";
        }

        public static class StartMenu
        {
            public const string BUTTON_START = "button-start";
            public const string BUTTON_CONTINUE = "button-continue";
            public const string BUTTON_QUIT = "button-quit";
        }

        public static class Hud
        {
            public const string BUTTON_MENU = "button-menu";
            
            public const string WIDGET_HP = "widget-hp";
            public const string WIDGET_MP = "widget-mp";
            public const string WIDGET_SP = "widget-sp";
            public const string WIDGET_CRYSTAL = "widget-crystal";
            
            public const string WIDGET_BAR_MASK = "mask";

            public const string TEXT_LABEL = "text-label";
            public const string TEXT_CURRENT = "text-current";
            public const string TEXT_MAX = "text-max";
        }

        public static class LoadingScreen
        {
            public const string CONTAINER = "loading-screen";
        }
    }
}