using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Generates a dictionary from a ScriptableObject, easier to manage from inspector

[CreateAssetMenu]
public class NumberAndWordScriptableObject : ScriptableObject
{
    [SerializeField] protected int[] keys;
    [SerializeField] protected string[] values;

    public Dictionary<int, string> dictionary;

    private void OnEnable()
    {
        dictionary = new Dictionary<int, string>(keys.Length);

        for(int i = 0; i < keys.Length; i++)
        {
            dictionary.Add(keys[i], values[i]);
        }
    }
}
