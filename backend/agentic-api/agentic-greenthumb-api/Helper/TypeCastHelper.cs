namespace AgenticGreenthumbApi.Helper
{
    public static class TypeCastHelper
    {
        //https://learn.microsoft.com/en-us/dotnet/api/system.convert.changetype?view=net-9.0
        public static T ChangeType<T>(object value)
        {
            if(value == null)
            {
                return default;
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
