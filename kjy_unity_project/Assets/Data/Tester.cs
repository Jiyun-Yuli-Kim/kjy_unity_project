using UnityEngine;
using CustomUtility.IO;

public class Tester : MonoBehaviour
{
    // 유니티 인스펙터 및 생성자에서 초기화
    [SerializeField] private CsvTable _tableCSV;
    [SerializeField] private CsvDictionary _dictCsv = new("DataTable/CsvTemp.csv", ',');
    
    public Vector2Int dataTablePos;
    
    private void Start()
    {
        // 읽기
        CsvReader.Read(_dictCsv);
        CsvReader.Read(_tableCSV);
        
        // Table 구조는 int타입 및 Vector2Int를 사용해 데이터 참조
        Debug.Log(_tableCSV.GetData(2, 3));
        Debug.Log(_tableCSV.GetData(dataTablePos));
        
        // Dictionary 구조는 string 두 개를 사용해 행, 열 데이터 참조
        Debug.Log(_dictCsv.GetData("2", "f"));
        
        // 두 형태 모두 하나의 행을 통째로 string배열로 받아오는 GetLine기능 지원
        string[] strArr1 = _tableCSV.GetLine(4);
        string[] strArr2 = _dictCsv.GetLine("2");
    }
}