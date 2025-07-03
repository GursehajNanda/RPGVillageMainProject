using UnityEngine;
using KambojGames.Utilities2D;

public class Player :Character
{
    [SerializeField] private LayerMask m_interactableObjectLayer;
    [SerializeField] private float m_interactRadius = 1.0f;
    [SerializeField] private bool m_drawInteractRange;
    void OnEnable()
    {
        InputReader.Instance.SubmitButtonPressedEvent += Interact;
    }

    private void OnDisable()
    {
        InputReader.Instance.SubmitButtonPressedEvent -= Interact;
    }

    private void Interact()
    {

        Vector2 checkPosition = transform.position;

        Collider2D obj = Physics2D.OverlapCircle(checkPosition, 0.1f, m_interactableObjectLayer);

        if (obj != null)
        {
            if (PhysicsFunctionLibrary.Instance.CheckLOS(obj.gameObject.transform.position, transform.position, MoveVector))
            {
                obj.GetComponent<IInteractable>()?.Interact(gameObject);
            }


        }
    }

    private void OnDrawGizmos()
    {
        if (m_drawInteractRange)
        {
            Vector2 checkPosition = transform.position;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(checkPosition, m_interactRadius);


            if (MoveVector != Vector2.zero)
            {
                Gizmos.color = Color.red;
                Vector2 forwardEndPoint = checkPosition + (MoveVector.normalized * m_interactRadius);
                Gizmos.DrawLine(checkPosition, forwardEndPoint);
            }
        }
    }
}
