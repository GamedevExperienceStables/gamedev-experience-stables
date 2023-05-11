using Game.Actors;
using Game.Stats;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace Game.BehaviourTree.Conditions
{
    [Category("System Events")]
    public class StatChanged : ConditionTask<IActorController>
    {
        public BBParameter<CharacterStats> stat = CharacterStats.Health;

        public CompareMethod checkType = CompareMethod.GreaterOrEqualTo;

        public BBParameter<float> difference = 0f;
        
        [SliderField(0, 0.1f)]
        public float floatingPoint = 0.1f;

        protected override string info =>
            $"<b>{stat.value.ToStringAdvanced()}</b> changes {OperationTools.GetCompareString(checkType)} {difference}";

        protected override void OnEnable()
            => agent.SubscribeStatChanged(stat.value, OnStatChanged);
        
        protected override void OnDisable()
            => agent.UnSubscribeStatChanged(stat.value, OnStatChanged);

        protected override bool OnCheck()
            => false;

        private void OnStatChanged(StatValueChange change)
        {
            float diff = change.newValue - change.oldValue;
            bool result = OperationTools.Compare(diff, difference.value, checkType, floatingPoint);
            
            if (result)
                YieldReturn(true);
        }
    }
}