namespace Lecture1
{
    public class Person
    {
       
        public bool Login(string userName, string password)
        {
            return password == "Secret";
        }
    }
}