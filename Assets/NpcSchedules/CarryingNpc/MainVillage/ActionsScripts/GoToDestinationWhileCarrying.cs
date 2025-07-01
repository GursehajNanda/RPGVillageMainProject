using UnityEngine;
using KambojGames.Utilities2D;

public class GoToDestinationWhileCarrying : ComplexNpcAction
{
    [Header("Game Event")]
    [SerializeField]GameEvent m_actionToDoWhenReachedDestination;

    public override void DoAction()
    {
        base.DoAction();
        m_actionToDoWhenReachedDestination?.Raise();
    }
}
