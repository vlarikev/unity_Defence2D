using System.Collections;
using UnityEngine;

public class AnimDelay : MonoBehaviour
{
    [SerializeField] private Animation anim;
    private float[] delayArray = new float[5] { 0.1f, 0.6f, 1.1f, 1.6f, 2f};

    private void Start()
    {
        StartCoroutine(AnimationDelay());
    }
    private IEnumerator AnimationDelay()
    {
        yield return new WaitForSeconds(delayArray[Random.Range(0, 5)]);
        anim.Play();
    }
}
