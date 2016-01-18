using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace HIS.DataAccess {

	public static class NHibernateConfigurationExtensions {
		public static void AddListener<TListener> (
			this Configuration configuration,
			Expression<Func<EventListeners, TListener []>> expression,
			TListener listenerImpl) {

		
			var propertyInfo = NHibernate.Linq.ReflectionHelper.GetProperty (expression) as PropertyInfo;
			var existentListeners = (TListener []) propertyInfo.GetValue (configuration.EventListeners, null);
			var newListeners = new List<TListener> (existentListeners) { listenerImpl }.ToArray ();
			propertyInfo.SetValue (configuration.EventListeners, newListeners, null);
		}

		public static void AddListeners<TListener> (
			this Configuration configuration,
			Expression<Func<EventListeners, TListener []>> expression,
			IEnumerable<TListener> listeners) {
			foreach ( var listener in listeners ) {
				configuration.AddListener (expression, listener);
			}
		}

        private static readonly PropertyInfo TableMappingsProperty =
               typeof(Configuration).GetProperty("TableMappings", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        public static void CreateIndexesForForeignKeys(this Configuration configuration)
        {
            configuration.BuildMappings();
            var tables = (ICollection<Table>)TableMappingsProperty.GetValue(configuration, null);
            foreach (var table in tables)
            {
                foreach (var foreignKey in table.ForeignKeyIterator)
                {
                    var idx = new Index();
                    idx.AddColumns(foreignKey.ColumnIterator);
                    idx.Name = "IDX" + foreignKey.Name.Substring(2);
                    idx.Table = table;
                    table.AddIndex(idx);
                }
            }
        }

        
	}    
}
