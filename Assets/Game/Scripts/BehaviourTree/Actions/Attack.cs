using Game.Enemies;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace Game.BehaviourTree.Actions
{
    [Category("Interact")]
    public class Attack : ActionTask<EnemyController>
    {
        
        protected override void OnExecute()
        {
            agent.Attack();
            EndAction(true);
        }
    }
}