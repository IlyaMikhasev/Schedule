using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Schedule
{
    public class Employer
    {
        private int age;
        private string name;
        private bool sex;
        private string position;
        public Employer(int _age, string _name , bool _sex, string _position = "worker")
        {
            age = _age;
            name = _name;
            sex = _sex;
            position = _position;
        }
        virtual public string Work() {
            if (position == "worker")
                return "5/2, 8 hours";
            else
                return "2/2, 12 hours";
        }
        public int Age { get { return age; } }
        public string Name { get { return name; } }
        public bool Sex { get {  return sex; } }
        public string Position { get { return position; } }
    }
    public class YoungEmployer : Employer
    {
        public YoungEmployer(int _age, string _name, bool _sex) : base(_age, _name, _sex)
        {
        }
        public override string Work() {
            return "5/2, 4 hours";
        }
    }
    public class OldEmployer : Employer
    {
        public OldEmployer(int _age, string _name, bool _sex) : base(_age, _name, _sex)
        {
        }
        public override string Work() {
            return "4/3, 8 hours";
        }
    }
}
