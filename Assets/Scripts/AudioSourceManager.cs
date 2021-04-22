using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(this);
    }
}
