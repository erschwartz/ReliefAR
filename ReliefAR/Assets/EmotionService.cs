using System.IO;
using UnityEngine;
using Emotion;

public class EmotionService : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    static byte[] GetImageAsByteArray(string imageFilePath)
    {
        FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
        BinaryReader binaryReader = new BinaryReader(fileStream);
        return binaryReader.ReadBytes((int)fileStream.Length);
    }

    static void MakeRequest(string imageFilePath)
    {
        // Request body. Try this sample with a locally stored JPEG image.
        byte[] byteData = GetImageAsByteArray(imageFilePath);
        Emotion.Emotion.MakeRequest(byteData);
    }
}
