using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Google.Protobuf;
using System.IO;
using ProtoBuf;
using UnityEngine.Playables;
using UnityEngine.Localization.Settings;
using rgamekeys;

namespace rgame
{
    /// <summary>
    /// Ana karakterter �o�lan veya eksilen alt karakter i�lemleri
    /// </summary>
    public class HelperProcessLibrary 
    {
        /// <summary>
        /// �arpma ��lemi
        /// </summary>
        /// <param name="incomingnum">Gelen ��lem say�s�(Karakterin �arpt��� say�lar)</param>
        /// <param name="Characters">��lem Yap�lan Karakter listesi</param>
        /// <param name="newposition">Olu�aca�� pozisyon</param>
        /// <param name="FormationEffects">Kullan�lacak efekt(Olu�ma efekti)</param>
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
                               // efitem.GetComponent<AudioSource>().Play();
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
        
        /// <summary>
        /// ��kartma ��lemi
        /// </summary>
        /// <param name="incomingnum">Gelen ��lem say�s�(Karakterin �arpt��� say�lar)</param>
        /// <param name="Characters">��lem Yap�lan Karakter listesi</param>
        /// <param name="ExtinctionnEffects">Kullan�lacak Efekt(yok olma efekti)</param>
        public void ExtractionClass(int incomingnum, List<GameObject> Characters, List<GameObject> ExtinctionnEffects)
        {
            if (GameManager.LiveCharacterNum < incomingnum)
            {
                foreach (var item in Characters)
                {
                    //efekt d�ng�s�
                    foreach (var efitem in ExtinctionnEffects)
                    {
                        if (!efitem.activeInHierarchy)
                        {
                            efitem.SetActive(true);
                            efitem.transform.position = efitem.transform.position;
                            efitem.GetComponent<ParticleSystem>().Play();
                           // efitem.GetComponent<AudioSource>().Play();
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
                                   // efitem.GetComponent<AudioSource>().Play();
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
       
        /// <summary>
        /// B�lme ��lemi
        /// </summary>
        /// <param name="incomingnum">Gelen ��lem say�s�(Karakterin �arpt��� say�lar)</param>
        /// <param name="Characters">��lem yap�lan karakter listesi</param>
        /// <param name="ExtinctionnEffects">Kullan�lacak efekt(yok olma efekti)</param>
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
                           // efitem.GetComponent<AudioSource>().Play();
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
                                   // efitem.GetComponent<AudioSource>().Play();
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
        
        /// <summary>
        /// Toplama i�lemi
        /// </summary>
        /// <param name="incomingnum">Gelen ��lem say�s�(Karakterin �arpt��� say�lar)</param>
        /// <param name="Characters">i�lem yap�lan karakter listesi</param>
        /// <param name="newposition">Olu�acak karakterin olu�mas� gerekn posizyon</param>
        /// <param name="FormationEffects">kullan�lacak efekt(Olu�ma efekti)</param>
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
                               // efitem.GetComponent<AudioSource>().Play();
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
    
    public class MemoryManagement //Player pref verileri kaydetme i�lemleri i�lemleri
    {
        //Verilerin string,int,float olarak keydedildi�i yer
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

        //Verilerin string,int,float olarak keydedildi�i yer
                        

        //Verilerin string,int,float olarak okundu�u yer
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
        //Verilerin string,int,float olarak okundu�u yer


        public void Check()
        {
            if (!PlayerPrefs.HasKey("LastLevel"))//son b�l�m adl� anahtar kay�tl� DE��L �SE olmas� gerekenler
            {//Son b�l�m=oyuncunun kald��� son b�l�m
                PlayerPrefs.SetInt(SaveKeys.LastLevel,5);//5.index teki scene a�mak i�in gerekli i�lemler(5.index=level 1)
                PlayerPrefs.SetInt(SaveKeys.Point,0);
                PlayerPrefs.SetInt(SaveKeys.ActiveHat,-1);
                PlayerPrefs.SetInt(SaveKeys.ActiveStick, -1);
                PlayerPrefs.SetInt(SaveKeys.ActiveManColor, -1);
                PlayerPrefs.SetInt(SaveKeys.ActiveHatColor, -1);
                PlayerPrefs.SetInt(SaveKeys.ActiveStickColor, -1);
                PlayerPrefs.SetFloat(SaveKeys.GameAudio,1);
                PlayerPrefs.SetFloat(SaveKeys.MenuAudio,1);
                PlayerPrefs.SetFloat(SaveKeys.MenuFxAudio, 1);
                PlayerPrefs.SetFloat(SaveKeys.SelectedLanguage, 0);//dropdownda 0 �nc� se�ili yani trk�e

            }
        }
        //level kontrollerinin yap�ld��� i�lemler
    }

    [ProtoContract]
    [Serializable]
    public class GameData //kald�r�lacak
    {
        [ProtoMember(1)] public List<ItemInformations> _ItemInformation = new List<ItemInformations>();
        [ProtoMember(2)] public List<ColorData> _HatColorName= new List<ColorData>();
        [ProtoMember(3)] public List<ColorData> _StickColorName= new List<ColorData>();
    }

    [ProtoContract]
    [Serializable]
    public class ItemInformations
    {
        [ProtoMember(1)] public int GroupIndex;//hangi modelde oldu�umuzu s�yleyecek(�rne�in; �apkalar,sopalar vs.)
        [ProtoMember(2)] public int ItemIndex;//item gruplar�nda ka��nc� itemde oldu�umuzu s�yleyecek(�rn; sopa1,sopa2)
        [ProtoMember(3)] public string ItemName;//item ad�n� verir
        [ProtoMember(4)] public string LocalizationKey;//item ad�n� verir
        [ProtoMember(5)] public int Point;//toplam puan
        [ProtoMember(6)] public bool BuyingStatus;//item sat�n al�nm��m�  diye kontrol eder
    }


    [ProtoContract]
    [Serializable]
    public class ColorData
    {
        [ProtoMember(1)] public string ColorName;        
    }


    /// <summary>
    /// veri kaydetme ve geri y�kleme i�lemleri
    /// </summary>
    public class DataManager
    {
        /// <summary>
        /// E�er oyunun hi� kayd� bulunmad�ysa yap�lan ilk kay�t
        /// </summary>
        /// <param name="_ItemInformation">�tem  bilgi listesi</param>
        public void FirstSave(GameData _GameData)
        {
            string filepath = Application.persistentDataPath + "/GameData.dat";
            if (!File.Exists(filepath))
            {
                using (FileStream file = File.Create(filepath))
                {
                    Serializer.Serialize(file, _GameData);
                }
            }
           
        }

        /// <summary>
        /// e�er ilk kay�t var ise �st�ne yazma(kaydetme) i�lemi
        /// </summary>
        /// <param name="_ItemInformation">�tem  bilgi listesi</param>
        public void DataSave(GameData _GameData)
        {
            string filepath = Application.persistentDataPath + "/GameData.dat";
            using (FileStream file =new FileStream(filepath,FileMode.Truncate,FileAccess.Write))
            {
                Serializer.Serialize(file,_GameData);
            }
        }


        private GameData IntermediateData;
        /// <summary>
        /// Verileri Geri y�kleme(Upload etme i�lemleri)
        /// </summary>
        public void DataUpload()
        {
            string filepath = Application.persistentDataPath + "/GameData.dat";
            if (File.Exists(filepath))
            {
                using (FileStream file = File.OpenRead(filepath))
                {
                    IntermediateData = Serializer.Deserialize<GameData>(file);
                }
            }
            else
            {
                Debug.LogWarning("Dosya bulunamad�");
            }
        }

        public GameData GetData()
        {
            return IntermediateData;
        }

    }

    /// <summary>
    /// Dil D�n���mleri i�in Kullan�l�r
    /// </summary>
    public class LanguageManager
    {
        MemoryManagement _MemoryManagement = new MemoryManagement();
        /// <summary>
        /// �stenen bir metni ilgili tablodaki ilgili keyi kullanarak D�lini de�i�tirme
        /// </summary>
        /// <param name="TableName">�lgili Localizationun Tablo Ad�</param>
        /// <param name="key">�lgili localization tablosundaki i�lem yap�lmak istenen key</param>
        /// <returns></returns>
        public string BringText(string TableName ,string key)
        {
          return  LocalizationSettings.StringDatabase.GetLocalizedString(TableName,key);
        }
    }


}

