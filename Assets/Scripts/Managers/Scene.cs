using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    public void Newgame()
    {
        SceneManager.LoadScene("Level Selection");
    }

    public void Level1()
    {
        SceneManager.LoadScene("LEVEL 1");
    }
   
}
