using System;
namespace FinalProj
{
    [Serializable]
    public class User
    {
		private String name;
		private String password;
        private String salt;

        public String gsName
		{
            get { return name; }
            set { name = value; }
		}

        public String gsPass
        {
            get { return password; }
            set { password = value; }
        }

        public String gsSalt
        {
            get { return salt; }
            set { salt = value; }
        }

        public int compareTo(User user)
        {
            return string.Compare(this.name, user.gsName, StringComparison.Ordinal);
        }
    }
}
