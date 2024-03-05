using System.Collections;
using System.Data.OleDb;
using UnityEngine;

public abstract class ContinuousSkill : ScriptableSkill
{
    public LinearInt duration;
    
    public override void Apply(Entity caster, int skillLevel, Vector2 direction)
    {
        caster.StartCoroutine(CSkillWork(caster, skillLevel, direction));
    }

    private IEnumerator CSkillWork(Entity caster, int skillLevel, Vector2 direction)
    {
        int time;
        int duration = this.duration.Get(skillLevel);

        for (int i = 0; i < duration; ++i)
        {
            WaitForSeconds wait = new WaitForSeconds(1f);
            yield return wait;
            time = i + 1;
            Perform();
        }


        void Perform()
        {
            PerformContinuousAction(caster, skillLevel, direction, duration, time);
        }
    }

    protected abstract void PerformContinuousAction(Entity caster, int skillLelel, Vector2 direction, int duration, int time);

    public override bool CheckDistance(Entity caster, int skillLevel, out Vector2 destination)
    {
        destination = caster.transform.position;
        return true;
    }

    public override bool CheckTarget(Entity caster)
    {
        caster.target = caster;
        return true;
    }
}