using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Messaging;

namespace ApuntaNotas.Messages
{

    /// <summary>
    /// This class represents a Message.
    /// This message is carrying information about the categories that changes
    /// </summary>
    public class CategoryEditorChangesMessage
    {
        private List<Guid> _notesToDelete;
        /// <summary>
        /// Here we put the notes that will be deleted (because the category died)
        /// </summary>
        /// <value>
        /// The notes to delete.
        /// </value>
        public List<Guid> NotesToDelete
        {
            get
            {
                if (_notesToDelete == null)
                    return new List<Guid>();
                return _notesToDelete;
            }
            set { _notesToDelete = value; }
        }

        private List<Guid> _notesToTrash;
        /// <summary>
        /// Here we put the notes that will be moved to the trash (because the category died)
        /// </summary>
        /// <value>
        /// The notes to trash.
        /// </value>
        public List<Guid> NotesToTrash
        {
            get
            {
                if (_notesToTrash == null)
                    return new List<Guid>();
                return _notesToTrash;
            }
            set { _notesToTrash = value; }
        }

        private List<Guid> _categoriesIds;
        /// <summary>
        /// Here we put the categories that changes information (name, color...)
        /// </summary>
        /// <value>
        /// The categories ids.
        /// </value>
        public List<Guid> CategoriesIds
        {
            get
            {
                if (_categoriesIds == null)
                    return new List<Guid>();
                return _categoriesIds;
            }
            set { _categoriesIds = value; }
        }
    }
}
