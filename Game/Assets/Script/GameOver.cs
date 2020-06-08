using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private float restartDelay=2f;
    void restart(){
        SceneManager.LoadScene(0);
    }
}
