using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    [SerializeReference] [SerializeField] private List<CutsceneAction> CutsceneActions;
    [SerializeField] private bool _triggerMoreThanOnce;

    private bool _triggered;

    public IEnumerator PlayCutscene()
    {
        Globals.GameState = GameState.Cutscene;
        foreach (CutsceneAction action in CutsceneActions)
        {
            if (action.PlayWithNext) StartCoroutine(action.Play());
            else yield return action.Play();
        }

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