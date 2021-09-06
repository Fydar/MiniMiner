using GBJam8.State;
using UnityEngine;
using UnityEngine.UI;

namespace GBJam8.DialgoueSystem
{
	public class EquipmentShopRenderer : MonoBehaviour
	{
		public RectTransform LevelHolder;
		public ImageRendererPool LevelPool;

		public Color SelectedBackgroundColor;
		public Color NormalBackgroundColor;

		[Space]
		public Text CostText;
		public Text NameText;
		public Image Background;
		public Image TinyIcon;
		public Sprite EmptySprite;
		public Sprite FullSprite;
		public Sprite LockedSprite;

		private EquipmentState state;
		private EquipmentItemTemplate template;

		public void Setup(EquipmentItemTemplate template, EquipmentState state)
		{
			this.template = template;
			this.state = state;

			FixedUpdate();

			Background.color = NormalBackgroundColor;
			TinyIcon.sprite = template.TinyIcon;

		}

		private void FixedUpdate()
		{
			if (template == null)
			{
				return;
			}
			LevelPool.Flush();

			bool canAfford = true;
			bool isUnlocked = true;

			if (template.Levels.Length == state.Level)
			{
				CostText.text = "MAX";
			}
			else
			{
				var levelData = template.Levels[state.Level];
				canAfford = levelData.Cost <= Game.Instance.State.Player.Money;
				CostText.text = $"${levelData.Cost:#,##0}";

				CostText.color = canAfford
					? SelectedBackgroundColor
					: NormalBackgroundColor;
			}
			if (state.Level == 0)
			{
				isUnlocked = false;
			}

			NameText.color = canAfford || isUnlocked
				? SelectedBackgroundColor
				: NormalBackgroundColor;

			for (int i = 0; i < template.Levels.Length; i++)
			{
				var levelSymbol = LevelPool.Grab(LevelHolder);

				levelSymbol.color = SelectedBackgroundColor;

				if (state.Level > i)
				{
					levelSymbol.sprite = FullSprite;
				}
				else if (state.Level == i)
				{
					levelSymbol.sprite = EmptySprite;
					levelSymbol.color = canAfford
						? SelectedBackgroundColor
						: NormalBackgroundColor;
				}
				else if (state.Level < i)
				{
					levelSymbol.sprite = LockedSprite;
				}

				NameText.text = template.DisplayName;
			}
		}
	}
}