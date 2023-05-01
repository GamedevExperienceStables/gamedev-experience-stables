using Game.Dialog;
using NaughtyAttributes;
using UnityEngine;
using VContainer;

namespace Game.Level
{
    [RequireComponent(typeof(ZoneTrigger))]
    public class DialogTrigger : MonoBehaviour
    {
        [SerializeField, Required]
        private DialogDefinition dialog;

        private ZoneTrigger _zoneTrigger;

        private DialogService _dialogService;

        [Inject]
        public void Construct(DialogService dialogService)
            => _dialogService = dialogService;


        private void Awake()
        {
            _zoneTrigger = GetComponent<ZoneTrigger>();
            _zoneTrigger.TriggerEntered += OnTriggerEntered;
            _zoneTrigger.TriggerExited += OnTriggerExited;
        }

        private void OnDestroy()
        {
            _zoneTrigger.TriggerEntered -= OnTriggerEntered;
            _zoneTrigger.TriggerExited -= OnTriggerExited;
        }

        private void OnTriggerEntered(GameObject obj)
        {
            if (!dialog)
            {
                Debug.LogWarning("Dialog is not set, trigger will not work");
                return;
            }
            
            _dialogService.ShowRequest(new DialogData(dialog));
        }

        private void OnTriggerExited(GameObject obj)
        {
            if (!dialog)
                return;

            _dialogService.CloseRequest(new DialogData(dialog));
        }
    }
}