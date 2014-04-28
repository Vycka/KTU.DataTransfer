using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adform.Academy.DataTransfer.WebApi.Contracts.Databases;

namespace Adform.Academy.DataTransfer.Web.Models
{
    public class DatabaseListModel
    {
        public List<DatabaseItem> DatabasesList { get; set; }
    }
}