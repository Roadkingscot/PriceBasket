using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceBasket
{
	public interface IGoods
	{
		string GetName();
		double GetPrice();
		double GetDiscount();
	}
	// Define my goods - which all implement the IGoods interface
	public class Soup : IGoods
	{
		#region IGoods members
		public string GetName()
		{
			return "Soup";
		}
		public double GetPrice()
		{
			return 0.65;
		}
		public double GetDiscount()
		{
			return 0;
		}
		#endregion
	}
	public class Bread : IGoods
	{
		#region IGoods members
		public string GetName()
		{
			return "Bread";
		}
		public double GetPrice()
		{
			return 0.80;
		}
		public double GetDiscount()
		{
			return 0;
		}
		#endregion
	}
	public class Milk : IGoods
	{
		#region IGoods members
		public string GetName()
		{
			return "Milk";
		}
		public double GetPrice()
		{
			return 1.30;
		}
		public double GetDiscount()
		{
			return 0;
		}
		#endregion
	}
	public class Apples : IGoods
	{
		#region IGoods members
		public string GetName()
		{
			return "Apples";
		}
		public double GetPrice()
		{
			return 1.00;
		}
		public double GetDiscount()
		{
			return 00.10;
		}
		#endregion
	}


	public class PriceBasket
	{
		public static void Main(string[] args)
		{
			try
			{
				//Create the correct goods object for each ingredient passed in the parameters
				List<IGoods> goods = new List<IGoods>();
				for (int i = 0; i < args.Length; i++)
				{
					//Don't forget to handle products that may be passed in any case. Handle this by converting to uppercase before checking.
					switch (args[i].ToUpper())
					{
					case "SOUP" :
						goods.Add(new Soup());
						break;
					case "BREAD" :
						goods.Add(new Bread());
						break;
					case "MILK" :
						goods.Add(new Milk());
						break;
					case "APPLES" :
						goods.Add(new Apples());
						break;
					default :
						throw new ArgumentException(args[i] + " is an invalid entry");
					}
				}
				//Now that we have a list of goods objects generate the output from the products.
				PriceOutput(goods);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

		}
		//This function loops through the list of goods and works out the subtotal, any discounts and a discounted price.
		private static void PriceOutput(List<IGoods> goods)
		{
			double subtotal = 0;
			string discounttext = "";
			double discount = 0;
			int matchingitems = 0;

			for (int i = 0; i < goods.Count(); i++)
			{
				subtotal += goods[i].GetPrice();
				if(goods[i].GetDiscount() > 0)
				{
					//Update discounttext checking that we havenít already mentioned this item
					if(!discounttext.Contains(goods[i].GetName()))
					{
						discounttext += goods[i].GetName() + " " + System.Convert.ToString(goods[i].GetDiscount() * 100) + "% off: - ";
						//Add up the discount for any matching items
						matchingitems = (from n in goods where n.GetName()==goods[i].GetName() select n).Count();
						discount += goods[i].GetDiscount() * matchingitems;
						discounttext += System.Convert.ToString(((goods[i].GetPrice() * 100) * goods[i].GetDiscount()) * matchingitems) + "p ";
					}
				}

			}
			discounttext += CombinationDiscount(goods, "Soup", 2, "Bread", 1, 00.50, ref discount);
			//If we havenít found a discount then set discounttext to indicate this
			if (discounttext.Length == 0)
			{
				discounttext = "(No offers available)";
			}
			//Output the subtotal
			Console.WriteLine("Subtotal: " + string.Format("{0:C}",subtotal));

			//Output the discount items
			Console.WriteLine(discounttext);

			//Output the total - Subtotal minus any discounts
			Console.WriteLine("Total price: " + string.Format("{0:C}", subtotal - discount));
			return;
		}
		//This function handles combination discounts. Combitem indicates the source combination items. Combamount indicates the
		//number of the combination items needed to achieve a combination discount. Discitem indicates the item that recieves the
		//combination discount. Discitemcount indicates how many items recieve a discount. DiscAmount indicates the amount of discount.
		//discount (passed by reference) is the value of all discounts recieved and is updated by this method.
		private static string CombinationDiscount(List<IGoods> goods, string combitem, int combamount, string discitem, int discitemcount, double discAmount, ref double discount)
		{

			int matchingitems = 0;
			int matchingdiscounts = 0;
			int applieddiscount = 0;
			double combdisc = 0;
			string discountresult = "";
			if(discitemcount > 0)
			{
				matchingitems = (from n in goods where n.GetName() == combitem select n).Count();
				matchingdiscounts = (from n in goods where n.GetName() == discitem select n).Count();

				if (matchingitems >= combamount && matchingdiscounts > 0)
				{
					discountresult = discitem + " " + discitemcount.ToString() + " item " + System.Convert.ToString(discAmount * 100) + "% off: - ";
					List<IGoods> discountItems = goods.Where(x => x.GetName() == discitem).ToList();
					for (int i = 0; i < discountItems.Count(); i++)
					{
						combdisc += (discountItems[i].GetPrice() * discAmount);
						applieddiscount = 1;
						if (applieddiscount >= discitemcount)
						{
							break;
						}
					}
					discountresult += System.Convert.ToString(combdisc * 100) + "p ";
				}
				discount += combdisc;
			}
			return discountresult;
		}

	}
}
