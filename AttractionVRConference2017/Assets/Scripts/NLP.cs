//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//using System.Collections.Generic;

//namespace FrostweepGames.SpeechRecognition.Google.Cloud.Examples
//{
//    public class NLP : MonoBehaviour
//    {
//        //Public variable to detect mode computer or vive
//		public bool inUnity = false;
//        //Object that shows voice status
//        public GameObject grabber;
//        //Pubic control we will use to activate voice
//        public GameObject controller;

//        //Select sound
//        public AudioSource selectSound;

//        //Variable to hold all data points
//        private List<GameObject> dataPoints = new List<GameObject>();
//        private GameObject openDataMovies;
//        //Variable to hold all unique genres
//        private List<string> uniq_genres = new List<string>();
//        //Variables for speech recog
//        private ILowLevelSpeechRecognition _speechRecognition;
//        private string _speechRecognitionResult;

//        //variable to hold attrs
//        private Transform[] attributesObjects;

//        //Variable to hold sphericalgravity
//        private GameObject sphericalGravityObject;
//        //Variable to hold predifinedPositions
//        private GameObject predifinedPositions;

//        //Variable to Hold Selected Attributes (Attrs that are active)
//        private List<GameObject> selectedAttr = new List<GameObject>();

//        //Variable to hold selected data from spherical Gravity
//        private List<GameObject> selectedObjects = new List<GameObject>();

//        Color grabberColor;
//        Color grabberColorStandBy;
//        Color grabberColorCorrect;
//        Color grabberColorWrong;
//        private void Start()
//        {
//				_speechRecognition = SpeechRecognitionModule.Instance;
//				_speechRecognition.SpeechRecognizedSuccessEvent += SpeechRecognizedSuccessEventHandler;
//				_speechRecognition.SpeechRecognizedFailedEvent += SpeechRecognizedFailedEventHandler;

//            //Get attributes from the arm at start.
//            attributesObjects = GameObject.Find("attrArm").GetComponentsInChildren<Transform>();

//            //get the sphericalgravity component to start and stop it and to grab the predifined position script in positions
//            sphericalGravityObject = GameObject.Find("SphericalGravity");

//            //Get Qualitative values
//            //Get all unique genres
//            uniq_genres = GameObject.Find("OpenDataMovies").GetComponent<OpenDataMovies>().unique_genres;
//            //Get all objects
//            foreach (GameObject o in UnityEngine.Object.FindObjectsOfType<GameObject>())
//            {
//                if (o.name == "dataPrefab(Clone)")
//                {
//                    dataPoints.Add(o);
//                }
//            }
//            //Get initial color of Grabber
//            grabberColor = grabber.GetComponent<Renderer>().material.GetColor("_Color");
//            grabberColorStandBy = new Vector4(1, 0.92f, 0.016F, 0.4f);
//            grabberColorWrong = new Vector4(1, 0, 0, 0.4f);
//            grabberColorCorrect = new Vector4(0, 1, 0, 0.4f);
//        }

//        private void Update()
//        {
//			if (inUnity) {
//				SteamVR_TrackedObject trackedObjWand = controller.GetComponent<SteamVR_TrackedObject> ();
//				SteamVR_Controller.Device device = null;
//				if (trackedObjWand != null)
//					device = SteamVR_Controller.Input((int)trackedObjWand.index);
//			}

            
//            //if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
//            if (Input.GetKeyDown("space"))
//            {
//                grabber.GetComponent<Renderer>().material.SetColor("_Color", grabberColorStandBy);
//                StartRuntimeDetectionButton();
//            }
//            else if (Input.GetKeyDown("q"))
//            {
//                StopRuntimeDetectionButton();
//            }
//        }

//        private void OnDestroy()
//        {
//            _speechRecognition.SpeechRecognizedSuccessEvent -= SpeechRecognizedSuccessEventHandler;
//            _speechRecognition.SpeechRecognizedFailedEvent -= SpeechRecognizedFailedEventHandler;
//        }

//        private void StartRuntimeDetectionButton()
//        {
//            _speechRecognitionResult = "";
//            _speechRecognition.StartRuntimeRecord();
//        }

//        private void StopRuntimeDetectionButton()
//        {
//            _speechRecognition.StopRuntimeRecord();
//            _speechRecognitionResult = "";
//        }


//        private void SpeechRecognizedFailedEventHandler(string obj)
//        {
//            _speechRecognitionResult = "Speech Recognition failed with error: " + obj;

//        }

//        private void SpeechRecognizedSuccessEventHandler(RecognitionResponse obj)
//        {


//            if (obj != null && obj.results.Length > 0)
//            {
//                _speechRecognitionResult = obj.results[0].alternatives[0].transcript;

//                string other = "\nDetected alternative: ";

//                foreach (var result in obj.results)
//                {
//                    foreach (var alternative in result.alternatives)
//                    {
//                        if (obj.results[0].alternatives[0] != alternative)
//                            other += alternative.transcript + ", ";
//                    }
//                }

//                //_speechRecognitionResult += other;
//            }
//            else
//            {
//                _speechRecognitionResult = "No words detected";

//            }
//            Debug.Log(_speechRecognitionResult);
//            if (_speechRecognitionResult != "No words detected") LanguageControl(_speechRecognitionResult);
//            StopRuntimeDetectionButton();
//        }

//        private void LanguageControl(string action)
//        {
//            string[] words = action.Split(' ');
//            string main = words[0].ToLower();
//            bool detected_command = false;
//            if (main == "activate")
//            {
//                Activate(words);
//                detected_command = true;
//            }
//            else if (main == "deactivate")
//            {
//                Deactivate(words);
//                detected_command = true;
//            }
//            else if (main == "stop")
//            {
//                sphericalGravityObject.GetComponent<SphericalGravity>().forceActive = false;
//                foreach(GameObject data in dataPoints)
//                {
//                    data.GetComponent<SphereCollider>().enabled = true;
//                }
//                detected_command = true;
//            }
//            else if (main == "start")
//            {
//                foreach (GameObject data in dataPoints)
//                {
//                    data.GetComponent<SphereCollider>().enabled = false;
//                }
//                sphericalGravityObject.GetComponent<SphericalGravity>().forceActive = true;
//                detected_command = true;
//            }
//            else if (main == "layout")
//            {
//                Position(words);
//                detected_command = true;
//            }
//            else if (main == "select")
//            {
//                Select(words);
//                detected_command = true;
//            }
//            else if (main == "unselect")
//            {
//                UnSelect(words);
//                detected_command = true;
//            }
//            else if (main == "hide")
//            {
//                Hide(words);
//                detected_command = true;
//            }
//            else if (main == "show")
//            {
//                Show(words);
//                detected_command = true;
//            }

//            if (detected_command == true)
//            {
//                grabber.GetComponent<Renderer>().material.SetColor("_Color", grabberColorCorrect);
//                foreach(Transform child in grabber.transform)
//                {
//                    if (child.gameObject.tag == "command")
//                    {
//                        child.GetComponent<TextMesh>().text = action;
//                    }
//                }
//            }else
//            {
//                grabber.GetComponent<Renderer>().material.SetColor("_Color", grabberColorWrong);
//                foreach (Transform child in grabber.transform)
//                {
//                    if (child.gameObject.tag == "command")
//                    {
//                        child.GetComponent<TextMesh>().text = action;
//                    }
//                }
//            }
//            StartCoroutine(returnColorToWand());
//        }


//        IEnumerator returnColorToWand(){
//            yield return new WaitForSeconds(5);
//            grabber.GetComponent<Renderer>().material.SetColor("_Color", grabberColor);
//       }

        



//        private void Activate(string[] sentence)
//        {
//          /*  if (sentence[1] == "this")
//            {
//                selectedObjects = grabber.GetComponent<Grabber>().selectedData;
//                foreach (GameObject selectedObject in selectedObjects)
//                {
//                    selectedObject.GetComponent<Light>().enabled = true;
//                    foreach (Transform child in selectedObject.transform)
//                    {
//                        if (child.gameObject.tag == "dataName")
//                        {
//                            child.GetComponent<Renderer>().enabled = true;
//                        }
//                    }
//                }
//            }*/
//            foreach (Transform child in attributesObjects)
//            {
//                foreach (string attrWord in child.name.Split(' '))
//                {
//                    foreach(string sentenceWord in sentence)
//                    {
//                        if (attrWord.ToLower() == sentenceWord.ToLower())
//                        {
//                            child.GetComponent<AttrProperties>().inWorld = true;
//                            child.GetComponent<AttrProperties>().isActive = true;
//                            child.GetComponent<AttrProperties>().isSelected = true;
//                            foreach(Transform text in child)
//                            {
//                                {
//                                    text.GetComponent<TextMesh>().characterSize = 0.5f;
//                                }
//                            }
//                            selectedAttr.Add(child.gameObject);
//                        }
//                    }
//                }
//               /* if (attr == child.name && child.GetComponent<AttrProperties>() != null)
//                {
//                    child.GetComponent<AttrProperties>().inWorld = true;
//                    child.GetComponent<AttrProperties>().isActive = true;
//                    child.GetComponent<AttrProperties>().isSelected = true;
//                }
//                else if (attr == "all" && child.GetComponent<AttrProperties>() != null)
//                {
//                    child.GetComponent<AttrProperties>().inWorld = true;
//                    child.GetComponent<AttrProperties>().isActive = true;
//                    child.GetComponent<AttrProperties>().isSelected = true;
//                }*/
//            }
//        }

//        private void Deactivate(string[] sentence)
//        {
//            int count = 0;
//            if (sentence.Length > 1)
//            {
//                if (sentence[1].ToLower() == "all")
//                {
//                    foreach (Transform child in attributesObjects)
//                    {
//                        if (child.GetComponent<AttrProperties>())
//                        {
//                            child.GetComponent<AttrProperties>().inWorld = false;
//                            child.GetComponent<AttrProperties>().isActive = false;
//                            child.GetComponent<AttrProperties>().isSelected = false;
//                            foreach (Transform text in child)
//                            {
//                                {
//                                    text.GetComponent<TextMesh>().characterSize = 0.1f;
//                                }
//                            }
//                            foreach (GameObject attr in selectedAttr)
//                            {
//                                if (child == attr.transform)
//                                {
//                                    selectedAttr.RemoveAt(count);
//                                }
//                                count++;
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    foreach (Transform child in attributesObjects)
//                    {
//                        foreach (string attrWord in child.name.Split(' '))
//                        {
//                            foreach (string sentenceWord in sentence)
//                            {
//                                if (attrWord.ToLower() == sentenceWord.ToLower())
//                                {
//                                    child.GetComponent<AttrProperties>().inWorld = false;
//                                    child.GetComponent<AttrProperties>().isActive = false;
//                                    child.GetComponent<AttrProperties>().isSelected = false;
//                                    foreach (Transform text in child)
//                                    {
//                                        {
//                                            text.GetComponent<TextMesh>().characterSize = 0.1f;
//                                        }
//                                    }
//                                    foreach (GameObject attr in selectedAttr)
//                                    {
//                                        if (child == attr.transform)
//                                        {
//                                            selectedAttr.RemoveAt(count);
//                                        }
//                                        count++;
//                                    }
//                                }
//                            }
//                        }
//                        /*
//                        foreach (Transform child in attributesObjects)
//                    {
//                        if (attr == child.name)
//                        {
//                            child.GetComponent<AttrProperties>().isActive = false;
//                            child.GetComponent<AttrProperties>().isSelected = false;
//                        }
//                        else if (attr == "all" && child.GetComponent<AttrProperties>() != null)
//                        {
//                            child.GetComponent<AttrProperties>().isActive = false;
//                        }
//                        */
//                    }
//                }
//            }
//        }

//        private void Position(string[] Positiontype)
//        {
//            if (Positiontype.Length > 2)
//            {
//                foreach(string options in Positiontype)
//                {
//                    if(options.ToLower() == "selected" || options.ToLower() == "attributes")
//                    {
//                        sphericalGravityObject.GetComponent<predifinedPos>().attributes = selectedAttr;
//                    }
//                }
//            }else
//            {
//                sphericalGravityObject.GetComponent<predifinedPos>().attributes = new List<GameObject>();
//            }
//            foreach (string type in Positiontype)
//            {
//                if (type.ToLower() == "circle")
//                {
//                    sphericalGravityObject.GetComponent<predifinedPos>().circle2D = true;
//                }
//                else if (type.ToLower() == "sphere")
//                {
//                    sphericalGravityObject.GetComponent<predifinedPos>().sphere3D = true;
//                }
//            }
//        }

//        private void Select(string[] Selecttype)
//        {
//            if (Selecttype[1] != null)
//            {
//                if (Selecttype[1] == "this")
//                {
//                    selectedObjects = grabber.GetComponent<Grabber>().selectedData;
//                    foreach (GameObject selectedObject in selectedObjects)
//                    {
//                        selectedObject.GetComponent<Light>().enabled = true;
//                        foreach (Transform child in selectedObject.transform)
//                        {
//                            if (child.gameObject.tag == "dataName")
//                            {
//                                child.GetComponent<Renderer>().enabled = true;
//                                selectSound.Play();
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    foreach (string type in Selecttype)
//                    {
//                        foreach (string genre in uniq_genres)
//                        {
//                            if (type.ToLower() == genre.ToLower())
//                            {
//                                foreach (GameObject data in dataPoints)
//                                {
//                                    foreach (string data_genre in data.GetComponent<ObjectProperties>().genres)
//                                    {
//                                        if (data_genre.ToLower() == genre.ToLower())
//                                        {
//                                            data.GetComponent<Light>().enabled = true;
//                                            selectSound.Play();
//                                            foreach (Transform child in data.transform)
//                                            {
//                                                if (child.gameObject.tag == "dataName")
//                                                {
//                                                    child.GetComponent<Renderer>().enabled = true;
//                                                }
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//         }  
//        }
//        private void UnSelect(string[] Selecttype)
//        {
//            if (Selecttype[1] == "this")
//            {
//                selectedObjects = grabber.GetComponent<Grabber>().selectedData;
//                foreach (GameObject selectedObject in selectedObjects)
//                {
//                    selectedObject.GetComponent<Light>().enabled = false;
//                    foreach (Transform child in selectedObject.transform)
//                    {
//                        if (child.gameObject.tag == "dataName")
//                        {
//                            child.GetComponent<Renderer>().enabled = false;
//                        }
//                    }
//                }
//            }else
//            {
//                foreach (string type in Selecttype)
//                {
//                    foreach (string genre in uniq_genres)
//                    {
//                        if (type.ToLower() == genre.ToLower())
//                        {
//                            foreach (GameObject data in dataPoints)
//                            {
//                                foreach (string data_genre in data.GetComponent<ObjectProperties>().genres)
//                                {
//                                    if (data_genre.ToLower() == genre.ToLower())
//                                    {
//                                        data.GetComponent<Light>().enabled = false;
//                                        foreach (Transform child in data.transform)
//                                        {
//                                            if (child.gameObject.tag == "dataName")
//                                            {
//                                                child.GetComponent<Renderer>().enabled = false;
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }  
//        }
//        private void Hide(string[] toHide)
//        {
//            if (toHide[1] == "all")
//            {
//                if (toHide.Length >2)
//                    {
//                        if (toHide[2] == "except")//Hide all genres except...
//                        {
//                            foreach (string element in toHide)
//                            {
//                                foreach (string genre in uniq_genres)
//                                {
//                                    if (element.ToLower() == genre.ToLower())
//                                    {
//                                        foreach (GameObject data in dataPoints)
//                                        {
//                                            bool genreNotMatched = true;
//                                            foreach (string data_genre in data.GetComponent<ObjectProperties>().genres)
//                                            {
//                                                if (data_genre.ToLower() == genre.ToLower() && genreNotMatched)
//                                                {
//                                                    data.GetComponent<Renderer>().enabled = true;
//                                                    foreach (Transform child in data.transform)
//                                                    {
//                                                        if (child.gameObject.tag == "dataName")
//                                                        {
//                                                            child.GetComponent<Renderer>().enabled = true;
//                                                        }
//                                                        genreNotMatched = false;
//                                                    }
//                                                }
//                                                else if (genreNotMatched)
//                                                {
//                                                    data.GetComponent<Renderer>().enabled = false;
//                                                    data.GetComponent<Light>().enabled = false;
//                                                    foreach (Transform child in data.transform)
//                                                    {
//                                                        if (child.gameObject.tag == "dataName")
//                                                        {
//                                                            child.GetComponent<Renderer>().enabled = false;
//                                                        }
//                                                    }
//                                                }
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                }else
//                {
//                    //Hide all
//                    foreach (GameObject data in dataPoints)
//                    {
//                        data.GetComponent<Renderer>().enabled = false;
//                        data.GetComponent<Light>().enabled = false;
//                        foreach (Transform child in data.transform)
//                        {
//                            if (child.gameObject.tag == "dataName")
//                            {
//                                child.GetComponent<Renderer>().enabled = false;
//                            }
//                        }
//                    }
//                }
//            }
//            else //Hide a genre
//            {
//                foreach (string element in toHide)
//                {
//                    foreach (string genre in uniq_genres)
//                    {
//                        if (element.ToLower() == genre.ToLower())
//                        {
//                            foreach (GameObject data in dataPoints)
//                            {
//                                foreach (string data_genre in data.GetComponent<ObjectProperties>().genres)
//                                {
//                                    if (data_genre.ToLower() == genre.ToLower())
//                                    {
//                                        data.GetComponent<Renderer>().enabled = false;
//                                        data.GetComponent<Light>().enabled = false;
//                                        foreach (Transform child in data.transform)
//                                        {
//                                            if (child.gameObject.tag == "dataName")
//                                            {
//                                                child.GetComponent<Renderer>().enabled = false;
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//        }
//        private void Show(string[] toHide)
//        {
//            foreach (string element in toHide)
//            {
//                foreach (string genre in uniq_genres)
//                {
//                    if (element.ToLower() == genre.ToLower())
//                    {
//                        foreach (GameObject data in dataPoints)
//                        {
//                            foreach (string data_genre in data.GetComponent<ObjectProperties>().genres)
//                            {
//                                if (data_genre.ToLower() == genre.ToLower())
//                                {
//                                    data.GetComponent<Renderer>().enabled = true;
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }
//}