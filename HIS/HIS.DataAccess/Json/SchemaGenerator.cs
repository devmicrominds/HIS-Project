using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.DataAccess 
{
    public class SchemaGenerator<T> where T : class
    {
        private Type dataType;
        private string schemaTitle;
        private JsonSchema dataSchema;
        
        public SchemaGenerator(string schemaTitle) {
            
            this.dataType = typeof(T);
            this.schemaTitle = schemaTitle;
        
        }

        protected JsonSchema Schema {

            get
            {   if(null == dataSchema)
                {
                    var schemaGenerator = new JsonSchemaGenerator();
                    schemaGenerator.UndefinedSchemaIdHandling = UndefinedSchemaIdHandling.UseTypeName;
                    dataSchema = schemaGenerator.Generate(dataType);
                    dataSchema.Title = schemaTitle;
                }

                return dataSchema;
            }
        
        }

        public string Generate()  {

            var writer =  new StringWriter();
            var jsonTextWriter = new JsonTextWriter(writer);
            Schema.WriteTo(jsonTextWriter);
            
            dynamic parsedJson = JsonConvert.DeserializeObject(writer.ToString());

            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }
    }
}
