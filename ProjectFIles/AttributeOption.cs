using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.ViewModels
{
    public class AttributeOption : ViewModelBase
    {
        private bool _isSelected;
        public AttributeOption(string name) => Name = name;
        public string Name { get; }
        public bool IsSelected 
        {
            get => _isSelected;
            set => Set(ref _isSelected, value);
        }
    }
}
