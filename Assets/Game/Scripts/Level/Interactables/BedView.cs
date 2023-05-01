using NaughtyAttributes;
using UnityEngine;

namespace Game.Level
{
    public class BedView : MonoBehaviour
    {
        [SerializeField, Required]
        private SavePoint savePoint;

        [SerializeField, Required]
        private GameObject savingFeedback;


        public void Awake() 
            => savePoint.Saving += OnSaving;

        private void OnDestroy() 
            => savePoint.Saving -= OnSaving;

        private void OnSaving()
        {
            savingFeedback.SetActive(false);
            savingFeedback.SetActive(true);
        }
    }
}