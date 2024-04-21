using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextImporter : MonoBehaviour
{
    public TextAsset textFile;
    public string[] textLines;

    // Start is called before the first frame update
    void Start()
    {
        if(textFile != null)
        {
            // Grab the text within the file and split it into separate lines.
            textLines = (textFile.text.Split('\n')); 
        }
    }
}
 