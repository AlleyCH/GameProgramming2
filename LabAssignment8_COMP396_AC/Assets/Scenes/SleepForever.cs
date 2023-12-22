using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;
using UnityEngine;
using Pada1.BBCore.Framework;

[Action("Chapter08/SleepForever")]
[Help("Low-cost infinite action that never ends. It does not consume CPU at all.")]

public class SleepForever : BasePrimitiveAction
{
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.SUSPENDED;
    }

}
