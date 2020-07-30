using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Manages the state of the level </summary>
public class LevelManager : MonoBehaviour
{
    public int Score { get; private set; }

    GameObject player;
    MainCharacter mainCharacter;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        mainCharacter = player.GetComponent<MainCharacter>();
    }

    public void IncrementScore()
    {
        Score++;
    }

    public void Reset()
    {
        Score = 0;
        player.transform.position = mainCharacter.initialTransform.position;
        player.transform.rotation = mainCharacter.initialTransform.rotation;
    }
}
