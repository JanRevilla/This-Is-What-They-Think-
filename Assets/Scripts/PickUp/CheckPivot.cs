using System.Collections.Generic;
using UnityEngine;

public class CheckPivot : MonoBehaviour
{
    private GameObject _currentPiece = null;

    public bool TryPickUp(GameObject p)
    {
        if (_currentPiece == null && transform.childCount == 0)
        {
            _currentPiece = p;
            return true;
        }
        return false;
    }

    public void RemovePiece()
    {
        _currentPiece = null;
    }

    public bool HasPiece()
    {
        return _currentPiece != null || transform.childCount > 0;
    }
}
