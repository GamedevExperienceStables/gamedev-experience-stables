namespace Game.UI
{
    public static class LayoutNames
    {
        public static class PauseMenu
        {
            public const string CONTAINER = "pause-menu";
            
            public const string BUTTON_MAIN_MENU = "button-exit";
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
            
            public const string SP_LEFT = "sp-left";
            public const string SP_CENTER = "sp-center";
            public const string SP_RIGHT = "sp-right";
            
            public const string WIDGET_BAR_MASK = "mask";
            
            public const string RUNE_SLOT_CLASS_NAME = "hud-rune-slot";
            public const string RUNE_SLOT_INTERACTABLE_CLASS_NAME = "hud-rune-slot--interactable";
            public const string RUNE_SLOT_ICON = "icon";
            public const string RUNE_SLOT_BACKGROUND = "background";
            
            public const string WIDGET_INTERACTION = "widget-interaction";
            public const string WIDGET_INTERACTION_TEXT = "text";
        }

        public static class LoadingScreen
        {
            public const string CONTAINER = "loading-screen";
        }

        public static class GameOver
        {
            public const string CONTAINER = "game-over-screen";
            
            public const string BUTTON_RESTART = "button-restart";
            public const string BUTTON_MAIN_MENU = "button-exit";
        }

        public static class Inventory
        {
            public const string CONTAINER = "book-screen";
            public const string BUTTON_CLOSE = "button-close";
            
            public const string PAGE_DETAILS = "page-details";
            
            public const string RUNE_NAME = "text-rune-name";
            public const string RUNE_DESCRIPTION = "text-rune-description";
            public const string RUNE_ICON = "image-rune";

            public const string RUNE_DRAGGER = "rune-dragger";
            public const string RUNE_DRAGGER_DRAG_CLASS_NAME = "rune-dragger--drag";
            
            public const string RUNE_SLOT_CLASS_NAME = "magic-book-slot";
            public const string SLOT_DISABLED_CLASS_NAME = "magic-book-slot--disabled";
            public const string RUNE_SLOT_ICON = "icon";
        }
    }
}