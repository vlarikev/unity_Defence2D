using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject collection;

    [SerializeField] private GameObject skeleton;
    [SerializeField] private GameObject zombi;
    [SerializeField] private GameObject skeleton_archer;
    [SerializeField] private GameObject skeleton_tank;
    [SerializeField] private GameObject ogre;
    [SerializeField] private TextMeshProUGUI waveText;
    private int wave;

    private float[] spawnPosArray = new float[5] { -3.4f, -3.55f, -3.7f, -3.85f, -4f };
    private void Start()
    {
        wave = PlayerPrefs.GetInt("wave");
        waveText.text = "wave " + wave.ToString();

        PlayerPrefs.SetInt("currencyTimer", 1);
        StartCoroutine(CurrencyTimer());

        SpawnWaves();
    }
    private void SpawnEnemy(GameObject unit)
    {
        int tempRandom = Random.Range(0, 5);
        GameObject s = Instantiate(unit, new Vector3(transform.position.x, spawnPosArray[tempRandom], transform.position.z), Quaternion.identity, collection.transform);
        s.GetComponentInChildren<SortingGroup>().sortingOrder = 10 + tempRandom;
    }

    private IEnumerator CurrencyTimer()
    {
        yield return new WaitForSeconds(600);
        PlayerPrefs.SetInt("currencyTimer", 0);
    }

    #region Waves
    private void SpawnWaves()
    {
        if (wave == 1)
        {
            StartCoroutine(WaveUnit(skeleton, 1, 0, 1, 1, 0, 2));
        }
        if (wave == 2)
        {
            StartCoroutine(WaveUnit(skeleton, 1, 0, 1, 1, 0, 2));
            StartCoroutine(WaveUnit(zombi,    0, 0, 1, 0, 0, 1));
        }
        if (wave == 3)
        {
            StartCoroutine(WaveUnit(skeleton, 1, 0, 1, 1, 0, 2));
            StartCoroutine(WaveUnit(zombi,    0, 1, 0, 1, 0, 1));
        }
        if (wave == 4)
        {
            StartCoroutine(WaveUnit(skeleton,        1, 0, 1, 1, 0, 2));
            StartCoroutine(WaveUnit(zombi,           0, 1, 0, 1, 0, 1));
            StartCoroutine(WaveUnit(skeleton_archer, 0, 0, 1, 0, 0, 1));
        }
        if (wave == 5)
        {
            StartCoroutine(WaveUnit(skeleton,        1, 0, 1, 1, 1, 2));
            StartCoroutine(WaveUnit(zombi,           0, 1, 0, 1, 0, 1));
            StartCoroutine(WaveUnit(skeleton_archer, 0, 0, 1, 0, 0, 1));
        }
        if (wave == 6)
        {
            StartCoroutine(WaveUnit(skeleton,        1, 0, 1, 1, 1, 2));
            StartCoroutine(WaveUnit(zombi,           0, 1, 0, 1, 0, 2));
            StartCoroutine(WaveUnit(skeleton_archer, 0, 0, 1, 0, 0, 1));
        }
        if (wave == 7)
        {
            StartCoroutine(WaveUnit(skeleton,        1, 0, 1, 1, 1, 2));
            StartCoroutine(WaveUnit(zombi,           0, 1, 0, 1, 0, 1));
            StartCoroutine(WaveUnit(skeleton_archer, 0, 0, 1, 0, 0, 1));
            StartCoroutine(WaveUnit(skeleton_tank,   0, 0, 0, 0, 0, 1));
        }
        if (wave == 8)
        {
            StartCoroutine(WaveUnit(skeleton,        1, 0, 1, 1, 1, 2));
            StartCoroutine(WaveUnit(zombi,           0, 1, 0, 1, 0, 2));
            StartCoroutine(WaveUnit(skeleton_archer, 0, 0, 1, 0, 0, 1));
            StartCoroutine(WaveUnit(skeleton_tank,   0, 0, 0, 0, 0, 1));
        }
        if (wave == 9)
        {
            StartCoroutine(WaveUnit(skeleton,        1, 0, 1, 1, 1, 2));
            StartCoroutine(WaveUnit(zombi,           0, 1, 0, 1, 0, 1));
            StartCoroutine(WaveUnit(skeleton_archer, 0, 0, 1, 0, 0, 1));
            StartCoroutine(WaveUnit(skeleton_tank,   0, 0, 1, 0, 0, 1));
        }
        if (wave == 10)
        {
            StartCoroutine(WaveUnit(skeleton,        1, 0, 1, 1, 1, 2));
            StartCoroutine(WaveUnit(zombi,           0, 1, 0, 1, 0, 1));
            StartCoroutine(WaveUnit(skeleton_archer, 0, 0, 1, 0, 0, 1));
            StartCoroutine(WaveUnit(skeleton_tank,   0, 0, 0, 1, 0, 0));
            StartCoroutine(WaveUnit(ogre,            0, 0, 0, 0, 0, 1));
        }
    }
    #endregion
    private IEnumerator SpawnPack(int value, GameObject unit)
    {
        if (value == 0)
        {
            yield return new WaitForSeconds(0);
        }
        else
        {
            for (int i = 0; i < value; i++)
            {
                SpawnEnemy(unit);
                yield return new WaitForSeconds(Random.Range(1, 4));
            }
        }
    }
    private IEnumerator WaveUnit(GameObject unit, int value1, int value2, int value3, int value4, int value5, int value6)
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            StartCoroutine(SpawnPack(value1, unit));

            yield return new WaitForSeconds(Random.Range(14, 17));
            StartCoroutine(SpawnPack(value2, unit));

            yield return new WaitForSeconds(Random.Range(14, 17));
            StartCoroutine(SpawnPack(value3, unit));

            yield return new WaitForSeconds(Random.Range(14, 17));
            StartCoroutine(SpawnPack(value4, unit));

            yield return new WaitForSeconds(Random.Range(14, 17));
            StartCoroutine(SpawnPack(value5, unit));

            yield return new WaitForSeconds(Random.Range(14, 17));
            StartCoroutine(SpawnPack(value6, unit));

            yield return new WaitForSeconds(10);
        }
    }
}
