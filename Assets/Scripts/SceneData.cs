using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneData", menuName = "ScriptableObject/SceneData")]
public class SceneData : ScriptableObject
{

    private static SceneData m_instance;
   

    public static SceneData Instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = Resources.Load<SceneData>("SceneData");
                if (m_instance == null)
                {
                    Debug.LogError("SceneData instance not found in Resources.");
                }
            }
            return m_instance;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }



    public void LoadScene(string sceneName)
    {
        HandleDataBeforeLoadScene();
        SceneManager.LoadScene(sceneName);
    }

    private void HandleDataBeforeLoadScene()
    {
        GameObject orbitCenter = GameObject.FindGameObjectWithTag("OrbitCenter");
        if (orbitCenter != null)
        {
            orbitCenter.transform.parent = null;
            Destroy(orbitCenter);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DayNightCycleController.OnSceneLoaded?.Invoke(scene, mode);
    }

    void OnSceneUnloaded(Scene scene)
    {
        DayNightCycleController.OnSceneUnloaded?.Invoke(scene);
    }



}

