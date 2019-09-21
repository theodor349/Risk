using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link
{
    private Province[] link;

    public Link(Province a, Province b)
    {
        link = new Province[] { a, b };
    }

    public Province GetOther(Province from)
    {
        return link[0] == from ? link[1] : link[0];
    }

    public KeyValuePair<Vector2, Vector2> GetEndPoints()
    {
        return new KeyValuePair<Vector2, Vector2>(link[0].transform.position, link[1].transform.position);
    }
}
