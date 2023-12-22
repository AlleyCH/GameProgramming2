using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;
using UnityEngine;

[Action("Chapter08/Shoot")]
[Help("Periodically clones a 'bullet' and shoots it through the Forward axis " +
      "with the specified velocity. This action never ends.")]

public class Shoot : ShootOnce
{

    [InParam("delay", DefaultValue = 30)]
    public int delay;

    // Game loops since the last shoot.
    private int elapsed = 0;

    public override TaskStatus OnUpdate()
    {
        if (delay > 0)
        {
            ++elapsed;
            elapsed %= delay;
            if (elapsed != 0)
                return TaskStatus.RUNNING;
        }

        base.OnUpdate();
        return TaskStatus.RUNNING;
    }
}
