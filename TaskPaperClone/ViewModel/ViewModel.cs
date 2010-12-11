using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using DelimiterDirected;
using IronyBased;
using TaskPaperDomain;
using TaskPaperDomain.Domain;

namespace TaskPaperClone.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<IInterpret> _interpreters;
        private string _dslText;

        private ObservableCollection<Project> _projects;

        public ObservableCollection<Project> Projects
        {
            get { return _projects; }
            private set
            {
                if(value != _projects)
                {
                    _projects = value;
                    OnPropertyChanged("Projects");
                }
            }
        }
        public ObservableCollection<IInterpret> Interpreters
        {
            get
            {
                return _interpreters;
            }
            set
            {
                if(_interpreters != value)
                {
                    _interpreters = value;
                    OnPropertyChanged("Interpreters");
                }
            }
        }
        public string DslText
        {
            get
            {
                return _dslText;
            }
            set
            {
                if(_dslText != value)
                {
                    _dslText = value;
                    OnPropertyChanged("DslText");
                }
            }
        }
        public ICommand InterpreteCommand { get; private set; }

        public ViewModel()
        {
            Interpreters = new ObservableCollection<IInterpret>
                               {
                                   new DelimiterParser(),
                                   new ParserCombinatorBased.FParsecParser() as IInterpret,
                                   new IronicParser()
                               };
            Projects = new ObservableCollection<Project>();
            InterpreteCommand = new InterpreteCommand(this);
            DslText = @"Project:
Some notes
- A Task
- Another Task
Write a DSL:
It's a thing
- A Task";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
