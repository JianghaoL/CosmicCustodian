using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InitializationEntry
{
    public MonoBehaviour script;
    public int order;
    [TextArea(2, 10)] public string note;
}

public class InitializationManager : MonoBehaviour
{
    [SerializeField]
    private List<InitializationEntry> initializationEntries;

    private void Start()
    {
        // Initialize all Awake() functions in Start().
        // Start() gives better order control as Awake() does not
        // Guarantee the order of execution.
        
        SortEntries();
        for (int i = 0; i < initializationEntries.Count; i++)
        {
            var target = initializationEntries[i].script;

            if (!target)
            {
                Debug.LogError($"Initialization entry {i} failed to initialize.");
                continue;
            }

            if (target is not IInitializable initializable)
            {
                Debug.LogError($"Initialization entry {i} (Script: {initializationEntries[i].script}) does not implement IInitializable.");
                continue;
            }
            
            initializable.InitializeOnAwake();
            StartCoroutine(InitializeOnStart(initializable));
        }
    }

    private IEnumerator InitializeOnStart(IInitializable initializable)
    {
        yield return new WaitForSecondsRealtime(0.01f);
        initializable.InitializeOnStart();
    }

    private void SortEntries()
    {
        initializationEntries.Sort(CompareOrder);

        int CompareOrder(InitializationEntry x, InitializationEntry y)
        {
            if (x.order < y.order)
                return -1;
            if (x.order > y.order)
                return 1;
            return 0;
        }
    }
}
