using NUnit.Framework.Constraints;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movespeed = 5f;
    public Transform movePoint;

    // Start is called before the first frame update
    void start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, movespeed * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {

        }

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
        {
            movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
        }

        if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
        }
    }
}