using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Text textTimeElapsed;
    public float timeElapsed { get; private set; }

    [SerializeField] float timeForIncreasingSpawnRate = 5f;
    private float timeNext;
    private void Awake()
    {
        timeElapsed = 0f;
        timeNext = timeForIncreasingSpawnRate;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        textTimeElapsed.text = "Time: " + timeElapsed.ToString("0");

        if(timeElapsed > timeNext) //Increase spawn rate and speed of platforms
        {
            timeNext += timeForIncreasingSpawnRate;
        }
    }
}
