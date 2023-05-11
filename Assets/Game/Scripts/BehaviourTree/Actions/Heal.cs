using Game.Actors;
using Game.Stats;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace Game.BehaviourTree.Actions
{
    [Category("Interact")]
    public class Heal : ActionTask<ActorController>
    {
        [SliderField(0, 100)]
        public BBParameter<float> percentage = 100f;
        
        protected override string info 
            => $"Heal {percentage}%";
        
        protected override void OnExecute()
        {
            float maxHealth = agent.GetCurrentValue(CharacterStats.HealthMax) * percentage.value / 100;
            agent.ApplyModifier(CharacterStats.Health, maxHealth);
            EndAction(true);
        }
    }
}