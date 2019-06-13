using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SurvivalGameManager : MonoBehaviour
{
    public static SurvivalGameManager instance;
    public Text gameOverText;
    public Text roundText;
    public GameObject PrefabArchP1;
    public GameObject PrefabArchP2;
    public GameObject PrefabMinerP1;
    public GameObject PrefabMinerP2;
    public GameObject PrefabEvilWormP1;
    public GameObject PrefabEvilWormP2;
    public GameObject PrefabKnightP1;
    public GameObject PrefabKnightP2;
    public GameObject PrefabGolemP1;
    public GameObject PrefabGolemP2;
    public GameObject PrefabNecromancerP1;
    public GameObject PrefabNecromancerP2;
    public GameObject PrefabArrowStormP1;
    public RectTransform ArrowStormCoverP1;
    public RectTransform HealTeamCoverP1;
    public RectTransform EvilWormCoverP1;
    public RectTransform GolemCoverP1;
    public RectTransform NecromancerCoverP1;
    public Text player1MoneyText;
    public enum Team { Player1, Player2 }
    public const int leftEdge = -10;
    public const int rightEdge = 80;

    private List<GameObject> unitsP1 = new List<GameObject>();
    private List<GameObject> unitsP2 = new List<GameObject>();
    private GameObject lastArrowStorm;
    private bool gameOver = false;
    private const int startMoney = 100;
    private const int updateMoney = 50;
    private int player1Money = startMoney;
    private int player2Money = startMoney;
    private const int archerCost = 50;
    private const int evilWormCost = 150;
    private const int minerCost = 100;
    private const int knightCost = 75;
    private const int golemCost = 150;
    private const int necromancerCost = 200;
    private const float updateTime = 2;
    private float lastMoneyTime = 0;
    private int player1Miners = 0;
    private int player2Miners = 0;
    private const int rightCameraEdge = 74;
    private const int leftCameraEdge = -4;
    private float furthestXP1;
    private float furthestXP2;
    private const int distanceBetweenCameras = 12;
    private const int IconResetWidth = 70;
    private const int CameraSpeed = 20;
    private int roundNumber = 1;
    private const int coolDownTimeForWorms = 30;
    private const int coolDownTimeForNecromancers = 25;
    private const int coolDownTimeForGolems = 20;
    private const int coolDownTimeForArrowStorm = 40;
    private const int coolDownTimeForHealTeam = 60;
    private float p1TimeOfLastGolem = 0;
    private float p2TimeOfLastGolem = 0;
    private float p1TimeOfLastWorm = 0;
    private float p2TimeOfLastWorm = 0;
    private float p1TimeOfLastNecromancer = 0;
    private float p2TimeOfLastNecromancer = 0;
    private float p1TimeOfLastArrowStorm = 0;
    private float p1TimeOfLastHealTeam = 0;
    private bool dragStarted = false;
    private float dragStart = 0;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
            return;

        if (unitsP2.Count == 0 && player2Money < archerCost)
        {
            UpdateMoney();
            roundNumber++;
            roundText.text = "Round " + roundNumber;
        }

        // Spawn for P1?
        if (Input.GetKeyDown(KeyCode.S))
            CreateArcher();
        
        else if (Input.GetKeyDown(KeyCode.D))
            CreateEvilWorm();
        
        else if (Input.GetKeyDown(KeyCode.A))
            CreateKnight();
        
        else if (Input.GetKeyDown(KeyCode.Q))
            CreateGolem();
        
        else if (Input.GetKeyDown(KeyCode.W))
            CreateNecromancer();
        
        else if (Input.GetKeyDown(KeyCode.E))
            CreateArrowStorm();
        
        else if (Input.GetKeyDown(KeyCode.R))
            CreateHealTeam();
        
        UpdatePlayer1MoneyText();

        // Spawn for P2?
        int rand = Random.Range(0, 5);
        
        if (player2Money >= archerCost && rand == 0)
        {
            player2Money -= archerCost;
            unitsP2.Add(Instantiate(PrefabArchP2));
        }
        else if (player2Money >= evilWormCost && rand == 1 && Time.time - p2TimeOfLastWorm > coolDownTimeForWorms)
        {
            player2Money -= evilWormCost;
            unitsP2.Add(Instantiate(PrefabEvilWormP2));
            p2TimeOfLastWorm = Time.time;
        }
        else if (player2Money >= knightCost && rand == 2)
        {
            player2Money -= knightCost;
            unitsP2.Add(Instantiate(PrefabKnightP2));
        }
        else if (player2Money >= golemCost && rand == 3 && Time.time - p2TimeOfLastGolem > coolDownTimeForGolems)
        {
            player2Money -= golemCost;
            unitsP2.Add(Instantiate(PrefabGolemP2));
            p2TimeOfLastGolem = Time.time;
        }
        else if (player2Money >= necromancerCost && rand == 4 && Time.time - p2TimeOfLastNecromancer > coolDownTimeForNecromancers)
        {
            player2Money -= necromancerCost;
            unitsP2.Add(Instantiate(PrefabNecromancerP2));
            p2TimeOfLastNecromancer = Time.time;
        }

        if (lastArrowStorm != null && p1TimeOfLastArrowStorm + 5 < Time.time)
            Destroy(lastArrowStorm);

        unitsP1.RemoveAll(IsNullOrOutOfBounds);
        unitsP2.RemoveAll(IsNullOrOutOfBounds);

        UpdateAbilityCovers();
        MoveCamera();
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateAbilityCovers()
    {
        ArrowStormCoverP1.sizeDelta = new Vector2(Mathf.Max(((coolDownTimeForArrowStorm - (Time.time - p1TimeOfLastArrowStorm)) / coolDownTimeForArrowStorm)
                                                    * IconResetWidth, 0), ArrowStormCoverP1.sizeDelta.y);

        HealTeamCoverP1.sizeDelta = new Vector2(Mathf.Max(((coolDownTimeForHealTeam - (Time.time - p1TimeOfLastHealTeam)) / coolDownTimeForHealTeam)
                                                    * IconResetWidth, 0), HealTeamCoverP1.sizeDelta.y);

        EvilWormCoverP1.sizeDelta = new Vector2(Mathf.Max(((coolDownTimeForWorms -  (Time.time - p1TimeOfLastWorm)) / coolDownTimeForWorms)
                                                    * IconResetWidth, 0), EvilWormCoverP1.sizeDelta.y);

        GolemCoverP1.sizeDelta = new Vector2(Mathf.Max(((coolDownTimeForGolems - (Time.time - p1TimeOfLastGolem)) / coolDownTimeForGolems)
                                                    * IconResetWidth, 0), GolemCoverP1.sizeDelta.y);

        NecromancerCoverP1.sizeDelta = new Vector2(Mathf.Max(((coolDownTimeForNecromancers - (Time.time - p1TimeOfLastNecromancer)) / coolDownTimeForNecromancers)
                                                    * IconResetWidth, 0), NecromancerCoverP1.sizeDelta.y);
    }

    // If is time to update money we do and reset counter
    private void UpdateMoney()
    {
        player1Money += startMoney + updateMoney * roundNumber;
        player2Money += startMoney + (updateMoney + 10) * roundNumber;

        UpdatePlayer1MoneyText();

        lastMoneyTime -= updateTime;
    }
        
    public void CreateKnight()
    {
        if (player1Money >= knightCost)
        {
            player1Money -= knightCost;
            unitsP1.Add(Instantiate(PrefabKnightP1));
        }
    }

    public void CreateArcher()
    {
        if (player1Money >= archerCost)
        {
            player1Money -= archerCost;
            unitsP1.Add(Instantiate(PrefabArchP1));
        }
    }

    public void CreateEvilWorm()
    {
        if (player1Money >= evilWormCost && Time.time - p1TimeOfLastWorm > coolDownTimeForWorms)
        {
            player1Money -= evilWormCost;
            unitsP1.Add(Instantiate(PrefabEvilWormP1));
            p1TimeOfLastWorm = Time.time;
        }
    }

    public void CreateGolem()
    {
        if (player1Money >= golemCost && Time.time - p1TimeOfLastGolem > coolDownTimeForGolems)
        {
            player1Money -= golemCost;
            unitsP1.Add(Instantiate(PrefabGolemP1));
            p1TimeOfLastGolem = Time.time;
        }
    }

    public void CreateNecromancer()
    {
        if (player1Money >= necromancerCost && Time.time - p1TimeOfLastNecromancer > coolDownTimeForNecromancers)
        {
            player1Money -= necromancerCost;
            unitsP1.Add(Instantiate(PrefabNecromancerP1));
            p1TimeOfLastNecromancer = Time.time;
        }
    }

    public void CreateArrowStorm()
    {
        if (Time.time - p1TimeOfLastArrowStorm > coolDownTimeForArrowStorm)
        {
            lastArrowStorm = Instantiate(PrefabArrowStormP1);
            p1TimeOfLastArrowStorm = Time.time;
        }
    }

    public void CreateHealTeam()
    {
        if (Time.time - p1TimeOfLastHealTeam > coolDownTimeForHealTeam)
        {
            unitsP1.ForEach(HealUnit);
            p1TimeOfLastHealTeam = Time.time;
        }
    }

    private void MoveCamera()
    {
        float xInput = Input.GetAxis("Horizontal");

        if (xInput > .2f && transform.position.x < rightCameraEdge - 5 || xInput < -.2f && transform.position.x > leftCameraEdge + 5)
            transform.Translate(xInput * Time.deltaTime * CameraSpeed, 0, 0);

        if (Input.GetMouseButtonDown(0))
        {
            dragStart = Input.mousePosition.x;
            dragStarted = true;
        }
        else if (dragStarted)
        {
            float distance = dragStart - Input.mousePosition.x;
            if (distance > .2f && transform.position.x < rightCameraEdge - 5 || distance < -.2f && transform.position.x > leftCameraEdge + 5)
                transform.Translate(distance * Time.deltaTime * 2, 0, 0);

            if (Input.GetMouseButtonUp(0))
                dragStarted = false;
            else
                dragStart = Input.mousePosition.x;
        }

        if (transform.position.x > rightCameraEdge - 5)
            transform.position = new Vector3(rightCameraEdge - 5, transform.position.y, transform.position.z);

        else if (transform.position.x < leftCameraEdge + 5)
            transform.position = new Vector3(leftCameraEdge + 5, transform.position.y, transform.position.z);


    }

    private bool IsNullOrOutOfBounds(GameObject obj)
    {
        if (obj == null)
            return true;
        
        if (obj.transform.position.x > rightEdge + 7 || obj.transform.position.x < leftEdge - 7)
        {
            obj.SendMessage("Kill", SendMessageOptions.DontRequireReceiver);
            return true;
        }

        return false;
    }

    private void HealUnit(GameObject obj)
    {
        obj.SendMessage("GiveHealth", 75, SendMessageOptions.DontRequireReceiver);
    }

    private void FindFurthestXP1(GameObject obj)
    {
        if (obj.transform.position.x > furthestXP1)
            furthestXP1 = obj.transform.position.x;
    }

    private void FindFurthestXP2(GameObject obj)
    {
        if (obj.transform.position.x < furthestXP2)
            furthestXP2 = obj.transform.position.x;
    }

    public void GameOver()
    {
        gameOver = true;
        gameOverText.text = "You Survived " + roundNumber + " Round(s)!";
        gameOverText.gameObject.SetActive(true);
    }

    public void AddToPlayer1Units(GameObject obj)
    {
        unitsP1.Add(obj);
    }

    public void AddToPlayer2Units(GameObject obj)
    {
        unitsP2.Add(obj);
    }

    public void Player1AddMiner()
    {
        player1Miners++;
    }

    public void Player2AddMiner()
    {
        player2Miners++;
    }

    public void Player1RemoveMiner()
    {
        player1Miners--;
    }

    public void Player2RemoveMiner()
    {
        player2Miners--;
    }

    private void UpdatePlayer1MoneyText()
    {
        player1MoneyText.text = "$" + player1Money;
    }

    public void PlayAgainButtonClicked()
    {
        ResetScene();
    }

    public void QuitButtonClicked()
    {
        SceneManager.LoadScene("Menu");
    }
}
