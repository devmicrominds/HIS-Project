using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class DocumentExtension  : BaseDomain
    {
        private readonly ICollection<DocumentExtensionLink> documentLinks;

        public DocumentExtension()   {   

            this.documentLinks = new HashSet<DocumentExtensionLink>();
        }

        public virtual IEnumerable<DocumentExtensionLink> DocumentLinks
        {
            get { return documentLinks; }
        }

        public virtual string GetName() {

            return String.Empty;
        }


        public virtual DocumentExtensionLink AddLink(IPathLocatorResolver pathLocator, string pathToPhysicalDocument) {

            var link = new DocumentExtensionLink(this, pathLocator.GetFromPath(pathToPhysicalDocument));

            this.documentLinks.Add(link);

            return link;
        }


    }
}
