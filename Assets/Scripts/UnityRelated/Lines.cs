using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lines : MonoBehaviour
{
    private int count;
    void Start()
    {
        count = 1;
        gameObject.GetComponent<TMP_Text>().text = "1";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            count++;
            gameObject.GetComponent<TMP_Text>().text += '\n';
            gameObject.GetComponent<TMP_Text>().text += count;
        }
    }
}
