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

        private float _oldValue;


        protected override string info
            =>
                $"<b>{stat.value.ToStringAdvanced()}</b> changed {OperationTools.GetCompareString(checkType)} {difference}";


        protected override bool OnCheck()
        {
            float currentValue = agent.GetCurrentValue(stat.value);
            float diff = currentValue - _oldValue;
            _oldValue = currentValue;

            return OperationTools.Compare(diff, difference.value, checkType, 0.1f);
        }
    }
}