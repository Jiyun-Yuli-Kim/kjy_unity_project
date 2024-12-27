using System.Collections.Generic;
using UnityEngine;

namespace CustomUtility
{
    namespace IO
    {
        /// <summary>
        /// # Summary
        /// A class representing a CSV file as a string-based dictionary.
        /// This class allows access to CSV data using row and column keys.
        /// CSV 파일을 string 기반 Dictionary 구조로 표현하는 클래스.
        /// 이 클래스는 CSV 데이터를 행과 열 키를 통해 접근할 수 있도록 한다.
        /// 
        /// ## Features:
        /// - Allows data access by specifying row and column keys.
        /// - Provides methods for retrieving entire rows of data.
        /// ## 기능:
        /// - 행과 열 키를 지정하여 데이터에 접근할 수 있다.
        /// - 전체 행의 데이터를 검색하기 위한 메서드를 제공한다.
        /// </summary>
        [System.Serializable] public class CsvDictionary : Csv
        {
            public Dictionary<string, Dictionary<string, string>> Dict { get; set; }
            public CsvDictionary(string path, char splitSymbol) : base(path, splitSymbol) { }

            /// <summary>
            /// Retrieves data from a specific cell in the dictionary.
            /// Dictionary에서 특정 셀의 데이터를 가져옴.
            /// </summary>
            /// <param name="row">The row key of the cell. 셀의 행 키.</param>
            /// <param name="column">The column key of the cell. 셀의 열 키.</param>
            /// <returns>The data at the specified row and column, or null if not found. 지정된 행과 열의 데이터 또는 없으면 null 반환.</returns>
            public string GetData(string row, string column)
            {
                if (Dict.TryGetValue(row, out var columns) && columns.TryGetValue(column, out var value))
                {
                    return value;
                }

                Debug.LogError($"Data not found for row '{row}' and column '{column}'.");
                return null;
            }

            /// <summary>
            /// Retrieves all data from a specific row as a string array.
            /// 특정 행의 모든 데이터를 문자열 배열로 가져옴.
            /// </summary>
            /// <param name="row">The row key to get data from. 데이터를 가져올 행 키.</param>
            /// <returns>All data from the specified row as a string array. 지정된 행의 모든 데이터를 문자열 배열로 반환.</returns>
            public string[] GetLine(string row)
            {
                if (!Dict.ContainsKey(row))
                {
                    Debug.LogError($"Row '{row}' not found in the CSV data.");
                    return null;
                }

                Dictionary<string, string> rowData = Dict[row];
                string[] line = new string[rowData.Count];
                int index = 0;

                foreach (var kvp in rowData)
                {
                    line[index++] = kvp.Value;
                }

                return line;
            }
        }
    }
}
