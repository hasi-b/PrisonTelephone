using System;
using System.Collections.Generic;
using UnityEngine;

public class AllCalls : MonoBehaviour
{
   public CallDetails callDetails;
}

[Serializable]
public class CallDetails 
{
   public AudioClip clip;

   public float timeBeforeNextCall;
   
   public int group;
    [HideInInspector]
   public bool isCalldone;

    public List <int> lista = new();
}

