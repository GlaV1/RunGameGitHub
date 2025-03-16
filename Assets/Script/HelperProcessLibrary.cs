using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Google.Protobuf;
using System.IO;
using ProtoBuf;
using UnityEngine.Playables;

namespace rgame
{
    public class HelperProcessLibrary // karakterimizin çoðalma iþlermleri
    {
        //çarpma iþlemi
        public void MultiplacationClass(int incomingnum, List<GameObject> Characters, Transform newposition, List<GameObject> FormationEffects)
        {                           
            int LoopNumber = (GameManager.LiveCharacterNum * incomingnum) - GameManager.LiveCharacterNum;
            int sayi = 0;
            foreach (var item in Characters)
            {
                if (sayi < LoopNumber)
                {
                    if (!item.activeInHierarchy)
                    {
                        foreach (var efitem in FormationEffects)
                        {
                            if (!efitem.activeInHierarchy)
                            {
                                efitem.SetActive(true);
                                efitem.transform.position = newposition.position;
                                efitem.GetComponent<ParticleSystem>().Play();
                                efitem.GetComponent<AudioSource>().Play();
                                break;

                            }
                        }
                        item.transform.position = newposition.position;
                        item.SetActive(true);
                        sayi++;
                    }
                }
                else
                {
                    sayi = 0;
                    break;
                }
            }
            GameManager.LiveCharacterNum *= incomingnum;
        } 
        //çýkarma iþlemi
        public void ExtractionClass(int incomingnum, List<GameObject> Characters, List<GameObject> ExtinctionnEffects)
        {
            if (GameManager.LiveCharacterNum < incomingnum)
            {
                foreach (var item in Characters)
                {
                    //efekt döngüsü
                    foreach (var efitem in ExtinctionnEffects)
                    {
                        if (!efitem.activeInHierarchy)
                        {
                            efitem.SetActive(true);
                            efitem.transform.position = efitem.transform.position;
                            efitem.GetComponent<ParticleSystem>().Play();
                            efitem.GetComponent<AudioSource>().Play();
                            break;
                        }
                    }
                    item.transform.position = Vector3.zero;
                    item.SetActive(false);
                }
                GameManager.LiveCharacterNum = 1;
            }
            else
            {
                int sayi = 0;
                foreach (var item in Characters)
                {
                    if (sayi != incomingnum)
                    {
                        if (item.activeInHierarchy)
                        {
                            foreach (var efitem in ExtinctionnEffects)
                            {
                                if (!efitem.activeInHierarchy)
                                {
                                    Vector3 newpos = new Vector3(item.transform.position.x, .23f, item.transform.position.z);
                                    efitem.SetActive(true);
                                    efitem.transform.position = newpos;
                                    efitem.GetComponent<ParticleSystem>().Play();
                                    efitem.GetComponent<AudioSource>().Play();
                                    break;
                                }
                            }

                            item.transform.position = Vector3.zero;
                            item.SetActive(false);
                            sayi++;
                        }
                    }
                    else
                    {
                        sayi = 0;
                        break;
                    }
                }
                GameManager.LiveCharacterNum -= incomingnum;
            }
        }
        //bölme iþlemi
        public void DivisionClass(int incomingnum, List<GameObject> Characters, List<GameObject> ExtinctionnEffects)
        {
            if (GameManager.LiveCharacterNum <= incomingnum)
            {
                foreach (var item in Characters)
                {
                    foreach (var efitem in ExtinctionnEffects)
                    {
                        if (!efitem.activeInHierarchy)
                        {
                            Vector3 newpos = new Vector3(item.transform.position.x, .23f, item.transform.position.z);
                            efitem.SetActive(true);
                            efitem.transform.position = newpos;
                            efitem.GetComponent<ParticleSystem>().Play();
                            efitem.GetComponent<AudioSource>().Play();
                            break;
                        }
                    }
                    item.transform.position = Vector3.zero;
                    item.SetActive(false);

                }
                GameManager.LiveCharacterNum = 1;
            }
            else
            {
                int bolen = GameManager.LiveCharacterNum / incomingnum;
                int sayi = 0;
                foreach (var item in Characters)
                {
                    if (sayi != bolen)
                    {
                        if (item.activeInHierarchy)
                        {
                            foreach (var efitem in ExtinctionnEffects)
                            {
                                if (!efitem.activeInHierarchy)
                                {
                                    Vector3 newpos = new Vector3(item.transform.position.x, .23f, item.transform.position.z);
                                    efitem.SetActive(true);
                                    efitem.transform.position = newpos;
                                    efitem.GetComponent<ParticleSystem>().Play();
                                    efitem.GetComponent<AudioSource>().Play();
                                    break;
                                }
                            }

                            item.transform.position = Vector3.zero;
                            item.SetActive(false);
                            sayi++;
                        }
                    }
                    else
                    {
                        sayi = 0;
                        break;
                    }
                }
                if (GameManager.LiveCharacterNum % incomingnum == 0)
                {
                    GameManager.LiveCharacterNum /= incomingnum;
                }
                else if (GameManager.LiveCharacterNum % incomingnum == 1)
                {
                    GameManager.LiveCharacterNum /= incomingnum;
                    GameManager.LiveCharacterNum++;
                }
                else if (GameManager.LiveCharacterNum % incomingnum == 2)
                {
                    GameManager.LiveCharacterNum /= incomingnum;
                    GameManager.LiveCharacterNum += 2;
                }
            }
        }
        //toplama iþlemi
        public void CollectionClass(int incomingnum, List<GameObject> Characters, Transform newposition, List<GameObject> FormationEffects)
        {
            int sayi = 0;
            foreach (var item in Characters)
            {
                if (sayi < incomingnum)
                {
                    if (!item.activeInHierarchy)
                    {
                        foreach (var efitem in FormationEffects)
                        {
                            if (!efitem.activeInHierarchy)
                            {
                                efitem.SetActive(true);
                                efitem.transform.position = newposition.position;
                                efitem.GetComponent<ParticleSystem>().Play();
                                efitem.GetComponent<AudioSource>().Play();
                                break;

                            }
                        }
                        item.transform.position = newposition.position;
                        item.SetActive(true);
                        sayi++;
                    }
                }
                else
                {
                    sayi = 0;
                    break;
                }
            }
            GameManager.LiveCharacterNum += incomingnum;
        }
    }
    
    public class MemoryManagement //kaydedilecek verilerin iþlemleri
    {
        //Verilerin string,int,float olarak keydedildiði yer
        public void SaveData_string(string Key,string Value)
        {
            PlayerPrefs.SetString(Key, Value);
            PlayerPrefs.Save();
        }

        public void SaveData_int(string Key, int Value)
        {
            PlayerPrefs.SetInt(Key,Value);
            PlayerPrefs.Save();
        }

        public void SaveData_float(string Key, float Value)
        {
            PlayerPrefs.SetFloat(Key, Value);
            PlayerPrefs.Save();
        }

        //Verilerin string,int,float olarak keydedildiði yer
                        

        //Verilerin string,int,float olarak okunduðu yer
        public string ReadData_string(string Key)
        {
            return PlayerPrefs.GetString(Key);
        }

        public int ReadData_int(string Key)
        {
            return PlayerPrefs.GetInt(Key);
        }

        public float ReadData_float(string Key)
        {
            return PlayerPrefs.GetFloat(Key);
        }
        //Verilerin string,int,float olarak okunduðu yer


        public void Check()
        {
            if (!PlayerPrefs.HasKey("LastLevel"))//son bölüm adlý anahtar kayýtlý DEÐÝL ÝSE olmasý gerekenler
            {//Son bölüm=oyuncunun kaldýðý son bölüm
                PlayerPrefs.SetInt("LastLevel",5);//5.index teki scene açmak için gerekli iþlemler
                PlayerPrefs.SetInt("Point",0);
                PlayerPrefs.SetInt("ActiveHat",-1);
                PlayerPrefs.SetInt("ActiveStick", -1);
                PlayerPrefs.SetInt("ActiveManColor", -1);
                PlayerPrefs.SetInt("ActiveHatColor", -1);
                PlayerPrefs.SetInt("ActiveStickColor", -1);
                PlayerPrefs.SetFloat("GameAudio",1);
                PlayerPrefs.SetFloat("MenuAudio",1);
                PlayerPrefs.SetFloat("MenuFxAudio", 1);

            }
        }
        //level kontrollerinin yapýldýðý iþlemler
    }

    [ProtoContract]
    [Serializable]
    public class GameData //kaldýrýlacak
    {
        [ProtoMember(1)] public List<ItemInformations> _ItemInformation = new List<ItemInformations>();
    }

    [ProtoContract]
    [Serializable]
    public class ItemInformations
    {
        [ProtoMember(1)] public int GroupIndex;//hangi modelde olduðumuzu söyleyecek(örneðin; þapkalar,sopalar vs.)
        [ProtoMember(2)] public int ItemIndex;//item gruplarýnda kaçýncý itemde olduðumuzu söyleyecek(örn; sopa1,sopa2)
        [ProtoMember(3)] public string ItemName;//item adýný verir
        [ProtoMember(4)] public int Point;//toplam puan
        [ProtoMember(5)] public bool BuyingStatus;//item satýn alýnmýþmý  diye kontrol eder
    }

    public class DataManager
    {

        public void FirstSave(List<ItemInformations> _ItemInformation)
        {
            string filepath = Application.persistentDataPath + "/GameData.dat";
            if (!File.Exists(filepath))
            {
                using (FileStream file = File.Create(filepath))
                {
                    Serializer.Serialize(file, _ItemInformation);
                }
            }
           
        }

        public void DataSave(List<ItemInformations> _ItemInformation)
        {
            string filepath = Application.persistentDataPath + "/GameData.dat";
            using (FileStream file =new FileStream(filepath,FileMode.Truncate,FileAccess.Write))
            {
                Serializer.Serialize(file, _ItemInformation);
            }
        }


        List<ItemInformations> IntermediateItemInformation;
        public void DataUpload()
        {
            string filepath = Application.persistentDataPath + "/GameData.dat";
            if (File.Exists(filepath))
            {
                using (FileStream file = File.OpenRead(filepath))
                {
                    IntermediateItemInformation = Serializer.Deserialize<List<ItemInformations>>(file);
                }
            }
            else
            {
                Debug.LogWarning("Dosya bulunamadý");
            }
        }

        public List<ItemInformations> TransferList()
        {
            return IntermediateItemInformation;
        }

    }
}

