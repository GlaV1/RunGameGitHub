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

namespace rgame
{
    /// <summary>
    /// Ana karakterter çoðlan veya eksilen alt karakter iþlemleri
    /// </summary>
    public class HelperProcessLibrary 
    {
        /// <summary>
        /// Çarpma Ýþlemi
        /// </summary>
        /// <param name="incomingnum">Gelen Ýþlem sayýsý(Karakterin çarptýðý sayýlar)</param>
        /// <param name="Characters">Ýþlem Yapýlan Karakter listesi</param>
        /// <param name="newposition">Oluþacaðý pozisyon</param>
        /// <param name="FormationEffects">Kullanýlacak efekt(Oluþma efekti)</param>
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
        
        /// <summary>
        /// Çýkartma Ýþlemi
        /// </summary>
        /// <param name="incomingnum">Gelen Ýþlem sayýsý(Karakterin çarptýðý sayýlar)</param>
        /// <param name="Characters">Ýþlem Yapýlan Karakter listesi</param>
        /// <param name="ExtinctionnEffects">Kullanýlacak Efekt(yok olma efekti)</param>
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
       
        /// <summary>
        /// Bölme ÝÞlemi
        /// </summary>
        /// <param name="incomingnum">Gelen Ýþlem sayýsý(Karakterin çarptýðý sayýlar)</param>
        /// <param name="Characters">Ýþlem yapýlan karakter listesi</param>
        /// <param name="ExtinctionnEffects">Kullanýlacak efekt(yok olma efekti)</param>
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
        
        /// <summary>
        /// Toplama iþlemi
        /// </summary>
        /// <param name="incomingnum">Gelen Ýþlem sayýsý(Karakterin çarptýðý sayýlar)</param>
        /// <param name="Characters">iþlem yapýlan karakter listesi</param>
        /// <param name="newposition">Oluþacak karakterin oluþmasý gerekn posizyon</param>
        /// <param name="FormationEffects">kullanýlacak efekt(Oluþma efekti)</param>
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
    
    public class MemoryManagement //Player pref verileri kaydetme iþlemleri iþlemleri
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
                PlayerPrefs.SetInt("LastLevel",5);//5.index teki scene açmak için gerekli iþlemler(5.index=level 1)
                PlayerPrefs.SetInt("Point",0);
                PlayerPrefs.SetInt("ActiveHat",-1);
                PlayerPrefs.SetInt("ActiveStick", -1);
                PlayerPrefs.SetInt("ActiveManColor", -1);
                PlayerPrefs.SetInt("ActiveHatColor", -1);
                PlayerPrefs.SetInt("ActiveStickColor", -1);
                PlayerPrefs.SetFloat("GameAudio",1);
                PlayerPrefs.SetFloat("MenuAudio",1);
                PlayerPrefs.SetFloat("MenuFxAudio", 1);
                PlayerPrefs.SetFloat("SelectedLanguage", 0);//dropdownda 0 ýncý seçili yani trkçe

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
        [ProtoMember(4)] public string LocalizationKey;//item adýný verir
        [ProtoMember(5)] public int Point;//toplam puan
        [ProtoMember(6)] public bool BuyingStatus;//item satýn alýnmýþmý  diye kontrol eder
    }

    /// <summary>
    /// veri kaydetme ve geri yükleme iþlemleri
    /// </summary>
    public class DataManager
    {
        /// <summary>
        /// Eðer oyunun hiç kaydý bulunmadýysa yapýlan ilk kayýt
        /// </summary>
        /// <param name="_ItemInformation">Ýtem  bilgi listesi</param>
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

        /// <summary>
        /// eðer ilk kayýt var ise üstüne yazma(kaydetme) iþlemi
        /// </summary>
        /// <param name="_ItemInformation">Ýtem  bilgi listesi</param>
        public void DataSave(List<ItemInformations> _ItemInformation)
        {
            string filepath = Application.persistentDataPath + "/GameData.dat";
            using (FileStream file =new FileStream(filepath,FileMode.Truncate,FileAccess.Write))
            {
                Serializer.Serialize(file, _ItemInformation);
            }
        }


        List<ItemInformations> IntermediateItemInformation;
        /// <summary>
        /// Verileri Geri yükleme(Upload etme iþlemleri)
        /// </summary>
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

    /// <summary>
    /// Dil Dönüþümleri için Kullanýlýr
    /// </summary>
    public class LanguageManager
    {
        MemoryManagement _MemoryManagement = new MemoryManagement();
        /// <summary>
        /// Seçilen Dili etkin yapar ve sahnelerde uygular
        /// </summary>
        /// <param name="SelectionLanguage">Seçilen Dil indexi</param>
        public void LanguageSelection(int SelectionLanguage)
        {
            _MemoryManagement.SaveData_int("SelectedLanguage", SelectionLanguage);
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[SelectionLanguage];
        }
        /// <summary>
        /// Ýstenen bir metni ilgili tablodaki ilgili keyi kullanarak DÝlini deðiþtirme
        /// </summary>
        /// <param name="TableName">Ýlgili Localizationun Tablo Adý</param>
        /// <param name="key">Ýlgili localization tablosundaki iþlem yapýlmak istenen key</param>
        /// <returns></returns>
        public string BringText(string TableName ,string key)
        {
          return  LocalizationSettings.StringDatabase.GetLocalizedString(TableName,key);
        }
    }
}

