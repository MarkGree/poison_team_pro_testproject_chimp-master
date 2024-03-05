using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "uMMORPG Skill/Blink", order = 999)]
public class BlinkSkill : ScriptableSkill
{
    public override void Apply(Entity caster, int skillLevel, Vector2 lookDirection)
    {
        Vector2 blinkDirection;
        Vector2 destination;

        float range = base.castRange.Get(skillLevel);
        Vector2 moveVelocity = caster.movement.GetVelocity();
        float moveSpeed = moveVelocity.magnitude;

        if (moveSpeed == 0f)
            blinkDirection = lookDirection;
        else
            blinkDirection = moveVelocity.normalized;
        
        destination = (Vector2)caster.transform.position + blinkDirection * range;


        if (NavMesh2D.Raycast(caster.transform.position, destination, out NavMeshHit2D hit, NavMesh2D.AllAreas))
        {
            destination = hit.position;
        }

        caster.movement.Warp(destination);
    }

    public override bool CheckDistance(Entity caster, int skillLevel, out Vector2 destination)
    {
        destination = (Vector2)caster.transform.position + caster.movement.GetVelocity().normalized;
        return true;
    }

    public override bool CheckTarget(Entity caster)
    {
        caster.target = caster;
        return true;
    }

}