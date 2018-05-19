using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveController : MonoBehaviour
{
    public static List<Instruction> savedGames = new List<Instruction>();

    public static void Save(List<Instruction> program)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedInstructions.gd");
        bf.Serialize(file, program);
        file.Close();
    }

    public static List<Instruction> Load()
    {
        List<Instruction> program = new List<Instruction>();

        if (File.Exists(Application.persistentDataPath + "/savedInstructions.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedInstructions.gd", FileMode.Open);
            program = (List<Instruction>)bf.Deserialize(file);
            file.Close();
        }
        return program;
    }
}
