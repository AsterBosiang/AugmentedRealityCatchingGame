using UnityEngine;

public class Apple : MonoBehaviour
{
    private RectTransform rectTransform;
    private GameManager gameManager;
    
    [SerializeField]
    private float checkRadius = 50f;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (rectTransform == null) return;
        
        if (rectTransform.anchoredPosition.y < -500f)
        {
            Destroy(gameObject);
            return;
        }

        if (gameManager == null || gameManager.arCamera == null || gameManager.basket == null)
        {
            return;
        }

        Vector2 basketScreenPos = RectTransformUtility.WorldToScreenPoint(
            gameManager.arCamera,
            gameManager.basket.position
        );

        float distance = Vector2.Distance(basketScreenPos, rectTransform.position);

        if (distance < checkRadius)
        {
            gameManager.IncreaseScore();
            Destroy(gameObject);
        }
    }
}
