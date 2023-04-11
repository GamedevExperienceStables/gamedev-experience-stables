namespace Game.UI
{
    public static class LayoutNames
    {
        public static class Pause
        {
            public const string CONTAINER = "pause-screen";
        }
        
        public static class PauseMenu
        {
            public const string CONTAINER = "pause-menu";
            
            public const string BUTTON_MAIN_MENU = "button-exit";
            public const string BUTTON_RESUME = "button-resume";
            public const string BUTTON_SETTINGS = "button-settings";
        }
        
        public static class PauseSettings
        {
            public const string CONTAINER = "pause-settings";
            public const string BUTTON_BACK = "button-back";
        }

        public static class StartMenu
        {
            public const string BUTTON_NEW_GAME = "button-start";
            public const string BUTTON_CONTINUE = "button-continue";
            public const string BUTTON_QUIT = "button-quit";
            public const string BUTTON_SETTINGS = "button-settings";
            public const string BUTTON_ABOUT = "button-about";
            public const string BUTTON_ART = "button-art";
            
            public const string BUTTON_BACK = "button-back";

            public const string BUTTON_ICON = "icon";
            
            public const string MENU = "menu";
            public const string PAGES = "pages";
            public const string MODAL = "menu-modal";
            
            public const string PREVIEW = "preview";
            public const string PREVIEW_IMAGE = "image";
            public const string PREVIEW_CAPTION = "caption";

            public const string PAGE_SETTINGS = "page-settings";
            public const string PAGE_ABOUT = "page-about";
            public const string PAGE_ART = "page-art";
            
            public const string PAGE_HEADING = "header";
            
            public const string BUTTON_ACTIVE_CLASS_NAME = "menu-button--active";
            
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
            
            public const string WIDGET_SAVING = "widget-saving";
            public const string WIDGET_SAVING_CONTAINER = "container";
            public const string WIDGET_SAVING_TEXT = "text";

            public const string WIDGET_SAVING_CLASS_NAME = "widget-saving";
            public const string WIDGET_SAVING_ENABLED_CLASS_NAME = "widget-saving--enabled";
            
            public const string DIALOG = "widget-dialog";
            public const string DIALOG_TITLE = "title";
            public const string DIALOG_TEXT = "text";
        }
        
        public static class MiniMap
        {
            public const string ROOT = "mini-map";

            public const string PLAYER = "player";
            
            public const string MAP = "map";
            public const string MAP_WRAPPER = "map-wrapper";
            public const string MAP_CONTAINER = "map-container";
            public const string MAP_COORDINATES = "map-coordinates";
            
            public const string MARKER_CLASS_NAME = "mini-map__marker";
        }

        public static class LoadingScreen
        {
            public const string CONTAINER = "loading-screen";
        }

        public static class GameOver
        {
            public const string CONTAINER = "game-over-screen";
            
            public const string CAPTION = "caption";
            public const string DESCRIPTION = "description";
            
            public const string BUTTON_RESTART = "button-restart";
            public const string BUTTON_MAIN_MENU = "button-exit";
        }

        public static class Inventory
        {
            public const string SCREEN = "book-screen";
            public const string BOOK = "book";
            public const string BUTTON_CLOSE = "button-close";
            
            public const string PAGE_DETAILS = "page-details";
            
            public const string RUNE_NAME = "text-rune-name";
            public const string RUNE_DESCRIPTION = "text-rune-description";
            public const string RUNE_LEVEL = "text-rune-level";
            public const string RUNE_ICON = "image-rune-icon";

            public const string RUNE_DRAGGER = "rune-dragger";
            public const string RUNE_DRAGGER_DRAG_CLASS_NAME = "rune-dragger--drag";
            
            public const string RUNE_SLOT_CLASS_NAME = "magic-book-slot";
            public const string RUNE_PASSIVE_SLOT_CLASS_NAME = "magic-book-slot-passive";
            public const string SLOT_DISABLED_CLASS_NAME = "magic-book-slot--disabled";
            public const string RUNE_SLOT_ICON = "icon";

            public const string BOOK_CLASS_NAME = "magic-book";
            public const string BOOK_HIDDEN_CLASS_NAME = "magic-book--hidden";

            public const string BOOK_DETAILS_CLASS_NAME = "magic-book-details";
            public const string BOOK_DETAILS_HIDDEN_CLASS_NAME = "magic-book-details--hidden";
        }
        
        public static class Modal
        {
            public const string CONTAINER = "window-modal";
            
            public const string TITLE = "text-title";
            public const string BLOCK_MESSAGE = "block-message";
            public const string TEXT_MESSAGE = "text-message";
            
            public const string BUTTON_CONFIRM = "button-confirm";
            public const string BUTTON_CANCEL = "button-cancel";
        }
    }
}