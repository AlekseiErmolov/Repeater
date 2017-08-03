using System;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Repeater.Infrastructure.Enums;
using Repeater.Infrastructure.Helpers;
using Repeater.Infrastructure.MetroMenu;
using Repeater.Infrastructure.Themes;
using Repeater.Interfaces;
using Repeater.Model;
using Repeater.Properties;
using Repeater.View;

namespace Repeater.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region Variables

        private readonly ILesson _model;
        private readonly IRepository _repository;
        private readonly ICardService _cardsService;

        private ICommand _enterCommand;
        private EnterState _enterState;
        private ICard _displayedCard;

        private readonly ViewModelInfoWindow _infoViewModel;

        #endregion

        #region Constructors

        /// <summary>
        ///     .ctor
        /// </summary>
        public MainWindowViewModel(ILesson lesson, IRepository repo, ICardService cardService,
            ViewModelInfoWindow infoViewModel)
        {
            _model = lesson;
            _repository = repo;
            _infoViewModel = infoViewModel;

            _cardsService = cardService;
            _menu = new MetroMenu();

            _menu.MenuSelectEvent += _menu_MenuSelectEvent;
            _isEnableToEnterFileName = false;
            NotifyPropertyChanged("IsEnableToEnterFileName");

            LoadTheme();

            CorrectAnswerCnt = 0;
            IncorrectAnswerCnt = 0;

            IsRepeat = false;

            _model.LessonsNames = _repository.LoadLessonsName();

            //Создадим стандартный урок
            if (!_model.LessonsNames.Any(x => x.Equals(Constants.DefaultLessonName)))
            {
                _repository.CreateNewLesson(Constants.DefaultLessonName);
                _model.LessonsNames = _repository.LoadLessonsName();
            }
            _model.LessonsNames.Remove(Constants.DefaultLessonName);
            _model.LessonsNames.Insert(0, Constants.DefaultLessonName);

            ViewLessonInfo = new RelayCommand(OpenInfoWindow);
            IsEditableTitle = false;
        }

        /// <summary>
        ///     Menu select event.
        /// </summary>
        /// <param name="name"></param>
        private void _menu_MenuSelectEvent(string name)
        {
            ClearCard();

            OpenSelectedLesson(name);
            _model.OpenedLessonName = name;
            OpenedLesson = name;
            IsRepeat = false;
        }

        #endregion

        #region Properties

        public RelayCommand ViewLessonInfo { get; set; }

        private string _newLessonName;

        public string NewLessonName
        {
            get { return _newLessonName; }

            set
            {
                _newLessonName = value;
                NotifyPropertyChanged("NewLessonName");
            }
        }

        private bool _isRepeat;

        public bool IsRepeat
        {
            get { return _isRepeat; }
            set
            {
                _isRepeat = value;
                NotifyPropertyChanged("IsRepeat");
            }
        }


        public bool IsEditableTitle { get; set; }


        private ITheme _theme;

        public ITheme Theme
        {
            get { return _theme; }
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

        public string TitleColor
        {
            get { return Theme.TitleColor; }
        }

        public string TextColor
        {
            get { return Theme.TextColor; }
        }

        public string MenuColor
        {
            get { return Theme.MenuColor; }
        }

        public string ProgressColor
        {
            get { return Theme.ProgressColor; }
        }

        public string ProgressStatusColor
        {
            get { return Theme.ProgressStatusColor; }
        }

        /// <summary>
        ///     Flag that allows to add new card.
        /// </summary>
        private bool _isEnableMakeCard;

        public bool IsEnableMakeCard
        {
            get { return _isEnableMakeCard; }
            set
            {
                _isEnableMakeCard = value;
                NotifyPropertyChanged("IsEnableMakeCard");
            }
        }

        /// <summary>
        ///     Flag that allows to enter card name.
        /// </summary>
        private bool _isEnableToEnterFileName;

        public bool IsEnableToEnterFileName
        {
            get { return _isEnableToEnterFileName; }
            set
            {
                _isEnableToEnterFileName = value;
                NotifyPropertyChanged("IsEnableToEnterFileName");
            }
        }

        /// <summary>
        ///     Quantity of done cards.
        /// </summary>
        private int _correctAnswerCnt;

        public int CorrectAnswerCnt
        {
            get { return _correctAnswerCnt; }

            set
            {
                _correctAnswerCnt = value;
                NotifyPropertyChanged("CorrectAnswerCnt");
            }
        }

        /// <summary>
        ///     Lesson menu.
        /// </summary>
        private MetroMenu _menu;

        public MetroMenu Menu
        {
            get
            {
                _menu.Clear();
                if (_model.LessonsNames != null)
                {
                    foreach (var lesson in _model.LessonsNames)
                    {
                        if (string.IsNullOrEmpty(lesson)) continue;
                        _menu.Add(lesson.Length <= 15 ? lesson : lesson.Substring(0, 15));
                    }
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
        ///     Wrong cards quantity.
        /// </summary>
        private int _incorrectAnswerCnt;

        public int IncorrectAnswerCnt
        {
            get { return _incorrectAnswerCnt; }
            set
            {
                _incorrectAnswerCnt = value;
                NotifyPropertyChanged("IncorrectAnswerCnt");
            }
        }

        /// <summary>
        ///     Foreground user answer text.
        /// </summary>
        private Brush _foregroundColor = Brushes.Black;

        public Brush Forecolor
        {
            get { return _foregroundColor; }

            set
            {
                _foregroundColor = value;
                NotifyPropertyChanged("Forecolor");
            }
        }

        /// <summary>
        ///     Entered user answer.
        /// </summary>
        private string _userText;

        public string UserText
        {
            get { return _userText; }

            set
            {
                _userText = value;
                NotifyPropertyChanged("UserText");
            }
        }

        /// <summary>
        ///     Native answer field.
        /// </summary>
        public string NativeTask
        {
            get
            {
                if (_displayedCard == null)
                    return string.Empty;

                if (_invertTask != true)
                {
                    if (!IsEnableMakeCard)
                    {
                        if (_enterState == EnterState.First)
                            return string.Empty;
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
        ///     Foreign answer field.
        /// </summary>
        public string ForeignTask
        {
            get
            {
                if (_displayedCard == null)
                    return string.Empty;

                if (_invertTask)
                {
                    if (!IsEnableMakeCard)
                    {
                        if (_enterState == EnterState.First)
                            return string.Empty;
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
        ///     Comment answer field.
        /// </summary>
        public string Comment
        {
            get
            {
                if (_displayedCard == null)
                    return string.Empty;

                if (!IsEnableMakeCard)
                {
                    if (_enterState == EnterState.First)
                        return string.Empty;
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
        ///     Flag of lesson inversion.
        /// </summary>
        private bool _invertTask;

        public bool InvertTask
        {
            get
            {
                try
                {
                    _invertTask = Settings.Default.Invert;
                }
                catch
                {
                    Settings.Default.Invert = false;
                    Settings.Default.Save();
                    _invertTask = false;
                }

                return _invertTask;
            }

            set
            {
                _invertTask = value;
                Settings.Default.Invert = value;
                Settings.Default.Save();
                NotifyPropertyChanged("InvertTask");
            }
        }

        /// <summary>
        ///     Cards quantity.
        /// </summary>
        private int _cardsCount;

        public int CardsCount
        {
            get { return _cardsCount; }
            set
            {
                _cardsCount = value;
                NotifyPropertyChanged("CardsCount");
                NotifyPropertyChanged("CardsCountString");
            }
        }

        public string CardsCountString
        {
            get { return CardsCount.ToString(); }
        }

        public string OpenedLesson
        {
            get { return _model.OpenedLessonName; }

            set
            {
                _model.OpenedLessonName = value;
                NotifyPropertyChanged("OpenedLesson");
            }
        }

        #endregion

        #region ThemesCommands

        private ICommand _cyanThemeCommand;

        public ICommand CyanThemeCommand
        {
            get { return _cyanThemeCommand ?? (_cyanThemeCommand = new RelayCommand(CyanThemeCommandHandler)); }
            set { _cyanThemeCommand = value; }
        }

        private void CyanThemeCommandHandler(object obj)
        {
            Theme = new ThemeCyan();
            Settings.Default.Theme = "Cyan";
            Settings.Default.Save();
        }

        private ICommand _grayThemeCommand;

        public ICommand GrayThemeCommand
        {
            get { return _grayThemeCommand ?? (_grayThemeCommand = new RelayCommand(GrayThemeCommandCommandHandler)); }
        }

        private ICommand _greenThemeCommand;

        public ICommand GreenThemeCommand
        {
            get { return _greenThemeCommand ?? (_greenThemeCommand = new RelayCommand(GreenThemeCommandHandler)); }
        }

        private void GreenThemeCommandHandler(object obj)
        {
            Theme = new ThemeGreen();
            Settings.Default.Theme = "Green";
            Settings.Default.Save();
        }

        private void GrayThemeCommandCommandHandler(object obj)
        {
            Theme = new ThemeBlueGrey();
            Settings.Default.Theme = "Gray";
            Settings.Default.Save();
        }

        private ICommand _blueThemeCommand;

        public ICommand BlueThemeCommand
        {
            get { return _blueThemeCommand ?? (_blueThemeCommand = new RelayCommand(BlueThemeCommandHandler)); }
            set { _blueThemeCommand = value; }
        }

        private void BlueThemeCommandHandler(object obj)
        {
            Theme = new ThemeBlue();
            Settings.Default.Theme = "Blue";
            Settings.Default.Save();
        }

        #endregion

        #region Commands

        private ICommand _renameLesson;

        public ICommand RenameLessonCommand
        {
            get { return _renameLesson ?? (_renameLesson = new RelayCommand(RenameLessonCommandHandler)); }
            set { _renameLesson = value; }
        }

        /// <summary>
        ///     Add card to Hard group.
        /// </summary>
        private ICommand _addHardLesson;

        public ICommand AddHardLesson
        {
            get { return _addHardLesson ?? (_addHardLesson = new RelayCommand(AddHardLessonCommandHandler)); }
            set { _addHardLesson = value; }
        }

        /// <summary>
        ///     Repeat lesson command.
        /// </summary>
        private ICommand _repeatCommand;

        public ICommand RepeatCommand
        {
            get { return _repeatCommand ?? (_repeatCommand = new RelayCommand(RepeatCommandHandler)); }
            set { _repeatCommand = value; }
        }


        private void RepeatCommandHandler(object obj)
        {
            Repeat();
        }

        /// <summary>
        ///     Add card to Hard group handler.
        /// </summary>
        private void AddHardLessonCommandHandler(object obj)
        {
            var presentedCards = _repository.LoadLesson(Constants.DefaultLessonName);
            if (_displayedCard != null
                && !presentedCards.Any(
                    x =>
                        x.ForeignTask.Equals(_displayedCard.ForeignTask) &&
                        x.NativeTask.Equals(_displayedCard.NativeTask)))
                _repository.SaveToLessonNewCard(Constants.DefaultLessonName, _displayedCard);
        }

        /// <summary>
        ///     Rename lesson handler.
        /// </summary>
        private void RenameLessonCommandHandler(object obj)
        {
            IsEditableTitle = !IsEditableTitle;

            Menu.Menu.ElementAt(2).IsEditableTitle = IsEditableTitle;
        }

        /// <summary>
        ///     New lesson command.
        /// </summary>
        private ICommand _newLessonCommand;

        public ICommand NewLessonCommand
        {
            get { return _newLessonCommand ?? (_newLessonCommand = new RelayCommand(NewLessonCommandHandler)); }
            set { _newLessonCommand = value; }
        }

        private void NewLessonCommandHandler(object obj)
        {
            IsEnableToEnterFileName = true;
        }

        private ICommand _createNewLessonFileCommand;

        public ICommand CreateNewLessonFileCommand
        {
            get
            {
                return _createNewLessonFileCommand ??
                       (_createNewLessonFileCommand = new RelayCommand(CreateNewLessonFileCommandHandler));
            }
            set
            {
                _createNewLessonFileCommand = value;
                NotifyPropertyChanged("CreateNewLessonFileCommand");
            }
        }

        private void CreateNewLessonFileCommandHandler(object obj)
        {
            IsEnableToEnterFileName = false;

            _repository.CreateNewLesson(NewLessonName);
            _model.LessonsNames = _repository.LoadLessonsName();
            NotifyPropertyChanged("Menu");
        }

        /// <summary>
        ///     Enter event command.
        /// </summary>
        public ICommand EnterCommand
        {
            get { return _enterCommand ?? (_enterCommand = new RelayCommand(EnterCommandHandler)); }

            set { _enterCommand = value; }
        }

        /// <summary>
        ///     Enter event command handler.
        /// </summary>
        /// <param name="obj"></param>
        private void EnterCommandHandler(object obj)
        {
            switch (_enterState)
            {
                case EnterState.First:
                    _enterState = EnterState.Second;
                    if (_cardsCount - _correctAnswerCnt - _incorrectAnswerCnt != 0)
                        SpellChecking();
                    else
                        Repeat();
                    break;

                case EnterState.Second:
                    if (_cardsCount - _correctAnswerCnt - _incorrectAnswerCnt == 0)
                        IsRepeat = true;
                    else
                    {
                        _displayedCard = _cardsService.GetCardReduceList(_model.Cards);
                        UserText = string.Empty;
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
        ///     Open selected lesson from repo.
        /// </summary>
        private void OpenSelectedLesson(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                _model.Cards = _repository.LoadLesson(name);
                CardsCount = _model.Cards.Count;

                _displayedCard = _cardsService.GetCardReduceList(_model.Cards);

                _enterState = EnterState.First;
                RefreshCard();
            }
        }


        private void RefreshCard()
        {
            NotifyPropertyChanged("Comment");
            NotifyPropertyChanged("ForeignTask");
            NotifyPropertyChanged("NativeTask");
            NotifyPropertyChanged("UserText");
        }


        private void SpellChecking()
        {
            if (string.IsNullOrEmpty(UserText))
                UserText = string.Empty;

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

        private void ClearCard()
        {
            ForeignTask = string.Empty;
            NativeTask = string.Empty;
            Comment = string.Empty;
            UserText = string.Empty;
            _enterState = EnterState.First;
            Forecolor = Brushes.Black;

            CorrectAnswerCnt = 0;
            IncorrectAnswerCnt = 0;
        }

        private void LoadTheme()
        {
            var theme = Settings.Default.Theme;
            switch (theme)
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
        ///     Repeat selected lesson.
        /// </summary>
        private void Repeat()
        {
            if (!string.IsNullOrEmpty(OpenedLesson))
            {
                ClearCard();
                OpenSelectedLesson(OpenedLesson);
                IsRepeat = false;
            }
        }


        /// <summary>
        ///     Show information window with cards list.
        /// </summary>
        private void OpenInfoWindow(object parameter)
        {
            _model.Cards = _repository.LoadLesson(_model.OpenedLessonName);

            _infoViewModel.SetLessonContext(_model);
            var win = new LessonInfo(_infoViewModel);
            win.Show();
        }

        #endregion
    }
}