namespace AgenticGreenthumbApi.Domain
{
    public class Plant
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ScientificName { get; set; }
        public string LightRequirement { get; set; } //Use Related Enum Key Names, But Spaced. Best guess and based on Light Requirement Details
        public string LightRequirementDetails { get; set; }
        public int MinLightingValue { get; set; } //Use Related Enum Values associated with Key
        public string WaterRequirement { get; set; } //Use Related Enum Key Names, But Spaced
        public string WaterRequirementDetails { get; set; } //Use Related Enum Key Names, But Spaced. Best guess and based on Water Requirement Details
        public int MinMoistureValue { get; set; } //Use Related Enum Values associated with Key
        public string SoilRequirement { get; set; }
        public int SoilVoidRatio { get; set; }
        public int RowSpacingIn { get; set; }
        public int PlantSpacingIn { get; set; }
    }
}
