using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp1.Models;

namespace MauiApp1.Strategies
{
    public class SearchContext
    {
        private IAnalysisStrategy? _strategy;
        public void SetStrategy(IAnalysisStrategy strategy) => _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    public List<Resident> ExecuteSearch(string path, SearchCriteria criteria)
        {
            if (_strategy == null)
            throw new InvalidOperationException("Strategy not set.");
            return _strategy.Search(path, criteria);
        }
            }
}
