using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace rgamekeys
{
    /// <summary>
    /// Oyun içerisinde kayýt edilen-edilmesi gereken anahtarlarýn keyleri bulunmakta
    /// </summary>
    public class SaveKeys
    {
        public const string LastLevel = "LastLevel";
        public const string Point = "Point";
        public const string ActiveHat = "ActiveHat";
        public const string ActiveStick = "ActiveStick";
        public const string ActiveManColor = "ActiveManColor";
        public const string ActiveHatColor = "ActiveHatColor";
        public const string ActiveStickColor = "ActiveStickColor";
        public const string GameAudio = "GameAudio";
        public const string MenuAudio = "MenuAudio";
        public const string MenuFxAudio = "MenuFxAudio";
        public const string SelectedLanguage = "SelectedLanguage";
        public const string SelectedQuality = "SelectedQuality";
    }

    /// <summary>
    /// Oyun içerisindeki Dil seçeneklerinin keyleri bulunmakta- Dil tablolarýnýn isimleri bulunmakta
    /// </summary>
    public class LanguageKeys
    {
        public const string CustomizeShowAlertInsufficientFunds = "CustomizeShowAlertInsufficientFunds";
        public const string CustomizeShowAlertRecorded = "CustomizeShowAlertRecorded";
        public const string CustomizeShowAlertPurchase = "CustomizeShowAlertPurchase";
        public const string CustomizeItemPurchaseControlText = "CustomizeItemPurchaseControlText";
        public const string Customize_MidItemPanel_ManColorPanel_txt_NoManColorText = "Customize_MidItemPanel_ManColorPanel_txt_NoManColorText";
        public const string Customize_MidItemPanel_SticksPanel_txt_NoStickText = "Customize_MidItemPanel_SticksPanel_txt_NoStickText";
        public const string Customize_MidItemPanel_HatPanel_txt_NoHatText = "Customize_MidItemPanel_HatPanel_txt_NoHatText";
        public const string LocalizationCutomizeItemTableName = "Customize_Table"; //Ýtem textlerinin tutulduðu tablo
        public const string LocalizationCustomizeTextTableName = "Game_Text"; //oyun textlerinin tutulduðu tablo
    }
   
    /// <summary>
    /// Oyun içerisindeki engellerin,objelerin keyleri bulunmakta
    /// </summary>
    public class GameObstacles
    {
        public const string LastTrigger = "LastTrigger";
        public const string Mast = "Mast";
        public const string NeedleBox = "NeedleBox";
        public const string PropallerNeedle = "PropallerNeedle";
        public const string Saw = "Saw";
        public const string Hammer = "Hammer";
    }

    /// <summary>
    /// Oyun içerisindeki karakterlerin keyleri bulunmakta
    /// </summary>
    public class GameCharacters
    {
        public const string LowerCharacters = "LowerCharacters";
        public const string EnemyCharacters = "EnemyCharacters";
        public const string EmptyCharacters = "EmptyCharacters";
        public const string Player = "Player";
    }
   
    /// <summary>
    /// Oyun içindeki Karakter çoðaltma azaltma iþlemlerinin keyleri bulunmakta
    /// </summary>
    public class GameCharactersProcess
    {
        public const string DivisionProcess = "DivisionProcess";
        public const string CollectionProcess = "CollectionProcess";
        public const string MultiplacationProcess = "MultiplacationProcess";
        public const string ExtractionProcess = "ExtractionProcess";
    }

}
