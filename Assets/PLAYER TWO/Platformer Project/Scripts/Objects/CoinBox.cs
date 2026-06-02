using UnityEngine;

public class CoinBox : MonoBehaviour
{
    [SerializeField] private Coin[] coins;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material emptyBoxMaterial;

    private readonly float coinOffset = 1.5f;
    private readonly float duration = 0.5f;
    private bool valid = true;
    private int index = 0;

    private void Start()
    {
        if (coins.Length == 0)
        {
            Disable();
        }
        foreach (Coin coin in coins)
        {
            coin.gameObject.SetActive(false);
        }
    }

    public void Trigger()
    {
        if (!valid) return;

        coins[index].gameObject.SetActive(true);
        coins[index].Collect();
        Coin coin = coins[index];
        DoTween.To<Vector3>(
            () => coin.transform.position,
            position => coin.transform.position = position,
            coin.transform.position + new Vector3(0, coinOffset, 0),
            duration
        ).OnComplete(() => coin.gameObject.SetActive(false));
        ++index;
        if (index == coins.Length)
        {
            Disable();
        }
    }

    private void Disable()
    {
        if (valid)
        {
            valid = false;
            meshRenderer.sharedMaterial = emptyBoxMaterial;
        }
    }
}