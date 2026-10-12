using TMPro;
using UnityEngine;

public class ExpansionButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI priceText;

    // Changing world references
    [SerializeField] private GameObject secondHouseBF;
    [SerializeField] private GameObject secondHouse1F;

    // Expansion visual changes
    [SerializeField] private GameObject emptyRoomPrefab;
    [SerializeField] private GameObject emptyBasementPrefab;
    [SerializeField] private Sprite[] firstExpansionSprites = new Sprite[2];

    private GameObject basement0;
    private GameObject basement1;

    [SerializeField] private CreateGenerator createGenerator;

    [SerializeField] private CameraMovement camMovement;
    [SerializeField] private RoomManager roomManager;

    public int currentExpansion;
    private int[] expansionCosts = new int[2];

    private void Start()
    {
        expansionCosts[0] = 30000;
        expansionCosts[1] = 500000;

        //CheckForExpansion(); checked in roommanager instead
        camMovement.ExpansionZoneChange();

        if (currentExpansion == 0)
            InitialExpansionChanges();
        else if (currentExpansion == 1)
            FirstExpansionChanges();
    }

    public void CheckForExpansion()
    {
        if (GameManager.Instance.roomsMade == 2)
        {
            if (currentExpansion == 0)
            {
                gameObject.transform.position = new Vector3(-6.77f, 2.14f, 0);
                priceText.text = GameManager.Instance.FormatNumber(expansionCosts[0]);
                //createGenerator.gameObject.SetActive(false);

                camMovement.ExpansionZoneChange();
            }
            else
                return;
        }
        if (GameManager.Instance.roomsMade == 6)
        {
            if (currentExpansion == 1)
            {
                Debug.Log("Second expansion time");
            }
        }
        // else if (next time to expand here)
    }

    public void onClickExpansionButton()
    {
        FindObjectOfType<AudioManager>().Play("buttonPress", 0.5f);

        if (currentExpansion == 0)
        {
            if (GameManager.Instance.moneyAmount >= expansionCosts[currentExpansion])
            {
                GameManager.Instance.moneyAmount -= expansionCosts[currentExpansion];

                Destroy(basement0);
                Destroy(basement1);
                FirstExpansionChanges();

                FindObjectOfType<AudioManager>().Play("expansionSound", 0.6f);
                FindObjectOfType<AudioManager>().Play("buildingSound", 0.6f);
                camMovement.ExpansionZoneChange();

                gameObject.transform.position = new Vector3(-31.43f, 9.91f, 0);
            
                currentExpansion++;
                GameManager.Instance.roomManager.InstantiateButtons();
                GameManager.Instance.roomManager.UpdateMaxsForExpansion();

                SaveManager.Instance.SaveNow();
            }
        }
        else if (currentExpansion == 1)
        {
            // and so on

            // destroy basement0 n basement1 
        }


    }

    private void InitialExpansionChanges()
    {
        basement0 = Instantiate(emptyBasementPrefab, new Vector3(-0.460000008f, -1.14300001f, 0), Quaternion.identity);
        basement1 = Instantiate(emptyBasementPrefab, new Vector3(-0.460000008f, -3.40499997f, 0), Quaternion.identity);
    }

    private void FirstExpansionChanges() // Change function to account for all expansion changes rather than only first one.
    {
        // temp visual change
        SpriteRenderer spriteRendererB = secondHouseBF.GetComponent<SpriteRenderer>(); spriteRendererB.sprite = firstExpansionSprites[0];
        SpriteRenderer spriteRenderer1 = secondHouse1F.GetComponent<SpriteRenderer>(); spriteRenderer1.sprite = firstExpansionSprites[0];

        basement0 = Instantiate(emptyBasementPrefab, new Vector3(-7.44099998f, -1.13999999f, 0), Quaternion.identity);
        basement1 = Instantiate(emptyBasementPrefab, new Vector3(-7.44099998f, -3.40899992f, 0), Quaternion.identity);
    }
}
