using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveUnit
{

    public void saveData(SaveData data)
    {

        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;
        try
        {

            if (File.Exists(destination)) file = File.OpenWrite(destination);
            else file = File.Create(destination);


            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(file, data);
            file.Close();
        }
        catch
        {
            Debug.LogError("Error saving save file");
        }
        
    }

    public SaveData loadData()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;
        try
        {

            if (File.Exists(destination)) file = File.OpenRead(destination);
            else
            {
                Debug.LogError("File not found");
                return null;
            }

            BinaryFormatter bf = new BinaryFormatter();
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            data.setLoadAttributes();


            return data;
        }
        catch
        {
            Debug.LogError("Error loading save file");
            return null;
        }
    }

}
