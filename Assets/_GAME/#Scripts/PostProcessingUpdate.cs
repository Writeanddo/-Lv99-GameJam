using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingUpdate : MonoBehaviour
{
    private PostProcessVolume volume;

    [SerializeField] private float startWeight = 0.25f;
    [SerializeField] private float blocksOn = 0.75f;

    private PlayerController player;

    private void Awake()
    {
        volume = GetComponent<PostProcessVolume>();
        player = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        volume.weight = 0.25f;

        player.PlayerOxygen.OnUpdateOxygen += PlayerOxygen_OnUpdateOxygen;
    }

    private void OnDestroy()
    {
        player.PlayerOxygen.OnUpdateOxygen -= PlayerOxygen_OnUpdateOxygen;
    }

    private void PlayerOxygen_OnUpdateOxygen(float current, float max)
    {
        var oxygenPercent = current / max;

        if (oxygenPercent > blocksOn)
            return;

        volume.weight = (max - current) / max;
    }
}
