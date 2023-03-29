namespace Game.UI
{
    public class ArtViewModel
    {
        private readonly MainMenuViewRouter _router;

        public ArtViewModel(MainMenuViewRouter router) 
            => _router = router;

        public void Back() 
            => _router.Back();
    }
}