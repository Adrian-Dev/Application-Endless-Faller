using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField] Text textCurrentScore;

    LevelManager levelManager;
    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    void Update()
    {
        textCurrentScore.text = levelManager.Score.ToString(); //TODO think of a more efficient way of doing this instead of using Update. Maybe delegates in LevelManager?
    }
}
