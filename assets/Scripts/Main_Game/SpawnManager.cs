using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject _fishPrefab;
    public bool _isSpawning;
    private Coroutine spawnCoroutine;

    void Start()
    {
        _isSpawning = false;
    }
    void Update()
    {
        if (_isSpawning && spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnFishRoutine());
        }
        else if (!_isSpawning && spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

    }

    IEnumerator SpawnFishRoutine()
    {

        yield return new WaitForSeconds(2.0f);
        while (_isSpawning)
        {
            int randomFishGenerator = Random.Range(1, 3);
            for (int i = 0; i <= randomFishGenerator; i++)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-12f, 12f), Random.Range(-6, 2), 0);
                GameObject newFish = Instantiate(_fishPrefab, posToSpawn, Quaternion.identity);
                float randomScale = Random.Range(0.5f, 1.5f); // Adjust the range as needed
                newFish.transform.localScale = new Vector3(randomScale, randomScale, 0);
            }
            float randomSecond = Random.Range(3.0f, 8.0f);
            yield return new WaitForSeconds(randomSecond);
        }
    }
}
