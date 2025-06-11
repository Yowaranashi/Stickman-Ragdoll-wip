using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionChecker : MonoBehaviour
{
    [SerializeField] private Transform _head, _body, _lh, _rh, _ll, _rl;
    public void FaceRight(bool face)
    {
        var scale = Vector3.one;
        scale.x = face ? 1f : -1f;
            _head.transform.localScale = scale;
            _body.transform.localScale = scale;
            _lh.transform.localScale = scale;
            _rh.transform.localScale = scale;
            _ll.transform.localScale = scale;
            _rl.transform.localScale = scale;
    }
}
