using System.Collections.Generic;
using UnityEngine;

public class BowController : MonoBehaviour
{
    [SerializeField]
    private GameObject bow;
    [SerializeField]
    private List<GameObject> arrows;

    public void Shoot(float speed)
    {
        GameObject go = Instantiate(bow, transform.position, transform.rotation);
        go.GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
        arrows.Add(go);

        if (arrows.Count > 10)
        {
            Destroy(arrows[0]);
            arrows.RemoveAt(0);
        }
    }
}
