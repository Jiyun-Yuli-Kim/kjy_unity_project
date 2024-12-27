using System.IO;
using UnityEngine;

namespace CustomUtility
{
    namespace IO
    {
        /// <summary>
        /// # Summary
        /// Base class for CSV structures.
        /// This class provides common properties and methods for handling CSV file paths and delimiters.
        /// CSV 구조를 위한 기본 클래스.
        /// 이 클래스는 CSV 파일 경로와 구분 기호를 처리하기 위한 공통 속성과 메서드를 제공한다.
        /// 
        /// ## Features:
        /// - Provides a common interface for CsvTable and CsvDictionary.
        /// - Handles file path and delimiter settings for CSV data.
        /// ## 기능:
        /// - CsvTable과 CsvDictionary를 위한 공통 인터페이스를 제공한다.
        /// - CSV 데이터의 파일 경로와 구분 기호 설정을 처리한다.
        /// </summary>
        public class Csv
        {
            [SerializeField] private string _filePath;
            public string FilePath => Path.Combine(CsvReader.BasePath, _filePath);
            
            [field: SerializeField] public char SplitSymbol { get; private set; }

            protected Csv(string path, char splitSymbol)
            {
                _filePath = path;
                SplitSymbol = splitSymbol;
            }
        }
    }
}