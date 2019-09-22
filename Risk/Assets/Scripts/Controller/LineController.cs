using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public static LineController Instance;
    public static List<Link> Links = new List<Link>();

    [SerializeField] private GameObject lineObj;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var l in Links)
        {
            SetupLink(l.GetEndPoints());
        }
    }

    public void SetupLink(KeyValuePair<Vector2, Vector2> pos)
    {
        var o = Instantiate(lineObj);
        o.transform.SetParent(transform);
        var l = o.GetComponent<LineRenderer>();
        l.SetPosition(0, pos.Key);
        l.SetPosition(1, pos.Value);
    }
}
