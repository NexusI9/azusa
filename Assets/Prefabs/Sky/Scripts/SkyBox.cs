using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SkyBox : MonoBehaviour
{
    public Material[] skyMaterials = new Material[]{ };
    private int currentIndex = 0;

    private void OnEnable()
    {
        EnvironmentManager.OnUpdateMoment += OnUpdateMoment;
        if (skyMaterials[currentIndex])
        {
            RenderSettings.skybox = skyMaterials[currentIndex];
        }

    }

    private void OnDisable()
    {
        EnvironmentManager.OnUpdateMoment -= OnUpdateMoment;
    }


    private void OnUpdateMoment(int index, DayMoment dayMoment)
    {
        Material currentMaterial = skyMaterials[currentIndex];
        Material newMaterial = skyMaterials[index];
        Debug.Log(index);
        if (newMaterial && currentMaterial && index != currentIndex)
        {
            currentIndex = index;
            StopCoroutine("SwitchSkyboxMaterial");
            StartCoroutine(SwitchSkyboxMaterial(currentMaterial, newMaterial, dayMoment.transitionTime));
        }

    }

    private IEnumerator SwitchSkyboxMaterial(Material startMaterial, Material endMaterial, float duration)
    {
        float time = 0;
        Material transitionMaterial = new Material(startMaterial);

        while(time < duration){

            float lerpFactor = time / duration;
            transitionMaterial.Lerp(startMaterial, endMaterial, lerpFactor);
            RenderSettings.skybox = transitionMaterial;

            time += Time.deltaTime;
            yield return null;
        }

        //assign end material to make sure
        RenderSettings.skybox = endMaterial;

    }
}
