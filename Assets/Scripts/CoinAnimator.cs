using System.Collections;
using UnityEngine;

public class CoinAnimator : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
   [SerializeField] private float rotationSpeed = 200f; // Speed of rotation
   [SerializeField] private float disableAfterSeconds = 2f; // Time after which the coin will be disabled
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing on the coin object.");
            return;
        }
        initialPosition = transform.position;
        initialRotation = transform.rotation;
         StartCoroutine(DisableCoinAfterTime());

         var force = new Vector3(Random.Range(-3f, 3f), Random.Range(12f, 12f), Random.Range(-3f, 3f));
        ApplyForce(force);
    }

    public void ApplyForce(Vector3 force)
    {
        if (rb != null)
        {
            rb.AddForce(force, ForceMode.Impulse);

        }
        else
        {
            Debug.LogError("Rigidbody is not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
    IEnumerator DisableCoinAfterTime()
    {
        yield return new WaitForSeconds(disableAfterSeconds);
        gameObject.SetActive(false);
    }
    
}
