using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

/** 
 * shorthand for accessing text and 
 * buttons shown to user on top of camera
 */
public class GuiControl : MonoBehaviour 
{
	public QuoteBox[] quoteBoxArray;

	private Queue<QuoteBox> quoteBoxes;

	void Start () 
	{
		quoteBoxes = new Queue<QuoteBox> (quoteBoxArray);
	}

	public void Say(string quote, NpcControl speaker)
	{
		var qb = quoteBoxes.Dequeue ();
		qb.Say (quote, speaker);
		quoteBoxes.Enqueue (qb);
	}

	public void EndTalk()
	{
		quoteBoxes.ToList ().ForEach ((qb) => qb.RemoveSpeaker());
	}

	void Update () 
	{
	}
}
