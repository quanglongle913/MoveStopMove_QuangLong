using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BotAISpawner : PooledObject
{
    [SerializeField] GameObject poolMaster;
    [SerializeField] private ObjectPool poolObject;
    [SerializeField] float offset;
    [SerializeField] private float size_x;
    [SerializeField] private float size_z;
    [SerializeField] private GameManager _GameManager;

   /* private void Start()
    {
        _GameManager = GameManager.Instance;
    }*/
    private void Update()
    {
        if (_GameManager.GameState == GameState.Loading && !_GameManager.IsInitBotAI)
        {
  
            GenerateBotAI(_GameManager.TotalBotAI, GeneratePoolObjectPosition(poolMaster.transform.position, _GameManager.TotalBotAI));
            _GameManager.IsInitBotAI = true;
        }
        else if (_GameManager.GameState == GameState.InGame && _GameManager.BotAIListEnable.Count < _GameManager.TotalBotAI_InGame && _GameManager.IsInitBotAI)
        {
            if (_GameManager.BotAIListStack.Count > 0)
            {
                int randomIndex = Random.Range(0, _GameManager.BotAIListStack.Count);
                _GameManager.BotAIListStack[randomIndex].gameObject.SetActive(true);
                _GameManager.BotAIListEnable.Add(_GameManager.BotAIListStack[randomIndex]);
                _GameManager.BotAIListStack.Remove(_GameManager.BotAIListStack[randomIndex]);
                _GameManager.TotalBotAI--;
            }
            else
            { 
                //WIN Action GameState = Endgame
            }
           
        }
    }
    protected List<Vector3> GeneratePoolObjectPosition(Vector3 a_root, int numCount)
    {
        List<Vector3> listPoolObjectPosition = new List<Vector3>();
        int Row = Mathf.CeilToInt(Mathf.Sqrt(numCount));
        int Column = Row;
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Column; j++)
            {
                int index = Row * j + i;
                Vector3 objectPosition = new Vector3((j - (Row / 2)) + offset * j + a_root.x, 0.05f + a_root.y, ((Column / 2) - i) - offset * i + a_root.z);
                listPoolObjectPosition.Add(objectPosition);
            }
        }
        return listPoolObjectPosition;
    }
    
    private void GenerateBotAI(int totalBotAI, List<Vector3> listPoolObjectPosition)
    {
        for (int i = 0; i < totalBotAI; i++)
        {
            //Debug.Log("BotAI:" + listPoolObjectPosition.Count);
            int randomIndex = Random.Range(0, listPoolObjectPosition.Count);
            PooledObject botAIObject = Spawner(poolObject, poolMaster,false);
            botAIObject.transform.position = listPoolObjectPosition[randomIndex];
            listPoolObjectPosition.Remove(listPoolObjectPosition[randomIndex]);
            int randomColor = Random.Range(0, 5);
            ColorType _colorType = (ColorType)randomColor;
            BotAI botAI = botAIObject.GetComponent<BotAI>();
            botAI.ColorType = _colorType;
            botAI.InGamneExp = 100;
            botAI.ChangeColor(botAI.gameObject, _colorType);
            //Bot weapon = random (.) weapon List Have
            //int weaponRandom = Random.Range(0, gameManager.SaveData.BotAIData.botAIInfo[i].weapon); //weapon power (bot have)
            //Debug.Log("BotAI:" + weapon);
            botAI.WeaponIndex = _GameManager.SaveData.BotAIData.BotAIInfo[i].Weapon;
            //set paint= _characterSkin[1] skin with inde =random 0->index
            //int paintRandom = Random.Range(0, gameManager.SaveData.BotAIData.botAIInfo[i]._characterSkin[1].index);
            //Debug.Log(gameManager.SaveData.BotAIData.botAIInfo[i].characterSkin[1].Index);
            UpdateAccessoriesSkinShop(botAI,i);
            botAI.CharacterName = _GameManager.SaveData.BotAIData.BotAIInfo[i].BotAI_name;
            _GameManager.BotAIListStack.Add(botAI);
            //Debug.Log("BotAI:" +weapon);
        }
    }
    private void UpdateAccessoriesSkinShop(BotAI botAI, int indexBotAI)
    {
        BotAIInfo botAIInfo = _GameManager.SaveData.BotAIData.BotAIInfo[indexBotAI];
        botAI.HideAllSetFullsSkin();
        botAI.HideAllSkin();
        if (botAIInfo.playerSkinShopState == PlayerSkinShopState.SetFull)
        {
            //Active Setfull Skin
            botAI.ActiveSetFullsSkin(botAIInfo.CharacterSkin[(int)SkinType.SetFull].Index);
        }
        else
        {
            if (GetAccessoriesSelected(_GameManager.HatsData))
            {
                botAI.ActiveHatsSkin(botAIInfo.CharacterSkin[(int)SkinType.Hat].Index);
            }
            if (GetAccessoriesSelected(_GameManager.PantsData))
            {
                botAI.SetAccessorisSkinMat(botAI.PantsSkin, _GameManager.PantsData, botAIInfo.CharacterSkin[(int)SkinType.Pant].Index);
                botAI.ShowPantsSkin();
            }
            if (GetAccessoriesSelected(_GameManager.ShieldData))
            {
                botAI.ActiveSheildsSkin(botAIInfo.CharacterSkin[(int)SkinType.Sheild].Index);
            }
        }
    }
    private bool GetAccessoriesSelected(AccessoriesData accessoriesData)
    {
        bool isCheck = false;
        for (int i = 0; i < accessoriesData.Accessories.Length; i++)
        {
            if (accessoriesData.Accessories[i].Selected)
            {
                isCheck = true;
                break;
            }

        }
        return isCheck;
    }
    /*void OnDrawGizmos()
    {
            Gizmos.color = Color.blue;
            int Row = Mathf.CeilToInt(Mathf.Sqrt(_GameManager.TotalBotAI));
            int Column = Row;
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    int index = Row * j + i;
                    Vector3 objectPosition = new Vector3((j - (Row / 2)) + offset * j + poolMaster.transform.position.x, 0.05f + poolMaster.transform.position.y, ((Column / 2) - i) - offset * i + poolMaster.transform.position.z);
                    drawRectangle(objectPosition);
                }
            }
        
    }*/
    /*    private void drawRectangle(Vector3 point)
        {
            //Top Left
            Vector3 topL = new Vector3(point.x - size_x, point.y, point.z + size_z);
            //Top Right
            Vector3 topR = new Vector3(point.x + size_x, point.y, point.z + size_z);
            //Bot Right
            Vector3 botR = new Vector3(point.x + size_x, point.y, point.z - size_z);
            //Bot Left
            Vector3 botL = new Vector3(point.x - size_x, point.y, point.z - size_z);

            Gizmos.DrawLine(topL, topR);
            Gizmos.DrawLine(topR, botR);
            Gizmos.DrawLine(botR, botL);
            Gizmos.DrawLine(botL, topL);
        }*/
}
