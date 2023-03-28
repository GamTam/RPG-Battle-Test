using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChestTrigger : DialogueTrigger
{
    [SerializeField] private sItem[] _rewards;
    private string _ID;
    private string _encryptedID;

    public void Start()
    {
        _ID = $"{SceneManager.GetActiveScene().name} {(int) transform.position.x * (int) transform.position.y * (int) transform.position.z}";

        Debug.Log(_ID);
        if (Globals.OpenedChests.Contains(_ID))
        {
            enabled = false;
        }
    }

    public override void TriggerDialogue()
    {
        if (Globals.OpenedChests.Contains(_ID))
        {
            enabled = false;
            return;
        }
        
        Globals.OpenedChests.Add(_ID);
        string[] dialogue = new string[_rewards.Length];

        for (int i = 0; i < _rewards.Length; i++)
        {
            dialogue[i] = $"Got <color=red>{_rewards[i].Item.Name}</color>! (x{_rewards[i].Count})";

            bool found = false;
            for (int j = 0; j < Globals.Items.Count; j++)
            {
                sItem item = Globals.Items[j];
                if (item.Item == _rewards[i].Item)
                {
                    item.Count += _rewards[i].Count;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Globals.Items.Add(_rewards[i]);
            }
        }
        
        FindObjectOfType<DialogueManager>().StartText(dialogue, gameObject.transform, _spriteRenderer, _skipTextboxCloseAnimation, _forceBottom);
        enabled = false;
    }
}
