namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
    [TaskCategory("Unity/SharedVariable")]
    [TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
    public class CompareTogetherCounterLarger : Conditional
    {
        [Tooltip("The first variable to compare")]
        public SharedFloat variable;
        [Tooltip("The variable to compare to")]
        public SharedFloat compareTo;

        public override TaskStatus OnUpdate()
        {
            if (variable.Value > compareTo.Value)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }

        public override void OnReset()
        {
            variable = 0;
            compareTo = 0;
        }
    }
}