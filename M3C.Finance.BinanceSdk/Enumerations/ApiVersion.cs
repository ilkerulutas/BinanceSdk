namespace M3C.Finance.BinanceSdk.Enumerations
{
    public class ApiVersion
    {
        private ApiVersion(string value) { Value = value; }

        private string Value { get; }

        public static ApiVersion Version1 => new ApiVersion("v1");
        public static ApiVersion Version3 => new ApiVersion("v3");

        public static implicit operator string(ApiVersion version) => version.Value;
        public override string ToString() => Value;
    }
}
