using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cursovaya
{
    internal class MyPicture
    {
        public string Name { get; set; }
        public string Img { get; set; }
        public string Categ { get; set; }
        public List<string> Tags { get; set; }
        public MyPicture() { }
        public MyPicture(string name, string img)
        {
            Name = name;
            Img = img;
            Categ = "";
            Tags = new List<string>();
        }
        public MyPicture(string name, string img, string categ)
        {
            Name = name;
            Img = img;
            Categ = categ;
        }
        public void add_tag(string tag)
        {
            Tags.Add(tag);
        }

    }
}
