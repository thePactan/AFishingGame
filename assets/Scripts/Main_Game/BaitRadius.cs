using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitRadius : MonoBehaviour
{

    public GameObject baitObject;
    private Collider2D baitCollider;
    private Collider2D radiusCollider;
    public bool _isBaitOccupied = false;

    void Start()
    {
        baitCollider = baitObject.GetComponent<Collider2D>();
        radiusCollider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(baitCollider, radiusCollider, true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fish") && !_isBaitOccupied)
        {

            Fish fish = other.GetComponent<Fish>();
            if (fish != null)
            {
                fish.MoveTowardsBait(baitObject.transform.position);
                _isBaitOccupied = true;
            }
        }
    }

    public void ResetBaitOccupation()
    {
        _isBaitOccupied = false;
    }

}
