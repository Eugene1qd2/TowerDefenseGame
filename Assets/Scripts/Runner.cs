using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class Runner : MonoBehaviour
{
    [SerializeField] GameObject HealthBar;
    public float MaxHealth = 100f;
    public float Health;
    public float movementSpeed = 1f;
    private float defaultMovementSpeed;
    public int coins = 1;

    public float defDamageAspect = 1f;
    public float fireDamageAspect = 3f;
    public float iceDamageAspect = 2f;
    public float nuklearDamageAspect = 5f;
    public float explosiveDamegeAspect = 5f;

    private float FreezTime = 0f;
    private float NukeTime = 0f;
    public float NukeDelay = 0.35f;

    private int pos = 0;
    private Vector2 curPoint;
    private BuildingGrid BuildingGrid;


    private float timeAfterDeath = 3f;

    public void Hit(float damage, DamageType type)
    {
        switch (type)
        {
            case DamageType.defDamage:
                Health -= damage * defDamageAspect;
                break;
            case DamageType.fireDamage:
                Health -= damage * fireDamageAspect;
                break;
            case DamageType.iceDamage:
                FreezTime = 1.5f;
                movementSpeed = defaultMovementSpeed / 2f;
                Health -= damage * iceDamageAspect;
                break;
            case DamageType.nukeDamage:
                NukeTime = 5.5f;
                Health -= damage * nuklearDamageAspect;
                break;
            case DamageType.explDamage:
                Health -= damage * explosiveDamegeAspect;
                break;
            case DamageType.poisonDamage:
                Health -= damage;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            BuildingGrid.UpdateHealth(-1);
            Destroy(gameObject);
        }
        if (other.tag == "Tower")
        {
            TowerAttack tower = other.gameObject.GetComponent<TowerAttack>();
            tower.AddRunner(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Tower")
        {
            TowerAttack tower = other.gameObject.GetComponent<TowerAttack>();
            tower.RemoveRunner(this);
        }
    }

    void Start()
    {
        defaultMovementSpeed = movementSpeed;
        BuildingGrid = GameObject.FindGameObjectWithTag("Floar").GetComponent<BuildingGrid>();
        MaxHealth = MaxHealth * BuildingGrid.DiffValue + (MaxHealth * 0.3f) * (BuildingGrid.spawner.WavesCount / 5);
        Health = MaxHealth;
        curPoint = new Vector3(BuildingGrid.fieldPath[pos].X, BuildingGrid.fieldPath[pos].Y);
    }
    private void Update()
    {
        if (Health > 0)
        {
            HealthBar.transform.LookAt(Camera.main.transform);
            Image[] images = HealthBar.GetComponentsInChildren<Image>();
            images[1].fillAmount = Health / MaxHealth;
        }
    }
    void FixedUpdate()
    {
        if (Health <= 0)
        {
            if (HealthBar != null)
            {
                Destroy(HealthBar.gameObject);
                Destroy(gameObject.GetComponent<CapsuleCollider>());
                BuildingGrid.KillsCount++;
            }
            BuildingGrid.UpdateCoins(coins);
            coins = 0;
            this.transform.localScale = this.transform.localScale - new Vector3(0.004f, 0.004f, 0.004f);
            timeAfterDeath -= Time.fixedDeltaTime;
            if (timeAfterDeath <= 0)
            {
                Destroy(this.gameObject);
            }
            return;
        }
        if (FreezTime >= 0f)
        {
            FreezTime -= Time.fixedDeltaTime;
        }
        else
        {
            movementSpeed = defaultMovementSpeed;
        }
        if (NukeTime>=0f)
        {
            NukeTime -= Time.fixedDeltaTime;
            NukeDelay -= Time.fixedDeltaTime;
            if (NukeDelay<0f)
            {
                NukeDelay = 0.35f;
                Hit(2f, DamageType.poisonDamage);
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(curPoint.x, transform.position.y, curPoint.y), movementSpeed * Time.fixedDeltaTime);
        Vector3 newDir = Vector3.RotateTowards(transform.forward, (new Vector3(curPoint.x, transform.position.y, curPoint.y) - transform.position), 1f, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);
        Vector3 dist = (transform.position - new Vector3(curPoint.x, transform.position.y, curPoint.y));
        dist.y = 0;
        float distance = dist.sqrMagnitude;

        if (distance <= .1f)
        {
            if (pos < BuildingGrid.fieldPath.Count - 1)
            {
                pos++;
                curPoint = new Vector2(BuildingGrid.fieldPath[pos].X, BuildingGrid.fieldPath[pos].Y);
            }
            else
            {
                curPoint = new Vector2(37, 7);
            }
        }

    }
}
