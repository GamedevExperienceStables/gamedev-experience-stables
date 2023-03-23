using UnityEngine;

namespace Game.UI
{
    public class PageRouter : MonoBehaviour
    {
        private PageView _activePage;

        protected void Open(PageView view)
        {
            if (_activePage)
                _activePage.Hide();

            _activePage = view;
            _activePage.Show();
        }
    }
}