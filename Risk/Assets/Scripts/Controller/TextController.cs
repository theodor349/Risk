using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    public static TextController Instance;

    [SerializeField] private GameObject textObj;

    private void Awake()
    {
        Instance = this;
    }

    public TextMeshProUGUI SetupText(Vector2 pos)
    {
        var t = Instantiate(textObj);
        t.transform.SetParent(transform);
        t.transform.position = pos;
        return t.GetComponent<TextMeshProUGUI>();
    }

}
