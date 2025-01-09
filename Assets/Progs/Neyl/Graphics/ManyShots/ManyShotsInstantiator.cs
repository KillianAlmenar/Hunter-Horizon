using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class ManyShotsInstantiator : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject target;
    [SerializeField] GameObject fireManyShots;
    [SerializeField] GameObject iceManyShots;
    [SerializeField] GameObject thunderManyShots;
    [SerializeField] GameObject windManyShots;
    [SerializeField] GameObject firePrepForManyShots;
    [SerializeField] GameObject icePrepForManyShots;
    [SerializeField] GameObject thunderPrepForManyShots;
    [SerializeField] GameObject windPrepForManyShots;
    public float distance = 10.0f; // Exemple : 10 unités Unity
    public float distanceToPoint = 10.0f; // Exemple : 10 unités Unity
    public int minuser;
    public int numberOfPrefabs;
    bool doInstantiate;
    public bool launch;
    public Elements.Element elem;
    float timer;
    public float maxTimer;
    List<Vector3> posList;
    public Vector3 targetPos;
    public Transform[] tabPos;
    public AttackScript attScript;
    [SerializeField] TargetLock locking;
    // Start is called before the first frame update
    void Start()
    {
        doInstantiate = false;
        timer = 0;
        posList = new List<Vector3>();
        elem = Elements.Element.FIRE;

    }

    // Update is called once per frame
    void Update()
    {



        targetPos = target.transform.position;


        if (launch)
        {

            posList = CalculateSemiCirclePositions(player.transform, distance, numberOfPrefabs);
            for (int i = 0; i < tabPos.Length; i++)
            {

                switch (elem)
                {
                    case (Elements.Element.FIRE):
                        GameObject goFire = Instantiate(firePrepForManyShots, tabPos[i].position, transform.rotation);
                        Destroy(goFire, maxTimer + 1f);
                        OrientTowardsPoint(targetPos, goFire);
                        break;
                    case (Elements.Element.ICE):
                        GameObject goIce = Instantiate(icePrepForManyShots, tabPos[i].position, transform.rotation);
                        Destroy(goIce, maxTimer + 1f);

                        OrientTowardsPoint(targetPos, goIce);
                        break;
                    case (Elements.Element.ELECTRICITY):
                        GameObject goElec = Instantiate(thunderPrepForManyShots, tabPos[i].position, transform.rotation);
                        Destroy(goElec, maxTimer + 1f);

                        OrientTowardsPoint(targetPos, goElec);
                        break;
                    case (Elements.Element.WIND):
                        GameObject goWind = Instantiate(windPrepForManyShots, tabPos[i].position, transform.rotation);
                        Destroy(goWind, maxTimer + 1f);

                        OrientTowardsPoint(targetPos, goWind);
                        break;
                }
            }
            doInstantiate = true;
            launch = false;
        }

        if (doInstantiate)
        {
            timer += Time.deltaTime;
        }

        if (timer > maxTimer)
        {
            for (int i = 0; i < tabPos.Length; i++)
            {
                switch (elem)
                {
                    case (Elements.Element.FIRE):
                        GameObject goFire = Instantiate(fireManyShots, tabPos[i].position, player.transform.rotation);
                        goFire.GetComponent<BulletScript>().attackScript = attScript;
                        OrientTowardsPoint(targetPos, goFire);
                        break;
                    case (Elements.Element.ICE):
                        GameObject goIce = Instantiate(iceManyShots, tabPos[i].position, player.transform.rotation);
                        goIce.GetComponent<BulletScript>().attackScript = attScript;
                        OrientTowardsPoint(targetPos, goIce);
                        break;
                    case (Elements.Element.ELECTRICITY):
                        GameObject goElec = Instantiate(thunderManyShots, tabPos[i].position, player.transform.rotation);
                        goElec.GetComponent<BulletScript>().attackScript = attScript;
                        OrientTowardsPoint(targetPos, goElec);
                        break;
                    case (Elements.Element.WIND):
                        GameObject goWind = Instantiate(windManyShots, tabPos[i].position, player.transform.rotation);
                        goWind.GetComponent<BulletScript>().attackScript = attScript;
                        OrientTowardsPoint(targetPos, goWind);
                        break;
                }

            }
            doInstantiate = false;
            timer = 0;

        }
    }
    public void OrientTowardsPoint(Vector3 targetPoint, GameObject startPoint)
    {
        // Calcul du vecteur direction entre la position de l'objet et le point cible
        Vector3 direction = targetPoint - startPoint.transform.position;

        // Si le vecteur direction n'est pas nul
        if (direction != Vector3.zero)
        {
            // Calcul de la rotation pour faire face à la direction
            Quaternion rotation = Quaternion.LookRotation(direction);

            // Appliquer la rotation à l'objet
            startPoint.transform.rotation = rotation;
        }
    }
    List<Vector3> CalculateSemiCirclePositions(Transform center, float radius, int count)
    {
        List<Vector3> positions = new List<Vector3>();


        float startAngle = 20;
        float endAngle = 160;

        for (int i = 0; i < count; i++)
        {
            float angle = Mathf.Lerp(startAngle, endAngle, (float)i / (count - 1));

            float x = center.position.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float y = center.position.y + radius * Mathf.Sin(Mathf.Deg2Rad * angle);

            positions.Add((new Vector3(x, y, center.position.z)));

        }
        for (int i = 0; i < posList.Count; i++)
        {
            Debug.Log(posList[i]);
        }

        return positions;
    }

    public void LaunchAttack(Elements.Element element)
    {
        Debug.Log("launch");

        posList = CalculateSemiCirclePositions(player.transform, distance, numberOfPrefabs);
        for (int i = 0; i < tabPos.Length; i++)
        {
            Debug.Log("for");

            switch (elem)
            {
                case (Elements.Element.FIRE):
                    Debug.Log("feu");
                    GameObject goFire = Instantiate(firePrepForManyShots, tabPos[i].position, transform.rotation);
                    Destroy(goFire, maxTimer + 1f);
                    OrientTowardsPoint(targetPos, goFire);
                    break;
                case (Elements.Element.ICE):
                    GameObject goIce = Instantiate(icePrepForManyShots, tabPos[i].position, transform.rotation);
                    Destroy(goIce, maxTimer + 1f);

                    OrientTowardsPoint(targetPos, goIce);
                    break;
                case (Elements.Element.ELECTRICITY):
                    GameObject goElec = Instantiate(thunderPrepForManyShots, tabPos[i].position, transform.rotation);
                    Destroy(goElec, maxTimer + 1f);

                    OrientTowardsPoint(targetPos, goElec);
                    break;
                case (Elements.Element.WIND):
                    GameObject goWind = Instantiate(windPrepForManyShots, tabPos[i].position, transform.rotation);
                    Destroy(goWind, maxTimer + 1f);

                    OrientTowardsPoint(targetPos, goWind);
                    break;
            }
        }
        doInstantiate = true;
        launch = false;

        if (doInstantiate)
        {
            timer += Time.deltaTime;
        }

        if (timer > maxTimer)
        {
            for (int i = 0; i < tabPos.Length; i++)
            {
                switch (elem)
                {
                    case (Elements.Element.FIRE):
                        GameObject goFire = Instantiate(fireManyShots, tabPos[i].position, player.transform.rotation);
                        goFire.GetComponent<BulletScript>().attackScript = attScript;
                        OrientTowardsPoint(targetPos, goFire);
                        Destroy(goFire, 3f);
                        break;
                    case (Elements.Element.ICE):
                        GameObject goIce = Instantiate(iceManyShots, tabPos[i].position, player.transform.rotation);
                        goIce.GetComponent<BulletScript>().attackScript = attScript;
                        OrientTowardsPoint(targetPos, goIce);
                        Destroy(goIce, 3f);

                        break;
                    case (Elements.Element.ELECTRICITY):
                        GameObject goElec = Instantiate(thunderManyShots, tabPos[i].position, player.transform.rotation);
                        goElec.GetComponent<BulletScript>().attackScript = attScript;
                        OrientTowardsPoint(targetPos, goElec);
                        Destroy(goElec, 3f);

                        break;
                    case (Elements.Element.WIND):
                        GameObject goWind = Instantiate(windManyShots, tabPos[i].position, player.transform.rotation);
                        goWind.GetComponent<BulletScript>().attackScript = attScript;
                        OrientTowardsPoint(targetPos, goWind);
                        Destroy(goWind, 3f);

                        break;
                }

            }
            doInstantiate = false;
            timer = 0;
        }
    }
}

