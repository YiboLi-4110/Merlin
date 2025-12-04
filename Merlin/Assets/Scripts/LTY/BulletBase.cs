using UnityEngine;

public class BulletBase : MonoBehaviour
{

    public int collidetime = 1;
    public void Destroy_bullet()
    {
        if(collidetime <= 1)
            Destroy(gameObject);
        else
            collidetime--;
    }



    // Start is called before the first frame update
}
