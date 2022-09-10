using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpawnerScript : MonoBehaviour
{
    public int Difficulty;
    public float DiffValue;
    [SerializeField] List<Runner> Runners = new List<Runner>();
    [SerializeField] GameObject wavesCounter;
    [SerializeField] GameObject WinMenu;
    float coolDown = 0.45f;
    float timeSpan = 0;
    public int WavesCount = 0;
    List<Runner> wave;
    public List<Runner> GenerateWaveByCount(int count)
    {
        List<Runner> list = new List<Runner>();
        for (int i = 0; i < count; i++)
        {
            list.Add(Runners[Random.Range(0, Runners.Count)]);
        }
        return list;
    }
    public List<Runner> GenerateWaveByWaves()
    {
        List<Runner> list = new List<Runner>();
        int count = 5 + 5 * (WavesCount % 10) + Random.Range(0,5);
        for (int i = 0; i < count; i++)
        {
            list.Add(Runners[Random.Range(0, Runners.Count)]);
        }
        return list;
    }
    void Start()
    {
        DiffValue = ChooseDifficulty.DiffValue;
        Difficulty = ChooseDifficulty.difficulty;
        wave = GenerateWaveByWaves();
        timeSpan = 0.45f;
    }

    void Update()
    {
        if(WavesCount>=100)
        {
            WinMenu.active = true;
            Time.timeScale = 0f;
        }
        if (wave.Count > 0)
        {
            if (timeSpan <= 0)
            {
                timeSpan = coolDown;
                Instantiate(wave[0]).transform.position=transform.position;
                wave.RemoveAt(0);
            }
            timeSpan -= Time.deltaTime;
        }
        else
        {
            if (GameObject.FindGameObjectsWithTag("Runner").Length <= 0)
            {
                WavesCount++;
                wavesCounter.GetComponent<Text>().text = WavesCount.ToString();
                wave = GenerateWaveByWaves();
                GameObject.FindGameObjectWithTag("Floar").GetComponent<BuildingGrid>().UpdateCoins(5);
            }
        }
    }
}
