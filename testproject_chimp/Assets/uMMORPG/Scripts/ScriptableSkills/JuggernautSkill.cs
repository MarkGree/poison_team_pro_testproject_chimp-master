using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "uMMORPG Skill/Juggernaut", order = 999)]
public class JuggernautSkill : ContinuousSkill
{
    public LinearInt damageTotal;

    static Collider2D[] hitsBuffer = new Collider2D[1000];
    static HashSet<Entity> candidatesBuffer = new HashSet<Entity>(1000);

    protected override void PerformContinuousAction(Entity caster, int skillLevel, Vector2 direction, int duration, int time)
    {
        candidatesBuffer.Clear();

        var contactFilter = new ContactFilter2D().NoFilter();
        int hits = Physics2D.OverlapCircle(caster.transform.position, base.castRange.Get(skillLevel), contactFilter, hitsBuffer);
        for (int i = 0; i < hits; i++)
        {
            Collider2D co = hitsBuffer[i];
            Entity candidate = co.GetComponentInParent<Entity>();
            if (candidate != null &&
                candidate != caster &&
                candidate.GetType() != caster.GetType() &&
                candidate.health.current > 0)
            {
                candidatesBuffer.Add(candidate);
            }
        }

        int totalDamage = this.damageTotal.Get(skillLevel);
        int damage = totalDamage / duration;
        
        if (duration == time)
            damage += totalDamage % duration;

        foreach (Entity candidate in candidatesBuffer)
        {
            caster.combat.DealDamageAt(candidate, damage);
        }
    }
}