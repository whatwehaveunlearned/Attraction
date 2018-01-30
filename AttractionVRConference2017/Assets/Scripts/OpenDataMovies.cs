using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;

public class OpenDataMovies : MonoBehaviour {

	public string path;
	public GameObject dataPrefab;
	public GameObject attrPrefab;
	public float max;
	public float min;
	//Cuantitative Attributes values
	public List<string> attrs = new List<string>();
	List<float> duration = new List<float>();
	List<float> income = new List<float>();
	List<float> votes = new List<float>();
	List<float> score = new List<float>();
	List<float> cast_fb_likes = new List<float>();
    List<float> budget = new List<float>();
    List<float> faces = new List<float>();
    List<float> movie_fb_likes = new List<float>();
    List<float> ratio = new List<float>();
    //Data element name
	public List<string> dataName = new List<string> ();
    //Categorical Attributes
    List<string> genres = new List<string>();
    List<int> year = new List<int>();
    //Expose lists of uniques to show the user for searches
    public List<string> unique_genres = new List<string>();
    public List<int> unique_years = new List<int>();
    //Maximun and minimuns
    float maxduration = 0;
	float maxincome = 0;
	float maxvotes = 0;
	float maxscore = 0;
	float maxfb_likes = 0;
    float maxbudget = 0;
    float maxfaces = 0;
    float maxmovie_fb_likes = 0;
    float maxratio = 0;

    float minduration = 0;
    float minincome = 0;
    float minvotes = 0;
    float minscore = 0;
    float minfb_likes = 0;
    float minbudget = 0;
    float minfaces = 0;
    float minmovie_fb_likes = 0;
    float minratio = 0;


    void Awake()
	{
		Load (Application.dataPath + "/Data/" + path);
		instantiateData (dataName.Count);
	}

	private bool Load(string fileName)
	{
		int attrCount = 0;
		int javato=0;
		// Handle any problems that might arise when reading the text
		try
		{
			string line;
			// Create a new StreamReader, tell it which file to read and what encoding the file
			// was saved as
			StreamReader theReader = new StreamReader(fileName, Encoding.Default);
            //Byte to transform to UTF and then to String
            byte[] bytes;
            using (theReader)
			{
				//Read Attr names
				line = theReader.ReadLine();
				//Split Line
				string[] firstLine = line.Split(',');
				foreach(string element in firstLine){
					if(element == "duration" || element == "income" || element == "num_voted_users" || element == "imdb_score" || element == "cast_popularity" || element == "budget" || element== "number_faces_in_poster" || element=="aspect_ratio" || element== "movie_popularity")
						attrs.Add(element.Replace('_',' '));
				}
				instantiateAttrObject(attrs,attrCount);
                for   (javato = 0; javato < 900; javato++)
                {
                    //While there's lines left in the text file, do this:
                    //do
                    //{
                        line = theReader.ReadLine();
                        if (line != null)
                        {
                            //Split Line
                            string[] entries = line.Split(',');

                            //Add the attr values to the Lists
                            if (entries.Length > 0 && entries[3] != "" && entries[8] != "" && entries[12] != "" && entries[25] != "" && entries[13] != "" && entries[22] != "" && entries[15]!="" && entries[26]!="" && entries[27]!="")
                            {
                                //Read values and add them to arrays
                                duration.Add(Convert.ToSingle(entries[3]));
                                income.Add(Convert.ToSingle(entries[8]));
                                genres.Add(entries[9]);
                                votes.Add(Convert.ToSingle(entries[12]));
                                budget.Add(Convert.ToSingle(entries[22]));
                                score.Add(Convert.ToSingle(entries[25]));
                                cast_fb_likes.Add(Convert.ToSingle(entries[13]));
                                faces.Add(Convert.ToSingle(entries[15]));
                                movie_fb_likes.Add(Convert.ToSingle(entries[27]));
                                ratio.Add(Convert.ToSingle(entries[26]));
                                   
                                //Read the text to byte to transform to UTF
                                bytes = Encoding.Default.GetBytes(entries[11]);
                                //Read Qualitative Values
                                dataName.Add(Encoding.UTF8.GetString(bytes));
                                year.Add(Convert.ToInt16(entries[23]));
                                genres.Add(entries[9]);
                                
                                //Intermidiate step to get unique genres I process here and split them to get uniques later
                                foreach (string genre in entries[9].Split('|'))
                                {
                                    unique_genres.Add(genre);
                                }
                            }
                        }
                    //}
                   // while (line != null);
                }

				// Done reading, close the reader and return true to broadcast success    
				theReader.Close();
				//Calculate max value for each attribute to normalize later
				maxduration = FindMax(duration);
				maxincome = FindMax(income);
				maxvotes = FindMax(votes);
				maxscore = FindMax(score);
                maxbudget = FindMax(budget);
                maxfaces = FindMax(faces);
				maxfb_likes = FindMax(cast_fb_likes);
                maxratio = FindMax(ratio);
                maxmovie_fb_likes = FindMax(movie_fb_likes);

                minduration = FindMin(duration);
                minduration = FindMin(income);
                minduration = FindMin(votes);
                minduration = FindMin(score);
                minduration = FindMin(cast_fb_likes);
                minbudget = FindMin(budget);
                minfaces = FindMin(faces);
                minratio = FindMin(ratio);
                minmovie_fb_likes = FindMin(movie_fb_likes);

                //Save unique Qualitative Values
                unique_genres = unique_genres.Distinct().ToList();
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

	private float NormalizeData(float value,float maxData, float minData){
        int maxVal = 1;
		return maxVal*(value-minData)/(maxData-minData);
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

    private float FindMin(List<float> list)
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("Empty list");
        }
        float min = 10000000000000000000000000f;
        foreach (float element in list)
        {
            if (element < min)
            {
                min = element;
            }
        }
        return min;
    }

	private void instantiateData(int length){
		for (int i = 0; i < length; i++) {
			GameObject thisdata = Instantiate(dataPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			thisdata.transform.parent = GameObject.Find ("dataHolder").transform; 
			thisdata.GetComponent<ObjectProperties> ().attr0=NormalizeData(Convert.ToSingle(duration[i]),maxduration,minduration);
			thisdata.GetComponent<ObjectProperties> ().noNormalizedattr0 = Convert.ToSingle (duration [i]);
			thisdata.GetComponent<ObjectProperties> ().attr1=NormalizeData(Convert.ToSingle(income[i]),maxincome,minincome);
			thisdata.GetComponent<ObjectProperties> ().noNormalizedattr1 = Convert.ToSingle (income [i]);
			thisdata.GetComponent<ObjectProperties> ().attr2=NormalizeData(Convert.ToSingle(votes[i]),maxvotes,minvotes);
			thisdata.GetComponent<ObjectProperties> ().noNormalizedattr2 =  Convert.ToSingle(votes[i]);
			thisdata.GetComponent<ObjectProperties> ().attr3 = NormalizeData(Convert.ToSingle(cast_fb_likes[i]),maxfb_likes,minfb_likes);
			thisdata.GetComponent<ObjectProperties> ().noNormalizedattr3 = Convert.ToSingle (cast_fb_likes [i]);
			thisdata.GetComponent<ObjectProperties> ().attr4=NormalizeData(Convert.ToSingle(score[i]),maxscore,minscore);
			thisdata.GetComponent<ObjectProperties> ().noNormalizedattr4 = Convert.ToSingle (score [i]);
            thisdata.GetComponent<ObjectProperties>().attr5 = NormalizeData(Convert.ToSingle(budget[i]), maxbudget, minbudget);
            thisdata.GetComponent<ObjectProperties>().noNormalizedattr5 = Convert.ToSingle(budget[i]);
            thisdata.GetComponent<ObjectProperties>().attr6 = NormalizeData(Convert.ToSingle(faces[i]), maxfaces, minfaces);
            thisdata.GetComponent<ObjectProperties>().noNormalizedattr6 = Convert.ToSingle(faces[i]);
            thisdata.GetComponent<ObjectProperties>().attr7 = NormalizeData(Convert.ToSingle(movie_fb_likes[i]), maxmovie_fb_likes, minmovie_fb_likes);
            thisdata.GetComponent<ObjectProperties>().noNormalizedattr7 = Convert.ToSingle(movie_fb_likes[i]);
            thisdata.GetComponent<ObjectProperties>().attr8 = NormalizeData(Convert.ToSingle(ratio[i]), maxratio, minratio);
            thisdata.GetComponent<ObjectProperties>().noNormalizedattr8 = Convert.ToSingle(ratio[i]);
            //Save Qualitative Values
            thisdata.GetComponent<ObjectProperties>().year = year[i];
            thisdata.GetComponent<ObjectProperties>().dataName = dataName[i];
            foreach (string genre in genres[i].Split('|'))
            {
                thisdata.GetComponent<ObjectProperties>().genres.Add(genre);
            }
        }
        Debug.Log("Data Points = " + length);
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
