using UnityEngine;

public class HeroGetResource : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var generator = other.gameObject.GetComponent<ResourceGenerator>();
        if (generator == null) return;

        generator.GenerateResource(generator.GetProduction());
    }
}
