using System;
using System.Collections.Generic;
using System.Text;

namespace Email
{
    public class PentahoUrlBuilder
    {
        public static string ROOT_PATH = "http://kes.it-serv.ru/pentaho/api/repos/%3Apublic%3Avote%3A2021%3A08%3A";

        private string Format { get; set; }

        private string UserId { get; set; }
        private string Password { get; set; }
        private string AppendParams { get; set; }
        public string Extension { get; private set; }
        private string Name { get; set; }
        public string Description { get; private set; }

        public PentahoUrlBuilder(string name, string description, string appendParams = "", ReturnFormat returnFormat = ReturnFormat.Excel)
        {
            Description = description;
            Name = name;
            AppendParams = appendParams;
            UserId = "Admin";
            Password = "qwe-123";

            switch (returnFormat)
            {
                case ReturnFormat.Excel:
                    Format = "table/excel;page-mode=flow";
                    Extension = ".xls";
                    break;

                default:
                case ReturnFormat.PDF:
                    Format = "pageable/pdf";
                    Extension = ".pdf";
                    break;
            }
        }

        public string Url {
            get
            {
                string url = ROOT_PATH + Name + ".prpt/generatedContent";
                if (AppendParams.Length > 0)
                {
                    url += "?" + AppendParams;
                }

                url += (AppendParams.Length > 0 ? "&" : "?") + "userid=" + UserId + "&password=" + Password + "&output-target=" + Format;
                return url;
            }
        }

        public enum ReturnFormat
        {
            Excel,
            PDF
        }
    }
}
