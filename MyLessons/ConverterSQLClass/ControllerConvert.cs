using Newtonsoft.Json;
namespace MyLessons.ConverterSQLClass
{
	public static class ControllerConvert
	{
		//string => List<lesson>
		public static List<lesson> ConvertToLesson(string data) 
		{
			var list = new List<lesson>();
			string[] lessons = data.Split('|');
			for (int i = 0; i < lessons.Length; i++)
			{
				list.Add(JsonConvert.DeserializeObject<lesson>(lessons[i]));
			}
			return list;
		}
		//string => List<string>
		public static List<string> FindAllClass(string data)
		{
			var list = ConvertToLesson(data);		
			List<string> socials = new List<string>();
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] != null && list[i].clas != null)
				{
					socials.Add(Convert.ToString(list[i].clas));
				}
			}
			socials = socials.Distinct().ToList();
			return socials;
		}
		//string => List<string>
		public static List<string> SelectTeachersName(string data)
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
		//string => List<string>
		public static List<string> SelectTeachersItem(string data)
		{
			List<string> list = new List<string>();
			string[] blocks = data.Split('|');
			for (int i = 0; i < blocks.Length; i++)
			{
				string[] names = blocks[i].Split("`");
				try
				{
                    list.Add(names[1]);

                }
                catch { }
			}
			return list;
		}
		//string => List<string>
		public static List<string> GetListTeacherWithObjec(string data)
		{
			List<string> list = new List<string>();
			string[] Help = data.Split("|");
			foreach (string s in Help)
			{
				string a = s.Replace("`", "  ");
                list.Add(a);
			}
            return list;
        }
		public static string TeachersParametrToBase(string Base, string name,string subject)
		{
			if (string.IsNullOrEmpty(Base))
			{
				return name + "`" + subject;
			}
			return Base + "|" +name + "`" + subject;
		}
		public static string ConvertLessonToString(lesson les)
		{
			string result = "";
			result = JsonConvert.SerializeObject(les);
			return result;
		}
		public static string CleanStringForBase(string value)
		{
			value = value.Trim('|');
			value = value.Replace("||", "|");
			return value;
		}
		public static string ConvertTeachersToString(List<string> teach, List<string> Objec)
		{
			string result = "";
			for (int i = 0; i < teach.Count; i++)
			{
				result += "|" + teach[i] + "`" + Objec[i] + "|";
			}
			result = result.Replace("||","|");
			result = result.Trim('|');
			return result;
		}
        public static string ConvertLessonsArrayToString(List<lesson> list)
        {
            string result = "";
            foreach (lesson item in list)
            {
                result += "|" + JsonConvert.SerializeObject(item);
            }
            result = result.Replace("||", "|");
            result = result.Trim('|');
            return result;
        }
    }
}
