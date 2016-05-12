using Repeater.Classes;
using Repeater.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System;
using Repeater.Model;

namespace Repeater.ViewModel
{
    public class ViewModelInfoWindow : ViewModelBase
    {
        IRepository _repository;


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
                var retval = new ObservableCollection<ICard>(Lesson.Cards);
                return retval;
            }

            set
            {
                Lesson.Cards = value.ToList();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model"></param>
        public ViewModelInfoWindow(ILesson model, IRepository repository)
        {
            Lesson = model;
            _repository = repository;
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
            Lesson.Cards.Add(new Card());
            NotifyPropertyChanged("Cards");
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
            _repository.ResaveLesson(Lesson.OpenedLessonName, Lesson.Cards);
        }

        #endregion
    }
}
