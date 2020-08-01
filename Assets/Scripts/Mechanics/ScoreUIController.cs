using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles current UI elements
/// </summary>
public class ScoreUIController : MonoBehaviour
{
    [Tooltip("Reference to players score UI element")]
    [SerializeField] Text textCurrentScore;

    LevelController levelController;
    private void Awake()
    {
        levelController = FindObjectOfType<LevelController>();
    }

    void Update()
    {
        textCurrentScore.text = levelController.score.ToString();
    }
}
