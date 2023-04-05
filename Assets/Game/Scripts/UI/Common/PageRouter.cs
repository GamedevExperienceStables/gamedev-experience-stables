using UnityEngine;

namespace Game.UI
{
    public class PageRouter : MonoBehaviour
    {
        protected PageView ActivePage { get; private set; }

        protected virtual void Open(PageView view)
        {
            if (ActivePage)
                ActivePage.Hide();

            ActivePage = view;
            ActivePage.Show();
        }
    }
}