using Game.Enemies;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace Game.BehaviourTree.Actions
{
    [Category("Interact")]
    public class Attack : ActionTask<EnemyController>
    {
        public BBParameter<float> attackRange;
        public BBParameter<float> meleeAttackRange;
        
        protected override void OnExecute()
        {
            if (attackRange.value <= meleeAttackRange.value) 
                agent.MeleeAttack();
            
            if (attackRange.value > meleeAttackRange.value) 
                agent.RangeAttack();
            
            EndAction(true);
        }
    }
}