using DormitoryXML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DormitoryXML.ViewModels
{
    public class AttributeOption : ViewModelBase
    {
        
        public AttributeOption(string name) { Name = name; Title = AttributeTitles.Get(name); }
        public string Name { get; }
        public string Title { get; }
        public override string ToString() => Title;

    }
}
