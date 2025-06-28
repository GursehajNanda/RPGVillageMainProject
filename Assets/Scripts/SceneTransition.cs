using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class SceneTransition : MonoBehaviour
{
    [SerializeField] string m_loadSceneName;

    private void Start()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void LoadScene()
    {
        SceneData.Instance.LoadScene(m_loadSceneName);
    }
}
