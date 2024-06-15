using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScenePauseController : MonoBehaviour
{
    private List<Behaviour> behavioursToPause;
    private List<Rigidbody> rigidbodiesToPause;
    private List<NavMeshAgent> agentsToPause;

    void Awake()
    {
        behavioursToPause = new List<Behaviour>();
        rigidbodiesToPause = new List<Rigidbody>();
    }

    public void PauseScene()
    {
     
        foreach (var behaviour in FindObjectsOfType<MonoBehaviour>())
        {
            if (behaviour.enabled && behaviour.gameObject.activeInHierarchy)
            {
                behavioursToPause.Add(behaviour);
                behaviour.enabled = false;
            }
        }

        
        foreach (var rb in FindObjectsOfType<Rigidbody>())
        {
            if (!rb.isKinematic && rb.gameObject.activeInHierarchy)
            {
                rigidbodiesToPause.Add(rb);
                rb.isKinematic = true;
            }
        }
   
    }

    public void ResumeScene()
    {
        
        foreach (var behaviour in behavioursToPause)
        {
            if (behaviour != null)
            {
                behaviour.enabled = true;
            }
        }
        behavioursToPause.Clear();

        
        foreach (var rb in rigidbodiesToPause)
        {
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }
        rigidbodiesToPause.Clear();
        agentsToPause.Clear();
    }
}
