using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class WallsChecker : MonoBehaviour
{
    public event Action Collided;
    [SerializeField] private LayerMask _wallsLayer;
    [SerializeField] private Balance _rightHand, _leftHand, _leftLeg, _rightLeg, _head;
    public bool Climbing { get; private set; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Ground ground))
        {
            Collided?.Invoke();
                Climbing = true;
                ActivateHands(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Ground ground))
        {
            ActivateHands(false);
            Climbing = false;
        }
    }
    private void ActivateHands(bool active)
    {
        _rightHand.enabled = active;
        _leftHand.enabled = active;
        print("SET TRIGGERS " + active);
        _rightHand.Collider.isTrigger = active;
        _leftHand.Collider.isTrigger = active;
        _rightLeg.Collider.isTrigger = active;
        _leftLeg.Collider.isTrigger = active;
        _head.Collider.isTrigger = active;  
    }
}
