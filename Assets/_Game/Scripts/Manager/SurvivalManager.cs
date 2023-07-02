using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SurvivalManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private List<Survival> survivalPrefabs;

    private Survival currentLevel;
    private int botAmount;
    private int survivalIndex;
    private List<Transform> startPoints;
    private List<Animal> animals = new List<Animal>();
    private List<Animal> animalsInGame = new List<Animal>();

    private void Awake()
    {
        survivalIndex = PlayerPrefs.GetInt(Constant.SURVIVAL, 0);
    }

    private void Start()
    {
        //OnStart Game
        LoadSurvival(survivalIndex);
        UIManager.Instance.OpenUI<Loading>();
        OnInit();
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.IsMode(GameMode.Survival))
        {
            if (GameManager.Instance.IsState(GameState.InGame))
            {
                if (animalsInGame.Count < botAmount)
                {
                    for (int i = 0; i < animals.Count; i++)
                    {
                        if (!animals[i].gameObject.activeSelf && !animals[i].IsDeath)
                        {
                            animals[i].gameObject.SetActive(true);
                            animals[i].ChangeState(new PatrolStateA());
                            animalsInGame.Add(animals[i]);
                            animals.Remove(animals[i]);
                        }
                    }
                    if (animals.Count == 0)
                    {
                        GenerateSurvivalAnimal();
                    }
                }
            }
        }

    }
    private void GenerateSurvivalAnimal()
    {
        for (int i = 0; i < botAmount; i++)
        {
            int randomIndex = Random.Range(0, startPoints.Count);
            Animal animal = SimplePool.Spawn<Animal>(PoolType.Animal, startPoints[randomIndex].position, Quaternion.identity);
            animal.OnInit();
            animal.gameObject.SetActive(false);
            animals.Add(animal);
        }

    }
    public void OnStartGame()
    {
        OnRetry();
        GameManager.Instance.ChangeMode(GameMode.Survival);
        GameManager.Instance.ChangeState(GameState.InGame);

    }
    public void OnFinishGame()
    {
        GameManager.Instance.ChangeState(GameState.EndGame);
        ResetListAnimal(animals);
        ResetListAnimal(animalsInGame);
        //Save Gold
        //UNDONE
        OnResetSurvival();
    }
    public void LoadSurvival(int survival)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }

        if (survival < survivalPrefabs.Count)
        {
            currentLevel = Instantiate(survivalPrefabs[survival]);

        }
        else
        {
            //TODO: level vuot qua limit
        }
    }
    public void OnInit()
    {

        botAmount = survivalPrefabs[survivalIndex].GetBotAmount();
        startPoints = survivalPrefabs[survivalIndex].GetStartPoints();

        //update navmesh data
        NavMesh.RemoveAllNavMeshData();
        NavMesh.AddNavMeshData(currentLevel.GetNavMeshData());

        GenerateSurvivalAnimal();

        player.OnInitSurvival();
        player.transform.position = survivalPrefabs[survivalIndex].GetStartPoint().position;

    }

    public void OnResetSurvival()
    {
        SimplePool.CollectAll();
        animals.Clear();
        animalsInGame.Clear();
        player.SetTransformPosition(survivalPrefabs[survivalIndex].GetStartPoint());
        player.CharacterInfo.Hide();
        player.OnInit();
    }

    internal void OnRetry()
    {
        OnResetSurvival();
        LoadSurvival(survivalIndex);
        OnInit();
        GameManager.Instance.ChangeState(GameState.GameMenu);
        UIManager.Instance.OpenUI<GameMenu>();
    }

    internal void OnNextLevelSurvival()
    {
        survivalIndex++;
        PlayerPrefs.SetInt(Constant.SURVIVAL, survivalIndex);
        PlayerPrefs.Save();
        OnResetSurvival();
        LoadSurvival(survivalIndex);
        OnInit();
        UIManager.Instance.OpenUI<GameMenu>();
    }
    public List<Animal> GetAnimalsInGame()
    {
        return animalsInGame;
    }
    public void ResetListAnimal(List<Animal> lists)
    {
        for (int i = 0; i < lists.Count; i++)
        {
            lists[i].ChangeState(null);
            lists[i].MoveStop();
        }
    }
}