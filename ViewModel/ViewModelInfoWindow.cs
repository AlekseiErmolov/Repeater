using Repeater.Classes;
using Repeater.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System;
using Repeater.Model;
using Repeater.Classes.TranslateFacade;
using Microsoft.Practices.Unity;

namespace Repeater.ViewModel
{
    public class ViewModelInfoWindow : ViewModelBase
    {
        IRepository _repository;
        ITranslate _translateFacade;
        string TranslateKey { get; set; }

        #region Properties
        public ILesson Lesson
        {
            get;
            set;
        }

        private ICard _selectedCard;
        public ICard SelectedCard
        {
            get
            {
                return _selectedCard;
            }

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

            set
            {
                Lesson.Cards = value.ToList();
            }
        }
        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model"></param>
        public ViewModelInfoWindow(ILesson model, IUnityContainer container)
        {
            Lesson = model;
            _repository = container.Resolve<IRepository>();
            _translateFacade = container.Resolve<ITranslate>();
        }

        #region Commands

        /// <summary>
        /// Add new card to collection
        /// </summary>
        private ICommand _newCard;
        public ICommand NewCard
        {
            get { return _newCard ?? (_newCard = new RelayCommand(NewCardHandler)); }
            set
            {
                _newCard = value;
            }
        }

        /// <summary>
        /// New card handler
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
        /// Remove selected card from collection
        /// </summary>
        private ICommand _removeCard;
        public ICommand RemoveCard
        {
            get { return _removeCard ?? (_removeCard = new RelayCommand(RemoveCardHandler)); }
            set
            {
                _removeCard = value;
            }
        }


        /// <summary>
        /// Remove card handler
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
        /// Save cards to repo
        /// </summary>
        private ICommand _autoTranslate;
        public ICommand AutoTranslate
        {
            get { return _autoTranslate ?? (_autoTranslate = new RelayCommand(AutoTranslateHandler)); }
            set
            {
                _autoTranslate = value;
            }
        }

        /// <summary>
        /// Save cards to repo
        /// </summary>
        private ICommand _saveCards;
        public ICommand SaveCards
        {
            get { return _saveCards ?? (_saveCards = new RelayCommand(SaveCardHandler)); }
            set
            {
                _saveCards = value;
            }
        }

        /// <summary>
        /// Save cards handler
        /// </summary>
        /// <param name="obj"></param>
        private void SaveCardHandler(object obj)
        {
            if (Lesson.Cards != null)
            {
                _repository.ResaveLesson(Lesson.OpenedLessonName, Lesson.Cards);
            }
        }

        /// <summary>
        /// Save cards handler
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
        /// Translate completion event - get the result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
