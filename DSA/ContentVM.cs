using System;
using System.Collections.Generic;
using System.Text;

namespace DSA
{
    class ContentVM
    {
        public ContentVM(string name, int index)
        {
            Name = name;
            Index = index;
        }
        public string Name { get; set; }
        public int Index { get; set; }
    }
}
