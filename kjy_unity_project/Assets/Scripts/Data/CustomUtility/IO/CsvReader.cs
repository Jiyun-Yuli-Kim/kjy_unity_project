using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CustomUtility
{
    namespace IO
    {
        /// <summary>
        /// # Summary
        /// A static class for reading CSV files and loading them into different data structures.
        /// This class supports loading CSV data into either a 2D array or a Dictionary structure.
        /// CSV 파일을 읽어 다양한 데이터 구조로 로드하는 정적 클래스.
        /// 이 클래스는 CSV 데이터를 2D 배열 또는 Dictionary 구조로 로드하는 것을 지원한다.
        /// 
        /// ## Usage:
        /// - Create an instance of a class that inherits from `Csv`, such as `CsvTable` or `CsvDictionary`.
        /// - Call the `CsvReader.Read` method to load data from a CSV file.
        /// - Access the data through the properties or methods provided by the `CsvTable` or `CsvDictionary` class.
        /// ## 사용 방법:
        /// - `CsvTable` 또는 `CsvDictionary`와 같은 Csv를 상속받은 클래스의 인스턴스를 생성한다.
        /// - `CsvReader.Read` 메서드를 호출하여 CSV 파일에서 데이터를 로드한다.
        /// - `CsvTable` 또는 `CsvDictionary` 클래스가 제공하는 속성이나 메서드를 통해 데이터를 접근한다.
        ///
        /// ## Features:
        /// - Supports both 2D array and string-based Dictionary structures for storing data.
        /// - Provides utility methods for validating file paths and checking file contents.
        /// - Logs errors and success messages in the Unity Editor.
        /// ## 기능:
        /// - 데이터 저장을 위한 테이블(2D 배열)과 string 기반 Dictionary 구조를 지원한다.
        /// - 파일 경로와 내용을 검증하기 위한 유틸리티 메서드를 제공한다.
        /// - 유니티 에디터에서 오류와 성공 메시지를 로깅한다.
        /// </summary>

        public static class CsvReader
        {
            /// <summary>
            /// Gets the base path for loading files based on the environment.
            /// 파일을 로드할 기본 경로를 가져옴.
            /// </summary>
            public static string BasePath
            {
                get
                {
#if UNITY_EDITOR
                    return Application.dataPath + Path.DirectorySeparatorChar;
#else
                    return Application.persistentDataPath + Path.DirectorySeparatorChar;
#endif
                }
            }

            /// <summary>
            /// Reads a CSV file and loads it into the appropriate data structure.
            /// CSV 파일을 읽고 적절한 데이터 구조로 로드한다.
            /// </summary>
            /// <param name="csv">The CSV object to be loaded. 데이터를 로드할 CSV 객체.</param>
            public static void Read(Csv csv)
            {
                if (!IsValidPath(csv) || 
                    !IsValidEmpty(csv, out string[] lines))
                    return;

                bool isReadSuccessful;

                switch (csv)
                {
                    case CsvTable table:
                        isReadSuccessful = ReadToTable(table, lines);
                        break;
                    case CsvDictionary dictionary:
                        isReadSuccessful = ReadToDictionary(dictionary, lines);
                        break;
                    default:
                        isReadSuccessful = false;
                        break;
                }

                PrintResult(csv, isReadSuccessful);
            }

            /// <summary>
            /// Reads a CSV file and loads it into a CsvDictionary structure.
            /// CSV 파일을 읽어 CsvDictionary로 로드한다.
            /// </summary>
            /// <param name="csv">The CsvDictionary object to load data into. 데이터를 로드할 CsvDictionary 객체.</param>
            /// <param name="lines">The lines read from the CSV file. CSV 파일에서 읽은 줄.</param>
            /// <returns>True if reading was successful, otherwise false. 읽기가 성공하면 true, 그렇지 않으면 false.</returns>
            private static bool ReadToDictionary(CsvDictionary csv, string[] lines)
            {
                string[] fieldsIndex = lines[0].Split(csv.SplitSymbol);
                int columns = fieldsIndex.Length;
                csv.Dict = new Dictionary<string, Dictionary<string, string>>();

                for (int r = 1; r < lines.Length; r++)
                {
                    string[] fields = lines[r].Split(csv.SplitSymbol);

                    if (fields.Length < columns)
                    {
                        return false;
                    }

                    string rowKey = fields[0];
                    csv.Dict[rowKey] = new Dictionary<string, string>(columns);

                    for (int c = 1; c < columns; c++)
                    {
                        csv.Dict[rowKey][fieldsIndex[c]] = fields[c];
                    }
                }

                return true;
            }

            /// <summary>
            /// Reads a CSV file and loads it into a 2D array (Table) structure.
            /// CSV 파일을 읽어 2차원 배열(Table) 구조로 로드한다.
            /// </summary>
            /// <param name="csv">The CsvTable object to load data into. 데이터를 로드할 CsvTable 객체.</param>
            /// <param name="lines">The lines read from the CSV file. CSV 파일에서 읽은 줄.</param>
            /// <returns>True if reading was successful, otherwise false. 읽기가 성공하면 true, 그렇지 않으면 false.</returns>
            private static bool ReadToTable(CsvTable csv, string[] lines)
            {
                string[] firstLineFields = lines[0].Split(csv.SplitSymbol);
                int rows = lines.Length;
                int columns = firstLineFields.Length;

                csv.Table = new string[rows, columns];

                for (int r = 0; r < rows; r++)
                {
                    string[] fields = lines[r].Split(csv.SplitSymbol);
                    if (fields.Length < columns)
                    {
                        return false;
                    }

                    for (int c = 0; c < columns; c++)
                    {
                        csv.Table[r, c] = fields[c];
                    }
                }

                return true;
            }

            /// <summary>
            /// Validates whether a file exists at the specified path.
            /// 지정된 경로에 파일이 존재하는지 검증한다.
            /// </summary>
            /// <param name="csv">The CSV object to check. 확인할 CSV 객체.</param>
            /// <returns>True if the file exists, otherwise false. 파일이 존재하면 true, 그렇지 않으면 false.</returns>
            private static bool IsValidPath(Csv csv)
            {
                if (!File.Exists(csv.FilePath))
                {
#if UNITY_EDITOR
                    Debug.LogError($"Error: CSV file not found at path: {csv.FilePath}");
#endif
                    return false;
                }
                return true;
            }

            /// <summary>
            /// Reads all lines from a file and checks if it is empty.
            /// 파일의 모든 줄을 읽고 비어 있는지 확인한다.
            /// </summary>
            /// <param name="csv">The CSV object to be loaded. 데이터를 로드할 CSV 객체.</param>
            /// <param name="lines">If the file is not empty, assigns the lines read from the file. 파일이 비어 있지 않으면, 읽은 줄을 대입한다.</param>
            /// <returns>True if the file is not empty, otherwise false. 파일이 비어 있지 않으면 true, 그렇지 않으면 false.</returns>
            private static bool IsValidEmpty(Csv csv, out string[] lines)
            {
                lines = File.ReadAllLines(csv.FilePath);

                if (lines.Length == 0)
                {
#if UNITY_EDITOR
                    Debug.LogError($"Error: CSV file at path {csv.FilePath} is empty.");
#endif
                    return false;
                }
                return true;
            }

            /// <summary>
            /// Logs the result of the CSV file loading process.
            /// CSV 파일 로딩 프로세스의 결과를 로그로 출력한다.
            /// </summary>
            /// <param name="csv">The CSV object processed. 처리된 CSV 객체.</param>
            /// <param name="result">True if loading was successful, otherwise false. 로딩이 성공하면 true, 그렇지 않으면 false.</param>
            private static void PrintResult(Csv csv, bool result)
            {
#if UNITY_EDITOR
                if (result)
                {
                    Debug.Log($"Successfully loaded CSV file from path: {csv.FilePath}");
                }
                else
                {
                    Debug.LogError($"Error: CSV file at path {csv.FilePath} has inconsistent column lengths.");
                }
#endif
            }
        }
    }
}