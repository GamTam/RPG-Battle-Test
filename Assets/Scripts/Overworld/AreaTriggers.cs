using System;
using UnityEngine;

public class AreaTriggers : MonoBehaviour
{
    public GameObject virtualCam;

    //Used to disable spawning in the tutorial area
    public GameObject _enemySpawner;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !(other.isTrigger))
        {
            virtualCam.SetActive(true);
            if(_enemySpawner != null)
            {
                _enemySpawner.SetActive(false);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !(other.isTrigger))
        {
            virtualCam.SetActive(false);
            if(_enemySpawner != null)
            {
                _enemySpawner.SetActive(true);
            }
        }
    }
}
