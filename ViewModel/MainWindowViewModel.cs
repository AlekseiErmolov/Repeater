using MetroListBox;
using Repeater.Classes;
using Repeater.Interfaces;
using Repeater.Model;
using Repeater.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Repeater.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        #region Variables
        private ILesson _model;
        private IRepository _loader;
        
        private ICommand _enterCommand;
        private IRandomize _cardsService;
        private EnterState _enterState;
        private ICard _displayedCard;
        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор ViewModel главного окна
        /// </summary>
        /// <param name="model"></param>
        public MainWindowViewModel(ILesson model, IRepository repository)
        {
            LessonsLoaderXml x = new LessonsLoaderXml();
            var s = x.LoadLesson("1");

            x.SaveToLessonNewCard("bla", new Card() { Comment = "new", ForeignTask = "new", NativeTask = "new" });

            x.CreateNewLesson("bla");

            _model = model;
            _loader = repository;
            _cardsService = new CardsService();
            _menu = new MetroMenu();
            _menu.MenuSelectEvent += _menu_MenuSelectEvent;
            _IsEnableToEnterFileName = false;
            NotifyPropertyChanged("IsEnableToEnterFileName");

            LoadTheme();

            CorrectAnswerCnt = 0;
            IncorrectAnswerCnt = 0;

            IsRepeat = false;

            _model.LessonsNames = _loader.LoadLessonsName();
        }

        /// <summary>
        /// Событие выбора урока
        /// </summary>
        /// <param name="name"></param>
        private void _menu_MenuSelectEvent(string name)
        {
            ClearCard();
            
            OpenSelectedLesson(name);
            NewLessonFileName = name;
            IsRepeat = false;
        }

        #endregion

        #region Properties

        private bool _IsRepeat;
        public bool IsRepeat
        {
            get
            {
                return _IsRepeat;
            }
            set
            {
                _IsRepeat = value;
                NotifyPropertyChanged("IsRepeat");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private ITheme _theme;
        public ITheme Theme
        {
            get
            {
                return _theme;
            }
            set
            {
                _theme = value;
                NotifyPropertyChanged("TitleColor");
                NotifyPropertyChanged("TextColor");
                NotifyPropertyChanged("MenuColor");
                NotifyPropertyChanged("ProgressColor");
                NotifyPropertyChanged("ProgressStatusColor");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TitleColor
        {
            get
            {
                return Theme.TitleColor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TextColor
        {
            get
            {
                return Theme.TextColor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string MenuColor
        {
            get
            {
                return Theme.MenuColor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ProgressColor
        {
            get
            {
                return Theme.ProgressColor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ProgressStatusColor
        {
            get
            {
                return Theme.ProgressStatusColor;
            }
        }

        /// <summary>
        /// Флаг разрешения добавления новой карточки
        /// </summary>
        private bool _IsEnableMakeCard = false;
        public bool IsEnableMakeCard
        {
            get
            {
                return _IsEnableMakeCard;
            }
            set
            {
                _IsEnableMakeCard = value;
                NotifyPropertyChanged("IsEnableMakeCard");
            }
        }

        /// <summary>
        /// Разрешено ли вводить имя нового урока
        /// </summary>
        private bool _IsEnableToEnterFileName;
        public bool IsEnableToEnterFileName
        {
            get
            {
                return _IsEnableToEnterFileName;
            }
            set
            {
                _IsEnableToEnterFileName = value;
                NotifyPropertyChanged("IsEnableToEnterFileName");
            }
        }

        /// <summary>
        /// Количество пройденных карточек в уроке
        /// </summary>
        private int _correctAnswerCnt;
        public int CorrectAnswerCnt
        {
            get
            {
                return _correctAnswerCnt;
            }

            set
            {
                _correctAnswerCnt = value;
                NotifyPropertyChanged("CorrectAnswerCnt");
            }
        }

        /// <summary>
        /// Меню уроков
        /// </summary>
        private MetroMenu _menu;
        public MetroMenu Menu
        {
            get
            {
                _menu.Clear();
                foreach (var lesson in _model.LessonsNames)
                {
                    if (String.IsNullOrEmpty(lesson)) continue;
                    _menu.Add(lesson.Length <= 15 ? lesson : lesson.Substring(0, 15));
                }

                return _menu;
            }

            set
            {
                _menu = value;
                NotifyPropertyChanged("Menu");
            }
        }

        /// <summary>
        /// Количество неправильных ответов
        /// </summary>
        private int _incorrectAnswerCnt;
        public int IncorrectAnswerCnt
        {
            get
            {
                return _incorrectAnswerCnt;
            }
            set
            {
                _incorrectAnswerCnt = value;
                NotifyPropertyChanged("IncorrectAnswerCnt");
            }
        }

        /// <summary>
        /// Цвет пользовательского ответа
        /// </summary>
        private Brush _foregroundColor = Brushes.Black;
        public Brush Forecolor
        {
            get
            {
                return _foregroundColor;
            }

            set
            {
                _foregroundColor = value;
                NotifyPropertyChanged("Forecolor");
            }
        }

        /// <summary>
        /// Введенный ответ от пользователя
        /// </summary>
        private string _userText;
        public string UserText
        {
            get
            {
                return _userText;
            }

            set
            {
                _userText = value;
                NotifyPropertyChanged("UserText");
            }
        }

        /// <summary>
        /// Поле задания на родном
        /// </summary>
        public string NativeTask
        {
            get
            {
                if (_displayedCard == null)
                    return String.Empty;

                if (_invertTask != true)
                {
                    if (!IsEnableMakeCard)
                    {
                        if (_enterState == EnterState.First)
                            return String.Empty;
                    }
                }

                return _displayedCard.NativeTask;
            }

            set
            {
                if (_displayedCard == null)
                    _displayedCard = new Card();

                _displayedCard.NativeTask = value;
                NotifyPropertyChanged("NativeTask");
            }
        }

        /// <summary>
        /// Поле задания на иностранном
        /// </summary>
        public string ForeignTask
        {
            get
            {
                if (_displayedCard == null)
                    return String.Empty;

                if (_invertTask == true)
                {
                    if (!IsEnableMakeCard)
                    {
                        if (_enterState == EnterState.First)
                            return String.Empty;
                    }
                }

                return _displayedCard.ForeignTask;
            }

            set
            {
                if (_displayedCard == null)
                    _displayedCard = new Card();

                _displayedCard.ForeignTask = value;
                NotifyPropertyChanged("ForeignTask");
            }
        }

        /// <summary>
        /// Поле комментария
        /// </summary>
        public string Comment
        {
            get
            {
                if (_displayedCard == null)
                    return String.Empty;

                if (!IsEnableMakeCard)
                {
                    if (_enterState == EnterState.First)
                        return String.Empty;
                }

                return _displayedCard.Comment;
            }

            set
            {
                if (_displayedCard == null)
                    _displayedCard = new Card();

                _displayedCard.Comment = value;
                NotifyPropertyChanged("Comment");
            }
        }

        /// <summary>
        /// Флаг инверсии заданий
        /// </summary>
        private bool _invertTask;
        public bool InvertTask
        {
            get
            {
                try
                {
                    _invertTask = Properties.Settings.Default.Invert;
                }
                catch
                {
                    Properties.Settings.Default.Invert = false;
                    Properties.Settings.Default.Save();
                    _invertTask = false;
                }

                return _invertTask;
            }

            set
            {
                _invertTask = value;
                Properties.Settings.Default.Invert = value; ;
                Properties.Settings.Default.Save();
                NotifyPropertyChanged("InvertTask");
            }
        }

        /// <summary>
        /// Количество карточек в уроке
        /// </summary>
        private int _cardsCount;
        public int CardsCount
        {
            get
            {
                return _cardsCount;
            }
            set
            {
                _cardsCount = value;
                NotifyPropertyChanged("CardsCount");
                NotifyPropertyChanged("CardsCountString");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CardsCountString
        {
            get
            {
                return CardsCount.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _NewLessonFileName;
        public string NewLessonFileName
        {
            get
            {
                return _NewLessonFileName;
            }

            set
            {
                _NewLessonFileName = value;
                NotifyPropertyChanged("NewLessonFileName");
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// 
        /// </summary>
        private ICommand _RepeatCommand;
        public ICommand RepeatCommand
        {
            get
            {
                if (_RepeatCommand == null)
                    _RepeatCommand = new RelayCommand<object>(RepeatCommandHandler);
                return _RepeatCommand;
            }
            set
            {
                _RepeatCommand = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void RepeatCommandHandler(object obj)
        {
            Repeat();
        }

        /// <summary>
        /// 
        /// </summary>
        private ICommand _GreyBlueThemeCommand;
        public ICommand GreyBlueThemeCommand
        {
            get
            {
                if (_GreyBlueThemeCommand == null)
                    _GreyBlueThemeCommand = new RelayCommand<object>(GreyBlueThemeCommandHandler);
                return _GreyBlueThemeCommand;
            }
            set
            {
                _GreyBlueThemeCommand = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void GreyBlueThemeCommandHandler(object obj)
        {
            Theme = new ThemeBlueGrey();
            Properties.Settings.Default.Theme = "GreyBlue";
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        private ICommand _CyanThemeCommand;
        public ICommand CyanThemeCommand
        {
            get
            {
                if (_CyanThemeCommand == null)
                    _CyanThemeCommand = new RelayCommand<object>(CyanThemeCommandHandler);
                return _CyanThemeCommand;
            }
            set
            {
                _CyanThemeCommand = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void CyanThemeCommandHandler(object obj)
        {
            Theme = new ThemeCyan();
            Properties.Settings.Default.Theme = "Cyan";
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        private ICommand _GreenThemeCommand;
        public ICommand GreenThemeCommand
        {
            get
            {
                if (_GreenThemeCommand == null)
                    _GreenThemeCommand = new RelayCommand<object>(GreenThemeCommandHandler);
                return _GreenThemeCommand;
            }
            set
            {
                _GreenThemeCommand = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void GreenThemeCommandHandler(object obj)
        {
            Theme = new ThemeGreen();
            Properties.Settings.Default.Theme = "Green";
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        private ICommand _BlueThemeCommand;
        public ICommand BlueThemeCommand
        {
            get
            {
                if (_BlueThemeCommand == null)
                    _BlueThemeCommand = new RelayCommand<object>(BlueThemeCommandHandler);
                return _BlueThemeCommand;
            }
            set
            {
                _BlueThemeCommand = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void BlueThemeCommandHandler(object obj)
        {
            Theme = new ThemeBlue();
            Properties.Settings.Default.Theme = "Blue";
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        private ICommand _SaveCardCommand;
        public ICommand SaveCardCommand
        {
            get
            {
                if (_SaveCardCommand == null)
                    _SaveCardCommand = new RelayCommand<object>(SaveCardCommandHandler);
                return _SaveCardCommand;
            }
            set
            {
                _SaveCardCommand = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void SaveCardCommandHandler(object obj)
        {
            _loader.SaveToLessonNewCard(NewLessonFileName, _displayedCard);
            IsEnableMakeCard = false;
            ClearCard();
        }

        /// <summary>
        /// 
        /// </summary>
        private ICommand _SaveCardAndNewCommand;
        public ICommand SaveCardAndNewCommand
        {
            get
            {
                if (_SaveCardAndNewCommand == null)
                    _SaveCardAndNewCommand = new RelayCommand<object>(SaveCardAndNewCommandHandler);
                return _SaveCardAndNewCommand;
            }
            set
            {
                _SaveCardAndNewCommand = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void SaveCardAndNewCommandHandler(object obj)
        {
            _loader.SaveToLessonNewCard(NewLessonFileName, _displayedCard);
            IsEnableMakeCard = true;
            ClearCard();
        }

        /// <summary>
        /// 
        /// </summary>
        private ICommand _AddCardCommand;
        public ICommand AddCardCommand
        {
            get
            {
                if (_AddCardCommand == null)
                    _AddCardCommand = new RelayCommand<object>(AddCardCommandHandler);
                return _AddCardCommand;
            }
            set
            {
                _AddCardCommand = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void AddCardCommandHandler(object obj)
        {
            ClearCard();

            if (_menu.SelectedLesson != null)
                IsEnableMakeCard = true;
        }

        /// <summary>
        /// Команда создания нового урока
        /// </summary>
        private ICommand _NewLessonCommand;
        public ICommand NewLessonCommand
        {
            get
            {
                if (_NewLessonCommand == null)
                    _NewLessonCommand = new RelayCommand<object>(NewLessonCommandHandler);
                return _NewLessonCommand;
            }
            set
            {
                _NewLessonCommand = value;
            }
        }

        /// <summary>
        /// Событие создания нового урока
        /// </summary>
        /// <param name="obj"></param>
        private void NewLessonCommandHandler(object obj)
        {
            IsEnableToEnterFileName = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private ICommand _CreateNewLessonFileCommand;
        public ICommand CreateNewLessonFileCommand
        {
            get
            {
                if (_CreateNewLessonFileCommand == null)
                    _CreateNewLessonFileCommand = new RelayCommand<object>(CreateNewLessonFileCommandHandler);
                return _CreateNewLessonFileCommand;
            }
            set
            {
                _CreateNewLessonFileCommand = value;
                NotifyPropertyChanged("CreateNewLessonFileCommand");
            }
        }

        /// <summary>
        /// Событие создания нового урока
        /// </summary>
        /// <param name="obj"></param>
        private void CreateNewLessonFileCommandHandler(object obj)
        {
            IsEnableToEnterFileName = false;

            _loader.CreateNewLesson(NewLessonFileName);
            _model.LessonsNames = _loader.LoadLessonsName();
            NotifyPropertyChanged("Menu");
        }

        /// <summary>
        /// Команда нажатия Enter на поле ответов
        /// </summary>
        public ICommand EnterCommand
        {
            get
            {
                if (_enterCommand == null)
                    _enterCommand = new RelayCommand<object>(EnterCommandHandler);
                return _enterCommand;
            }

            set
            {
                _enterCommand = value;
            }
        }


        /// <summary>
        /// Обработчик нажатия Enter на поле с ответами
        /// </summary>
        /// <param name="obj"></param>
        private void EnterCommandHandler(object obj)
        {
            switch (_enterState)
            {
                case EnterState.First:
                    _enterState = EnterState.Second;
                    if (_cardsCount - _correctAnswerCnt - _incorrectAnswerCnt != 0)
                    {
                        SpellChecking();
                    }
                    else
                    {
                        Repeat();
                    }
                    break;

                case EnterState.Second:
                    if (_cardsCount - _correctAnswerCnt - _incorrectAnswerCnt == 0)
                    {
                        IsRepeat = true;
                    }
                    else
                    {
                        var tmp = _model.Cards;
                        _displayedCard = _cardsService.GetCardReduceList(ref tmp);
                        _model.Cards = tmp;
                        UserText = String.Empty;
                        Forecolor = Brushes.Black;
                    }
                    _enterState = EnterState.First;

                    break;
            }

            RefreshCard();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Открытие нового урока по выбору пользователя
        /// </summary>
        private void OpenSelectedLesson(string name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                _model.Cards = _loader.LoadLesson(name);
                CardsCount = _model.Cards.Count();

                var tmp = _model.Cards;
                _displayedCard = _cardsService.GetCardReduceList(ref tmp);
                _model.Cards = tmp;

                _enterState = EnterState.First;
                RefreshCard();
            }
        }


        /// <summary>
        /// Получение новой карточки для отображения
        /// </summary>
        private void RefreshCard()
        {
            NotifyPropertyChanged("Comment");
            NotifyPropertyChanged("ForeignTask");
            NotifyPropertyChanged("NativeTask");
            NotifyPropertyChanged("UserText");
        }


        /// <summary>
        /// Проверка правильности ввода ответа
        /// </summary>
        private void SpellChecking()
        {
            if (String.IsNullOrEmpty(UserText))
                UserText = String.Empty;

            if (_invertTask)
            {
                if (UserText.Equals(ForeignTask, StringComparison.OrdinalIgnoreCase))
                {
                    Forecolor = Brushes.Black;
                    CorrectAnswerCnt++;
                }
                else
                {
                    Forecolor = Brushes.Red;
                    IncorrectAnswerCnt++;
                }
            }
            else
            {
                if (UserText.Equals(NativeTask, StringComparison.OrdinalIgnoreCase))
                {
                    Forecolor = Brushes.Black;
                    CorrectAnswerCnt++;
                }
                else
                {
                    Forecolor = Brushes.Red;
                    IncorrectAnswerCnt++;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearCard()
        {
            ForeignTask = String.Empty;
            NativeTask = String.Empty;
            Comment = String.Empty;
            UserText = String.Empty;
            _enterState = EnterState.First;
            Forecolor = Brushes.Black;

            CorrectAnswerCnt = 0;
            IncorrectAnswerCnt = 0;
        }

        /// <summary>
        /// Загрузка темы
        /// </summary>
        private void LoadTheme()
        {
            var theme = Properties.Settings.Default.Theme;
            switch (@theme)
            {
                case "Cyan":
                    Theme = new ThemeCyan();
                    break;

                case "BlueGrey":
                    Theme = new ThemeBlueGrey();
                    break;

                case "Green":
                    Theme = new ThemeGreen();
                    break;

                case "Blue":
                    Theme = new ThemeBlue();
                    break;

                default:
                    Theme = new ThemeBlueGrey();
                    break;
            }
        }

        /// <summary>
        /// Повтор выбранного урока
        /// </summary>
        private void Repeat()
        {
            if (!String.IsNullOrEmpty(NewLessonFileName))
            {
                ClearCard();
                OpenSelectedLesson(NewLessonFileName);
                IsRepeat = false;
            }
        }

        #endregion
    }
}
