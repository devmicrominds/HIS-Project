using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class DocumentExtensionLink : BaseDomain
    {
        private readonly DocumentExtension documentExtension;
        private readonly string documentPathLocator;
        private string pathToPhysicalDocument;

        protected DocumentExtensionLink() { }

        public DocumentExtensionLink(DocumentExtension documentExtension, string documentPathLocator)
        {
            if (null == documentExtension)
                throw new ArgumentNullException("documentExtension is null!");
            this.documentExtension = documentExtension;
            this.documentPathLocator = documentPathLocator;
        }

        public virtual string PathToPhysicalDocument
        {
            get { return pathToPhysicalDocument; }
            set { pathToPhysicalDocument = value; }
        }

        public virtual DocumentExtension DocumentExtension
        {
            get { return documentExtension; }
        }

        public virtual string DocumentPathLocator
        {     
            get { return documentPathLocator; }
        }
    }
}
