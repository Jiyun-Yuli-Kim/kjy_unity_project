using UnityEngine;

namespace CustomUtility
{
    namespace IO
    {
        /// <summary>
        /// # Summary
        /// A class that represents a CSV file as a table (2D array).
        /// This class allows CSV data to be accessed via row and column indices.
        /// CSV 파일을 테이블(2차원 배열)로 표현하는 클래스.
        /// 이 클래스는 CSV 데이터를 행과 열 인덱스를 통해 접근할 수 있도록 한다.
        /// 
        /// ## Features:
        /// - Supports accessing data by specifying row and column indices.
        /// - Provides methods for retrieving entire rows of data.
        /// ## 기능:
        /// - 행과 열 인덱스를 지정하여 데이터에 접근할 수 있다.
        /// - 전체 행의 데이터를 검색하기 위한 메서드를 제공한다.
        /// </summary>
        [System.Serializable] public class CsvTable : Csv
        {
            public string[,] Table { get; set; }

            public CsvTable(string path, char splitSymbol) : base(path, splitSymbol) { }

            /// <summary>
            /// Retrieves data from a specific cell in the table.
            /// 테이블에서 특정 셀의 데이터를 가져옴.
            /// </summary>
            /// <param name="row">Row index of the cell. 셀의 행 인덱스.</param>
            /// <param name="column">Column index of the cell. 셀의 열 인덱스.</param>
            /// <returns>The data at the specified cell. 지정된 셀의 데이터.</returns>
            public string GetData(int row, int column)
            {
                (int, int) rc = GetClampTableIndex(row, column);
                return Table[rc.Item1, rc.Item2];
            }

            /// <summary>
            /// Retrieves data from a specific cell using a Vector2Int for indexing.
            /// Vector2Int를 사용하여 특정 셀의 데이터를 가져옴.
            /// </summary>
            /// <param name="vector2">A Vector2Int representing the cell position. 셀 위치를 나타내는 Vector2Int.</param>
            /// <returns>The data at the specified cell. 지정된 셀의 데이터.</returns>
            public string GetData(Vector2Int vector2)
            {
                (int, int) rc = GetClampTableIndex(vector2.y, vector2.x);
                return Table[rc.Item1, rc.Item2];
            }

            /// <summary>
            /// Retrieves all data from a specific row as a string array.
            /// 특정 행의 모든 데이터를 문자열 배열로 가져옴.
            /// </summary>
            /// <param name="row">Row index to get data from. 데이터를 가져올 행 인덱스.</param>
            /// <returns>All data from the specified row as a string array. 지정된 행의 모든 데이터를 문자열 배열로 반환.</returns>
            public string[] GetLine(int row)
            {
                int columns = Table.GetLength(1);
                string[] line = new string[columns];

                for (int c = 0; c < columns; c++)
                {
                    line[c] = Table[Mathf.Clamp(row, 0, Table.GetLength(0) - 1), c];
                }

                return line;
            }

            /// <summary>
            /// Clamps the table indices to ensure they are within the array bounds.
            /// 테이블 인덱스를 클램핑해 배열 범위 내로 조정한다.
            /// </summary>
            /// <param name="r">Row index to clamp. 클램핑할 행 인덱스.</param>
            /// <param name="c">Column index to clamp. 클램핑할 열 인덱스.</param>
            /// <returns>A tuple containing the clamped row and column indices. 클램핑된 행과 열 인덱스를 포함하는 튜플.</returns>
            private (int, int) GetClampTableIndex(int r, int c)
            {
                return (Mathf.Clamp(r, 0, Table.GetLength(0) - 1),
                        Mathf.Clamp(c, 0, Table.GetLength(1) - 1));
            }
        }
    }
}