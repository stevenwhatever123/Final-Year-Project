﻿/*
 * This class is responsible comparing the counter is smaller than the assigned value 
 * Author: Steven Ho
 * Date: 2-2-2021
 * Code version: 1.0
 */
namespace BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables
{
    [TaskCategory("Unity/SharedVariable")]
    [TaskDescription("Returns success if the variable value is equal to the compareTo value.")]
    public class CompareSeenCounter : Conditional
    {
        [Tooltip("The first variable to compare")]
        public SharedFloat variable;
        [Tooltip("The variable to compare to")]
        public SharedFloat compareTo;

        public SharedBool isPlayerDetected;

        public override TaskStatus OnUpdate()
        {
            if (variable.Value < compareTo.Value)
            {
                return TaskStatus.Success;
            }
            else
            {
                isPlayerDetected.Value = true;
                return TaskStatus.Failure;
            }
        }

        public override void OnReset()
        {
            variable = 0;
            compareTo = 0;
        }
    }
}