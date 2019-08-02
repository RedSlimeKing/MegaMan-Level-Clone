using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class LoadSaveManager : MonoBehaviour
{
    [XmlRoot("GameData")]
    public class GameStateData
    {
        public struct DataTransform
        {
            public float X;
            public float Y;
            public float Z;

            public float RotX;
            public float RotY;
            public float RotZ;

            public float ScaleX;
            public float ScaleY;
            public float ScaleZ;
        }
        public class DataEnemy
        {
            public DataTransform PosRotScale;

            public int Health;

            public int EnemyID;
        }
        public class DataPlayer
        {
            public DataTransform PosRotScale;

            public int Health;

            public int Score;
        }
        public List<DataEnemy> Enemies = new List<DataEnemy>();

        public DataPlayer Player = new DataPlayer();
    }
    public GameStateData GameState = new GameStateData();

    public void Save(string FileName = "GameData.xml")
    {
        XmlSerializer Serializer = new XmlSerializer(typeof(GameStateData));
        FileStream Stream = new FileStream(FileName, FileMode.Create);
        Serializer.Serialize(Stream, GameState);
        Stream.Close();
    }
    public void Load(string FileName = "GameData.xml")
    {
        XmlSerializer Serializer = new XmlSerializer(typeof(GameStateData));
        FileStream Stream = new FileStream(FileName, FileMode.Open);
        GameState = Serializer.Deserialize(Stream) as GameStateData;
        Stream.Close();
    }

}