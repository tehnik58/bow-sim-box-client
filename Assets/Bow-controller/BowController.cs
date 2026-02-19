using UnityEngine;

public class BowController : MonoBehaviour
{
    [SerializeField]
    private GameObject bow;

    void Start()
    {
        
    }

    public void Shoot(float speed)
    {
        GameObject go = Instantiate(bow, transform.position, transform.rotation);
        go.GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
    }
}
