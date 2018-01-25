using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExcelReport
{
    [XmlRoot(ElementName = "Row")]
    public class Row
    {
        [XmlAttribute(AttributeName = "Cell")]
        public string Cell { get; set; }
        [XmlAttribute(AttributeName = "Style")]
        public string Style { get; set; }
        [XmlText]
        public string Text { get; set; }
        [XmlElement(ElementName = "Values")]
        public Values Values { get; set; }
        [XmlElement(ElementName = "Caption")]
        public Caption Caption { get; set; }
    }

    [XmlRoot(ElementName = "Header")]
    public class Header
    {
        [XmlElement(ElementName = "Row")]
        public List<Row> Rows { get; set; }
    }

    [XmlRoot(ElementName = "Value")]
    public class Value
    {
        [XmlAttribute(AttributeName = "Cell")]
        public string Cell { get; set; }
        [XmlAttribute(AttributeName = "IsFormula")]
        public bool IsFormula { get; set; }
        [XmlAttribute(AttributeName = "IsPrevDays")]
        public bool IsPrevDays { get; set; }
        [XmlText]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "DateFormat")]
        public string DateFormat { get; set; }
        [XmlAttribute(AttributeName = "IsNotConvert")]
        public bool IsNotConvert { get; set; }
        [XmlAttribute(AttributeName = "Data")]
        public string Data { get; set; }
    }

    [XmlRoot(ElementName = "Values")]
    public class Values
    {
        [XmlElement(ElementName = "Value")]
        public List<Value> Items { get; set; }
    }

    [XmlRoot(ElementName = "Caption")]
    public class Caption
    {
        [XmlAttribute(AttributeName = "Cell")]
        public string Cell { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Table")]
    public class Table
    {
        [XmlElement(ElementName = "Row")]
        public List<Row> Rows { get; set; }
        [XmlAttribute(AttributeName = "Cell")]
        public string Cell { get; set; }
    }

    [XmlRoot(ElementName = "NumberFormat")]
    public class NumberFormat
    {
        [XmlText]
        public string Text { get; set; }
        [XmlAttribute(AttributeName = "Cell")]
        public string Cell { get; set; }
    }

    [XmlRoot(ElementName = "ReportStructure")]
    public class ReportStructure
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Header")]
        public Header Header { get; set; }
        [XmlElement(ElementName = "Table")]
        public Table Table { get; set; }
        [XmlElement(ElementName = "HideColumn")]
        public List<string> HideColumns { get; set; }
        [XmlElement(ElementName = "CoditionalFormat")]
        public List<string> CoditionalFormats { get; set; }
        [XmlElement(ElementName = "WrapText")]
        public List<string> WrapTexts { get; set; }
        [XmlElement(ElementName = "Merge")]
        public List<string> Merges { get; set; }
        [XmlElement(ElementName = "NumberFormat")]
        public List<NumberFormat> NumberFormats { get; set; }
    }
}
