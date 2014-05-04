using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Adform.Academy.DataTransfer.WebApi.Contracts.Databases;
using Adform.Academy.DataTransfer.WebApi.Contracts.Projects;
using Newtonsoft.Json;

namespace Adform.Academy.DataTransfer.Web.Models
{
    public class ProjectEditorModel
    {
        public ProjectEditorModel()
        {
            Init();
        }

        public ProjectEditorModel(GetProjectResponse response)
        {
            Init();

            ProjectName = response.ProjectName;
            ProjectId = response.ProjectId;
            SourceDatabaseId = response.SourceDatabaseId;
            DestinationDatabaseId = response.DestinationDatabaseId;
            Filters = response.Filters;
        }

        private void Init()
        {
            Filters = new List<FilterItem>();
            Tables = new List<TableInformation>();
            Databases = Enumerable.Empty<SelectListItem>();
        }

        public int ProjectId { get; set; }


        [Required(ErrorMessage = "Project Name is Required")]
        [DataType(DataType.Text)]
        [DisplayName("Project Name")]
        public string ProjectName { get; set; }


        [Required(ErrorMessage = "Source Database is Required")]
        [DisplayName("Source Database")]
        public int SourceDatabaseId { get; set; }


        [Required(ErrorMessage = "Target Database is Required")]
        [DisplayName("Target Database")]
        public int DestinationDatabaseId { get; set; }


        public List<FilterItem> Filters { get; set; }
        public string FiltersJson 
        {
            get { return JsonConvert.SerializeObject(Filters); }
            set { Filters = JsonConvert.DeserializeObject<List<FilterItem>>(value); } 
        }

        public string SelectedFieldsRawJson
        {
            get { return ""; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    var fields = JsonConvert.DeserializeObject<SelectedFieldsStruct[]>(value);
                    foreach (var filter in Filters)
                    {
                        filter.Columns.Clear();
                    }
                    foreach (var field in fields)
                    {
                        var filter = Filters.FirstOrDefault(f => f.TableName == field.TableName);
                        if (filter == null)
                        {
                            filter = new FilterItem
                            {
                                TableName = field.TableName,
                                FilterValue = new FilterValueItem(),
                                Columns = new List<ColumnItem>(),
                            };
                            Filters.Add(filter);
                        }
                        filter.Columns.Add(new ColumnItem
                        {
                            ColumnName = field.ColumnName,
                            ColumnType = field.ColumnType
                        });
                    }
                    for (int x = 0; x < Filters.Count; x++)
                    {
                        if (Filters[x].Columns.Count == 0)
                        {
                            Filters.RemoveAt(x);
                        }
                    }
                }
            }
        }

        public string SourceDatabaseName { get; set; }
        public string DestinationDatabaseName { get; set; }
        public IEnumerable<SelectListItem> Databases { get; set; }
        public List<TableInformation> Tables { get; set; }

    }

    public struct SelectedFieldsStruct
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
    }
}