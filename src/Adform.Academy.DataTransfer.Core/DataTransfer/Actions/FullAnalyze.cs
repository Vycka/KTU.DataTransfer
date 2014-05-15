using System;
using System.Data.SqlClient;
using System.Globalization;
using Adform.Academy.DataTransfer.Core.DataTransfer.ValueParsers;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Adform.Academy.DataTransfer.Core.DTO.Types;
using Microsoft.SqlServer.Management.Common;

namespace Adform.Academy.DataTransfer.Core.DataTransfer.Actions
{
    class FullAnalyze : ActionBase, IAction
    {
        public void ExecuteAction(ExecutingProjectData data)
        {
            SetStep(data, ExecutionStepsTypes.FullAnalyze);

            foreach (var filter in data.Project.Filters)
            {
                var parsedFilter = new FilterValueParsed(filter);
                var minMax = GetMinMaxValues(data.SrcConnection, parsedFilter);
                GenerateBatches(minMax, filter, parsedFilter);
            }

            SetStep(data, ExecutionStepsTypes.CreatingTables);
        }


        public bool ValidateStepExecution(Project project)
        {
            return (project.ExecutionState == ExecutionStepsTypes.FullAnalyze ||
                    project.ExecutionState == ExecutionStepsTypes.NotStarted ||
                    project.ExecutionState == ExecutionStepsTypes.Canceled);
        }

        private static MinMaxItems GetMinMaxValues(SqlConnection srcConnection, FilterValueParsed parsedFilter)
        {
            string minMaxQuery = string.Format(
                    "SELECT MIN([{0}]),MAX([{0}]) FROM [{1}]",
                    parsedFilter.IndexColumn,
                    parsedFilter.TableName
                );

            MinMaxItems minMax;
            using (var sqlCommand = srcConnection.CreateCommand())
            {
                sqlCommand.CommandText = minMaxQuery;
                using (SqlDataReader sqlReader = sqlCommand.ExecuteReader())
                {
                    sqlReader.Read();

                    minMax.MinValue = sqlReader[0];
                    minMax.MaxValue = sqlReader[1];
                }
            }

            return minMax;
        }

        private static void GenerateBatches(MinMaxItems minMax, Filter filter, FilterValueParsed parsedFilter)
        {

            filter.Batches.Clear();

            switch (parsedFilter.IndexColumnType)
            {
                case "datetime":
                    GenerateBatchesDateTime((DateTime)minMax.MinValue, (DateTime)minMax.MaxValue, filter, parsedFilter);
                    break;
                case "int":
                    GenerateBatchesInt((int)minMax.MinValue, (int)minMax.MaxValue, filter, parsedFilter);
                    break;
                default:
                    throw new InvalidArgumentException("Unsupported index field " + parsedFilter.IndexColumnType);
            }
        }

        private static void GenerateBatchesInt(int min, int max, Filter filter, FilterValueParsed parsedFilter)
        {
            int stepSize = int.Parse(parsedFilter.IndexStep);
            for (
                int first = min, second = first + stepSize;
                first <= max;
                first = second, second += stepSize
                )
            {
                filter.Batches.Add(new Batch
                {
                    BatchFilterMin = first.ToString(CultureInfo.InvariantCulture),
                    BatchFilterMax = second.ToString(CultureInfo.InvariantCulture),
                    Filter = filter
                });
            }
        }

        private static void GenerateBatchesDateTime(DateTime min, DateTime max, Filter filter, FilterValueParsed parsedFilter)
        {
            int stepSize = int.Parse(parsedFilter.IndexStep);
            for (
                DateTime first = min, second = first.AddSeconds(stepSize);
                second <= max;
                first = second, second = second.AddSeconds(stepSize)
            )
            {
                filter.Batches.Add(new Batch
                {
                    BatchFilterMin = first.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                    BatchFilterMax = second.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                    Filter = filter
                });
            }
        }

        private struct MinMaxItems
        {
            public object MinValue;
            public object MaxValue;
        }
    }
}
