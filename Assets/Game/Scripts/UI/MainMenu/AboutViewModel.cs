namespace Game.UI
{
    public class AboutViewModel
    {
        private readonly MainMenuViewRouter _router;

        public AboutViewModel(MainMenuViewRouter router) 
            => _router = router;

        public void Back() 
            => _router.Back();
    }
}