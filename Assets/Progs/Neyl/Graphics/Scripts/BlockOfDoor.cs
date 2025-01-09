using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockOfDoor : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotationSpeed = 50f;
    public float amplitude = 1f; // Amplitude du mouvement
    public float frequency = 1f; // Fréquence du mouvement
    public float verticalSpeed = 1f; // Vitesse verticale de déplacement

    private Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.left* rotationSpeed * Time.deltaTime);
        float verticalMovement = Mathf.Sin(Time.time * frequency) * amplitude;

        // Calcule la nouvelle position en fonction du déplacement vertical
        Vector3 newPosition = startPosition + Vector3.up * verticalMovement;

        // Applique le déplacement vertical à l'objet
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * verticalSpeed);
    }
}
