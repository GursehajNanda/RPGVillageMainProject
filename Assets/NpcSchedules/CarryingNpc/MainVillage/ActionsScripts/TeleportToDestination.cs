using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToDestination : ComplexNpcAction
{
    [SerializeField] Vector3 m_teleportDestination;
    [SerializeField] string m_sortingLayerName;
    [SerializeField] int m_sortingLayerOrder;

    public override void DoAction()
    {
        if (Rb.transform.position != m_teleportDestination) return;
        base.DoAction();
        Rb.transform.position = m_teleportDestination;
        SpriteRenderer sprite = Rb?.GetComponentInChildren<SpriteRenderer>();
        if(sprite)
        {
            sprite.sortingLayerName = m_sortingLayerName;
            sprite.sortingOrder = m_sortingLayerOrder;
        }
     
    }
}
