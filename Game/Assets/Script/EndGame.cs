using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    bool gamehasEnded=false;
    private float restartDelay=1f;
    public void endGame(){
        if(gamehasEnded==false){
            gamehasEnded= true;
            Debug.Log("Game End");
            // Invoke("Restart", restartDelay);
            restart();
        }
        
    }
    void restart(){
        SceneManager.LoadScene(2);
    }
}
