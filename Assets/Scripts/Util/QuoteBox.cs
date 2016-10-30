using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Interfaces;

namespace GameLogic
{
    public class QuoteBox : MonoBehaviour
    {
        public RawImage icon;
        public Text textfield;

        private float? hideAt = null;

        private INpc npc = null;

        void Awake ()
        {
            gameObject.SetActive (false);
        }

        void Update()
        {
            if (hideAt != null && hideAt < Time.fixedTime) {
                hideAt = null;
                RemoveSpeaker ();
            }
        }

        public void Say(string quote, INpc npc)
        {
            gameObject.SetActive (true);
            this.npc = npc;
            icon.texture = npc.GetPortrait();
            textfield.text = quote;
        }

        public void RemoveSpeaker()
        {
            gameObject.SetActive (false);
            npc = null;
            icon.texture = null;
            textfield.text = "";
        }

        public void ShowStats(string text, INpc npc)
        {
            if (this.npc == null || this.npc == npc) {
                gameObject.SetActive (true);
                this.npc = npc;
                icon.texture = npc.GetPortrait();
                textfield.text = text;

                hideAt = Time.fixedTime + 3;
            }
        }
    }
}