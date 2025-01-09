using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class TextureModifier : MonoBehaviour
{
    public float brushSize = 10f; // Taille du pinceau
    Texture2D tex;
    private Renderer objectRenderer;

    private void Awake()
    {
    }
    void Start()
    {

        tex = new Texture2D(512, 512, TextureFormat.ARGB32, false);
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer == null)
        {
            objectRenderer = GetComponentInChildren<Renderer>();
            if (objectRenderer != null)

                Debug.Log(objectRenderer.gameObject);
            enabled = false; // Désactiver le script si le Renderer est manquant
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    // Obtenez les coordonnées UV du point de collision
                    Vector2 uvCoord = hit.textureCoord;

                    // Appliquez la texture modifiée à l'emplacement du clic
                    ModifyTexture(uvCoord);
                }
            }
        }
    }

    void ModifyTexture(Vector2 uvCoord)
    {
        
        // Assurez-vous que la texture modifiée est valide
        if (tex == null)
        {
            Debug.LogError("La texture modifiée n'est pas définie.");
            return;
        }

        // Obtenez la position pixel sur la texture modifiée
        int texX = (int)(uvCoord.x * tex.width);
        int texY = (int)(uvCoord.y * tex.height);

        // Obtenez la couleur du pinceau
        Color brushColor = Color.black; // Vous pouvez personnaliser la couleur du pinceau

        // Appliquez la couleur du pinceau à la texture modifiée
        for (int x = Mathf.Max(0, texX - (int)(brushSize / 2)); x < Mathf.Min(tex.width, texX + (int)(brushSize / 2)); x++)
        {
            for (int y = Mathf.Max(0, texY - (int)(brushSize / 2)); y < Mathf.Min(tex.height, texY + (int)(brushSize / 2)); y++)
            {
                tex.SetPixel(x, y, brushColor);
            }
        }

        // Appliquez les modifications à la texture
        tex.Apply();

        // Appliquez la texture modifiée à l'objet dans la scène
        objectRenderer.material.SetTexture("_ModifierTexture", tex);
    }
    //private void OnCollisionEnter(Collision collision)
    //{   
    //    Bounds MyBound = gameObject.GetComponent<MeshFilter>().mesh.bounds;
    //    Bounds OtherBoud = collision.gameObject.GetComponent<MeshFilter>().mesh.bounds;



    //    for(int i = 0; i < collision.contacts.Length; i++)
    //    {
    //        ModifyTexture(new Vector2(collision.contacts[i].point.x, collision.contacts[i].point.y));

    //    }
    //}

    void OnCollisionEnter(Collision collision)
    {
        // Récupérer le composant MeshFilter de l'objet en collision
        MeshFilter otherMeshFilter = collision.collider.GetComponent<MeshFilter>();

        // Vérifier que l'objet en collision a un MeshFilter
        if (otherMeshFilter != null)
        {
            // Obtenez les coordonnées UV du point de collision en projetant le point sur le Mesh
            Vector2 uvCoord = GetUVFromCollision(collision);

            // Appliquez la modification de la texture à l'emplacement du contact
            ModifyTexture(uvCoord);
            Debug.Log(uvCoord);
        }
    }

    // Fonction pour obtenir les coordonnées UV à partir d'une collision
    Vector2 GetUVFromCollision(Collision collision)
    {
        // Assurez-vous que la collision a au moins un point de contact
        if (collision.contacts.Length > 0)
        {
            // Obtenez le point de contact en local space de l'objet
            Vector3 localContactPoint = transform.InverseTransformPoint(collision.contacts[0].point);

            // Utilisez ce point local pour calculer les coordonnées UV
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter != null && meshFilter.mesh != null)
            {
                return GetUVFromLocalPoint(meshFilter.mesh, localContactPoint);
            }
        }

        return Vector2.zero;
    }

    // Fonction pour obtenir les coordonnées UV à partir d'un point en local space
    //Vector2 GetUVFromLocalPoint(Mesh mesh, Vector3 localPoint)
    //{
    //    // Convertissez le point local en coordonnées UV
    //    Vector2 uvCoord = new Vector2(
    //        Mathf.InverseLerp(mesh.bounds.min.x, mesh.bounds.max.x, localPoint.x),
    //        Mathf.InverseLerp(mesh.bounds.min.y, mesh.bounds.max.y, localPoint.y)
    //    );
        
    //    return uvCoord;
    //}

    public Vector2 GetUVFromLocalPoint(Mesh mesh, Vector3 localPoint)
    {
        // Obtenir les triangles et les vertices du mesh
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;

        // Parcourir les triangles du mesh
        for (int i = 0; i < triangles.Length; i += 3)
        {
            // Obtenir les indices des vertices du triangle
            int vertexIndex1 = triangles[i];
            int vertexIndex2 = triangles[i + 1];
            int vertexIndex3 = triangles[i + 2];

            // Obtenir les positions des vertices du triangle
            Vector3 vertex1 = vertices[vertexIndex1];
            Vector3 vertex2 = vertices[vertexIndex2];
            Vector3 vertex3 = vertices[vertexIndex3];

            // Convertir le point local en coordonnées du monde
            Vector3 worldPoint = transform.TransformPoint(localPoint);

            // Vérifier si le point est à l'intérieur du triangle
            if (IsPointInTriangle(worldPoint, vertex1, vertex2, vertex3))
            {
                // Calculer les coordonnées barycentriques
                Vector3 barycentricCoords = CalculateBarycentricCoordinates(worldPoint, vertex1, vertex2, vertex3);

                // Calculer les coordonnées UV en utilisant les coordonnées barycentriques
                Vector2 uv = CalculateUVFromBarycentricCoords(mesh.uv, vertexIndex1, vertexIndex2, vertexIndex3, barycentricCoords);

                // Retourner les coordonnées UV
                return uv;
            }
        }

        // Si le point n'est à l'intérieur d'aucun triangle, retourner Vector2.zero ou une valeur par défaut
        return Vector2.zero;
    }

    // Fonction pour vérifier si un point est à l'intérieur d'un triangle en 3D
    private bool IsPointInTriangle(Vector3 point, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
    {
        float d1, d2, d3;
        bool hasNeg, hasPos;

        d1 = CalculateSign(point, vertex1, vertex2);
        d2 = CalculateSign(point, vertex2, vertex3);
        d3 = CalculateSign(point, vertex3, vertex1);

        hasNeg = (d1 < 0) || (d2 < 0) || (d3 < 0);
        hasPos = (d1 > 0) || (d2 > 0) || (d3 > 0);

        return !(hasNeg && hasPos);
    }

    // Fonction pour calculer le signe de la distance entre un point et une ligne formée par deux autres points
    private float CalculateSign(Vector3 point, Vector3 vertex1, Vector3 vertex2)
    {
        return (point.x - vertex2.x) * (vertex1.z - vertex2.z) - (vertex1.x - vertex2.x) * (point.z - vertex2.z);
    }

    // Fonction pour calculer les coordonnées barycentriques d'un point par rapport à un triangle
    private Vector3 CalculateBarycentricCoordinates(Vector3 point, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
    {
        float denominator = ((vertex2.z - vertex3.z) * (vertex1.x - vertex3.x) + (vertex3.x - vertex2.x) * (vertex1.z - vertex3.z));
        float alpha = ((vertex2.z - vertex3.z) * (point.x - vertex3.x) + (vertex3.x - vertex2.x) * (point.z - vertex3.z)) / denominator;
        float beta = ((vertex3.z - vertex1.z) * (point.x - vertex3.x) + (vertex1.x - vertex3.x) * (point.z - vertex3.z)) / denominator;
        float gamma = 1 - alpha - beta;

        return new Vector3(alpha, beta, gamma);
    }

    // Fonction pour calculer les coordonnées UV en utilisant les coordonnées barycentriques
    private Vector2 CalculateUVFromBarycentricCoords(Vector2[] uv, int index1, int index2, int index3, Vector3 barycentricCoords)
    {
        Vector2 uv1 = uv[index1];
        Vector2 uv2 = uv[index2];
        Vector2 uv3 = uv[index3];

        float u = barycentricCoords.x * uv1.x + barycentricCoords.y * uv2.x + barycentricCoords.z * uv3.x;
        float v = barycentricCoords.x * uv1.y + barycentricCoords.y * uv2.y + barycentricCoords.z * uv3.y;

        return new Vector2(u, v);
    }
}