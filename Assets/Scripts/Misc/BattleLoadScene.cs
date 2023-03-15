using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class BattleLoadScene : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _whiteFlash;
    [SerializeField] private Volume _monochromeVolume;
    [SerializeField] private GameObject _fieldEffects;
    
    public IEnumerator BattleTransition(string scene = "Battle Scene")
    {
        float movementDuration = 0.1f;
        float timeElapsed = 0;
     
        Globals.MusicManager.Stop();
        Globals.SoundManager.Play("battleStart");
        
        while (timeElapsed < movementDuration)
        {
            timeElapsed += Time.deltaTime;
           _whiteFlash.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, timeElapsed / movementDuration));
            yield return null;
        }

        _whiteFlash.color = new Color(1, 1, 1, 1);
        _fieldEffects.SetActive(false);

        _monochromeVolume.weight = 1;
        
        movementDuration = 2f;
        timeElapsed = 0;
            
        while (timeElapsed < movementDuration)
        {
            timeElapsed += Time.deltaTime;
            _whiteFlash.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, timeElapsed / movementDuration));
            yield return null;
        }
        _whiteFlash.color = new Color(1, 1, 1, 0);
        
        StartCoroutine(Globals.LoadScene("Battle Scene", true));
        yield return new WaitForSeconds(0.5f);
        Globals.BeginSceneLoad = true;
    }
}
