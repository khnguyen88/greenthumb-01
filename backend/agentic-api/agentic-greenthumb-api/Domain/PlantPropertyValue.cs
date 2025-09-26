namespace AgenticGreenthumbApi.Domain
{
    public class PlantPropertyValue
    {
        public enum LightReq
        {
            Low,
            Medium,
            Moderate,
            High,
            VeryHigh,
            Max,
            FullShade,
            PartialShade,
            IndirectSun,
            PartialSun,
            DirectSun,
            FullSun
        }

        public enum WaterReq
        {
            VeryLow,
            Low,
            MediumLow,
            Fair,
            Medium,
            Moderate,
            MediumHigh,
            High,
            VeryHigh,
            Max
        }

        /// <summary>
        /// This dictionary maps the LightReq requirements of plants to a LightReq intensity value that ranges from 0 to 100.
        /// </summary>
        /// <remarks>
        /// The dictionary reflects the MINIMUM required LightReq intensity value required to meet a specific LightReqing condition.
        /// A plant with that requires partial sun will need a LightReq source that provides a minimum of 
        /// </remarks>
        public static Dictionary<LightReq, int> MinLightReqValue { get; } = new()
        {
            [LightReq.Low] = 20,
            [LightReq.Medium] = 40,
            [LightReq.Moderate] = 60,
            [LightReq.High] = 75,
            [LightReq.VeryHigh] = 85,
            [LightReq.Max] = 85,
            [LightReq.FullShade] = 15,
            [LightReq.PartialShade] = 25,
            [LightReq.IndirectSun] = 45,
            [LightReq.PartialSun] = 60,
            [LightReq.DirectSun] = 85,
            [LightReq.FullSun] = 85,
        };

        /// <summary>
        /// This dictionary maps the soil satutration requirements of plants to soil moisture value that ranges from 0 to 100.
        /// </summary>
        /// <remarks>
        /// The dictionary reflects the MINIMUM required saturation required to meet a plant's WaterReqing needs.
        /// A plant with moderate water requirements will need a Water source that provides a minimum of 40 minimum water moisture ratio in the soil.
        /// </remarks>
        public static Dictionary<WaterReq, int> MinWaterReqValue { get; } = new()
        {
            [WaterReq.VeryLow] = 5,
            [WaterReq.Low] = 10,
            [WaterReq.Fair] = 20,
            [WaterReq.Medium] = 40,
            [WaterReq.Moderate] = 40,
            [WaterReq.MediumHigh] = 50,
            [WaterReq.High] = 60,
            [WaterReq.VeryHigh] = 70,
            [WaterReq.Max] = 80
        };
    }
}
