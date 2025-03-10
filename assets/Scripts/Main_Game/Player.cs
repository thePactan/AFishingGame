using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10f;
    private float pressDuration = 0f;

    public GameObject PlayerBait; // Reference to the current bait object
    public Transform rodTip;
    private bool _isFishing;
    private bool _isCooldown;
    private bool _isFacingRight;
    private Animator _anim;
    private LineRenderer lineRenderer;
    public float cooldownDuration = 1f; // Cooldown duration in seconds
    public int _score = 0;

    [SerializeField]
    private UIManager _uiManager;



    void Start()
    {
        _isFishing = false;
        _isCooldown = false;
        _isFacingRight = true;
        _anim = GetComponentInChildren<Animator>();
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component not found!");
        }

        lineRenderer.positionCount = 2;

        if (PlayerBait == null)
        {
            Debug.LogError("PlayerBait not assigned!");
        }

        if (rodTip == null)
        {
            Debug.LogError("rodTip not assigned!");
        }

        _uiManager = FindObjectOfType<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UIManager not found!");
        }

    }

    void Update()
    {
        // Move Normally
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !_isFishing && !_isCooldown)
        {
            float move = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            Vector3 newPosition = transform.position + new Vector3(move, 0, 0);

            // Clamp the x position between -15 and 15
            newPosition.x = Mathf.Clamp(newPosition.x, -15f, 15f);
            // Keep y and z positions constant
            newPosition.y = 4.5f;
            newPosition.z = 0f;

            transform.position = newPosition;
            if (move < 0 && _isFacingRight)
            {
                Flip();
            }
            else if (move > 0 && !_isFacingRight)
            {
                Flip();
            }
            _anim.SetBool("_isWalking", true);
        }

        // Destroy bait when movement keys are pressed
        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) && _isFishing && !_isCooldown)
        {
            if (PlayerBait.activeSelf)
            {
                ResetFromFishingToIdle();
            }
        }

        else
        {
            _anim.SetBool("_isWalking", false);
        }

        // Track space key press duration
        if (Input.GetKey(KeyCode.Space) && !_isFishing && !_isCooldown)
        {
            _anim.SetBool("_isThrowing", true);
            StartCoroutine(ResetAnimParameter("_isThrowing"));
            pressDuration += Time.deltaTime;
        }

        // Spawn or destroy bait when space key is released
        if (Input.GetKeyUp(KeyCode.Space) && !_isCooldown)
        {
            if (PlayerBait.activeSelf)
            {
                ResetFromFishingToIdle();
            }
            else
            {
                SpawnBait();
                _isFishing = true;
            }
            pressDuration = 0f; // Reset press duration
        }

        if (_isFishing)
        {
            lineRenderer.SetPosition(0, rodTip.position);
            lineRenderer.SetPosition(1, PlayerBait.transform.position);
        }
    }

    private void SpawnBait()
    {
        float yPosition = Mathf.Clamp(-2f - pressDuration * 5f, -7f, -2f);
        Vector3 _baitPosition;
        Vector3 _scale;

        if (_isFacingRight)
        {
            _baitPosition = new Vector3(transform.position.x + 2.48f, yPosition, 0f);
            _scale = new Vector3(Mathf.Abs(PlayerBait.transform.localScale.x), PlayerBait.transform.localScale.y, PlayerBait.transform.localScale.z);
        }
        else
        {
            _baitPosition = new Vector3(transform.position.x - 2.48f, yPosition, 0f);
            _scale = new Vector3(-Mathf.Abs(PlayerBait.transform.localScale.x), PlayerBait.transform.localScale.y, PlayerBait.transform.localScale.z);
        }

        StartCoroutine(BaitPlace(_baitPosition, _scale));
    }


    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public void ResetFromFishingToIdle()
    {
        _anim.SetBool("_isPulling", true);
        StartCoroutine(ResetAnimParameter("_isPulling"));
        StartCoroutine(DestroyBait());
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
        CheckForBestScore(_score);
    }

    private void CheckForBestScore(int playerScore)
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (playerScore > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", playerScore);
            _uiManager.UpdateBestScore(playerScore);
        }
    }


    IEnumerator BaitPlace(Vector3 baitPosition, Vector3 baitScale)
    {

        yield return new WaitForSeconds(1.0f); // Wait for cooldown duration
        PlayerBait.transform.position = baitPosition;
        PlayerBait.transform.localScale = baitScale;
        PlayerBait.transform.parent = transform;
        PlayerBait.SetActive(true);

    }

    IEnumerator DestroyBait()
    {
        if (PlayerBait.activeSelf)
        {
            PlayerBait.SetActive(false);
        }

        _isFishing = false;
        _isCooldown = true; // Start cooldown
        yield return new WaitForSeconds(cooldownDuration); // Wait for cooldown duration
        _isCooldown = false; // End cooldown
    }

    IEnumerator ResetAnimParameter(string parameterName)
    {
        yield return new WaitForSeconds(1f);
        _anim.SetBool(parameterName, false);
    }
}
