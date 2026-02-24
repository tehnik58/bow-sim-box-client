using UnityEngine;

public class BowController : MonoBehaviour
{
    [SerializeField]
    private GameObject bow;

    public void Shoot(float speed)
    {
        GameObject go = Instantiate(bow, transform.position, transform.rotation);
        //print($"{controller.transform.position} - {controller.transform.rotation}");
        go.GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
    }
}
