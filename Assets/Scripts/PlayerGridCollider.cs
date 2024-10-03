using UnityEngine;

public class PlayerGridCollider : MonoBehaviour
{

    public bool _canPutBomb = true;

    #region Triggers

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            _canPutBomb = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            _canPutBomb = true;
        }
    }

    #endregion Triggers
}
