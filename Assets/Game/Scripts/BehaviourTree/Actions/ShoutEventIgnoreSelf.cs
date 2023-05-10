using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace Game.BehaviourTree.Actions
{

    [Category("✫ Utility")]
    [Name("Shout Event (Ignore Self)")]
    [Description("Sends an event to all GraphOwners within range of the agent and over time like a shockwave.")]
    public class ShoutEventIgnoreSelf : ActionTask<Transform>
    {

        [RequiredField]
        public BBParameter<string> eventName;
        public BBParameter<float> shoutRange = 10;
        public BBParameter<float> completionTime = 1;

        private GraphOwner[] owners;
        private bool[] receivedOwners;
        private float traveledDistance;

        protected override string info => $"Shout Event [{eventName}]";

        protected override void OnExecute() {
            owners = Object.FindObjectsOfType<GraphOwner>();
            receivedOwners = new bool[owners.Length];
        }

        protected override void OnUpdate() {
            traveledDistance = Mathf.Lerp(0, shoutRange.value, elapsedTime / completionTime.value);
            for ( int i = 0; i < owners.Length; i++ ) {
                GraphOwner owner = owners[i];
                if (owner.transform == agent)
                    continue;
                
                float distance = ( agent.position - owner.transform.position ).magnitude;
                if (distance > traveledDistance || receivedOwners[i]) 
                    continue;
                
                owner.SendEvent(eventName.value, null, this);
                receivedOwners[i] = true;
            }

            if ( elapsedTime >= completionTime.value ) {
                EndAction();
            }
        }

        public override void OnDrawGizmosSelected() {
            if ( agent != null ) {
                Gizmos.color = new Color(1, 1, 1, 0.2f);
                Gizmos.DrawWireSphere(agent.position, traveledDistance);
                Gizmos.DrawWireSphere(agent.position, shoutRange.value);
            }
        }
    }
}