using UnityEngine;


/// <summary>
/// NOT USED
/// </summary>


public class ExplosionObject : MonoBehaviour
{
    private int _fuse = 3;


    // Update is called once per frame
    void Update()
    {
        Boom();
    }

    private void Boom()
    {
        Destroy(gameObject, _fuse);
    }
}
