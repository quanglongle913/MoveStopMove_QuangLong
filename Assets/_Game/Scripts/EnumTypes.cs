using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSkinShopState {None, UnSetFull, SetFull }
public enum GameState { Loading = 0, GameMenu = 1, InGame = 2, EndGame = 3 , SkinShop=4}
public enum GameMode {Normal, Survival}
public enum AnimType { None, Idle, Run, Attack, Dead }
public enum ColorType { Blue = 0, Yellow = 1, Red = 2, Green = 3, Purple = 4, Player =5 }
public enum WeaponType { Arrow = 0, Axe_0 = 1, Axe_1 = 2, Boomerang = 3, Candy_0 = 4, Candy_1 = 5, Candy_2 = 6, Candy_4 = 7, Hammer = 8, Knife = 9, Uzi = 10, Z = 11 }
public enum SkinType { Hat = 0, Pant = 1, Sheild = 2, SetFull = 3 }
public enum BuffType { Range = 0, AttackSpeed = 1, MoveSpeed = 2, Gold = 3 }
public enum ZoneType { Table = 0, Kitchen = 1, City = 2, Stadium = 3 }
public enum BotAINameType { Pro1 = 0, No1Killer = 1, SpeedUp = 2, 
                        Maxping = 3 , ACE =4, AceNo1=5, Skull = 6, 
                        Zombie = 7, Jack = 8, Kassin = 9, Nana =10, Kunka=11,
                        Ashe=12, Kuna=13, MissF=14, MissS=15, MissT=16,
                        Ziggle=17, Kassac =18, Null2=19, Null3=20,
                        Comment = 21, Skill = 22,Juk=23, MissJuk=24,
                        AsheNo1=25, AsheNo2=26, AsheNo3=27, AsheNo4=28,
                        AsheNo5=29, AsheNo6=30, AsheNo7=31, AsheNo8=32,
                        AsheNo9=33, AsheNo10=34, AsheNo11=35, AsheNo12=36,
                        AsheNo13=37, AsheNo14=38, AsheNo15=39,  AsheNo16=40,
                        AsheNo17=42, AsheNo18=43, AsheNo19=44, MissF1=45,
                        MissF2=46, MissF3=47,   MissF4=48, MissF5=49,
                        MissF6=50
                        }