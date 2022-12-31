using Game.Input;
using Game.UI;
using UnityEngine;

namespace Game.Managers
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField]
        private GameManager game;
        
        [SerializeField]
        private LevelManager level;

        [SerializeField]
        private UIManager ui;

        private void Start()
        {
            var input = new InputService();

            game.Construct(ui, level, input);
            level.Construct(input);
            ui.Construct(game, level);

            game.EnterMainMenu();
        }
    }
}