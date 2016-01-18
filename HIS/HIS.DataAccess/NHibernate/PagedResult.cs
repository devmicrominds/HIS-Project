using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS.DataAccess
{
    /// <summary>
    /// A paged result set, will have the items in the page of data
    /// and a total item count for the total number of results.
    /// </summary>
    public class PagedResult<TEntity>
    {

        #region Properties

        /// <summary>
        /// The items for the current page.
        /// </summary>
        public IList<TEntity> Items { get; protected set; }

        /// <summary>
        /// Gets the total count of items.
        /// </summary>
        public int ItemCount { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initialise an instance of the paged result, 
        /// intiailise the internal collection.
        /// </summary>
        public PagedResult()
        {
            this.Items = new List<TEntity>();
        }

        /// <summary>
        /// Initialise our page result, set the items and the current page + total count
        /// </summary>
        /// <param name="items"></param>
        /// <param name="itemCount"></param>
        public PagedResult(IList<TEntity> items, int itemCount)
        {
            Items = items;
            ItemCount = itemCount;
        }

        #endregion

    }
}
