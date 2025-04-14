using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;   // Required for bitmaps

namespace PromptQuest.Models {
	public class Player {
		/// <summary> Primary Key </summary>
		public int PlayerId { get; set; }
		[Required] // Data annotation to specify that the Name property is Required
		[RegularExpression(@"^[a-zA-Z\s]+$",ErrorMessage = "Name must not contain numbers or special characters.")]
		public string Name { get; set; } = "";
		public int HealthPotions { get; set; }
		public int MaxHealth { get; set; }
		public int CurrentHealth { get; set; }
		public int Defense { get; set; }
		public int Attack { get; set; }
		[Required]
		public string Class { get; set; } = "";
		/// <summary>The index of the equipped item in Player.Items</summary>
		public int IndexEquippedItem { get; set; }
		public List<Item> Items { get; set; } = new List<Item> { //Default items
																							new Item { PlayerId = 0, ItemId = 1, Name = "Jeweled Helmet", Attack = 0, Defense = 2, ImageSrc = "/images/PlaceholderItem1.png" },
																							new Item { PlayerId = 0, ItemId = 2, Name = "Fiery Sword", Attack = 4, Defense = 0, ImageSrc = "/images/PlaceholderItem2.png" },
																							new Item { PlayerId = 0, ItemId = 3, Name = "Frozen Shield", Attack = 1, Defense = 3, ImageSrc = "/images/PlaceholderItem3.png" },
																							new Item { PlayerId = 0, ItemId = 4, Name = "Warded Sword", Attack = 3, Defense = 2, ImageSrc = "/images/PlaceholderItem4.png" }
																						};
	}
}