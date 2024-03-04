using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "uMMORPG Skill/Blink", order = 999)]
public class BlinkSkill : ScriptableSkill
{
    static RaycastHit2D[] hitsBuffer = new RaycastHit2D[2];

    public override void Apply(Entity caster, int skillLevel, Vector2 direction)
    {
        Vector2 targetPosition;

        // ���� �������, ����� ������ �� ��������� warp ��-�� ������� ��������� � �����
        const float warpFix = 0.5f;

        // ����� ���� ��� ��������, ����� �������������� ��������, �� ����� � �������� ������ �� ����
        float range = base.castRange.Get(skillLevel);
        int hitsCount = Physics2D.RaycastNonAlloc(caster.transform.position, direction, hitsBuffer, range);
        
        if (hitsCount > 0)
        {
            float distanceToHit = Vector2.Distance(caster.transform.position, hitsBuffer[1].point);
            targetPosition = (Vector2)caster.transform.position + direction * (distanceToHit - warpFix);
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