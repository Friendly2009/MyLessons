using Newtonsoft.Json;
namespace MyLessons.ConverterSQLClass
{
	public static class ControllerConvert
	{
		public static List<lesson> ConvertToData(string data)
		{
			var list = new List<lesson>();
			string[] lessons = data.Split('|');
			for (int i = 0; i < lessons.Length; i++)
			{
				list.Add(JsonConvert.DeserializeObject<lesson>(lessons[i]));
			}
			return list;
		}

		public static List<string> FindAllClass(string data)
		{
			var list = ConvertToData(data);		
			List<string> socials = new List<string>();
			for (int i = 0; i < list.Count; i++)
			{
				socials.Add(Convert.ToString(list[i].clas));
			}
			socials = socials.Distinct().ToList();
			return socials;
		}
		public static List<string> SelectTeachers(string data)
		{
			List<string> list = new List<string>();
			string[] blocks = data.Split('|');
			for(int i = 0; i < blocks.Length; i++)
			{
				string[] names = blocks[i].Split("`");
				list.Add(names[0]);
			}
			return list;
		}
		public static List<string> SelectObjects(string data)
		{
			List<string> list = new List<string>();
			string[] blocks = data.Split('|');
			for (int i = 0; i < blocks.Length; i++)
			{
				string[] names = blocks[i].Split("`");
				list.Add(names[1]);
			}
			return list;
		}
	}
}
