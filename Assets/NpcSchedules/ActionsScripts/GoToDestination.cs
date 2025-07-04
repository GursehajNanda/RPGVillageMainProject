using UnityEngine;
using KambojGames.Pathfinding2D;
using KambojGames.DialogueSystem;

public class GoToDestination : NpcAction
{
    [Tooltip("Starting position for npc, if any")]
    [SerializeField] Vector2 m_startingPosition;
    [SerializeField] Vector2 m_destinationPosition;

    [Header("PathFinding")]
    [SerializeField] float m_npcMoveSpeed = 1.0f;
    [SerializeField] float m_smothingfactor = 2.5f;
    [SerializeField] float m_distBetweenWayPoints = 0.4f;
    [SerializeField] float m_pathUpdateInterval = 0.5f;

    [Header("Dialogues")]
    [SerializeField] private ConditionalDialogue m_actionDialogue;

    [Header("Game Event")]
    [SerializeField]GameEvent m_actionToDoWhenReachedDestination;

    private AIPathfinding2D m_pathFinding;
    private DialogueInteraction m_dI;
    NpcBehaviourControl m_controller;

    public override void Initialize(Rigidbody2D rb, NpcTaskMediatorSO mediator)
    {
        base.Initialize(rb, mediator);
        if(m_startingPosition != null)
        {
            Rb.transform.position = m_startingPosition;
        }

        m_pathFinding = new AIPathfinding2D();
        m_pathFinding.InitializePath(Rb, m_destinationPosition, m_smothingfactor, m_distBetweenWayPoints, m_pathUpdateInterval);
        m_controller = rb.GetComponent<NpcBehaviourControl>();
        m_dI = rb.GetComponentInChildren<DialogueInteraction>();
        m_dI.Initialzie();
        m_dI.AddDialogueObject(m_actionDialogue.GetDialogue());
    }
    public override void DoAction()
    {
        base.DoAction();

        if (m_pathFinding != null)
        {
            if (m_pathFinding.isPathCompleted())
            {
                m_dI.ClearDialogueObject();
                Rb.velocity = Vector2.zero;
                m_pathFinding = null;
                IsActionCompleted = true;
                m_dI = null;
                m_controller = null;
                m_actionToDoWhenReachedDestination?.Raise();
               
            }
            else
            {
                m_pathFinding.MoveToPath(m_destinationPosition, m_npcMoveSpeed);
            }
        }
     
       
        
    }
}
