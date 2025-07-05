using UnityEngine;
using KambojGames.Pathfinding2D;
using KambojGames.Utilities2D.Attributes;

public class GoToDestination : NpcAction
{
    [Tooltip("Starting position for npc, if any")]
    [SerializeField] float m_npcMoveSpeed = 1.0f;

    [Header("PathFinding")]
    [SerializeField] bool m_usePathfinding = true;
    [SerializeField][ConditionalField("m_usePathfinding")] Vector2 m_startingPosition;
    [SerializeField][ConditionalField("m_usePathfinding")] Vector2 m_destinationPosition;
    [SerializeField][ConditionalField("m_usePathfinding")]float m_smothingfactor = 2.5f;
    [SerializeField][ConditionalField("m_usePathfinding")]float m_distBetweenWayPoints = 0.4f;
    [SerializeField][ConditionalField("m_usePathfinding")]float m_pathUpdateInterval = 0.5f;
 

    [Header("Game Event")]
    [SerializeField]GameEvent m_actionToDoWhenReachedDestination;

    private AIPathfinding2D m_pathFinding;
    NpcBehaviourControl m_controller;
    NpcFollowSpline m_followSpline;

 

    public override void Initialize(Rigidbody2D rb, NpcTaskMediatorSO mediator)
    {
        base.Initialize(rb, mediator);
        if(m_startingPosition != null)
        {
            Rb.transform.position = m_startingPosition;
        }

        m_pathFinding = null;
        m_followSpline = null;
        m_controller = rb.GetComponent<NpcBehaviourControl>();

        if (m_usePathfinding)
        {
            m_pathFinding = new AIPathfinding2D();
            m_pathFinding.InitializePath(Rb, m_destinationPosition, m_smothingfactor, m_distBetweenWayPoints, m_pathUpdateInterval);   
        }
        else
        {
            m_followSpline = new();
            m_followSpline.Initialize(Rb, m_controller.GetSplineActionPath(ActionID),m_npcMoveSpeed);
        }

    }
    public override void DoAction()
    {
        base.DoAction();

        if (!IsActionCompleted)
        {
            NpcSpeedModifier();

            bool canMove = CharComp.IsPassable() && !DI.IsInteractingWithDialogue();
            Vector2 moveVec = Rb.velocity.normalized;
            if(moveVec != Vector2.zero)
            {
                CharComp.MoveVector = moveVec;
            }

            if (canMove && Rb.velocity != Vector2.zero)
            {
                ActionMovingAnimationState(CharComp.MoveVector);
            }
            else
            {
                ActionIdleAnimationState(CharComp.MoveVector);
            }

            if (m_usePathfinding && m_pathFinding != null)
            {
                if (m_pathFinding.isPathCompleted())
                {
                    EndAction();
                }
                else if (canMove)
                {
                    m_pathFinding.MoveToPath(m_destinationPosition, m_npcMoveSpeed);

                }
            }
            else if (!m_usePathfinding && m_followSpline != null)
            {
                if (m_followSpline.IsPathCompleted())
                {
                    EndAction();
                }
                else if (canMove)
                {
                    m_followSpline.Update();
                }
            }
        }
        else
        {
            EndAction();
        }

    }

    //For testing
    private void NpcSpeedModifier()
    {
        float baseNpcSpeed = 1;
        float baseDayLengthInSeconds = 24f * 60f;

        float currentDayLengthInSecs = ClimateData.Instance.MinutesToLastADay * 60;

        m_npcMoveSpeed = baseNpcSpeed * (baseDayLengthInSeconds / currentDayLengthInSecs);
    }

    private void EndAction()
    {
        ActionIdleAnimationState(CharComp.MoveVector);
        DI.ClearDialogueObject();
        Rb.velocity = Vector2.zero;
        m_pathFinding = null;
        m_followSpline = null;
        m_controller = null;
        IsActionCompleted = true;
        m_actionToDoWhenReachedDestination?.Raise();
    }
}
