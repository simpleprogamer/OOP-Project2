using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MauiApp1.Models;
using MauiApp1.Strategies;
using MauiApp1.Services;

namespace MauiApp1.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly XmlAttributeLoader _loader = new();
        private readonly SearchContext _context = new();
        private HtmlTransformer? _transformer;

        private string _filePath = string.Empty;
        private string _keywords = string.Empty;
        private string _outputText = string.Empty;
        private string _status = "Choose XML file";
        private bool _isHtmlVisible;
        private IAnalysisStrategy _selectedStrategy;
        private List<Resident> _results = new();
        public MainViewModel()
        {
            Strategies = new List<IAnalysisStrategy>
            {
                new SaxStrategy(), new DomStrategy(), new LinqToXmlStrategy()
            };
            _selectedStrategy = Strategies[0];
        }
        public List<IAnalysisStrategy> Strategies { get; }
        public ObservableCollection<AttributeOption> AvailableAttributes { get; } = new();
        public string FilePath
        {
            get => _filePath;
            private set => Set(ref _filePath, value);
        }
        public string Keywords
        {
            get => _keywords;
            set => Set(ref _keywords, value);
        }
        public string OutputText
        {
            get => _outputText;
            private set => Set(ref _outputText, value);
        }
        public string Status
        {
            get => _status;
            private set => Set(ref _status, value);
        }
        public string HtmlSource
        {
            get;
            private set;
        } = string.Empty;
        public IAnalysisStrategy SelectedStrategy
        {
            get => _selectedStrategy;
            set
            {
                if (value != null) Set(ref _selectedStrategy, value);
            }
        }
        public bool IsHtmlVisible
        {
            get => _isHtmlVisible;
            private set
            {
                if (Set(ref _isHtmlVisible, value)) OnPropertyChanged(nameof(IsTextVisible));
            }
        }
        public bool IsTextVisible => !_isHtmlVisible;
        public void SetXslPath(string xslPath) => _transformer = new HtmlTransformer(xslPath);
        public void LoadFile(string Path)
        {
            try 
            {
                var attributes = _loader.LoadAttributes(Path);
                if (attributes.Count == 0)
                {
                    Status = $"No attributes found in the XML file.";
                    return;
                }
                FilePath = Path;
                AvailableAttributes.Clear();
                foreach (var name in attributes)
                {
                    AvailableAttributes.Add(new AttributeOption(name));
                }
                OutputText = string.Empty;
                HtmlSource = string.Empty;
                IsHtmlVisible = false;
                Status = $"File downloaded. Attributes found {attributes.Count}. Choose the attribute to find";

            }
            catch (Exception ex)
            {
                Status = $"Error loading file: {ex.Message}";
            }
        }
        public void Search() 
        {
            if (string.IsNullOrEmpty(FilePath)) { Status = "Choose the XML file"; return; }
            var attributes = AvailableAttributes.Where(a => a.IsSelected).Select(a => a.Name).ToList();
            if (attributes.Count == 0) { Status = "Choose at least one attribute"; return; }
            var keywords = Keywords.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
            if (keywords.Count == 0) { Status = "Enter keywords (separated by commas)";
                return;
            }
            var criteria = new SearchCriteria();
            criteria.Attributes.AddRange(attributes);
            criteria.Keywords.AddRange(keywords);

            try 
            {
                _context.SetStrategy(SelectedStrategy);
                _results = _context.ExecuteSearch(FilePath, criteria);
                OutputText = FormatResults(criteria);
                IsHtmlVisible = false;
                Status = $"Method: {SelectedStrategy.Name}. Records found: {_results.Count}";
            }
            catch(Exception ex) 
            {
                Status = $"Search error: {ex.Message}";
            }
        }
        public void TransformToHtml()
        {
            if (string.IsNullOrEmpty(FilePath))
            { Status = "Choose the XML file";
                return;
            }
            if (_transformer == null)
            {
                Status = "XSLT file not set";
                return;
            }
            try 
            {/*
                var xslPath = Path.Combine(FileSystem.AppDataDirectory, "dormitory.xsl");
                Status = File.ReadAllText(xslPath).Substring(0, 150);
                return; перевірка трансформації ХТМЛ*/ 
                HtmlSource =_transformer.Transform(FilePath);
                var outPath = Path.Combine(FileSystem.AppDataDirectory, "dormitory.html");
                File.WriteAllText(outPath, HtmlSource, new UTF8Encoding(false));
                IsHtmlVisible = true;
                Status = $"HTML file created at {outPath}";
            }
            catch (Exception ex)
            {
                Status = $"Error transforming to HTML: {ex.Message}";
            }
        }
        public void Clear() 
        {
            OutputText = "";
                        HtmlSource = "";
            IsHtmlVisible = false;
            Keywords = "";
            foreach (var a in AvailableAttributes) a.IsSelected = false;
            _results = new List<Resident>();
            Status = "Search field and attributes cleared";
        }
        private string FormatResults(SearchCriteria criteria) 
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Analysis method: {SelectedStrategy.Name}");
            sb.AppendLine($"Search criteria: {criteria.BuildQuery()}");
            sb.AppendLine($"Results found: {_results.Count}");
            sb.AppendLine(new string('-', 40));
            for (int i = 0; i < _results.Count; i++)
            {
                sb.AppendLine(_results[i].Format(i + 1));
            }
            return sb.ToString();
        }
    }

}
