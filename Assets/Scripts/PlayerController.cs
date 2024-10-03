using UnityEngine;

/// <summary>
/// 
/// TODO
/// DIFFERENT CONTROLLER KEYS FOR OTHER PLAYERS 
/// INVOKING PLAYER DEACTIVATION - LONGER TIMES?
/// SUMMARY IS USED TO CREATE DESCRIPTION FOR METHODS
/// </summary>


public class PlayerController : MonoBehaviour
{
    private float _playerMaxSpeed;

    private CharacterController _playerCharacterController;
    private PlayerController _playerControllerScript;
    private BombController _bombController;
    private Material _playerMaterial;
    private Rigidbody _playerRB;

    private Vector3 _playerKeyInput;
    private readonly float _bombPushForce = 5.0f;

    void Start()
    {
        _playerCharacterController = GetComponent<CharacterController>();
        _playerRB = GetComponent<Rigidbody>();
        _playerControllerScript = GetComponent<PlayerController>();
        _bombController = GetComponent<BombController>();
        _playerMaterial = GetComponent<Renderer>().material;

        _playerMaxSpeed = 30.0f;
    }


    void Update()
    {
        GatherPlayerInput();
        MovePlayer();
    }

    //private void FixedUpdate()
    //{
    //    Vector3 pos = transform.position;
    //    Vector3 translation = _playerMaxSpeed * Time.fixedDeltaTime * _playerKeyInput;

    //    _playerRB.MovePosition(pos + translation);
    //}


    #region  Player movement

    //TODO: _playerKeyInput variable - change name to something else
    private void GatherPlayerInput()
    {
        _playerKeyInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        _playerKeyInput.Normalize();
    }

    private void MovePlayer()
    {
        _playerCharacterController.Move(_playerMaxSpeed * Time.deltaTime * _playerKeyInput);
    }
    #endregion  Player movement


    #region Triggers and Colliders
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            PlayerDeath();
        }

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody _bombBody = hit.collider.attachedRigidbody;

        //if no rigidbody
        if (_bombBody == null || _bombBody.isKinematic == true)
        {
            return;
        }

        if (hit.moveDirection.y < -0.3f)
        {
            return;
        }

        Vector3 _bombPushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        _bombBody.AddForceAtPosition(_bombPushDirection * _bombPushForce, hit.point, ForceMode.Impulse);
        //_bombBody.AddForce(_bombPushDirection * _bombPushForce, ForceMode.Impulse);

    }
    #endregion Triggers and Colliders

    private void PlayerDeath()
    {
        //_playerCharacterController.enabled = false;

        _bombController.enabled = false;
        _playerMaterial.color = Color.black;
        _playerControllerScript.enabled = false;

        Invoke(nameof(PlayerDeactivation), 1.25f);

    }

    private void PlayerDeactivation()
    {
        gameObject.SetActive(false);
    }
}
