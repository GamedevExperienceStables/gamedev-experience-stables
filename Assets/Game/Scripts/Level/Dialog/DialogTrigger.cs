using Game.Dialog;
using NaughtyAttributes;
using UnityEngine;
using VContainer;

namespace Game.Level
{
    [RequireComponent(typeof(ZoneTrigger))]
    public class DialogTrigger : MonoBehaviour, ISwitchObject
    {
        [SerializeField, Required]
        private DialogDefinition dialog;

        [SerializeField, Min(-1)]
        private int count = -1;

        private ZoneTrigger _zoneTrigger;

        private DialogService _dialogService;

        [ShowNonSerializedField]
        private int _remainCount;

        public bool IsDirty { get; set; }

        public bool IsActive
        {
            get => _remainCount is > 0 or -1;
            set => _remainCount = value ? count : 0;
        }

        [Inject]
        public void Construct(DialogService dialogService)
        {
            _dialogService = dialogService;
            _remainCount = count;
        }

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
            
            CloseDialog(true);
        }

        private void OnTriggerEntered(GameObject obj)
        {
            if (!dialog)
            {
                Debug.LogWarning("Dialog is not set, trigger will not work");
                return;
            }

            if (!IsActive)
                return;

            _dialogService.ShowRequest(new DialogData(dialog));
        }

        private void OnTriggerExited(GameObject obj)
        {
            if (!dialog)
                return;

            if (!IsActive)
                return;

            if (_remainCount > 0)
            {
                _remainCount--;
                IsDirty = true;
            }

            CloseDialog();
        }

        private void CloseDialog(bool immediate = false) 
            => _dialogService.CloseRequest(new DialogData(dialog), immediate);
    }
}