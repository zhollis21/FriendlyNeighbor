using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Text gameOverText;
    public Camera cameraP1;
    public Camera cameraP2;
    public GameObject dividingCameraLine;
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
    public GameObject PrefabArrowStormP2;
    public RectTransform ArrowStormCoverP1;
    public RectTransform ArrowStormCoverP2;
    public RectTransform HealTeamCoverP1;
    public RectTransform HealTeamCoverP2;
    public RectTransform EvilWormCoverP1;
    public RectTransform EvilWormCoverP2;
    public RectTransform GolemCoverP1;
    public RectTransform GolemCoverP2;
    public RectTransform NecromancerCoverP1;
    public RectTransform NecromancerCoverP2;
    public Text player1MoneyText;
    public Text player2MoneyText;
    public enum Team { Player1, Player2 }
    public const int leftEdge = -10;
    public const int rightEdge = 80;

    private List<GameObject> unitsP1 = new List<GameObject>();
    private List<GameObject> unitsP2 = new List<GameObject>();
    private bool gameOver = false;
    private const int startMoney = 100;
    private const int updateMoney = 10;
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
    private int minerProfit = 5;
    private const int rightCameraEdge = 74;
    private const int leftCameraEdge = -4;
    private float furthestXP1;
    private float furthestXP2;
    private const int distanceBetweenCameras = 12;
    private GameObject lastArrowStormP1;
    private GameObject lastArrowStormP2;
    private const int IconResetWidth = 40;
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
    private float p2TimeOfLastArrowStorm = 0;
    private float p1TimeOfLastHealTeam = 0;
    private float p2TimeOfLastHealTeam = 0;

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
        
        // Spawn for P1?
        if (Input.GetKeyDown(KeyCode.S))
            P1CreateArcher();

        else if (Input.GetKeyDown(KeyCode.D))
            P1CreateEvilWorm();

        else if (Input.GetKeyDown(KeyCode.Z))
            P1CreateMiner();

        else if (Input.GetKeyDown(KeyCode.A))
            P1CreateKnight();

        else if (Input.GetKeyDown(KeyCode.Q))
            P1CreateGolem();

        else if (Input.GetKeyDown(KeyCode.W))
            P1CreateNecromancer();

        else if (Input.GetKeyDown(KeyCode.E))
            P1CreateArrowStorm();

        else if (Input.GetKeyDown(KeyCode.R))
            P1CreateHealTeam();

        UpdatePlayer1MoneyText();

        // Spawn for P2?
        if (Input.GetKeyDown(KeyCode.K))
            P2CreateArcher();

        else if (Input.GetKeyDown(KeyCode.L))
            P2CreateEvilWorm();

        else if (Input.GetKeyDown(KeyCode.M))
            P2CreateMiner();

        else if (Input.GetKeyDown(KeyCode.J))
            P2CreateKnight();

        else if (Input.GetKeyDown(KeyCode.U))
            P2CreateGolem();

        else if (Input.GetKeyDown(KeyCode.I))
            P2CreateNecromancer();

        else if (Input.GetKeyDown(KeyCode.O))
            P2CreateArrowStorm();

        else if (Input.GetKeyDown(KeyCode.P))
            P2CreateHealTeam();

        UpdatePlayer2MoneyText();

        if (lastArrowStormP1 != null && p1TimeOfLastArrowStorm + 5 < Time.time)
            Destroy(lastArrowStormP1);
        if (lastArrowStormP2 != null && p2TimeOfLastArrowStorm + 5 < Time.time)
            Destroy(lastArrowStormP2);

        UpdateAbilityCovers();
        UpdateMoney();
        MoveCamera();
    }

    public void P1CreateKnight()
    {
        if (player1Money >= knightCost)
        {
            player1Money -= knightCost;
            unitsP1.Add(Instantiate(PrefabKnightP1));
        }
    }

    public void P1CreateArcher()
    {
        if (player1Money >= archerCost)
        {
            player1Money -= archerCost;
            unitsP1.Add(Instantiate(PrefabArchP1));
        }
    }

    public void P1CreateEvilWorm()
    {
        if (player1Money >= evilWormCost && Time.time - p1TimeOfLastWorm > coolDownTimeForWorms)
        {
            player1Money -= evilWormCost;
            Instantiate(PrefabEvilWormP1);
            p1TimeOfLastWorm = Time.time;
        }
    }

    public void P1CreateGolem()
    {
        if (player1Money >= golemCost && Time.time - p1TimeOfLastGolem > coolDownTimeForGolems)
        {
            player1Money -= golemCost;
            unitsP1.Add(Instantiate(PrefabGolemP1));
            p1TimeOfLastGolem = Time.time;
        }
    }

    public void P1CreateNecromancer()
    {
        if (player1Money >= necromancerCost && Time.time - p1TimeOfLastNecromancer > coolDownTimeForNecromancers)
        {
            player1Money -= necromancerCost;
            unitsP1.Add(Instantiate(PrefabNecromancerP1));
            p1TimeOfLastNecromancer = Time.time;
        }
    }

    public void P1CreateMiner()
    {
        if (player1Money >= minerCost)
        {
            player1Money -= minerCost;
            unitsP1.Add(Instantiate(PrefabMinerP1));
        }
    }

    public void P1CreateArrowStorm()
    {
        if (Time.time - p1TimeOfLastArrowStorm > coolDownTimeForArrowStorm)
        {
            lastArrowStormP1 = Instantiate(PrefabArrowStormP1);
            p1TimeOfLastArrowStorm = Time.time;
        }
    }

    public void P1CreateHealTeam()
    {
        if (Time.time - p1TimeOfLastHealTeam > coolDownTimeForHealTeam)
        {
            unitsP1.ForEach(HealUnit);
            p1TimeOfLastHealTeam = Time.time;
        }
    }

    public void P2CreateKnight()
    {
        if (player2Money >= knightCost)
        {
            player2Money -= knightCost;
            unitsP2.Add(Instantiate(PrefabKnightP2));
        }
    }

    public void P2CreateArcher()
    {
        if (player2Money >= archerCost)
        {
            player2Money -= archerCost;
            unitsP2.Add(Instantiate(PrefabArchP2));
        }
    }

    public void P2CreateEvilWorm()
    {
        if (player2Money >= evilWormCost && Time.time - p2TimeOfLastWorm > coolDownTimeForWorms)
        {
            player2Money -= evilWormCost;
            Instantiate(PrefabEvilWormP2);
            p2TimeOfLastWorm = Time.time;
        }
    }

    public void P2CreateGolem()
    {
        if (player2Money >= golemCost && Time.time - p2TimeOfLastGolem > coolDownTimeForGolems)
        {
            player2Money -= golemCost;
            unitsP2.Add(Instantiate(PrefabGolemP2));
            p2TimeOfLastGolem = Time.time;
        }
    }

    public void P2CreateNecromancer()
    {
        if (player2Money >= necromancerCost && Time.time - p2TimeOfLastNecromancer > coolDownTimeForNecromancers)
        {
            player2Money -= necromancerCost;
            unitsP2.Add(Instantiate(PrefabNecromancerP2));
            p2TimeOfLastNecromancer = Time.time;
        }
    }

    public void P2CreateMiner()
    {
        if (player2Money >= minerCost)
        {
            player2Money -= minerCost;
            unitsP2.Add(Instantiate(PrefabMinerP2));
        }
    }

    public void P2CreateArrowStorm()
    {
        if (Time.time - p2TimeOfLastArrowStorm > coolDownTimeForArrowStorm)
        {
            lastArrowStormP2 = Instantiate(PrefabArrowStormP2);
            p2TimeOfLastArrowStorm = Time.time;
        }
    }

    public void P2CreateHealTeam()
    {
        if (Time.time - p2TimeOfLastHealTeam > coolDownTimeForHealTeam)
        {
            unitsP2.ForEach(HealUnit);
            p2TimeOfLastHealTeam = Time.time;
        }
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateAbilityCovers()
    {

        ArrowStormCoverP1.sizeDelta = new Vector2(Mathf.Max(((coolDownTimeForArrowStorm - (Time.time - p1TimeOfLastArrowStorm)) / coolDownTimeForArrowStorm)
                                                    * IconResetWidth, 0), ArrowStormCoverP1.sizeDelta.y);

        ArrowStormCoverP2.sizeDelta = new Vector2(Mathf.Max(((coolDownTimeForArrowStorm - (Time.time - p2TimeOfLastArrowStorm)) / coolDownTimeForArrowStorm)
                                                    * IconResetWidth, 0), ArrowStormCoverP2.sizeDelta.y);

        HealTeamCoverP1.sizeDelta = new Vector2(Mathf.Max(((coolDownTimeForHealTeam - (Time.time - p1TimeOfLastHealTeam)) / coolDownTimeForHealTeam)
                                                    * IconResetWidth, 0), HealTeamCoverP1.sizeDelta.y);

        HealTeamCoverP2.sizeDelta = new Vector2(Mathf.Max(((coolDownTimeForHealTeam - (Time.time - p2TimeOfLastHealTeam)) / coolDownTimeForHealTeam)
                                                    * IconResetWidth, 0), HealTeamCoverP2.sizeDelta.y);

        EvilWormCoverP1.sizeDelta = new Vector2(Mathf.Max(((coolDownTimeForWorms - (Time.time - p1TimeOfLastWorm)) / coolDownTimeForWorms)
                                                    * IconResetWidth, 0), EvilWormCoverP1.sizeDelta.y);

        EvilWormCoverP2.sizeDelta = new Vector2(Mathf.Max(((coolDownTimeForWorms - (Time.time - p2TimeOfLastWorm)) / coolDownTimeForWorms)
                                                    * IconResetWidth, 0), EvilWormCoverP2.sizeDelta.y);

        GolemCoverP1.sizeDelta = new Vector2(Mathf.Max(((coolDownTimeForGolems - (Time.time - p1TimeOfLastGolem)) / coolDownTimeForGolems)
                                                    * IconResetWidth, 0), GolemCoverP1.sizeDelta.y);

        GolemCoverP2.sizeDelta = new Vector2(Mathf.Max(((coolDownTimeForGolems - (Time.time - p2TimeOfLastGolem)) / coolDownTimeForGolems)
                                                    * IconResetWidth, 0), GolemCoverP2.sizeDelta.y);

        NecromancerCoverP1.sizeDelta = new Vector2(Mathf.Max(((coolDownTimeForNecromancers - (Time.time - p1TimeOfLastNecromancer)) / coolDownTimeForNecromancers)
                                                    * IconResetWidth, 0), NecromancerCoverP1.sizeDelta.y);

        NecromancerCoverP2.sizeDelta = new Vector2(Mathf.Max(((coolDownTimeForNecromancers - (Time.time - p2TimeOfLastNecromancer)) / coolDownTimeForNecromancers)
                                                    * IconResetWidth, 0), NecromancerCoverP2.sizeDelta.y);

    }

    // If is time to update money we do and reset counter
    private void UpdateMoney()
    {
        lastMoneyTime += Time.deltaTime;
        if (lastMoneyTime > updateTime)
        {
            player1Money += updateMoney + (player1Miners * minerProfit);
            player2Money += updateMoney + (player2Miners * minerProfit);

            UpdatePlayer1MoneyText();
            UpdatePlayer2MoneyText();

            lastMoneyTime -= updateTime;
        }
    }

    private void MoveCamera()
    {
        // Prep to move camera right
        unitsP1.RemoveAll(IsNullOrOutOfBounds);
        furthestXP1 = leftCameraEdge;
        unitsP1.ForEach(FindFurthestXP1);
        if (furthestXP1 > rightCameraEdge - 10)
            furthestXP1 = rightCameraEdge - 10;

        // Prep to move vamera left
        unitsP2.RemoveAll(IsNullOrOutOfBounds);
        furthestXP2 = rightCameraEdge;
        unitsP2.ForEach(FindFurthestXP2);
        if (furthestXP2 < leftCameraEdge + 10)
            furthestXP2 = leftCameraEdge + 10;

        // Validate they aren't showing duplicate data
        if (furthestXP2 - furthestXP1 < distanceBetweenCameras)
        {
            if (cameraP2.enabled != false)
            {
                dividingCameraLine.SetActive(false);
                cameraP2.enabled = false;
                cameraP1.rect = new Rect(0, 0, 1, 1);
            }
            furthestXP1 = (furthestXP1 + furthestXP2) / 2;
        }
        else if (cameraP2.enabled == false)
        {
            dividingCameraLine.SetActive(true);
            cameraP2.enabled = true;
            cameraP1.rect = new Rect(0, 0, .5f, 1);
        }


        cameraP1.transform.position = new Vector3(furthestXP1, cameraP1.transform.position.y, cameraP1.transform.position.z);
        cameraP2.transform.position = new Vector3(furthestXP2, cameraP2.transform.position.y, cameraP2.transform.position.z);
    }

    private bool IsNullOrOutOfBounds(GameObject obj)
    {
        if (obj == null)
            return true;

        if (obj.transform.position.x > rightEdge + 5 || obj.transform.position.x < leftEdge - 5)
        {
            obj.SendMessage("Kill");
            return true;
        }

        return false;
    }

    private void HealUnit(GameObject obj)
    {
        obj.SendMessage("GiveHealth", 75);
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

    public void GameOver(Team loser)
    {
        gameOver = true;
        gameOverText.text = loser == Team.Player2 ? "Player 1 Wins!" : "Player 2 Wins!";
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
        player1MoneyText.text = "$" + player1Money + " (" + player1Miners + " Miners)";
    }

    private void UpdatePlayer2MoneyText()
    {
        player2MoneyText.text = "$" + player2Money + " (" + player2Miners + " Miners)";
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