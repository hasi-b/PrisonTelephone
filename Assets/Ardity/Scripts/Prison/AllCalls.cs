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

   public List<int> group;
    [HideInInspector]
   public bool IsAssignedToGroup = false;

    public int minsToPassIfAnswerd;
}

