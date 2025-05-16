using UnityEngine;

public class ObjectLifetime : MonoBehaviour
{
    public int age = 3;

    public void DecreaseAge()
    {
        age--;
        Debug.Log($"{gameObject.name} age decreased to {age}");

        if (age <= 0)
        {
            Destroy(gameObject);
            Debug.Log($"{gameObject.name} destroyed due to age 0");
        }
    }
}
