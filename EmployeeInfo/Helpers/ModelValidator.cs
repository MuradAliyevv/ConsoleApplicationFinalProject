using System;

namespace EmployeeInfo.Helpers
{
    public static class ModelValidator
    {
        public static bool CheckNullAndMaxLength(string value, int maxLength) =>
        !string.IsNullOrWhiteSpace(value) && value != string.Empty && value.Length <= maxLength;

        public static bool CheckHour(int hour) =>
        hour > 0 && hour < 24;

        public static bool CheckMinute(int minute) =>
        minute > 0 && minute < 60;
    }
}