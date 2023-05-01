using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class ArtView : PageView<ArtViewModel>
    {
        private ModalSettings _downloadModal;
        
        private IList<Label> _headerLabels;
        private Button _buttonDownload;
        private Button _buttonBack;

        private ArtSettings _settings;
        private ScrollView _scrollContainer;

        private ArtBookView _artBook;
        
        private string _url;
        
        private CommonFx _commonFx;

        [Inject]
        public void Construct(ArtSettings settings, ArtBookView artBook, CommonFx commonFx)
        {
            _settings = settings;
            _artBook = artBook;
            _downloadModal = settings.DownloadModal;
            _commonFx = commonFx;
        }

        protected override void OnAwake()
        {
            SetContent(LayoutNames.StartMenu.PAGE_ART);
            
            _headerLabels = Content.Q<VisualElement>(LayoutNames.StartMenu.PAGE_HEADING)
                .Query<Label>().ToList();
            _buttonDownload = Content.Q<Button>(LayoutNames.StartMenu.BUTTON_DOWNLOAD);
            _buttonBack = Content.Q<Button>(LayoutNames.StartMenu.BUTTON_BACK);

            _scrollContainer = Content.Q<ScrollView>(LayoutNames.StartMenu.SCROLL_CONTAINER);
            
            _artBook.Create(Content);

            _url = _settings.DownloadLink;
            
            RegisterCallbacks();
        }
        
        private void Start()
            => UpdateText();

        private void OnDestroy() 
            => UnregisterCallbacks();

        private void RegisterCallbacks()
        {
            _buttonBack.clicked += OnBackButton;
            _buttonDownload.clicked += OnDownloadButton;
            
            _commonFx.RegisterButton(_buttonBack, ButtonStyle.Primary);
            _commonFx.RegisterButton(_buttonDownload, ButtonStyle.Primary);

            localization.Changed += OnLocalisationChanged;
        }

        private void UnregisterCallbacks()
        {
            _buttonBack.clicked -= OnBackButton;
            _buttonDownload.clicked -= OnDownloadButton;
            
            _commonFx.UnRegisterButton(_buttonBack, ButtonStyle.Primary);
            _commonFx.UnRegisterButton(_buttonDownload, ButtonStyle.Primary);

            localization.Changed -= OnLocalisationChanged;
        }

        public override void Show()
        {
            Content.SetEnabled(true);
            Content.RemoveFromClassList(LayoutNames.StartMenu.PAGE_HIDDEN_CLASS_NAME);
            
            _scrollContainer.scrollOffset = Vector2.zero;
        }

        public override void Hide()
        {
            Content.SetEnabled(false);
            Content.AddToClassList(LayoutNames.StartMenu.PAGE_HIDDEN_CLASS_NAME);
        }
        
        private void OnLocalisationChanged()
            => UpdateText();
        
        private void UpdateText()
        {
            foreach (Label headerLabel in _headerLabels)
                headerLabel.text = _settings.header.GetLocalizedString();

            _buttonDownload.text = _settings.download.GetLocalizedString();
            _buttonBack.text = _settings.back.GetLocalizedString();
        }

        private void OnBackButton() 
            => ViewModel.Back();
        
        private void OnDownloadButton() 
            => ShowLinkModal();
        
        private void ShowLinkModal()
        {
            ModalContext context = ModalSettingsExtensions.CreateContext(_downloadModal, OpenLink);
            
            ViewModel.ShowModal(context);
        }

        private void OpenLink()
            => ViewModel.OpenURL(_url);
    }
}