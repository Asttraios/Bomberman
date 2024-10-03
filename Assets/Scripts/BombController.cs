using System.Collections;
using UnityEngine;

public class BombController : MonoBehaviour
{
    /// <summary>
    /// TODO: 
    ///
    /// SPAWN BOMB EXPLOSION EXACTLY ON CELL POSITION
    /// REST OF THE PLAYER'S BOMBS ON ARENA WON'T EXPLODE. BOMB CONTROLLER IS DISABLED THE MOMENT PLAYER DIES. 
    /// PLAYER SPAWNPOINT AND INSTANCES OF PLAYERS
    /// PLAYER DEATH AND WIN STATE
    /// DESTROYABLE BOXES
    /// PICK-UP POWERUPS
    /// 



    /// CAN_PUT_BOMB REMAINS FALSE AFTER DESTROYING BOMB AND STANDING INSIDE IT
    /// SOLVED? - PLAYER WILL DIE ANYWAY IN THE BOMB POSITION
    /// </summary>


    [SerializeField] private GameObject _Explosion;
    [SerializeField] private GameObject _Bomb;
    [SerializeField] private PlayerGridCollider _PlayerCellIndicator;
    [SerializeField] private ObjectPositionManager _ObjectPositionManager;

    Vector3 _explosionRange_Forward;
    Vector3 _explosionRange_Backward;
    Vector3 _explosionRange_Left;
    Vector3 _explosionRange_Right;

    private int _explosionRangeMax;
    private int _obstacle_Mask;             //change to static?
    private bool _canPutBomb;
    private readonly int _oneCellSize = 10;
    private readonly int _bombFuse = 3;
    public int _bombCapacity;

    private void Start()
    {

        _explosionRangeMax = 30;
        _bombCapacity = 3;
        _explosionRange_Forward = new Vector3(0, 0, _oneCellSize);     //this is only direction, try using just unit vectors
        _explosionRange_Backward = new Vector3(0, 0, -_oneCellSize);
        _explosionRange_Left = new Vector3(-_oneCellSize, 0, 0);
        _explosionRange_Right = new Vector3(_oneCellSize, 0, 0);
        _obstacle_Mask = 1 << LayerMask.NameToLayer("Obstacle");

    }

    private void Update()
    {
        _canPutBomb = _PlayerCellIndicator._canPutBomb;

        if (Input.GetKeyDown(KeyCode.F) && _canPutBomb && _bombCapacity > 0)
        {
            StartCoroutine(PutDownBomb());
        }
    }

    IEnumerator PutDownBomb()
    {

        GameObject _placedBomb = Instantiate(_Bomb, _ObjectPositionManager.ReturnCellCenter3D(), Quaternion.identity);
        _bombCapacity -= 1;

        yield return new WaitForSeconds(_bombFuse);     //FUSE 2 SECONDS//

        Vector3 _currentBombPos = _placedBomb.transform.position;

        Destroy(_placedBomb);
        GameObject _centreExplosion = Instantiate(_Explosion, _currentBombPos, Quaternion.identity);

        StartCoroutine(DeactivateTrigger(_centreExplosion));

        Destroy(_centreExplosion, .5f);



        Expl_Temp(_currentBombPos, _explosionRange_Forward);
        Expl_Temp(_currentBombPos, _explosionRange_Backward);
        Expl_Temp(_currentBombPos, _explosionRange_Left);
        Expl_Temp(_currentBombPos, _explosionRange_Right);

        _bombCapacity += 1;
    }


    private void Expl_Temp(Vector3 _explosionCenter, Vector3 _explosionDirection)
    {
        if (Physics.Raycast(_explosionCenter, _explosionDirection, out RaycastHit _explosionHit, _explosionRangeMax, _obstacle_Mask) && _explosionHit.distance > 0)
        {
            CreateSingleExplosion(_explosionCenter, _explosionDirection, (int)_explosionHit.distance / _oneCellSize);

        }
        else CreateSingleExplosion(_explosionCenter, _explosionDirection);
    }

    private void CreateSingleExplosion(Vector3 _explosionCenter, Vector3 _explosionDirection, int _hitDistance = 3)
    {

        if (_hitDistance > 0)
        {
            _explosionCenter += _explosionDirection;
            GameObject _explosion = Instantiate(_Explosion, _explosionCenter, Quaternion.identity);     //we can try do it recursively later
            StartCoroutine(DeactivateTrigger(_explosion));
            Destroy(_explosion, .5f);

            CreateSingleExplosion(_explosionCenter, _explosionDirection, _hitDistance - 1);

        }
        else return;

    }

    IEnumerator DeactivateTrigger(GameObject _objWithTrigger)
    {
        yield return new WaitForSeconds(.15f);

        Collider _triggerCollider = _objWithTrigger.GetComponent<Collider>();
        _triggerCollider.enabled = false;

    }

}




