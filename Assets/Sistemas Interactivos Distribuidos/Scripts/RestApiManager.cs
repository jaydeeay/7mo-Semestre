using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RestApiManager : MonoBehaviour
{
    [SerializeField] private RawImage YourRawImage;
    public int[] cards;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GetCharacterClick() 
    {
        StartCoroutine(GetCharacters());
    }

    IEnumerator GetCharacters()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://rickandmortyapi.com/api/character/");
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR:" + www.error);
        }
        else
        {
            //Debug.log(www.GetResponseHeader("content-type"));

            // Show results as text
            Debug.Log(www.downloadHandler.text);



            if(www.responseCode == 200)
            {
                CharacterList characters = JsonUtility.FromJson<CharacterList>(www.downloadHandler.text);
                Debug.Log(characters.info.count);

                foreach(Character character in characters.results)
                {
                    Debug.Log("Name:" + character.name);
                    Debug.Log("Image:" + character.image);
                    StartCoroutine(DownloadImage(character.image));
                    break;
                }
            }
            else
            {
                string mensaje = "Status:" + www.responseCode;
                mensaje += "\ncontent-type:" + www.GetResponseHeader("content-type");
                mensaje += "\nError:" + www.error;
                Debug.Log(mensaje);
            }

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }

    IEnumerator DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else YourRawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    }

}

[System.Serializable]
public class CharacterList
{
    public CharacterListInfo info;
    public List<Character> results;
}

[System.Serializable]
public class CharacterListInfo
{
    public int count;
    public int pages;
    public string prev;
    public string next;
}

[System.Serializable]
public class Character
{
    public int id;
    public string name;
    public string specie;
    public string image;
}
