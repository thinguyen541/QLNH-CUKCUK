namespace AssignmentGPBL.Domain.Object
{
    /// <summary>
    /// object of pagination
    /// created by Thi Nguyen (20-8-2023)
    /// </summary>
    public class FilterObject
    {
        public string Property { get; set; }
        public object Value { get; set; }
        public int PropertyType { get; set; }
        public int Operator { get; set; }
        public int RelationType { get; set; }
    }
}
