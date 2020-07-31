using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    //TODO use a class for score and highscore
    [SerializeField] Text textCurrentScore;

    LevelController levelController;
    private void Awake()
    {
        levelController = FindObjectOfType<LevelController>();
    }

    void Update()
    {
        textCurrentScore.text = levelController.score.ToString(); //TODO think of a more efficient way of doing this instead of using Update. Maybe delegates in LevelManager?
    }
}
