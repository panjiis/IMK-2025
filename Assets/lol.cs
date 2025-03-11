using UnityEngine;

public class lol : MonoBehaviour
{
    void Update()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movement += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            movement += new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            movement += new Vector3(0, 0, 1);
        }

        transform.position += movement * 5f * Time.deltaTime;
    }
}
