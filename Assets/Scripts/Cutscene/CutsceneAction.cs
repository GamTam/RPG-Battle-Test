using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class CutsceneAction
{
    [ReadOnly] [AllowNesting] public string Name;
    public bool PlayWithNext;
    [HideInInspector] public Cutscene Parent;

    public virtual IEnumerator Play()
    {
        yield break;
    }
}

[Serializable]
public class DialogueAction : CutsceneAction
{
    [SerializeField] private SpriteRenderer Speaker;
    [SerializeField] private bool SkipCloseAnimation;
    [SerializeField] [TextArea(3, 4)] private string[] Dialogue;

    public DialogueAction()
    {
        Name = "Speech Bubble";
    }

    public override IEnumerator Play()
    {
        DialogueManager manager = Object.FindObjectOfType<DialogueManager>();
        manager.StartText(Dialogue, Speaker.gameObject.transform, Speaker, SkipCloseAnimation);

        while (!manager._done)
        {
            yield return null;
        }
    }
}

[Serializable]
public class MoveObjAction : CutsceneAction
{
    [SerializeField] private GameObject _gameObject;
    [SerializeField] private bool _relative;
    [SerializeField] private int _speed;
    [SerializeField] private Vector2[] MoveList;
    
    public MoveObjAction()
    {
        Name = "Move Object";
    }

    public override IEnumerator Play()
    {
        for (int i = 0; i < MoveList.Length; i++)
        {
            Vector3 targetPos = MoveList[i];
            if (_relative) targetPos += _gameObject.transform.position;
            targetPos.z = _gameObject.transform.position.z;

            while (_gameObject.transform.position != targetPos)
            {
                _gameObject.transform.position = Vector3.MoveTowards(_gameObject.transform.position, targetPos, _speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}

[Serializable]
public class PlayAnimAction : CutsceneAction
{
    [SerializeField] private Animator _animator;
    [SerializeField] [ShowIf("HasChar")] [Dropdown("animationNameList")]  private string _animation;

    public PlayAnimAction()
    {
        Name = "Play Animation";
    }
    
    public override IEnumerator Play()
    {
        _animator.Play(_animation);
        yield return null;
    }
    
    public bool HasChar => _animator != null;
    
    List<string> animationNameList()
    {
        List<string> list = new List<string>();
        
        foreach(AnimationClip ac in _animator.runtimeAnimatorController.animationClips)
        {
            string name = ac.name.ToLower();
            list.Add(name);
        }

        return list;
    }
}

[Serializable]
public class WaitAction : CutsceneAction
{
    [SerializeField] private float _waitTime;
    
    public WaitAction()
    {
        Name = "Wait";
    }
    
    public override IEnumerator Play()
    {
        yield return new WaitForSeconds(_waitTime);
    }
}
