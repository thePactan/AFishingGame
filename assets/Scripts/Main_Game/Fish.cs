using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField]
    public float speed;
    private Vector2 _targetPosition;
    private Animator _anim;
    private bool _isCooldown = false;
    private bool _isFacingLeft = true;
    private bool _isMovingToBait = false;

    void Start()
    {
        float scale = transform.localScale.x; // Assuming uniform scale
        speed = Mathf.Lerp(4.5f, 1f, Mathf.Pow((scale - 0.5f) / (1.5f - 0.5f), 2));
        //Debug.Log("Speed: " + speed + " | Scale: " + scale);
        SetRandomTargetPosition();
        StartCoroutine(DestroyAfterDelay());
        _anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!_isCooldown)
        {
            _anim.SetBool("_isSwiming", true);
            MoveTowardsTarget();
        }
    }

    public void MoveTowardsBait(Vector2 baitPosition)
    {
        _targetPosition = baitPosition;
        _isMovingToBait = true;
        StopCoroutine(DestroyAfterDelay());
    }

    private void SetRandomTargetPosition()
    {
        float randomX = Random.Range(-13f, 13f);
        float randomY = Random.Range(-7f, 0f);
        _targetPosition = new Vector2(randomX, randomY);
    }

    private void MoveTowardsTarget()
    {
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = _targetPosition;
        float step = speed * Time.deltaTime;

        // Move towards the target position
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, step);

        // Calculate move direction
        float move = targetPosition.x - currentPosition.x;

        // Flip based on move direction
        if (move < 0 && !_isFacingLeft)
        {
            Flip();
        }
        else if (move > 0 && _isFacingLeft)
        {
            Flip();
        }

        // Check if the target position is reached
        if ((Vector2)transform.position == _targetPosition)
        {
            StartCoroutine(CooldownWhenReachDestination());
            if (!_isMovingToBait)
            {
                SetRandomTargetPosition();
            }
        }
    }

    //Fish now movw to bait by BaitRadius



    private void Flip()
    {
        _isFacingLeft = !_isFacingLeft;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }


    IEnumerator CooldownWhenReachDestination()
    {
        _isCooldown = true;
        _anim.SetBool("_isSwiming", false);
        float randomSecond = Random.Range(1f, 2f);
        yield return new WaitForSeconds(randomSecond);
        _isCooldown = false;
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(10.0f);
        Destroy(this.gameObject);
    }

}
