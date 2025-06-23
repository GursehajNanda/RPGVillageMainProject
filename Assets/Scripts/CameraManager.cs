using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera m_currentVirtualCamera;
    [SerializeField] LayerMask m_clickableLayerMask;
    [SerializeField] int m_cameraHighestPrioirity;
    [SerializeField] private GameObject m_focusObject;

    private Vector3 m_origin;
    private Vector3 m_difference;
    private Camera m_camera;
    private bool m_isDragging;
    private Collider2D m_cameraBound = null;
    private int m_previousCameraPrioirity;
 

    private void Awake()
    {
        m_camera = Camera.main;
        InitialzeCamera();
    }

    public void OnDrag(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            m_origin = GetMousePosition();
            m_isDragging = true;
        }
        else if (ctx.canceled)
        {
            m_isDragging = false;
        }
    }

    public void OnClick(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 worldPos = m_camera.ScreenToWorldPoint(mousePos);

        Collider2D hit = Physics2D.OverlapPoint(worldPos, m_clickableLayerMask);
        CameraTransition transition = hit?.GetComponent<CameraTransition>();
        if (hit != null && transition != null)
        {
               
            SwitchVirtualCameras(transition.VirtualCamera,m_currentVirtualCamera);

            GameObject m_newFocusObject = transition.FocusObject;
            m_focusObject?.SetActive(false);
            m_focusObject = null;

            if (m_newFocusObject != null)
            {     
                m_focusObject = m_newFocusObject;
                m_focusObject.SetActive(true);
            }

        }
    }

    private void LateUpdate()
    {
        if (!m_focusObject  || !m_focusObject.activeInHierarchy) return;
        if (!m_isDragging) return;

        m_difference = GetMousePosition() - m_origin;
        m_focusObject.transform.position -= m_difference;
        m_origin = GetMousePosition();
        ClampWithinBounds();
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = -m_camera.transform.position.z;
        return m_camera.ScreenToWorldPoint(mousePosition);
    }

    private void ClampWithinBounds()
    {
        if (!m_focusObject || !m_focusObject.activeInHierarchy) return;
        if (m_cameraBound == null) return;
     

        Bounds bounds = m_cameraBound.bounds;
        Vector3 newPos = m_focusObject.transform.position;


        newPos.x = Mathf.Clamp(newPos.x, bounds.min.x, bounds.max.x);
        newPos.y = Mathf.Clamp(newPos.y, bounds.min.y, bounds.max.y);

        m_focusObject.transform.position = newPos;
    }

    private void SwitchVirtualCameras(CinemachineVirtualCamera newCamera, CinemachineVirtualCamera previousCamera)
    {
        previousCamera.Priority = m_previousCameraPrioirity;
        m_previousCameraPrioirity = newCamera.Priority;
        newCamera.Priority = m_cameraHighestPrioirity;
        m_currentVirtualCamera = newCamera;

        SetCameraConfiner();

    }

    private void InitialzeCamera()
    {
        m_previousCameraPrioirity = m_currentVirtualCamera.Priority;
        m_currentVirtualCamera.Priority = m_cameraHighestPrioirity;
        SetCameraConfiner();
    }

    private void SetCameraConfiner()
    {
        var confiner = m_currentVirtualCamera?.GetComponent<CinemachineConfiner2D>();
        m_cameraBound = confiner?.m_BoundingShape2D;
    }
}
