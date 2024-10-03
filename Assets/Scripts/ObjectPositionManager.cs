using UnityEngine;

public class ObjectPositionManager : MonoBehaviour
{

    [SerializeField] Grid _MapGrid;
    [SerializeField] GameObject _Bomb, _PlayerGridCollider, _Player;
    Vector3 _objectLastGlobalPosition;

    #region Update
    void Update()
    {
        //GetObjectGlobalPosition(_Player);
        ReturnCellCenter3D();
        SetPlayerCellIndicatorPosition();
    }
    #endregion Update

    #region Get position funcs
    private Vector3 GetObjectGlobalPosition(GameObject _object)
    {
        Ray ray = new Ray(_object.transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit cell, 10))
        {
            _objectLastGlobalPosition = cell.point;
        }

        return _objectLastGlobalPosition;
    }

    public Vector3 ReturnCellCenter3D()
    {
        Vector3 _playerGlobalPos = GetObjectGlobalPosition(_Player);
        Vector3Int _playerCellPos = _MapGrid.WorldToCell(_playerGlobalPos);
        Vector3 _centerCellPosition = _MapGrid.CellToWorld(_playerCellPos) + new Vector3(_MapGrid.cellSize.x / 2, _Bomb.transform.lossyScale.y * 5, _MapGrid.cellSize.z / 2);
        return _centerCellPosition;
    }

    private void SetPlayerCellIndicatorPosition()
    {
        _PlayerGridCollider.transform.position = ReturnCellCenter3D();      //to jeszcze zmienic

    }

    #endregion Get position funcs

}
