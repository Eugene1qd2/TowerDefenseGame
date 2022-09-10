using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathFind;
using System.Drawing;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BuildingGrid : MonoBehaviour
{
    public int KillsCount = 0;
    public int Difficulty;
    public float DiffValue;
    public Vector2Int GridSize = new Vector2Int(25, 15);
    public SpawnerScript spawner;
    public Building[,] grid;
    private Building flyingBuilding;
    private int[,] field;
    public Camera mainCamera;
    private Point startPoint;
    private Point finishPoint;
    public List<Point> fieldPath;
    public GameObject dir;
    public GameObject dir_up;
    public GameObject dir_down;
    bool available = false;
    public int x, y;
    int PlayerHealth = 20;
    int Coins = 2000;
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject coinsBar;
    bool UIShown = false;
    [SerializeField] GameObject TowerUI;
    [SerializeField] Text SellCost;
    [SerializeField] Text UpCost;
    [SerializeField] GameObject LooseMenu;
    private void Start()
    {
        SetCoins(ChooseDifficulty.StartMoney);
        DiffValue = ChooseDifficulty.DiffValue;
        Difficulty = ChooseDifficulty.difficulty;
        SetCoins(200);

    }
    void Awake()
    {

        grid = new Building[GridSize.x, GridSize.y];
        mainCamera = Camera.main;
        field = new int[GridSize.x, GridSize.y];
        for (int i = 0; i < GridSize.x; i++)
        {
            for (int j = 0; j < GridSize.y; j++)
            {
                field[i, j] = 0;
            }
        }
        startPoint = new Point(0, (GridSize.y - 1) / 2);
        finishPoint = new Point((GridSize.x - 1), (GridSize.y - 1) / 2);
        Direction();
    }
    private void Direction()
    {
        fieldPath = PathNode.FindPath(field, startPoint, finishPoint);
        GameObject[] dirs = GameObject.FindGameObjectsWithTag("direction");
        for (int i = 0; i < dirs.Length; i++)
        {
            Destroy(dirs[i].gameObject);
        }
        for (int i = 0; i < fieldPath.Count; i++)
        {
            Instantiate(dir).transform.position = new Vector3(fieldPath[i].X, 0.5001f, fieldPath[i].Y);
        }
    }
    public void StartPlacingBuilding(Building buildingPrefab)
    {
        TowerUI.active = false;
        UIShown = false;
        if (Coins - buildingPrefab.GetCost() >= 0)
        {
            if (flyingBuilding != null)
            {
                Destroy(flyingBuilding.gameObject);
            }

            flyingBuilding = Instantiate(buildingPrefab);
            flyingBuilding.GetComponent<TowerAttack>().isFlying = true;
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (flyingBuilding != null)
            {
                if (available)
                {
                    PlaceFindBuilding(x, y);
                    Direction();
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (flyingBuilding != null)
            {
                Destroy(flyingBuilding.gameObject);
                flyingBuilding = null;
            }
            else
            {
                if(!UIShown)
                {
                    var groundPlane = new Plane(Vector3.up, Vector3.zero);
                    var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    if (groundPlane.Raycast(ray, out float position))
                    {
                        Vector3 worldPosition = ray.GetPoint(position);

                        x = Mathf.RoundToInt(worldPosition.x);
                        y = Mathf.RoundToInt(worldPosition.z);
                        if (x >= 0 && x <= GridSize.x - 1)
                            if (y >= 0 && y <= GridSize.y - 1)
                                if (grid[x, y] != null)
                                {
                                    TowerUI.active = true;
                                    TowerUI.transform.position = Input.mousePosition + new Vector3(0, -40, 0);
                                    SellCost.text = "SellCost:\n" + ((int)((grid[x, y].cost + grid[x, y].upgradeCost * grid[x, y].thisAttack.upgradeLVL) * 0.5)).ToString();
                                    UpCost.text = "UpCost:\n" + grid[x, y].upgradeCost.ToString();
                                    UIShown = true;

                                }
                    }
                }
                else
                {
                    UIShown = false;
                    TowerUI.active = false;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (flyingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);

                x = Mathf.RoundToInt(worldPosition.x);
                y = Mathf.RoundToInt(worldPosition.z);

                available = true;
                if (x < 0 || x > GridSize.x - flyingBuilding.Size.x) available = false;
                if (y < 0 || y > GridSize.y - flyingBuilding.Size.y) available = false;
                if (available && IsPlaceTaken(x, y)) available = false;
                if (new Point(x, y) == startPoint || new Point(x, y) == finishPoint) available = false;
                if (Coins - flyingBuilding.GetCost() < 0) available = false;
                if (available)
                {
                    int[,] new_field = field;
                    new_field[x, y] = 10;
                    if (PathNode.FindPath(new_field, startPoint, finishPoint) == null) available = false;
                    new_field[x, y] = 0;
                }
                flyingBuilding.transform.position = new Vector3(x, 1f, y);
                flyingBuilding.SetTransperent(available);
            }
        }
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        if (grid[placeX, placeY] != null)
        {
            return true;
        }
        return false;
    }

    private void PlaceFindBuilding(int placeX, int placeY)
    {
        grid[placeX, placeY] = flyingBuilding;
        field[placeX, placeY] = 10;
        flyingBuilding.SetNormal();
        flyingBuilding.gameObject.GetComponent<TowerAttack>().isFlying = false;
        flyingBuilding = null;
    }
    public void UpdateHealth(int amount)
    {
        PlayerHealth += amount;
        healthBar.GetComponent<Text>().text = PlayerHealth.ToString();
        if (PlayerHealth <= 0)
        {
            Time.timeScale = 0f;
            LooseMenu.active = true;
        }
    }
    public void UpdateCoins(int count)
    {
        Coins += count;
        coinsBar.GetComponent<Text>().text = Coins.ToString();
    }

    public void SetCoins(int count)
    {
        Coins = count;
        coinsBar.GetComponent<Text>().text = Coins.ToString();
    }
    public void BuyHealth()
    {
        if (Coins >= 5)
        {
            Coins -= 5;
            PlayerHealth += 1;
            coinsBar.GetComponent<Text>().text = Coins.ToString();
            healthBar.GetComponent<Text>().text = PlayerHealth.ToString();
        }
    }
    public void SellTower()
    {
        UpdateCoins((int)((grid[x, y].cost + grid[x, y].upgradeCost * grid[x, y].thisAttack.upgradeLVL) * 0.5));
        Destroy(grid[x, y].gameObject);
        field[x, y] = 0;
        Debug.Log(x + " " + y);
        Direction();
        UIShown = false;
        TowerUI.active = false;
    }
    public void UpdateTower()
    {
        if(Coins>= grid[x, y].upgradeCost)
        {
            UpdateCoins(-grid[x, y].upgradeCost);
            grid[x, y].thisAttack.LVLUP(); 
        }
        UIShown = false;
        TowerUI.active = false;
    }
}

