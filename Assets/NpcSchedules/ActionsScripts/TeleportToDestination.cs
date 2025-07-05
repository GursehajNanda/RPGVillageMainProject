using UnityEngine;
using KambojGames.Utilities2D;

public class TeleportToDestination : NpcAction
{
    [SerializeField] Vector3 m_teleportDestination;
    [SerializeField] string m_sortingLayerName;
    [SerializeField] int m_sortingLayerOrder;
    [SerializeField] Vector2 m_moveVectorAfterTeleportation;
    [SerializeField] bool m_completeActionOnTeleportation;
    private SpriteRenderer m_sprite;
    private bool m_teleported;

    public override void Initialize(Rigidbody2D rb, NpcTaskMediatorSO mediator)
    {
        base.Initialize(rb, mediator);
        rb.transform.position = m_teleportDestination;
        Character character = rb?.GetComponent<Character>();
        m_sprite = Rb?.GetComponentInChildren<SpriteRenderer>();
        m_teleported = false;

        if (character)
        {
            character.MoveVector = m_moveVectorAfterTeleportation;
        }
    }

    public override void DoAction()
    {
        base.DoAction();
        if (m_teleported) return;
        m_sprite.sortingLayerName = m_sortingLayerName;
        m_sprite.sortingOrder = m_sortingLayerOrder;
        m_teleported = true;
        if(m_completeActionOnTeleportation)
        {
            IsActionCompleted = true;
        }

    }
}
