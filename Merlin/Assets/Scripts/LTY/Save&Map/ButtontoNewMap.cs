using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtontoNewMap : MonoBehaviour
{
    // Start is called before the first frame update
    public void GotoNewMap()
    {
        if(!TheGlobalManager.TGM.IsSafe())
        {
            return;
        }
        Transform ourmerlin = PlayerController.EntityMerlin.transform;
        int direction;

        if(ourmerlin.position.x >= 0)
        {
            if(ourmerlin.position.y >= 0)
            {
                direction = 1;
            }
            else
            {
                direction = 4;
            }
        }
        else
        {
            if(ourmerlin.position.y >= 0)
            {
                direction = 2;
            }
            else
            {
                direction = 3;
            }
        }

        if(TheGlobalManager.TGM.EnterScene(direction, TheGlobalManager.TGM.getseed() + 5))
            SceneManager.LoadScene("LoadScene");
        else
            SceneManager.LoadScene("WarScene");

        

    }
}
