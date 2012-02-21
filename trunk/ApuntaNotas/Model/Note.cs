using System;
using System.Globalization;
using System.Threading;

namespace ApuntaNotas.Model
{
    /// <summary>
    /// This class is the note itself. It represent a note.
    /// We need it to be serializable.
    /// </summary>
    [Serializable]
    public class Note : Notifier
    {
        private string _content;
        private string _date;
        private Category _category;

        /// <summary>
        /// Initializes a new instance of the <see cref="Note"/> class.
        /// We need this constructor because we are creating empty
        /// notes in the VM.
        /// </summary>
        public Note() : this(string.Empty, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Note"/> class.
        /// </summary>
        /// <param name="content">The note content.</param>
        /// <param name="category">The note category.</param>
        public Note(string content, Category category)
        {
            _content = content;
            _category = category;

            // We need to take the actual date
            var dt = DateTime.Now;
            IFormatProvider ci = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
            _date = dt.ToString("d", ci);
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                RaisePropertyChanged("Content");
            }
        }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public string Date
        {
            get { return _date; }
            set
            {
                _date = value;
                RaisePropertyChanged("Date");
            }
        }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public Category Category
        {
            get { return _category; }
            set
            {
                _category = value;
                RaisePropertyChanged("Category");
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
                return false;

            var other = obj as Note;
            // We need to know if the other note has the same content and category.
            return other != null && other.Content == Content && other.Category.Id == Category.Id;
        }

        public override int GetHashCode()
        {
            return Content.GetHashCode();
        }
    }
}
