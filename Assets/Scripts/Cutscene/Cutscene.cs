using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    [SerializeReference] [SerializeField] private List<CutsceneAction> CutsceneActions;
    [SerializeField] private bool _triggerMoreThanOnce;
    [SerializeField] [HideIf("_triggerMoreThanOnce")] [AllowNesting] private string _cutsceneName;

    private bool _triggered;

    private void Start()
    {
        if (Globals.PlayedCutscenes.Contains(_cutsceneName) && !_triggerMoreThanOnce) _triggered = true;
    }

    public IEnumerator PlayCutscene()
    {
        Globals.GameState = GameState.Cutscene;
        foreach (CutsceneAction action in CutsceneActions)
        {
            if (action.PlayWithNext) StartCoroutine(action.Play());
            else yield return action.Play();
        }

        Globals.PlayedCutscenes.Add(_cutsceneName);
        Globals.GameState = GameState.Play;
    }
    
    public void AddAction(CutsceneAction action)
    {
        action.Parent = this;
        CutsceneActions.Add(action);
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if (!_triggered || _triggerMoreThanOnce)
            {
                _triggered = true;
                StartCoroutine(PlayCutscene());
            }
        }
    }
}