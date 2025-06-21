using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class IndoorTransition:MonoBehaviour 
{
    [SerializeField] private Transform m_EntryExit;
    [SerializeField] private Vector2 m_poschangeOnEnterExit;
    [SerializeField] private bool m_isIndoorTransition;
    [SerializeField] CinemachineConfiner2D m_confiner;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            if (m_confiner.isActiveAndEnabled && m_isIndoorTransition)
            {
                m_confiner.enabled = false;
            }
            else if(!m_confiner.isActiveAndEnabled && !m_isIndoorTransition)
            {
                m_confiner.enabled = true;
            }

            Vector3 newPos = m_EntryExit.position + new Vector3(m_poschangeOnEnterExit.x, m_poschangeOnEnterExit.y, 0);
            col.gameObject.transform.position = newPos;
        }
    }

}

