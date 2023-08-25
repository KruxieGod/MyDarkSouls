using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalEventManager 
{
    public static UnityEvent OnBossPhases = new UnityEvent();
    public static UnityEvent OnBossFightEventBegin = new UnityEvent();
    public static UnityEvent OnBossFightEventEnd = new UnityEvent();

    public static void SendBossFightEvent(bool isStart)
    {
        Debug.Log("SendBossFightEvent : " + isStart);
        if (isStart)
            OnBossFightEventBegin.Invoke();
        else
            OnBossFightEventEnd.Invoke();   
    }

    public static void SendBossPhasesEvent()
    {
        OnBossPhases.Invoke();
    }
}
