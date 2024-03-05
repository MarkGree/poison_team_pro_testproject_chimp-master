using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "uMMORPG Skill/Blink", order = 999)]
public class BlinkSkill : ScriptableSkill
{
    public override void Apply(Entity caster, int skillLevel, Vector2 direction)
    {
        Vector2 targetPosition;

        float range = base.castRange.Get(skillLevel);
        if (NavMesh2D.Raycast(caster.transform.position, (Vector2)caster.transform.position + direction * range, out NavMeshHit2D hit, NavMesh2D.AllAreas))
        {
            targetPosition = hit.position;
        }
        else
            targetPosition = (Vector2)caster.transform.position + direction * range;

        caster.movement.Warp(targetPosition);
    }

    public override bool CheckDistance(Entity caster, int skillLevel, out Vector2 destination)
    {
        destination = caster.transform.position + (Vector3)caster.lookDirection;
        return true;
    }

    public override bool CheckTarget(Entity caster)
    {
        caster.target = caster;
        return true;
    }

}