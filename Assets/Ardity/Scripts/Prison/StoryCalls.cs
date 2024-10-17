using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StoryCalls
{
    public List<AudioClip> clip;

    public List<Dialogues> talks;

    public int afterThisCall;

    public int minsToPassIfAnswerd;

    public bool IsSad;
    public bool IsHappy;
}