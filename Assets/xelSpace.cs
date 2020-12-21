using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rnd = UnityEngine.Random;
using KModkit;
public class xelSpace : MonoBehaviour {
    public KMSelectable spaceBar;
    public MeshRenderer background;
    public Material[] spaceMats;
    int spaceIndex;
    string[] quotes = new string[] {"Thats one small step for man one giant leap for mankind ",
    "Space is big Really big You just won't believe how vastly hugely mind bogglingly big it is I mean you may think its a long way down the road to the chemist but thats just peanuts to space ",
    "Who are we We find that we live on an insignificant planet of a humdrum star lost in a galaxy tucked away in some forgotten corner of a universe in which there are far more galaxies than people ",
    "These are the voyages of the Starship Enterprise Its five year mission to explore strange new worlds to seek out new life and new civilizations to boldly go where no man has gone before ",
    "Space It seems to go on and on forever Then you get to the end and a monkey starts throwing barrels at you ",
    "Houston we have a problem ",
    "If you want to have a program for moving out into the universe you have to think in centuries not in decades ",
    "The nitrogen in our DNA the calcium in our teeth the iron in our blood the carbon in our apple pies were made in the interiors of collapsing stars We are made of starstuff ",
    "Since in the long run every planetary society will be endangered by impacts from space every surviving civilization is obliged to become spacefaring  ",
    "Every one of us is in the cosmic perspective precious If a human disagrees with you let him live In a hundred billion galaxies you will not find another ",
    "If you wish to make an apple pie from scratch you must first invent the universe ",
    "A long time ago in a galaxy far far away ",
    "There are those who believe that life here began out there far across the universe with tribes of humans who may have been the forefathers of the Egyptians or the Toltecs or the Mayans ",
    "It was the dawn of the third age of mankind, ten years after the Earth Minbari war The Babylon Project was a dream given form ",
    "And the word went forth to every outpost of humanity and they came the Aries the Gemons the Virgos the Scorpios the Pisceans and the Sagitarrans "};
    string usedQuote;
    bool inputting;
    bool received;
    public KMBombInfo bomb;
    public KMBombModule module;
    public KMAudio sound;
    int moduleId;
    static int moduleIdCounter = 1;
    bool solved;

    void Awake()
    {
        moduleId = moduleIdCounter++;
        spaceBar.OnInteract += delegate { pressSpace(); return false; };
    }

    void Start ()
    {
        spaceIndex = rnd.Range(0, 15);
        usedQuote = quotes[spaceIndex];
        Debug.LogFormat("[Space #{0}] The quote is {1}.", moduleId, quotes[spaceIndex]);
        background.material = spaceMats[spaceIndex];
	}

	void pressSpace ()
    {
		if (!solved)
        {
            if (!inputting)
            {
                StartCoroutine(handleSpacePress());
                inputting = true;
            }
            else
            {
                received = true;
            }
        }
	}

     IEnumerator handleSpacePress()
    {
        int currentChar = 0;
        while (currentChar < usedQuote.Length)
        {
            int storedTime = (int)bomb.GetTime();
            while (storedTime == (int)bomb.GetTime())
            {
                yield return null;
            }
            if (usedQuote[currentChar] == ' ')
            {

                if (!received)
                {
                    module.HandleStrike();
                    StopCoroutine(handleSpacePress());
                    inputting = false;
                    Debug.LogFormat("[Space #{0}] Character {1} was a space, but the space bar was not pressed. Strike!", moduleId, currentChar+1);
                    StopAllCoroutines();
                    yield break;
                }

            }
            else if (received)
            {
                module.HandleStrike();
                StopCoroutine(handleSpacePress());
                inputting = false;
                Debug.LogFormat("[Space #{0}] Character {1} was not a space, but the space bar was pressed. Strike!", moduleId, currentChar+1);
                StopAllCoroutines();
                yield break;
            }

            currentChar++;
            received = false;
        }
        module.HandlePass();
        solved = true;
        sound.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
        Debug.LogFormat("[Space #{0}] Module solved.", moduleId);
    }
}
