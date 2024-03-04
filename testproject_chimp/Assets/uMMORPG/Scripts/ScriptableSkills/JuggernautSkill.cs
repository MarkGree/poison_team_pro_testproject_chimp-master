using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "uMMORPG Skill/Juggernaut", order = 999)]
public class JuggernautSkill : ContinuousSkill
{
    public LinearInt damage;
    static Collider2D[] hitsBuffer = new Collider2D[1000];
    static HashSet<Entity> candidatesBuffer = new HashSet<Entity>(1000);

#if UNITY_EDITOR
    public float dbg_TotalBasicDamage;
    private void OnValidate()
    {
        try
        {
            dbg_TotalBasicDamage = base.duration.Get(0) / base.actionInterval.Get(0) * damage.Get(0);
        }
        catch (DivideByZeroException)
        {
        }
    }
#endif

    protected override void PerformContinuousAction(Entity caster, int skillLevel, Vector2 direction, float duration, float time)
    {
        candidatesBuffer.Clear();

        int hits = Physics2D.OverlapCircleNonAlloc(caster.transform.position, base.castRange.Get(skillLevel), hitsBuffer);
        for (int i = 0; i < hits; i++)
        {
            Collider2D co = hitsBuffer[i];
            Entity candidate = co.GetComponentInParent<Entity>();
            if (candidate != null &&
                candidate != caster &&
                candidate.health.current > 0)
            {
                candidatesBuffer.Add(candidate);
            }
        }

        int damage = this.damage.Get(skillLevel);
        foreach (Entity candidate in candidatesBuffer)
        {
            caster.combat.DealDamageAt(candidate, damage);
        }
    }
}