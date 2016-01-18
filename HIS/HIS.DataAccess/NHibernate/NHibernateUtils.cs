using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Type;
using NHibernate.Metadata;
using System.Reflection;
using System.Configuration;
using System.IO;

namespace HIS.DataAccess
{
	public class NHibernateUtils
	{
		public static IType GetPropertyType(
			ISessionFactory sessionFactory,
			Type classType,
			string propertyName)
		{
			string[] properties = propertyName.Split(new char[] { '.' });
			IClassMetadata currentMetadata = sessionFactory.GetClassMetadata(classType);
			IType currentType = null;
			foreach (string s in properties)
			{
				// get metadata from type, if necessary
				if (currentMetadata == null)
				{
					currentMetadata = sessionFactory.GetClassMetadata(currentType.ReturnedClass);
				}
				// get type from metadata
				if ("id".Equals(s))
				{
					currentType = currentMetadata.IdentifierType;
				}
				else
				{
					currentType = currentMetadata.GetPropertyType(s);
				}

				currentMetadata = null;
			}
			return currentType;
		}

		public static IType GetType(TypeCode typeCode)
		{
			switch (typeCode)
			{
				case TypeCode.Boolean:
					return NHibernateUtil.Boolean;
				case TypeCode.Byte:
					return NHibernateUtil.Byte;
				case TypeCode.Char:
					return NHibernateUtil.Character;
				case TypeCode.DateTime:
					return NHibernateUtil.DateTime;
				case TypeCode.Decimal:
					return NHibernateUtil.Decimal;
				case TypeCode.Double:
					return NHibernateUtil.Double;
				case TypeCode.Int16:
					return NHibernateUtil.Int16;
				case TypeCode.Int32:
					return NHibernateUtil.Int32;
				case TypeCode.Int64:
					return NHibernateUtil.Int64;
				case TypeCode.SByte:
					return NHibernateUtil.SByte;
				case TypeCode.Single:
					return NHibernateUtil.Single;
				case TypeCode.String:
					return NHibernateUtil.String;
				case TypeCode.UInt16:
					return NHibernateUtil.UInt16;
				case TypeCode.UInt32:
					return NHibernateUtil.UInt32;
				case TypeCode.UInt64:
					return NHibernateUtil.UInt64;
				default:
					return null;
			}
		}

		public static void CreateDatabase(NHibernate.Cfg.Configuration configuration)
		{
			NHibernate.Tool.hbm2ddl.SchemaExport se =
				new NHibernate.Tool.hbm2ddl.SchemaExport(configuration);
			se.Create(true, true);
		}

		public static void DoInTransaction(ISession session, System.Action<ISession> action)
		{
			if (session.Transaction != null && session.Transaction.IsActive)
			{
				action(session);
			}
			else
			{
				ITransaction transaction = session.BeginTransaction();
				try
				{
					action(session);
					transaction.Commit();
				}
				catch (Exception)
				{
					transaction.Rollback();
					throw;
				}
			}
		}
	}
}
