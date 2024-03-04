using System.Collections;
using UnityEngine;

public abstract class ContinuousSkill : ScriptableSkill
{
    public LinearFloat duration;
    public LinearFloat actionInterval;

    public override void Apply(Entity caster, int skillLevel, Vector2 direction)
    {
        caster.StartCoroutine(CSkillWork(caster, skillLevel, direction));
    }

    private IEnumerator CSkillWork(Entity caster, int skillLevel, Vector2 direction)
    {
        float time = 0f;
        float duration = this.duration.Get(skillLevel);
        float interval = Mathf.Clamp(this.actionInterval.Get(skillLevel), 0f, float.MaxValue);
        
        if (interval == 0f)
        {
            while (time < duration)
            {
                yield return null;
                time += Time.deltaTime;
                Perform();
            }
        }
        else
        {
            while (time < duration)
            {
                WaitForSeconds wait = new WaitForSeconds(interval);
                yield return wait;
                time += interval;
                Perform();
            }
        }

        void Perform()
        {
            PerformContinuousAction(caster, skillLevel, direction, duration, time);
        }
    }

    protected abstract void PerformContinuousAction(Entity caster, int skillLelel, Vector2 direction, float duration, float time);

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