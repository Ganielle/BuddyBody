using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadObjects : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
