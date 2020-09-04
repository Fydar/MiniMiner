using UnityEngine;
using UnityEngine.UI;

namespace GBJam8.DialgoueSystem
{
	public class PopupOption : MonoBehaviour
	{
		public Image Selector;
		public Text OptionText;

		public void SetContent(string text)
		{
			OptionText.text = text;
		}

		public void SetState(bool selection)
		{
			Selector.enabled = selection;
		}
	}
}