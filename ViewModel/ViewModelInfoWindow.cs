using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Practices.Unity;
using Repeater.Classes;
using Repeater.Classes.TranslateFacade;
using Repeater.Interfaces;
using Repeater.Model;

namespace Repeater.ViewModel
{
    public class ViewModelInfoWindow : ViewModelBase
    {
        private readonly DispatcherTimer _dispatcherTimer;
        private readonly IRepository _repository;
        private readonly ITranslate _translateFacade;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="model"></param>
        /// <param name="container"></param>
        public ViewModelInfoWindow(ILesson model, IUnityContainer container)
        {
            Lesson = model;
            _repository = container.Resolve<IRepository>();
            _translateFacade = container.Resolve<ITranslate>();

            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += dispatcherTimer_Tick;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
        }

        private string TranslateKey { get; set; }

        #region Properties

        public ILesson Lesson { get; set; }

        private ICard _selectedCard;

        public ICard SelectedCard
        {
            get { return _selectedCard; }

            set
            {
                _selectedCard = value;
                NotifyPropertyChanged("SelectedCard");
            }
        }


        public ObservableCollection<ICard> Cards
        {
            get
            {
                if (Lesson.Cards != null)
                {
                    var retval = new ObservableCollection<ICard>(Lesson.Cards);
                    return retval;
                }
                return null;
            }

            set { Lesson.Cards = value.ToList(); }
        }

        private bool ClipboardModeEnabled { get; set; }

        /// <summary>
        ///     Clipboard Mode button text
        /// </summary>
        public string ClipboardMode
        {
            get { return ClipboardModeEnabled ? "Clipboard On" : "Clipboard Off"; }
        }

        #endregion

        #region Commands

        /// <summary>
        ///     Add new card to collection
        /// </summary>
        private ICommand _newCard;

        public ICommand NewCard
        {
            get { return _newCard ?? (_newCard = new RelayCommand(NewCardHandler)); }
            set { _newCard = value; }
        }

        /// <summary>
        ///     New card handler
        /// </summary>
        /// <param name="obj"></param>
        private void NewCardHandler(object obj)
        {
            if (Lesson.Cards != null)
            {
                Lesson.Cards.Add(new Card());
                NotifyPropertyChanged("Cards");
            }
        }


        /// <summary>
        ///     Remove selected card from collection
        /// </summary>
        private ICommand _removeCard;

        public ICommand RemoveCard
        {
            get { return _removeCard ?? (_removeCard = new RelayCommand(RemoveCardHandler)); }
            set { _removeCard = value; }
        }


        /// <summary>
        ///     Remove card handler
        /// </summary>
        /// <param name="obj"></param>
        private void RemoveCardHandler(object obj)
        {
            if (SelectedCard != null)
            {
                Lesson.Cards.Remove(SelectedCard);
                NotifyPropertyChanged("Cards");
            }
        }

        /// <summary>
        ///     Save cards to repo
        /// </summary>
        private ICommand _autoTranslate;

        public ICommand AutoTranslate
        {
            get { return _autoTranslate ?? (_autoTranslate = new RelayCommand(AutoTranslateHandler)); }
            set { _autoTranslate = value; }
        }

        /// <summary>
        ///     Clipboard mode Command
        /// </summary>
        private ICommand _clipboardMode;

        public ICommand ClipboardCommand
        {
            get { return _clipboardMode ?? (_clipboardMode = new RelayCommand(ClipboardCommandHandler)); }
            set { _clipboardMode = value; }
        }

        /// <summary>
        ///     Save cards to repo
        /// </summary>
        private ICommand _saveCards;

        public ICommand SaveCards
        {
            get { return _saveCards ?? (_saveCards = new RelayCommand(SaveCardHandler)); }
            set { _saveCards = value; }
        }

        /// <summary>
        ///     Save cards handler
        /// </summary>
        /// <param name="obj"></param>
        private void SaveCardHandler(object obj)
        {
            if (Lesson.Cards != null)
                _repository.ResaveLesson(Lesson.OpenedLessonName, Lesson.Cards);
        }

        /// <summary>
        ///     Save cards handler
        /// </summary>
        /// <param name="obj"></param>
        private void AutoTranslateHandler(object obj)
        {
            if (Lesson.Cards != null)
            {
                _translateFacade.GetTranslateResultEvent += _translateFacade_GetTranslateResultEvent;
                _translateFacade.Translate(TranslateKey, Lesson.Cards);
            }
        }

        /// <summary>
        ///     Autofill cards from clipboard
        /// </summary>
        /// <param name="obj"></param>
        private void ClipboardCommandHandler(object obj)
        {
            ClipboardModeEnabled = !ClipboardModeEnabled;
            NotifyPropertyChanged("ClipboardMode");

            if (ClipboardModeEnabled)
                _dispatcherTimer.Start();
            else
                _dispatcherTimer.Stop();
        }

        /// <summary>
        ///     DispatcherTimer event
        /// </summary>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                var clipboardText = Clipboard.GetText(TextDataFormat.Text);
                if (!string.IsNullOrEmpty(clipboardText))
                {
                    if (!Lesson.Cards.Any(x => x.ForeignTask.Equals(clipboardText, StringComparison.OrdinalIgnoreCase)))
                    {
                        Clipboard.SetText(string.Empty);

                        Lesson.Cards.Add(new Card
                        {
                            ForeignTask = clipboardText
                        });
                        NotifyPropertyChanged("Cards");
                    }
                }
            }
        }

        /// <summary>
        ///     Translate completion event - get the result
        /// </summary>
        private void _translateFacade_GetTranslateResultEvent(object sender, EventArgs e)
        {
            var ev = e as TranslateEventArgs;
            if (ev != null)
            {
                TranslateKey = ev.Key;
                Lesson.Cards = ev.Cards;
                NotifyPropertyChanged("Cards");
            }
        }

        #endregion
    }
}