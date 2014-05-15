using System.Collections;
using System.Collections.Generic;
using System.Data;
using Adform.Academy.DataTransfer.Core.DataTransfer;
using Adform.Academy.DataTransfer.Logger;

namespace Adform.Academy.DataTransfer.WebApi.Controllers
{
    public class ControllerBase : System.Web.Http.ApiController
    {
        public static ILogger Logger;
        public static DataTransferServiceRunner ServiceRunner;

        protected static DataTable CreateIdsTable(IEnumerable<int> idList)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof (int));

            foreach (var id in idList)
            {
                var row = table.NewRow();
                row[0] = id;
                table.Rows.Add(row);
            }

            return table;
        }
    }
}
