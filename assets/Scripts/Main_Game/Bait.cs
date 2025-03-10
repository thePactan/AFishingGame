using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bait : MonoBehaviour
{
    public GameObject baitRadiusObject, playerObject;
    private Collider2D baitCollider;
    private Collider2D radiusCollider;
    private BaitRadius _baitRadius;
    private Player _player;

    [SerializeField]
    private UIManager _uiManager;

    [SerializeField]
    private GameManager _gameManager;

    void Start()
    {
        // Ignore collision between bait collider and radius collider
        baitCollider = GetComponent<Collider2D>();
        radiusCollider = baitRadiusObject.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(baitCollider, radiusCollider, true);

        // Get the BaitRadius script
        _baitRadius = baitRadiusObject.GetComponent<BaitRadius>();
        // Correct the type from playerObject to Player
        _player = playerObject.GetComponent<Player>();
        _uiManager = FindObjectOfType<UIManager>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Fish"))
        {
            CalculateScore(other);
            Destroy(other.gameObject);
            _baitRadius.ResetBaitOccupation();
            _player.ResetFromFishingToIdle();
        }
    }

    private void CalculateScore(Collider2D other)
    {
        float scale = other.transform.localScale.x;
        int score = Mathf.RoundToInt(Mathf.Abs(scale) * 100);
        _gameManager.AddScore(score);

    }
}
