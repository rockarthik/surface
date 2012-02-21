using System;

namespace ApuntaNotas.Model
{
    /// <summary>
    /// This class represents a note's category.
    /// This class have to be serializable.
    /// </summary>
    [Serializable]
    public class Category : Notifier
    {
        private Guid _id;
        private string _name;
        private string _backgroundColor;
        private string _fontColor;
        private bool _isDefault;

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        /// <param name="name">The category name.</param>
        /// <param name="backgroundColor">Color of the category's background.</param>
        /// <param name="fontColor">Color of the category's font.</param>
        public Category (string name, string backgroundColor, string fontColor)
        {
            _id = Guid.NewGuid();
            _name = name;
            _backgroundColor = backgroundColor;
            _fontColor = fontColor;
            _isDefault = false;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public Guid Id
        {
            get { return _id; }
            private set
            {
                _id = value;
                RaisePropertyChanged("Id");
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>The color of the background.</value>
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                RaisePropertyChanged("BackgroundColor");
            }
        }

        /// <summary>
        /// Gets or sets the color of the font.
        /// </summary>
        /// <value>The color of the font.</value>
        public string FontColor
        {
            get { return _fontColor; }
            set
            {
                _fontColor = value;
                RaisePropertyChanged("FontColor");
            }
        }

        public bool IsDefault
        {
            get { return _isDefault; }
            set
            {
                _isDefault = value;
                RaisePropertyChanged("IsDefault");
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

            var other = obj as Category;
            return other != null && other.Id == Id && other.Name == Name;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
