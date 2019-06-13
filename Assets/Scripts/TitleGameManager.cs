using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleGameManager : MonoBehaviour
{     

    public void BattleClicked()
    {
        SceneManager.LoadScene("PvP_Battle");
    }

    public void SurvivalClicked()
    {
        SceneManager.LoadScene("PvC_Survival");
    }

    public void QuitClicked()
    {
        Application.Quit();
    }
}
