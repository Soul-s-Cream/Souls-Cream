using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForLoadLevel : MonoBehaviour
{
    public float time;
    public int level;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(time);
        GameManager.Instance.EndLevel(level);
    }
}
