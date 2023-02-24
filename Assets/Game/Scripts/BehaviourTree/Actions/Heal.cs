using Game.Actors;
using Game.Stats;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace Game.BehaviourTree.Actions
{
    [Category("Interact")]
    public class Heal : ActionTask<ActorController>
    {
        
        protected override void OnExecute()
        {
            var resetModifier = new StatModifier(agent.GetCurrentValue(CharacterStats.HealthMax), StatsModifierType.Override);
            agent.ApplyModifier(CharacterStats.Health, resetModifier);
            EndAction(true);
        }
    }
}