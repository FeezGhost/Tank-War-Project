using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private float restartDelay=2f;
    void restart(){
        SceneManager.LoadScene(0);
    }
    void  start(){
        Invoke("Restart", restartDelay);
    }
    void update(){
        
            Invoke("Restart", restartDelay);
    }
}
