using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Adform.Academy.DataTransfer.Core.DataTransfer.ValueParsers;
using Adform.Academy.DataTransfer.Core.DTO.Models;
using Microsoft.SqlServer.Management.Common;

namespace Adform.Academy.DataTransfer.Core.DataTransfer.Actions
{
    public class AnalyzeBase : ActionBase
    {

        protected static MinMaxItems GetMinMaxValues(SqlConnection srcConnection, FilterValueParsed parsedFilter)
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

        protected static void GenerateBatchesAsc(MinMaxItems minMax, Filter filter, FilterValueParsed parsedFilter)
        {
            switch (parsedFilter.IndexColumnType)
            {
                case "datetime":
                    GenerateBatchesDateTimeAsc((DateTime)minMax.MinValue, (DateTime)minMax.MaxValue, filter, parsedFilter);
                    break;
                case "int":
                    GenerateBatchesIntAsc((int)minMax.MinValue, (int)minMax.MaxValue, filter, parsedFilter);
                    break;
                default:
                    throw new InvalidArgumentException("Unsupported index field " + parsedFilter.IndexColumnType);
            }
        }

        protected static void GenerateBatchesDesc(MinMaxItems minMax, Filter filter, FilterValueParsed parsedFilter)
        {
            switch (parsedFilter.IndexColumnType)
            {
                case "datetime":
                    GenerateBatchesDateTimeDesc((DateTime)minMax.MinValue, (DateTime)minMax.MaxValue, filter, parsedFilter);
                    break;
                case "int":
                    GenerateBatchesIntDesc((int)minMax.MinValue, (int)minMax.MaxValue, filter, parsedFilter);
                    break;
                default:
                    throw new InvalidArgumentException("Unsupported index field " + parsedFilter.IndexColumnType);
            }
        }

        protected MinMaxItems GetCurrentMinMaxFromBatches(Filter filter, string indexColumnType)
        {
            MinMaxItems currentMinMax;

            switch (indexColumnType)
            {
                case "datetime":
                    currentMinMax = new MinMaxItems
                    {
                        MinValue = filter.Batches.Select(b => 
                            DateTime.ParseExact(b.BatchFilterMin, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture))
                        .Min(val => val),

                        MaxValue = filter.Batches.Select(b => 
                            DateTime.ParseExact(b.BatchFilterMax, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture))
                        .Max(val => val)
                    };
                    break;
                case "int":
                    currentMinMax = new MinMaxItems
                    {
                        MinValue = filter.Batches.Select(b =>
                            int.Parse(b.BatchFilterMin,CultureInfo.InvariantCulture))
                        .Min(val => val),

                        MaxValue = filter.Batches.Select(b =>
                            int.Parse(b.BatchFilterMax, CultureInfo.InvariantCulture))
                        .Max(val => val)
                    };
                    break;
                default:
                    throw new InvalidArgumentException("Unsupported index field " + indexColumnType);
            }
            return currentMinMax;
        }

        protected static void GenerateBatchesIntAsc(int min, int max, Filter filter, FilterValueParsed parsedFilter)
        {
            int stepSize = int.Parse(parsedFilter.IndexStep);
            for (
                int first = min, second = first + stepSize;
                first <= max;
                first = second, second += stepSize
                )
            {
                filter.Batches.Add(GenerateBatch(filter, first, second));
            }
        }

        protected static void GenerateBatchesDateTimeAsc(DateTime min, DateTime max, Filter filter, FilterValueParsed parsedFilter)
        {
            int stepSize = int.Parse(parsedFilter.IndexStep);
            for (
                DateTime first = min, second = first.AddSeconds(stepSize);
                first <= max;
                first = second, second = second.AddSeconds(stepSize)
            )
            {
                filter.Batches.Add(GenerateBatch(filter, first, second));
            }
        }

        protected static void GenerateBatchesIntDesc(int min, int max, Filter filter, FilterValueParsed parsedFilter)
        {
            int stepSize = int.Parse(parsedFilter.IndexStep);
            for (
                int second = max, first = second - stepSize;
                second >= min;
                second = first, first -= stepSize
                )
            {
                filter.Batches.Add(GenerateBatch(filter, first, second));
            }
        }

        protected static void GenerateBatchesDateTimeDesc(DateTime min, DateTime max, Filter filter, FilterValueParsed parsedFilter)
        {
            int stepSize = int.Parse(parsedFilter.IndexStep);
            for (
                DateTime second = max, first = second.AddSeconds(-stepSize);
                second >= min;
                second = first, first = first.AddSeconds(-stepSize)
            )
            {
                filter.Batches.Add(GenerateBatch(filter, first, second));
            }
        }

        private static Batch GenerateBatch(Filter filter, DateTime first, DateTime second)
        {
            return new Batch
            {
                BatchFilterMin = first.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                BatchFilterMax = second.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                Filter = filter
            };
        }

        private static Batch GenerateBatch(Filter filter, int first, int second)
        {
            return new Batch
            {
                BatchFilterMin = first.ToString(CultureInfo.InvariantCulture),
                BatchFilterMax = second.ToString(CultureInfo.InvariantCulture),
                Filter = filter
            };
        }

        protected struct MinMaxItems
        {
            public object MinValue;
            public object MaxValue;
        }
    }
}
