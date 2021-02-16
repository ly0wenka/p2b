using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatNetcode;

public class MrFusion : MonoBehaviour
{
    public bool debugger = false;

    private struct TrackableInterface
    {
        public MainScriptInterface MainScriptInterface;
        public Dictionary<System.Reflection.MemberInfo, System.Object> tracker;
    }

    private Dictionary<long, TrackableInterface[]> gameHistory = new Dictionary<long, TrackableInterface[]>();
    private MainScriptInterface[] MainScriptInterfaces;
    private CombatBehaviour[] combatBehaviours;


    void Start()
    {
        MainScriptInterfaces = GetComponentsInChildren<MainScriptInterface>();
        combatBehaviours = GetComponentsInChildren<CombatBehaviour>();
    }

    public void UpdateBehaviours()
    {
        if (combatBehaviours == null) return;
        foreach (CombatBehaviour combatBehaviour in combatBehaviours)
        {
            combatBehaviour.CombatFixedUpdate();
        }
    }

    public void SaveState(long frame)
    {
        List<TrackableInterface> newTrackableList = new List<TrackableInterface>();
        foreach (MainScriptInterface MainScriptInterface in MainScriptInterfaces)
        {
            TrackableInterface newTrackableInterface;
            newTrackableInterface.MainScriptInterface = MainScriptInterface;
            newTrackableInterface.tracker =
                RecordVar.SaveStateTrackers(MainScriptInterface, new Dictionary<System.Reflection.MemberInfo, object>());
            newTrackableList.Add(newTrackableInterface);
        }

        if (gameHistory.ContainsKey(frame))
        {
            gameHistory[frame] = newTrackableList.ToArray();
        }
        else
        {
            gameHistory.Add(frame, newTrackableList.ToArray());
        }
    }

    public void LoadState(long frame)
    {
        if (gameHistory.ContainsKey(frame))
        {
            TrackableInterface[] loadedInterfaces = gameHistory[frame];
            foreach (TrackableInterface trackableInterface in loadedInterfaces)
            {
                MainScriptInterface reflectionTarget = trackableInterface.MainScriptInterface;
                reflectionTarget =
                    RecordVar.LoadStateTrackers(trackableInterface.MainScriptInterface, trackableInterface.tracker);
                if (reflectionTarget == null && debugger)
                    Debug.LogWarning("Empty interface found at '" + trackableInterface.ToString() + "'");
            }
        }
        else
        {
            Debug.LogError("Frame data not found (" + frame + ")");
        }
    }
}