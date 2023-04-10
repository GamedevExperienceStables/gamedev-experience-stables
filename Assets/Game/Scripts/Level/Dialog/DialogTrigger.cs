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
            var dialogData = new DialogData(dialog);
            _dialogService.ShowRequest(dialogData);
        }

        private void OnTriggerExited(GameObject obj)
            => _dialogService.CloseRequest();
    }
}