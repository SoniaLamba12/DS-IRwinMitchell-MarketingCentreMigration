using ConsultCrm.DataFramework.Plugins;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarketingCentreMigration
{
    public class ImportDataTransformations: UploadProcessPlugin
    {
        public override IEnumerable<IDictionary<string, object>> DataStreamPostExtractDatabase(IEnumerable<IDictionary<string, object>> data)
        {
            foreach (var row in data)
            {
                //row["action"] = "insert";
                string website = ExtractWebSiteUrl(row["websiteurl"] as string);
                string[] address = ExtractAddress(row["addresslines"] as string);
                int line = 1;
                for(int i=0; i<=address.Length-1; i++)
                {
                    string addr = String.Empty;
                    row["address1_line" + line] = address[i] as string;
                    if (line == 3)
                    {
                        addr += address[i];
                        row["address1_line" + line] = addr;
                    }
                    else
                    {
                        line += 1;

                    }
                }
                row["websiteurl"] = website;
                yield return row;
            } 
        }

        public string ExtractWebSiteUrl(string text)
        {
            string website = String.Empty;
            Regex regex = new Regex(@"[\w-]+(\.[\w-]+)+[\w.,@?^=%;:\/~+#-]*[\w@?^=;\/~+#-]");
            if (!String.IsNullOrEmpty(text))
            {
                Match match = regex.Match(text);
                if (match.Success)
                    website = match.Value;
                return website;
            }
            else
                return String.Empty;
        }

        public static string[] ExtractAddress(string text)
        {
            string[] lines;
            if (!String.IsNullOrEmpty(text))
            {
                lines =
                 text.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
                return lines;
            }
            else
                return Array.Empty<string>();
        }

    }
}
