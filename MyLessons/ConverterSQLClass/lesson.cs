namespace MyLessons.ConverterSQLClass
{
    public class lesson
    {
        public string day {  get; set; }

        public string number { get; set; }

        public string less { get; set; }

        public string teacher { get; set; }

        public string clas { get; set; }

        public string room { get; set; }
        public lesson()
        {

        }
        public lesson(string day, string num,string selectedSubject, string clas, string room)
        {
            this.day = day;
            number = num;
            this.clas = clas;
            this.room = room;
            string[] array = selectedSubject.Split("  ");
            less = array[1];
            teacher = array[0];
        }
        public lesson(string day, string num, string less,string teach, string clas, string room)
        {
            this.day = day;
            number = num;
            this.clas = clas;
            this.room = room;
            this.less = less;
            teacher = teach;
        }
    }
}
