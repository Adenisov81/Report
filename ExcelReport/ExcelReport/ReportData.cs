using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExcelReport
{
    public class BaseData
    {
        [XmlElement(ElementName = "Date")]
        public string Date { get; set; }
        [XmlElement(ElementName = "IssuedAirCargoAeroflot")]
        public string IssuedAirCargoAeroflot { get; set; }
        [XmlElement(ElementName = "IssuedAirCargoBridge")]
        public string IssuedAirCargoBridge { get; set; }
        [XmlElement(ElementName = "IssuedAirCargoOther")]
        public string IssuedAirCargoOther { get; set; }
        [XmlElement(ElementName = "IssuedAirMailAeroflot")]
        public string IssuedAirMailAeroflot { get; set; }
        [XmlElement(ElementName = "IssuedAirMailBridge")]
        public string IssuedAirMailBridge { get; set; }
        [XmlElement(ElementName = "IssuedAirMailOther")]
        public string IssuedAirMailOther { get; set; }
        [XmlElement(ElementName = "IssuedMoscowVVLAeroflot")]
        public string IssuedMoscowVVLAeroflot { get; set; }
        [XmlElement(ElementName = "IssuedMoscowVVLOther")]
        public string IssuedMoscowVVLOther { get; set; }
        [XmlElement(ElementName = "IssuedMoscowMVLAeroflot")]
        public string IssuedMoscowMVLAeroflot { get; set; }
        [XmlElement(ElementName = "IssuedMoscowMVLOther")]
        public string IssuedMoscowMVLOther { get; set; }
        [XmlElement(ElementName = "IssuedSVHAeroflotStock")]
        public string IssuedSVHAeroflotStock { get; set; }
        [XmlElement(ElementName = "IssuedSVHAeroflotBoard")]
        public string IssuedSVHAeroflotBoard { get; set; }
        [XmlElement(ElementName = "IssuedSVHBridgeStock")]
        public string IssuedSVHBridgeStock { get; set; }
        [XmlElement(ElementName = "IssuedSVHBridgeBoard")]
        public string IssuedSVHBridgeBoard { get; set; }
        [XmlElement(ElementName = "IssuedDepartureAeroflot")]
        public string IssuedDepartureAeroflot { get; set; }
        [XmlElement(ElementName = "IssuedDepartureBridge")]
        public string IssuedDepartureBridge { get; set; }
        [XmlElement(ElementName = "IssuedDepartureOther")]
        public string IssuedDepartureOther { get; set; }
        [XmlElement(ElementName = "ReceivedAirCargoAeroflot")]
        public string ReceivedAirCargoAeroflot { get; set; }
        [XmlElement(ElementName = "ReceivedAirCargoBridgeImport")]
        public string ReceivedAirCargoBridgeImport { get; set; }
        [XmlElement(ElementName = "ReceivedAirCargoOther")]
        public string ReceivedAirCargoOther { get; set; }
        [XmlElement(ElementName = "ReceivedAirMailAeroflot")]
        public string ReceivedAirMailAeroflot { get; set; }
        [XmlElement(ElementName = "ReceivedAirMailBridge")]
        public string ReceivedAirMailBridge { get; set; }
        [XmlElement(ElementName = "ReceivedAirMailOther")]
        public string ReceivedAirMailOther { get; set; }
        [XmlElement(ElementName = "ReceivedMoscowVVLAeroflot")]
        public string ReceivedMoscowVVLAeroflot { get; set; }
        [XmlElement(ElementName = "ReceivedMoscowVVLOther")]
        public string ReceivedMoscowVVLOther { get; set; }
        [XmlElement(ElementName = "ReceivedMoscowMVLAeroflot")]
        public string ReceivedMoscowMVLAeroflot { get; set; }
        [XmlElement(ElementName = "ReceivedMoscowMVLOther")]
        public string ReceivedMoscowMVLOther { get; set; }
        [XmlElement(ElementName = "ReceivedSVHVVLAeroflot")]
        public string ReceivedSVHVVLAeroflot { get; set; }
        [XmlElement(ElementName = "ReceivedSVHVVLOther")]
        public string ReceivedSVHVVLOther { get; set; }
        [XmlElement(ElementName = "ReceivedSVHMVLAeroflot")]
        public string ReceivedSVHMVLAeroflot { get; set; }
        [XmlElement(ElementName = "ReceivedSVHMVLOther")]
        public string ReceivedSVHMVLOther { get; set; }
        [XmlElement(ElementName = "ReceivedDepartureAeroflot")]
        public string ReceivedDepartureAeroflot { get; set; }
        [XmlElement(ElementName = "ReceivedDepartureBridge")]
        public string ReceivedDepartureBridge { get; set; }
        [XmlElement(ElementName = "ReceivedDepartureOther")]
        public string ReceivedDepartureOther { get; set; }
        [XmlElement(ElementName = "StockImportVVLReady")]
        public string StockImportVVLReady { get; set; }
        [XmlElement(ElementName = "StockImportVVLProcess")]
        public string StockImportVVLProcess { get; set; }
        [XmlElement(ElementName = "StockImportMVLReady")]
        public string StockImportMVLReady { get; set; }
        [XmlElement(ElementName = "StockImportMVLProcess")]
        public string StockImportMVLProcess { get; set; }
        [XmlElement(ElementName = "StockExportVVLTransfer")]
        public string StockExportVVLTransfer { get; set; }
        [XmlElement(ElementName = "StockExportVVLExport")]
        public string StockExportVVLExport { get; set; }
        [XmlElement(ElementName = "StockExportMVLTransfer")]
        public string StockExportMVLTransfer { get; set; }
        [XmlElement(ElementName = "StockExportMVLExport")]
        public string StockExportMVLExport { get; set; }
    }

    public class ReportData
    {
        [XmlElement(ElementName = "Common")]
        public BaseData Common { get; set; }
        [XmlElement(ElementName = "PrevMonth")]
        public BaseData PrevMonth { get; set; }
        [XmlElement(ElementName = "CurrentMonth")]
        public BaseData CurrentMonth { get; set; }
        [XmlElement(ElementName = "PrevDay")]
        public List<BaseData> PrevDays { get; set; }
    }
}
