using UnityEngine;




public class BombColliderManager : MonoBehaviour
{
    [SerializeField] Collider _BombCollider;

    private void Start()
    {
        _BombCollider.isTrigger = true;
    }
    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _BombCollider.isTrigger = false;
        }

    }



}
