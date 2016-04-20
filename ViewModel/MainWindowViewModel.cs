﻿using MetroListBox;
using Repeater.Classes;
using Repeater.Classes.Helpers;
using Repeater.Interfaces;
using Repeater.Model;
using Repeater.Services;
using Repeater.View;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace Repeater.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        #region Variables
        private readonly ILesson _model;
        private readonly IRepository _loader;
        private readonly IRandomize _cardsService;

        private ICommand _enterCommand;
        private EnterState _enterState;
        private ICard _displayedCard;
        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор ViewModel главного окна
        /// </summary>
        /// <param name="model"></param>
        /// <param name="repository"></param>
        public MainWindowViewModel(ILesson model, IRepository repository)
        {
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
            NextExampleCommand = new Classes.Helpers.RelayCommand(OpenInfoWindow);
        }

        /// <summary>
        /// Событие выбора урока
        /// </summary>
        /// <param name="name"></param>
        private void _menu_MenuSelectEvent(string name)
        {
            ClearCard();
            
            OpenSelectedLesson(name);
            OpenedLesson = name;
            IsRepeat = false;
        }

        #endregion

        #region Properties

        public Classes.Helpers.RelayCommand NextExampleCommand { get; set; }

        private string _newLessonName;
        public string NewLessonName
        {
            get
            {
                return _newLessonName;
            }

            set
            {
                _newLessonName = value;
                NotifyPropertyChanged("NewLessonName");
            }
        }

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
                Properties.Settings.Default.Invert = value;
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
        private string _OpenedLesson;
        public string OpenedLesson
        {
            get
            {
                return _OpenedLesson;
            }

            set
            {
                _OpenedLesson = value;
                NotifyPropertyChanged("OpenedLesson");
            }
        }

        #endregion

        #region ThemesCommands

        /// <summary>
        /// 
        /// </summary>
        private ICommand _CyanThemeCommand;
        public ICommand CyanThemeCommand
        {
            get { return _CyanThemeCommand ?? (_CyanThemeCommand = new RelayCommand<object>(CyanThemeCommandHandler)); }
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
                return _GreenThemeCommand ?? (_GreenThemeCommand = new RelayCommand<object>(GreenThemeCommandHandler));
            }
            private set
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
            get { return _BlueThemeCommand ?? (_BlueThemeCommand = new RelayCommand<object>(BlueThemeCommandHandler)); }
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

        #endregion


        #region Commands

        /// <summary>
        /// 
        /// </summary>
        private ICommand _RepeatCommand;
        public ICommand RepeatCommand
        {
            get { return _RepeatCommand ?? (_RepeatCommand = new RelayCommand<object>(RepeatCommandHandler)); }
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
        private ICommand _SaveCardCommand;
        public ICommand SaveCardCommand
        {
            get { return _SaveCardCommand ?? (_SaveCardCommand = new RelayCommand<object>(SaveCardCommandHandler)); }
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
            _loader.SaveToLessonNewCard(OpenedLesson, _displayedCard);
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
                return _SaveCardAndNewCommand ??
                       (_SaveCardAndNewCommand = new RelayCommand<object>(SaveCardAndNewCommandHandler));
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
            _loader.SaveToLessonNewCard(OpenedLesson, _displayedCard);
            IsEnableMakeCard = true;
            ClearCard();
        }


        /// <summary>
        /// Команда изменить карточку
        /// </summary>
        private ICommand _EditCardCommand;
        public ICommand EditCardCommand
        {
            get { return _EditCardCommand ?? (_EditCardCommand = new RelayCommand<object>(EditCardCommandHandler)); }
            set
            {
                _EditCardCommand = value;
            }
        }

        /// <summary>
        /// Хендлер для операции изменения карточки
        /// </summary>
        /// <param name="obj"></param>
        private void EditCardCommandHandler(object obj)
        {
            _loader.DeleteCard(OpenedLesson, _displayedCard);
            IsEnableMakeCard = true;
        }


        /// <summary>
        /// 
        /// </summary>
        private ICommand _AddCardCommand;
        public ICommand AddCardCommand
        {
            get { return _AddCardCommand ?? (_AddCardCommand = new RelayCommand<object>(AddCardCommandHandler)); }
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
            get { return _NewLessonCommand ?? (_NewLessonCommand = new RelayCommand<object>(NewLessonCommandHandler)); }
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
                return _CreateNewLessonFileCommand ??
                       (_CreateNewLessonFileCommand = new RelayCommand<object>(CreateNewLessonFileCommandHandler));
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

            _loader.CreateNewLesson(NewLessonName);
            _model.LessonsNames = _loader.LoadLessonsName();
            NotifyPropertyChanged("Menu");
        }

        /// <summary>
        /// Команда нажатия Enter на поле ответов
        /// </summary>
        public ICommand EnterCommand
        {
            get { return _enterCommand ?? (_enterCommand = new RelayCommand<object>(EnterCommandHandler)); }

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
                CardsCount = _model.Cards.Count;

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
            if (!String.IsNullOrEmpty(OpenedLesson))
            {
                ClearCard();
                OpenSelectedLesson(OpenedLesson);
                IsRepeat = false;
            }
        }


        void OpenInfoWindow(object parameter)
        {
            var win = new LessonInfo(new ViewModelInfoWindow(_model));
            win.ShowDialog();
        }

        #endregion
    }
}
