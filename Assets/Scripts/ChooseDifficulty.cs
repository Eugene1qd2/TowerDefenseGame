using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseDifficulty : MonoBehaviour
{
    public static int StartMoney=20;
    public static float DiffValue=1f;
    
    public static int difficulty=3;

    public void SetDifficulty(int new_diff)
    {
        difficulty = new_diff;
        switch(difficulty)
        {
            case 0:
                StartMoney = 40;
                DiffValue = 0.8f;
                break;
            case 1:
                StartMoney = 30;
                DiffValue = 0.8f;
                break;
            case 2:
                StartMoney = 30;
                DiffValue = 1f;
                break;
            case 3:
                StartMoney = 20;
                DiffValue = 1f;
                break;
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
    }
}
