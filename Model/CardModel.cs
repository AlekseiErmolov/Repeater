using Repeater.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repeater.Model
{
    class Card : ICard
    {
        private string _comment;
        private string _foreignTask;
        private string _nativeTask;
        private string _userAnswer;


        public string Comment
        {
            get
            {
                return _comment;
            }

            set
            {
                _comment = value;
            }
        }

        public string ForeignTask
        {
            get
            {
                return _foreignTask;
            }

            set
            {
                _foreignTask = value;
            }
        }

        public string NativeTask
        {
            get
            {
                return _nativeTask;
            }

            set
            {
                _nativeTask = value;
            }
        }

        public string UserAnswer
        {
            get
            {
                return _userAnswer;
            }

            set
            {
                _userAnswer = value;
            }
        }
    }
}
