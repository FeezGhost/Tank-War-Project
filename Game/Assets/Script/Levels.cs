using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Levels : MonoBehaviour
{
    public void  Level1(){
        SceneManager.LoadScene(3);
    }
    public void  Level2(){
        SceneManager.LoadScene(0);
    }
    public void  Level3(){
        SceneManager.LoadScene(2);
    }
}
