using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrueMeshTrail : MonoBehaviour
{
    public float activeTime = 2.0f;
    public float refresMeshRate;
    private bool isActive;
    public Transform pos;
    // Start is called before the first frame update
    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    public Material mat;
    public float meshDestroy;
    public string ShaderVarRef;
    public float shaderVarRate = 0.1f;
    public float shaderVarRefreshRate = 0.05f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ShaderDash(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
        {
            return;
        }

        isActive = true;

        Debug.Log("pomme"); 
        StartCoroutine(ActivateTrail(activeTime));


    }


    IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= refresMeshRate;

            if (skinnedMeshRenderers == null)
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(pos.position, pos.rotation);
                gObj.transform.localScale = pos.localScale;

                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();
                Mesh mesh = new Mesh();

                skinnedMeshRenderers[i].BakeMesh(mesh);
                mf.mesh = mesh;
                mr.material = mat;

                StartCoroutine(AnimateFade(mr.material, 0, shaderVarRate, shaderVarRefreshRate));

                Destroy(gObj, meshDestroy);
            }

            yield return new WaitForSeconds(refresMeshRate);
        }
        isActive = false;
    }

    IEnumerator AnimateFade(Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(ShaderVarRef);

        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(ShaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
