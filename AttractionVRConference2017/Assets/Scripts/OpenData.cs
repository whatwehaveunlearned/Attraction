using UnityEngine;
using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;


public class OpenData : MonoBehaviour 
{
	public string path;
	public GameObject dataPrefab;
	public GameObject attrPrefab;
	public float max;
	public float min;
	//Attributes values
	public List<string> attrs = new List<string>();
	//If I want to make this general I have to use a dictionary like this http://stackoverflow.com/questions/5037997/dictionary-of-generic-lists-or-varying-types
	//public Dictionary<string,dynamic> attrsDict = new Dictionary<string,dynamic>();
	List<float> weight = new List<float>();
	List<float> horsepower = new List<float>();
	List<float> acceleration = new List<float>();
	List<float> displacement = new List<float>();
	List<float> cylinders = new List<float>();
	List<string> name = new List<string> ();
	float maxhorsepower=0;
	float maxweight=0;
	float maxacceleration=0;
	float maxdisplacement=0;
	float maxcylinders=0;

	


	
	void Awake()
	{
		Load (Application.dataPath + "/Data/" + path);
		instantiateData (horsepower.Count);
	}

	private bool Load(string fileName)
	{
        int attrCount = 0;
		// Handle any problems that might arise when reading the text
		try
		{
			string line;
			// Create a new StreamReader, tell it which file to read and what encoding the file
			// was saved as
			StreamReader theReader = new StreamReader(fileName, Encoding.Default);
			
			// Immediately clean up the reader after this block of code is done.
			// You generally use the "using" statement for potentially memory-intensive objects
			// instead of relying on garbage collection.
			// (Do not confuse this with the using directive for namespace at the 
			// beginning of a class!)
			using (theReader)
			{
				//Read Attr names
				line = theReader.ReadLine();
				//Split Line
				string[] firstLine = line.Split(',');
				foreach(string element in firstLine){
                    if(element == "cylinders" || element == "displacement" || element == "horsepower" || element == "weight" || element == "acceleration")
					attrs.Add(element);
				}
				instantiateAttrObject(attrs,attrCount);
                {
                    //While there's lines left in the text file, do this:

                    do
                    {
                        line = theReader.ReadLine();
                        if (line != null)
                        {
                            //Split Line
                            string[] entries = line.Split(',');

                            //Add the attr values to the Lists
                            if (entries.Length > 0)
                            {
                                //Read values and add them to arrays
                                horsepower.Add(Convert.ToSingle(entries[3]));
                                weight.Add(Convert.ToSingle(entries[4]));
                                acceleration.Add(Convert.ToSingle(entries[5]));
								displacement.Add(Convert.ToSingle(entries[2]));
								cylinders.Add(Convert.ToSingle(entries[1]));
								name.Add(entries[8]);
                            }
                        }
                    }
                    while (line != null);
                }

                // Done reading, close the reader and return true to broadcast success    
                theReader.Close();
				//Calculate max value for each attribute to normalize later
				maxhorsepower = FindMax(horsepower);
				maxweight = FindMax(weight);
				maxacceleration = FindMax(acceleration);
				maxdisplacement = FindMax(displacement);
				maxcylinders = FindMax(cylinders);
				return true;
			}
		}
		
		// If anything broke in the try block, we throw an exception with information
		// on what didn't work
		catch (Exception e)
		{
			Console.WriteLine("{0}\n", e.Message);
			return false;
		}
	}

	private float NormalizeData(float value,float maxData){
		return max*value/maxData;
	}

	private float FindMax(List<float> list)
	{
		if (list.Count == 0)
		{
			throw new InvalidOperationException("Empty list");
		}
		float max = 0;
		foreach (float element in list)
		{
			if (element > max)
			{
				max = element;
			}
		}
		return max;
	}

	private void instantiateData(int length){
		for (int i = 0; i < length; i++) {
			GameObject thisdata = Instantiate(dataPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			thisdata.transform.parent = GameObject.Find ("dataHolder").transform; 
			thisdata.GetComponent<ObjectProperties> ().attr0=NormalizeData(Convert.ToSingle(cylinders[i]),maxcylinders);
			thisdata.GetComponent<ObjectProperties> ().noNormalizedattr0 = Convert.ToSingle (cylinders [i]);
			thisdata.GetComponent<ObjectProperties> ().attr1=NormalizeData(Convert.ToSingle(displacement[i]),maxdisplacement);
			thisdata.GetComponent<ObjectProperties> ().noNormalizedattr1 = Convert.ToSingle (displacement [i]);
			thisdata.GetComponent<ObjectProperties> ().attr2=NormalizeData(Convert.ToSingle(horsepower[i]),maxhorsepower);
			thisdata.GetComponent<ObjectProperties> ().noNormalizedattr2 =  Convert.ToSingle(horsepower[i]);
			thisdata.GetComponent<ObjectProperties> ().attr3=NormalizeData(Convert.ToSingle(weight[i]),maxweight);
			thisdata.GetComponent<ObjectProperties> ().noNormalizedattr3 = Convert.ToSingle (weight [i]);
			thisdata.GetComponent<ObjectProperties> ().attr4 = NormalizeData(Convert.ToSingle(acceleration[i]),maxacceleration);
			thisdata.GetComponent<ObjectProperties> ().noNormalizedattr4 = Convert.ToSingle (acceleration [i]);
			thisdata.GetComponent<ObjectProperties> ().dataName = name [i];
		}
	}

	private void instantiateAttrObject(List<string>	attrsObjects,int attrCount){
		float position = -1.0f;//Y use count to position the attributes when they are rendered for the first time.
		foreach (String attr in attrsObjects) {
			GameObject thisAttr = Instantiate(attrPrefab, new Vector3(1.412f, 0.094f, position), Quaternion.identity) as GameObject;
			thisAttr.transform.parent = GameObject.Find ("attrArm").transform; 
 			thisAttr.name = attr;
			attrPrefab.GetComponent<AttrProperties> ().properties[0] = attr;
            attrPrefab.GetComponent<AttrProperties>().properties[1] = "" + attrCount;
            position = position+0.4f;
            attrCount = attrCount + 1;
		}
	}
}


