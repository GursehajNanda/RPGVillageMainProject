using UnityEngine;
using KambojGames.Utilities2D;

public class GoToDestination : NpcAction
{
    [Tooltip("Starting position for npc, if any")]
    [SerializeField] Vector2 m_startingPosition;
    [Header("Game Event")]
    [SerializeField]GameEvent m_actionToDoWhenReachedDestination;

    public override void Initialize(Rigidbody2D rb, NpcTaskMediatorSO mediator)
    {
        base.Initialize(rb, mediator);
        if(m_startingPosition != null)
        {
            Rb.transform.position = m_startingPosition;
        }
    }
    public override void DoAction()
    {
        base.DoAction();
        m_actionToDoWhenReachedDestination?.Raise();
        IsActionCompleted = true;
    }
}
