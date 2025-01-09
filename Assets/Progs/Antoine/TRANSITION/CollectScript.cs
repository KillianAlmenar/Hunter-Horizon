using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectScript : MonoBehaviour
{
    [SerializeField] GameObject canvasToLoad;
    [SerializeField] GameObject canvasToLoad2;
    private GameObject instantiatedCanvas;
    public int ID;
    public bool isCollected;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        IDObject.CurrentItems = ID;
        instantiatedCanvas = Instantiate(canvasToLoad);

    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(canvasToLoad2);
        }
    }
        private void OnTriggerExit(Collider other)
    {
        Destroy(instantiatedCanvas);

        instantiatedCanvas = null;
    }
}
