using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeCollider : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D m_collider1;
    [SerializeField]
    private BoxCollider2D m_collider2;
    Vector3 offset;
    private BoxCollider2D m_mainCollider;
    [SerializeField]
    private LayerMask m_targetLayerMask;

    private void Start()
    {
        m_mainCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if ((m_targetLayerMask.value & (1 << collision.gameObject.layer)) != 0)
        {

            Debug.Log("1");

            Vector3 playerPosition = collision.transform.position;
            Vector3 colliderPosition = transform.position;

            if(playerPosition.x > colliderPosition.x)
            {
                offset = new Vector2( playerPosition.x - (colliderPosition.x - m_mainCollider.size.x), playerPosition.y - colliderPosition.y);
            }
            else
            {
                offset = new Vector2(playerPosition.x - (colliderPosition.x + m_mainCollider.size.x), playerPosition.y - colliderPosition.y);
            }
            



            if (Mathf.Abs(offset.x) < Mathf.Abs(offset.y))
            {

                Debug.Log("2");

                SpriteRenderer renderer = collision?.GetComponentInChildren<SpriteRenderer>();

                if (renderer)
                {
                    Debug.Log("3");
                    renderer.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "AbovePlayer";
                    renderer.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    m_collider1.enabled = true;
                    m_collider2.enabled = true;
                }
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if ((m_targetLayerMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            Vector3 playerPosition = collision.transform.position;
            Vector3 colliderPosition = transform.position;
            Vector3 offset = playerPosition - colliderPosition;


            if (Mathf.Abs(offset.x) < Mathf.Abs(offset.y))
            {
                
                SpriteRenderer renderer = collision?.GetComponentInChildren<SpriteRenderer>();
                if (renderer)
                {
                    renderer.sortingLayerName = "Player";
                    renderer.sortingOrder = 0;
                    m_collider1.enabled = false;
                    m_collider2.enabled = false;
                }
            }

        }
    }





}
