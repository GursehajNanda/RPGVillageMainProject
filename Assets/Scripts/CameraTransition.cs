using UnityEngine;
using Cinemachine;
using KambojGames.Utilities2D.Attributes;

[RequireComponent(typeof(BoxCollider2D))]
public class CameraTransition : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera m_virtualCamera;
    [SerializeField] bool m_haveFocusObject;
    [SerializeField] [ConditionalField("m_haveFocusObject")] GameObject m_focusObject = null;
    public CinemachineVirtualCamera VirtualCamera  => m_virtualCamera;
    public GameObject FocusObject => m_focusObject;

    void Start()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

  
}
